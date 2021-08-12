using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PatientChecking.Services.Abstractions;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public DashboardController(IPatientService patientService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpGet("[controller]/getDashBoard")]
        public async Task<IActionResult> GetDashBoardData()
        {
            AppointmentDashboard appointmentDashboard = await _appointmentService.GetAppointmentSummary();
            int patientDashboard = await _patientService.GetPatientsSummary();
            return  new JsonResult(new {appointment = appointmentDashboard , patient = patientDashboard });
        }
    }

}
