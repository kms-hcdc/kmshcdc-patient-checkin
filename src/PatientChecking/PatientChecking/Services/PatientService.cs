using Microsoft.EntityFrameworkCore;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class PatientService : IPatientService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public PatientService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }

        public async Task<PatientDashboard> GetPatientsSummary()
        {
            var numberOfPatients = await _patientCheckInContext.Patients.ToListAsync();
            var numberOfPatientsInMonth = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Year == DateTime.Now.Year && x.CheckInDate.Month == DateTime.Now.Month)
                                                                 .GroupBy(p => p.PatientId)
                                                                 .Select(g => new { g.Key, count = g.Count() }).ToListAsync();
            return new PatientDashboard()
            {
                NumOfPatients = numberOfPatients.Count(),
                NumOfPatientsInMonth = numberOfPatientsInMonth.Count()
            };
        }
    }
}
