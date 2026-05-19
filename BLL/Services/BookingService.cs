using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class BookingService
    {
        BookingRepo repo;
        HotelRepo hotelRepo;
        EmailService emailService;
        Mapper mapper;

        public BookingService(BookingRepo repo, HotelRepo hotelRepo, EmailService emailService)
        {
            this.repo = repo;
            this.hotelRepo = hotelRepo;
            this.emailService = emailService;
            mapper = MapperConfig.GetMapper();
        }

        public decimal CalculateCost(DateTime checkIn, DateTime checkOut, decimal pricePerNight)
        {
            int nights = (checkOut.Date - checkIn.Date).Days;

            if(nights <= 0)
            {
                return 0;
            }
            
            return nights * pricePerNight;
        }

        public BookingDTO PrepareBooking(BookingDTO dto)
        {
            Hotel hotel = hotelRepo.Get(dto.HotelId);

            if(hotel == null)
            {
                return dto;
            }
               
            dto.PricePerNight = hotel.PricePerNight;
            dto.HotelName = hotel.Name;
            dto.HotelCity = hotel.City;
            dto.Nights = (dto.CheckOut.Date - dto.CheckIn.Date).Days;
            dto.TotalCost = CalculateCost(dto.CheckIn, dto.CheckOut, hotel.PricePerNight);
            dto.Status = "Booked";
            return dto;
        }

        public int Create(BookingDTO dto)
        {
            Booking entity = mapper.Map<Booking>(dto);
            entity.Status = "Booked";
            repo.Create(entity);

            Booking saved = repo.Get(entity.BookingId);
            if(saved != null && saved.User != null && saved.User.Email != null)
            {
                BookingDTO mailDto = mapper.Map<BookingDTO>(saved);
                mailDto.Status = "Booked";
                emailService.SendBookedBooking(saved.User.Email, mailDto);
            }

            return entity.BookingId;
        }

        public List<BookingDTO> GetHistory(int userId)
        {
            List<Booking> list = repo.GetByUserId(userId);
            List<BookingDTO> result = mapper.Map<List<BookingDTO>>(list);

            for(int i = 0; i < result.Count; i++)
            {
                result[i].CanCancel = CanUserCancel(result[i]);
            }

            return result;
        }

        public List<BookingDTO> GetAll()
        {
            List<Booking> list = repo.GetAll();
            return mapper.Map<List<BookingDTO>>(list);
        }

        public BookingDTO Get(int id)
        {
            Booking booking = repo.Get(id);

            if(booking == null)
            {
                return null;
            }

            BookingDTO dto = mapper.Map<BookingDTO>(booking);
            dto.CanCancel = CanUserCancel(dto);
            return dto;
        }

        public bool CancelByUser(int bookingId, int userId)
        {
            Booking booking = repo.Get(bookingId);
            if (booking == null)
            {
                return false; 
            }
            if(booking.UserId != userId)
            {
                return false; 
            }

            BookingDTO dto = mapper.Map<BookingDTO>(booking);

            if(!CanUserCancel(dto))
            {
                return false;
            }

            bool updated = repo.UpdateStatus(bookingId, "Cancelled");

            if(!updated)
            {
                return false;
            }

            dto.Status = "Cancelled";
            if(booking.User != null && booking.User.Email != null)
            {

                emailService.SendCancelledBooking(booking.User.Email, dto);

            }

            return true;
        }

        private bool CanUserCancel(BookingDTO dto)
        {
            if(dto.Status == "Cancelled")
            {
                return false;
            }
            if(dto.Status == "Completed")
            {
                return false;
            }

            if(dto.CheckIn.Date <= DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }
}
