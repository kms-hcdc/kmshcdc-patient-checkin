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

        public async Task<IActionResult> Index(int Option = (int)AppointmentSortSelection.ID, int PageSize = 10, int PageIndex = 1)
        {
            PagingRequest request = new PagingRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                SortSelection = Option
            };
            AppointmentList pagedResult = await _appointmentService.GetListAppoinmentsPaging(request);
            List<AppointmentViewModel> appointmentViewModels = new List<AppointmentViewModel>();
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
            AppointmentListViewModel mymodel = new AppointmentListViewModel()
            {
                AppointmentViewModels = appointmentViewModels,
                PageIndex = PageIndex,
                PageSize = PageSize,
                SortSelection = Option,
                TotalCount = pagedResult.TotalCount
            };
            return View(mymodel);
        }
    }
}
