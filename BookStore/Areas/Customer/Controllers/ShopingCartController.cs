﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShopingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
