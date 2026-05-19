using BLL.Services;
using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace App.Controllers
{
    public class PackageController : Controller
    {
        PackageService packageService;

        public PackageController(PackageService packageService)
        {
            this.packageService = packageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<HotPackageDTO> packages = packageService.GetAll();
            return View(packages);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            HotPackageDTO package = packageService.Get(id);
            if (package == null)
            {
                return NotFound();
            }
            return View(package);
        }
    }
}
