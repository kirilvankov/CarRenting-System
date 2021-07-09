using System;
using System.Collections.Generic;
namespace CarRenting_System.Controllers
{
    using System.Diagnostics;

    using CarRenting_System.Models;

    using Microsoft.AspNetCore.Mvc;
 

    public class HomeController : Controller
    {

        public IActionResult Index() => View();
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
