using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System.Collections.Generic;

namespace BLL.Services
{
    public class RecommendationService
    {
        BookingRepo bookingRepo;
        HotelRepo hotelRepo;
        PackageRepo packageRepo;
        Mapper mapper;

        public RecommendationService(BookingRepo bookingRepo, HotelRepo hotelRepo, PackageRepo packageRepo)
        {
            this.bookingRepo = bookingRepo;
            this.hotelRepo = hotelRepo;
            this.packageRepo = packageRepo;
            mapper = MapperConfig.GetMapper();
        }

        public (List<HotelDTO> Hotels, List<HotPackageDTO> Packages, string Heading) GetRecommendations(int? userId)
        {
            if (!userId.HasValue)
            {
                return (new List<HotelDTO>(), GetPopularPackages(), "Popular Deals");
            }

            List<Booking> history = bookingRepo.GetAllByUserId(userId.Value);

            if (history.Count == 0)
            {
                return (new List<HotelDTO>(), GetPopularPackages(), "Popular Deals");
            }

            List<string> bookedCities = new List<string>();
            List<string> bookedRoomTypes = new List<string>();
            List<int> bookedStarRatings = new List<int>();

            for (int i = 0; i < history.Count; i++)
            {
                if (history[i].Hotel == null)
                {
                    continue;
                }

                AddUnique(bookedCities, history[i].Hotel.City);
                AddUnique(bookedRoomTypes, history[i].Hotel.RoomType);
                if (history[i].Hotel.StarRating.HasValue)
                {
                    AddUniqueInt(bookedStarRatings, history[i].Hotel.StarRating.Value);
                }
            }

            List<Hotel> allHotels = hotelRepo.Get();
            List<Hotel> matchedHotels = new List<Hotel>();

            for (int i = 0; i < allHotels.Count; i++)
            {
                int score = CalculateScore(allHotels[i].City, allHotels[i].RoomType,allHotels[i].StarRating, bookedCities,bookedRoomTypes,bookedStarRatings);

                if (score >= 70)
                {
                    matchedHotels.Add(allHotels[i]);
                }
            }

            List<HotPackage> allPackages = packageRepo.GetActive();
            List<HotPackage> matchedPackages = new List<HotPackage>();

            for (int i = 0; i < allPackages.Count; i++)
            {
                if (allPackages[i].Hotel == null)
                {
                    continue;
                }

                int score = CalculateScore(allPackages[i].Hotel.City,allPackages[i].Hotel.RoomType, allPackages[i].Hotel.StarRating,bookedCities, bookedRoomTypes, bookedStarRatings);

                if (score >= 70)
                {
                    matchedPackages.Add(allPackages[i]);
                }
            }

            if (matchedHotels.Count == 0 && matchedPackages.Count == 0)
            {
                return (new List<HotelDTO>(), GetPopularPackages(), "Popular Deals");
            }

            return (mapper.Map<List<HotelDTO>>(matchedHotels), mapper.Map<List<HotPackageDTO>>(matchedPackages),"Recommended for You");
        }

        private int CalculateScore(string city, string roomType,int? starRating, List<string> bookedCities, List<string> bookedRoomTypes,List<int> bookedStarRatings)
        {
            int score = 0;

            if (IsInList(bookedCities, city))
            {
                score = score + 40;
            }

            if (IsInList(bookedRoomTypes, roomType))
            {
                score = score + 40;
            }

            if (starRating.HasValue && IsIntInList(bookedStarRatings, starRating.Value))
            {
                score = score + 20;
            }

            return score;
        }

        private bool IsInList(List<string> list, string value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsIntInList(List<int> list, int value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddUnique(List<string> list, string value)
        {
            if (value == null || value.Length == 0)
            {
                return;
            }

            if (!IsInList(list, value))
            {
                list.Add(value);
            }
        }

        private void AddUniqueInt(List<int> list, int value)
        {
            if (!IsIntInList(list, value))
                list.Add(value);
        }

        private List<HotPackageDTO> GetPopularPackages()
        {
            List<HotPackage> data = packageRepo.GetActive();
            return mapper.Map<List<HotPackageDTO>>(data);
        }
    }
}
