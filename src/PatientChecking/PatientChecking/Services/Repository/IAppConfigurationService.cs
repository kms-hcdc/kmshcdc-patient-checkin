using PatientChecking.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IAppConfigurationService
    {
        List<ProvinceCity> GetProvinceCities();
    }
}
