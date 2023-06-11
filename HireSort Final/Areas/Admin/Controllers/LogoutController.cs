using HireSort.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HireSort.Areas.Admin.Controllers
{
    public class LogoutController : Controller
    {
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(ILogger<LogoutController> logger)
        {
            _logger = logger;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}