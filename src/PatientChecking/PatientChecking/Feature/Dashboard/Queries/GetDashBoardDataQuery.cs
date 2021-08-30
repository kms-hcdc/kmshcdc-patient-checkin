using MediatR;
using PatientChecking.Services.Appointment;
using PatientChecking.Services.Patient;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Dashboard.Queries
{
    public class GetDashBoardDataQuery : IRequest<DashboardViewModel>
    {
        public GetDashBoardDataQuery()
        {
        }
    }

    public class GetDashBoardDataHandler : IRequestHandler<GetDashBoardDataQuery, DashboardViewModel>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;

        public GetDashBoardDataHandler(IAppointmentService appointmentService, IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
        }

        public async Task<DashboardViewModel> Handle(GetDashBoardDataQuery request, CancellationToken cancellationToken)
        {
            var appointmentSummary = await _appointmentService.GetAppointmentSummary();
            return new DashboardViewModel
            {
                NumOfAppointments = appointmentSummary.NumOfAppointments,
                NumOfAppointmentsInMonth = appointmentSummary.NumOfAppointmentsInMonth,
                NumOfAppointmentsInToday = appointmentSummary.NumOfAppointmentsInToday,
                NumOfPatients = await _patientService.GetPatientsSummary(),
                NumOfPatientsInMonth = appointmentSummary.NumOfPatientsInMonth
            };
        }
    }
}
