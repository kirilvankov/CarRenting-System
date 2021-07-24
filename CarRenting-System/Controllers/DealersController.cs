namespace CarRenting_System.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CarRenting_System.Models.Dealers;

    using Microsoft.AspNetCore.Mvc;

    public class DealersController : Controller
    {
        public IActionResult Become() => View();

        [HttpPost]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            if (!ModelState.IsValid)
            {
                return View(dealer);
            }
            return null;
        }
    }
}
