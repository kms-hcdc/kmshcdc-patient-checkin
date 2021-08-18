using PatientCheckIn.DataAccess.Models;
using PatientChecking.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class ProvinceCityService : IProvinceCityService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public ProvinceCityService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }
        public List<ProvinceCity> GetProvinceCities()
        {
            var result = _patientCheckInContext.ProvinceCities.ToList();
            return result;
        }
    }
}
