namespace CarRenting_System.Controllers
{

    using CarRenting_System.Data;
    using CarRenting_System.Data.Infrastucture;
    using CarRenting_System.Data.Models;
    using CarRenting_System.Models.Dealers;
    using CarRenting_System.Services.Dealers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class DealersController : Controller
    {
        private readonly CarRentingDbContext data;
        private readonly IDealerService dealerService;

        public DealersController(CarRentingDbContext data, IDealerService dealerService)
        {
            this.data = data;
            this.dealerService = dealerService;
        }

        [Authorize]
        public IActionResult Become()
        {
            
            if (this.dealerService.UserIsDealer(this.User.GetId()))
            {
                //TODO: return Error!
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            var userId = this.User.GetId();
            if (this.dealerService.UserIsDealer(userId))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dealer);
            }

            var dealerData = new Dealer
            {
                FullName = dealer.FullName,
                PhoneNumber = dealer.PhoneNumber,
                UserId = userId
            };

            this.data.Dealers.Add(dealerData);
            this.data.SaveChanges();

            //TODO: change magic string's!!!!!!!!!
            return RedirectToAction("All", "Cars");
        }
    }
}
