using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SolutionFrontEnd.Models;

namespace SolutionFrontEnd.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("Home/Index");
            }
            else if (User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return LocalRedirect("/Home/Index");
            }

            return View();

        }
    }
}
