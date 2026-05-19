using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        RecommendationService recommendationService;

        public HomeController(RecommendationService recommendationService)
        {
            this.recommendationService = recommendationService;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var result = recommendationService.GetRecommendations(userId);

            ViewBag.RecommendationHeading = result.Heading;
            ViewBag.RecommendedHotels = result.Hotels;

            return View(result.Packages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
