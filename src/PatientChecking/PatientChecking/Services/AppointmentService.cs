using Microsoft.EntityFrameworkCore;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly PatientCheckInContext patientCheckInContext;

        public AppointmentService(PatientCheckInContext patientCheckInContext)
        {
            this.patientCheckInContext = patientCheckInContext;
        }

        public async Task<int> GetNumberOfAppointments()
        {
            var result = await patientCheckInContext.Appointments.ToListAsync();
            return result.Count;
        }

        public async Task<int> GetNumberOfAppointmentsInCurrentMonth()
        {
            var result = await patientCheckInContext.Appointments.Where(x => x.CheckInDate.Month == DateTime.Now.Month).ToListAsync();
            return result.Count;
        }

        public async Task<int> GetNumberOfAppointmentsInToday()
        {
            var result = await patientCheckInContext.Appointments.Where(x => x.CheckInDate.Date.Day == DateTime.Now.Day).ToListAsync();
            return result.Count;
        }
    }
}
