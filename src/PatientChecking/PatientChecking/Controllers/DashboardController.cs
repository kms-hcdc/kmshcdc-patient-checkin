using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Home()
        {
            return View();
        }
    }

}
