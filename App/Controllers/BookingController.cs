using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace App.Controllers
{
    public class BookingController : Controller
    {
        BookingService bookingService;
        HotelService hotelService;

        public BookingController(BookingService bookingService, HotelService hotelService)
        {
            this.bookingService = bookingService;
            this.hotelService = hotelService;
        }

        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        [HttpGet]
        public IActionResult Create(int hotelId)
        {
            if (!GetCurrentUserId().HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            HotelDTO hotel = hotelService.Get(hotelId);
            if (hotel == null)
            {
                return NotFound();
            }

            int userId = GetCurrentUserId().Value;

            BookingDTO model = new BookingDTO();
            model.HotelId = hotelId;
            model.UserId = userId;
            model.CheckIn = DateTime.Today.AddDays(1);
            model.CheckOut = DateTime.Today.AddDays(3);
            model.HotelName = hotel.Name;
            model.HotelCity = hotel.City;
            model.PricePerNight = hotel.PricePerNight;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingDTO model)
        {
            if (!GetCurrentUserId().HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = GetCurrentUserId().Value;
            model.UserId = userId;

            HotelDTO hotel = hotelService.Get(model.HotelId);

            ModelState.Remove("HotelName");
            ModelState.Remove("HotelCity");
            ModelState.Remove("UserEmail");
            ModelState.Remove("Status");

            if (!ModelState.IsValid)
            {
                if (hotel != null)
                {
                    model.HotelName = hotel.Name;
                    model.HotelCity = hotel.City;
                    model.PricePerNight = hotel.PricePerNight;
                }
                return View(model);
            }

            model.Nights = (model.CheckOut.Date - model.CheckIn.Date).Days;
            if (hotel != null)
            {
                model.PricePerNight = hotel.PricePerNight;
                model.HotelName = hotel.Name;
                model.HotelCity = hotel.City;
            }
            model.TotalCost = model.Nights * model.PricePerNight;
            model.Status = "Booked";

            TempData["BookingPreview"] = JsonSerializer.Serialize(model);
            return RedirectToAction("Confirm");
        }

        [HttpGet]
        public IActionResult Confirm()
        {
            if (!GetCurrentUserId().HasValue)
                return RedirectToAction("Login", "Account");

            string json = TempData["BookingPreview"] as string;
            if (json == null || json.Length == 0)
                return RedirectToAction("Index", "Hotel");

            TempData.Keep("BookingPreview");

            BookingDTO model = JsonSerializer.Deserialize<BookingDTO>(json);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(BookingDTO model)
        {
            if (!GetCurrentUserId().HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            model.UserId = GetCurrentUserId().Value;

            BookingDTO prepared = bookingService.PrepareBooking(model);
            bookingService.Create(prepared);

            TempData["Success"] = "Payment successful! Your hotel is Booked";
            return RedirectToAction("History");
        }

        [HttpGet]
        public IActionResult History()
        {
            if (!GetCurrentUserId().HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            List<BookingDTO> bookings = bookingService.GetHistory(GetCurrentUserId().Value);
            return View(bookings);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!GetCurrentUserId().HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            BookingDTO booking = bookingService.Get(id);
            if (booking == null || booking.UserId != GetCurrentUserId().Value)
            {
                return NotFound();
            }

            return View(booking);
        }

        // Cancel a booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int bookingId)
        {
            if (!GetCurrentUserId().HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            bool success = bookingService.CancelByUser(bookingId, GetCurrentUserId().Value);
            if (success)
            {
                TempData["Success"] = "Booking is cancelled!";
            }
            else
            {
                TempData["Error"] = "Cannot cancel this booking";
            }

            return RedirectToAction("History");
        }
    }
}
