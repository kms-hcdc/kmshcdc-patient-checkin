using Microsoft.AspNetCore.Mvc;
using PatientChecking.Services.Repository;
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

        public async Task<IActionResult> Index()
        {
            return await Index((int)AppointmentSortSelection.ID, 10, 1).ConfigureAwait(false);
        }
       
        [Route("[Controller]/Index/{option}")]
        public async Task<IActionResult> Index(int option)
        {
            return await Index(option, 10, 1).ConfigureAwait(false);
        }

        [Route("[Controller]/Index/{option}-{pageSize}")]
        public async Task<IActionResult> Index(int option, int pageSize)
        {
            return await Index(option, pageSize, 1).ConfigureAwait(false);
        }

        [Route("[Controller]/Index/{option}-{pageSize}/{pageIndex}")]
        public async Task<IActionResult> Index(int option, int pageSize, int pageIndex)
        {
            var request = new PagingRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortSelection = option
            };

            var pagedResult = await _appointmentService.GetListAppoinmentsPaging(request);

            var appointmentViewModels = new List<AppointmentViewModel>();

            foreach(Appointment appointment in pagedResult.Appointments)
            {
                appointmentViewModels.Add(new AppointmentViewModel
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
            var myModel = new AppointmentListViewModel
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
