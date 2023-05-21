﻿using HireSort.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HireSort.Areas.Admin.Controllers
{
    public class ViewDeptVacancyController : Controller
    {
        private readonly ILogger<ViewDeptVacancyController> _logger;

        public ViewDeptVacancyController(ILogger<ViewDeptVacancyController> logger)
        {
            _logger = logger;
        }

        public IActionResult ViewDeptVacancy()
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