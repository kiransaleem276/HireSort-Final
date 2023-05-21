using HireSort.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HireSort.Areas.Admin.Controllers
{
    public class ViewJobDetailController : Controller
    {
        private readonly ILogger<ViewJobDetailController> _logger;

        public ViewJobDetailController(ILogger<ViewJobDetailController> logger)
        {
            _logger = logger;
        }

        public IActionResult ViewJobDetail()
        {
            if (HttpContext.Session.GetInt32("_ClientID") != 1)
            {
                Response.Redirect("/Admin");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}