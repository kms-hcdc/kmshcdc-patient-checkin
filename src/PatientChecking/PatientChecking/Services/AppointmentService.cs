using Microsoft.EntityFrameworkCore;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Services.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public AppointmentService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }

        public async Task<AppointmentDashboard> GetAppointmentSummary()
        {
            var numberOfAppointments = await _patientCheckInContext.Appointments.ToListAsync();
            var numberOfAppointmentsInMonth = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Date.Year == DateTime.Now.Year && x.CheckInDate.Date.Month == DateTime.Now.Month ).ToListAsync();
            var numberOfAppointmentsInToday = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Date == DateTime.Today && x.Status == AppointmentStatus.CheckIn.ToString()).ToListAsync();
            return new AppointmentDashboard()
            {
                NumOfAppointments = numberOfAppointments.Count(),
                NumOfAppointmentsInMonth = numberOfAppointmentsInMonth.Count(),
                NumOfAppointmentsInToday = numberOfAppointmentsInToday.Count(),
            };
        }
    }
}
