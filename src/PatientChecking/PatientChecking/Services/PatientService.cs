using Microsoft.EntityFrameworkCore;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class PatientService : IPatientService
    {
        private readonly PatientCheckInContext patientCheckInContext;

        public PatientService(PatientCheckInContext patientCheckInContext)
        {
            this.patientCheckInContext = patientCheckInContext;
        }

        public async Task<int> GetNumberOfPatients()
        {
            var result = await patientCheckInContext.Patients.ToListAsync();
            return result.Count;
        }

        public async Task<int> GetNumberOfPatientsInCurrentMonth()
        {
            var result = await patientCheckInContext.Appointments.Where(x => x.CheckInDate.Month == DateTime.Now.Month)
                                                                 .GroupBy(p => p.PatientId)
                                                                 .Select(g => new { g.Key, count = g.Count()}).ToListAsync();
            return result.Count;
        }
    }
}
