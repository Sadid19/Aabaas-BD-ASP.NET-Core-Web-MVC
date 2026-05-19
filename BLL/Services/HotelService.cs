using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System.Collections.Generic;

namespace BLL.Services
{
    public class HotelService
    {
        HotelRepo repo;
        Mapper mapper;

        public HotelService(HotelRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }

        public List<HotelDTO> Get()
        {
            List<Hotel> data = repo.Get();
            return mapper.Map<List<HotelDTO>>(data);
        }

        public HotelDTO Get(int id)
        {
            Hotel data = repo.Get(id);
            if (data == null)
            {
                return null;
            }
            return mapper.Map<HotelDTO>(data);
        }

        public List<HotelDTO> Search(HotelSearchFilter filter)
        {
            List<Hotel> data = repo.GetFiltered(filter.City, filter.StarRating, filter.RoomType, filter.MinPrice, filter.MaxPrice);

            List<Hotel> priceSorted = data.OrderBy(h => h.PricePerNight).ToList();

            return mapper.Map<List<HotelDTO>>(priceSorted);
        }

        
        public List<string> GetCityList()
        {
            return repo.Get().Select(h => h.City).Where(city => !string.IsNullOrEmpty(city)).Distinct().OrderBy(city => city).ToList();
        }

        
        public List<string> GetRoomTypeList()
        {
            return repo.Get().Select(h => h.RoomType).Where(type => !string.IsNullOrEmpty(type)).Distinct().OrderBy(type => type).ToList();
        }

        public bool Create(HotelDTO dto)
        {
            Hotel hotel = mapper.Map<Hotel>(dto);
            return repo.Create(hotel);
        }

        public bool Update(HotelDTO dto)
        {
            Hotel hotel = mapper.Map<Hotel>(dto);
            hotel.HotelId = dto.HotelId;
            return repo.Update(hotel);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
