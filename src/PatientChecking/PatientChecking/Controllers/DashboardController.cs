using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PatientChecking.Services.Repository;
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
            int numOfPatients = await _patientService.GetNumberOfPatients();
            int numOfPatientsInMonth = await _patientService.GetNumberOfPatientsInCurrentMonth();
            int numOfAppointment = await _appointmentService.GetNumberOfAppointments();
            int numOfAppointmentInMonth = await _appointmentService.GetNumberOfAppointmentsInCurrentMonth();
            int numOfAppointmentInToday = await _appointmentService.GetNumberOfAppointmentsInToday();
            DashboardViewModel dashboard = new()
            {
                NumOfPatients = numOfPatients,
                NumOfPatientsInMonth = numOfPatientsInMonth,
                NumOfAppointments = numOfAppointment,
                NumOfAppointmentsInMonth = numOfAppointmentInMonth,
                NumOfAppointmentsInToday = numOfAppointmentInToday,
            };
            return new JsonResult(dashboard);
        }
    }

}
