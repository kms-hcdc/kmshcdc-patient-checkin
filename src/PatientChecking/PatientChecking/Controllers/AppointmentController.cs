using Microsoft.AspNetCore.Mvc;
using PatientChecking.Services.Abstractions;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Services.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index(int option = (int)AppointmentSortSelection.ID, int pageSize = 10, int pageIndex = 1)
        {
            var request = new PagingRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortSelection = option
            };

            var pagedResult = await _appointmentService.GetListAppoinmentsPaging(request);

            var appointmentViewModels = new List<AppointmentViewModel>();

            foreach(Appointment appointment in pagedResult.Appointments)
            {
                appointmentViewModels.Add(new AppointmentViewModel()
                {
                    AppointmentId = appointment.AppointmentId,
                    CheckInDate = appointment.CheckInDate.ToString("dd-MM-yyyy"),
                    DoB = appointment.Patient?.DoB.ToString("dd-MM-yyyy"),
                    FullName = appointment.Patient?.FullName,
                    PatientIdentifier = appointment.Patient?.PatientIdentifier,
                    Status = appointment.Status,
                    AvatarLink = appointment.Patient?.AvatarLink
                });
            }
            var myModel = new AppointmentListViewModel()
            {
                AppointmentViewModels = appointmentViewModels,
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortSelection = option,
                TotalCount = pagedResult.TotalCount
            };
            return View(myModel);
        }
    }
}
