using MediatR;
using PatientChecking.Services.Appointment;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Appointment.Queries
{
    public class GetAppointmentPagingQuery : IRequest<AppointmentListViewModel>
    {
        public int option { get; set; }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }        
    }

    public class GetAppointmentPagingHandler : IRequestHandler<GetAppointmentPagingQuery, AppointmentListViewModel>
    {
        private readonly IAppointmentService _appointmentService;

        public GetAppointmentPagingHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<AppointmentListViewModel> Handle(GetAppointmentPagingQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _appointmentService.GetListAppoinmentsPaging(new PagingRequest { PageIndex = request.pageIndex, PageSize = request.pageSize, SortSelection = request.option });
            var appointmentViewModels = new List<AppointmentViewModel>();

            foreach (ServiceModels.Appointment appointment in pagedResult.Appointments)
            {
                appointmentViewModels.Add(new AppointmentViewModel
                {
                    AppointmentId = appointment.AppointmentId,
                    CheckInDate = appointment.CheckInDate.ToString("dd-MM-yyyy"),
                    DoB = appointment.Patient?.DoB.ToString("dd-MM-yyyy"),
                    FullName = appointment.Patient?.FullName,
                    PatientIdentifier = appointment.Patient?.PatientIdentifier,
                    Status = appointment.Status,
                    AvatarLink = appointment.Patient?.AvatarLink,
                });
            }

            var myModel = new AppointmentListViewModel
            {
                AppointmentViewModels = appointmentViewModels,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                SortSelection = request.option,
                TotalCount = pagedResult.TotalCount
            };

            return myModel;
        }
    }
}
