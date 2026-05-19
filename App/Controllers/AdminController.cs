using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace App.Controllers
{
    public class AdminController : Controller
    {
        HotelService hotelService;
        PackageService packageService;
        BookingService bookingService;

        public AdminController(HotelService hotelService, PackageService packageService, BookingService bookingService)
        {
            this.hotelService = hotelService;
            this.packageService = packageService;
            this.bookingService = bookingService;
        }

        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        public IActionResult Index()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            List<HotPackageDTO> packages = packageService.GetAll();
            return View(packages);
        }

        [HttpGet]
        public IActionResult Hotels(HotelSearchFilter filter)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

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
        public IActionResult HotelDetails(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            HotelDTO hotel = hotelService.Get(id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        [HttpGet]
        public IActionResult CreateHotel()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            return View("HotelForm", new HotelDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHotel(HotelDTO model)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (model.Name == null || model.Name.Length == 0)
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (!ModelState.IsValid)
            {
                return View("HotelForm", model);
            }

            hotelService.Create(model);
            TempData["Success"] = "Hotel is added successfully!";
            return RedirectToAction("Hotels");
        }

        [HttpGet]
        public IActionResult EditHotel(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            HotelDTO hotel = hotelService.Get(id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View("HotelForm", hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHotel(int id, HotelDTO model)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            model.HotelId = id;
            bool updated = hotelService.Update(model);
            if (!updated)
            {
                TempData["Error"] = "Could not update the hotel";
                return View("HotelForm", model);
            }

            TempData["Success"] = "Hotel updated successfully.";
            return RedirectToAction("Hotels");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteHotel(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            bool deleted = hotelService.Delete(id);
            if (!deleted)
            {
                TempData["Error"] = "Cannot delete hotel. It may have bookings";
            }
            else
            {
                TempData["Success"] = "Hotel is deleted!";
            }

            return RedirectToAction("Hotels");
        }

        [HttpGet]
        public IActionResult Packages()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            List<HotPackageDTO> list = packageService.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult PackageDetails(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            HotPackageDTO package = packageService.Get(id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        [HttpGet]
        public IActionResult CreatePackage()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Hotels = hotelService.Get();
            HotPackageDTO model = new HotPackageDTO();
            model.ValidUntil = DateTime.Today.AddMonths(1);
            return View("PackageForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePackage(HotPackageDTO model)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (model.Title == null || model.Title.Length == 0)
            {
                ModelState.AddModelError("Title", "Package title is required");
            }

            if (model.HotelId <= 0)
            {
                ModelState.AddModelError("HotelId", "Please select a hotel");
            }

            ModelState.Remove("HotelName");
            ModelState.Remove("HotelCity");

            if (!ModelState.IsValid)
            {
                ViewBag.Hotels = hotelService.Get();
                return View("PackageForm", model);
            }

            bool created = packageService.Create(model);
            if (!created)
            {
                TempData["Error"] = "Could not save the hot deal. Please try again!";
                ViewBag.Hotels = hotelService.Get();
                return View("PackageForm", model);
            }

            TempData["Success"] = "Hot deal added successfully!";
            return RedirectToAction("Packages");
        }

        [HttpGet]
        public IActionResult EditPackage(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            HotPackageDTO package = packageService.Get(id);
            if (package == null)
            {
                return NotFound();
            }

            ViewBag.Hotels = hotelService.Get();
            return View("PackageForm", package);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPackage(int id, HotPackageDTO model)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            model.PackageId = id;
            bool updated = packageService.Update(model);
            if (!updated)
            {
                TempData["Error"] = "Could not update the package";
                ViewBag.Hotels = hotelService.Get();
                return View("PackageForm", model);
            }

            TempData["Success"] = "Hot deal is updated";
            return RedirectToAction("Packages");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePackage(int id)
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            bool deleted = packageService.Delete(id);
            if (!deleted)
            {
                TempData["Error"] = "Could not delete the package";
            }
            else
            {
                TempData["Success"] = "Hot deal deleted";
            }

            return RedirectToAction("Packages");
        }

        [HttpGet]
        public IActionResult AllBookings()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            List<BookingDTO> bookings = bookingService.GetAll();
            return View(bookings);
        }
    }
}
