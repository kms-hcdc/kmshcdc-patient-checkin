using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class AppointmentController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
