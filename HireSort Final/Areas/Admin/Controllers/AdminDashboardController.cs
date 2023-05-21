using HireSort.Models;
using HireSort.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HireSort_Final.Areas.Admin.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ILogger<AdminDashboardController> _logger;
        private IDashboard _dashboard;
        const string SessionEmail = "_Email";
        const string ClientID = "_ClientID";
        public AdminDashboardController(ILogger<AdminDashboardController> logger, IDashboard dashboard)
        {
            _logger = logger;
            _dashboard = dashboard;
        }

        public IActionResult AdminDashboard()
        {
            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            if (HttpContext.Session.GetInt32(ClientID) != 1)
            {
                Response.Redirect("/Admin");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
