using PatientCheckIn.DataAccess.Models;
using PatientChecking.ServiceModels;
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
        public List<PatientChecking.ServiceModels.ProvinceCity> GetProvinceCities()
        {
            var query = _patientCheckInContext.ProvinceCities.ToList();
            var result = new List<PatientChecking.ServiceModels.ProvinceCity>();

            foreach (PatientCheckIn.DataAccess.Models.ProvinceCity pc in query)
            {
                result.Add(new ServiceModels.ProvinceCity
                {
                    ProvinceCityId = pc.ProvinceCityId,
                    ProvinceCityName = pc.ProvinceCityName
                });
            }

            return result;
        }
    }
}
