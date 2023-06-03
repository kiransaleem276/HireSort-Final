using HireSort.Models;
using HireSort.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace HireSort.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ILogger<ContactUsController> _logger;
        private IDashboard _dashboard;

        public ContactUsController(ILogger<ContactUsController> logger, IDashboard dashboard)
        {
            _logger = logger;
            _dashboard = dashboard;
        }

        public IActionResult ContactUs()
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