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
    public class GetAppointmentByIdQuery : IRequest<AppointmentDetailViewModel>
    {
        public int Id { get; set; }
        class GetPatientByIdHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDetailViewModel>
        {
            private readonly IAppointmentService _appointmentService;

            public GetPatientByIdHandler(IAppointmentService appointmentService)
            {
                _appointmentService = appointmentService;
            }

            public async Task<AppointmentDetailViewModel> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
            {
                var appointment = await _appointmentService.GetAppointmentById(request.Id);
                return new AppointmentDetailViewModel
                {
                    AppointmentId = appointment.AppointmentId,
                    CheckInDate = appointment.CheckInDate.ToString("yyyy-MM-dd"),
                    MedicalConcerns = appointment.MedicalConcerns,
                    Status = appointment.Status,
                    PatientId = appointment.PatientId
                };
            }
        }
    }
}
