using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace App.Controllers
{
    public class HotelController : Controller
    {
        HotelService hotelService;

        public HotelController(HotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet]
        public IActionResult Index(HotelSearchFilter filter)
        {
            List<HotelDTO> hotels;

            bool hasFilter = false;
            if (filter.City != null && filter.City.Length > 0)
            {
                hasFilter = true;
            }
            if (filter.StarRating.HasValue)
            {
                hasFilter = true;
            }

            if (filter.RoomType != null && filter.RoomType.Length > 0)
            {
                hasFilter = true;
            }
            if (filter.MinPrice.HasValue)
            {
                hasFilter = true;
            }
            if (filter.MaxPrice.HasValue)
            {
                hasFilter = true;
            }

            if (hasFilter)
            {
                hotels = hotelService.Search(filter);
            }
            else
            {
                hotels = hotelService.Get();
            }

            ViewBag.Filter = filter;
            ViewBag.Cities = hotelService.GetCityList();
            ViewBag.RoomTypes = hotelService.GetRoomTypeList();
            return View(hotels);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            HotelDTO hotel = hotelService.Get(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }
    }
}
