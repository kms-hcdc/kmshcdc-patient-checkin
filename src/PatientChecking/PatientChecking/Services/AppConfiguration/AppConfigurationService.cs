using Microsoft.EntityFrameworkCore;
using PatientCheckIn.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.AppConfiguration
{
    public class AppConfigurationService : IAppConfigurationService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public AppConfigurationService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }
        public async Task<List<string>> GetProvinceCitiesAsync()
        {
            var result = await _patientCheckInContext.ProvinceCities.Select(x=>x.ProvinceCityName).ToListAsync();
            return result;
        }
    }
}
