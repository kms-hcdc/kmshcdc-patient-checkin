using Microsoft.AspNetCore.Mvc;
using PatientChecking.ServiceModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
