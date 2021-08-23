using PatientChecking.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.AppConfiguration
{
    public interface IAppConfigurationService
    {
        List<ProvinceCity> GetProvinceCities();
    }
}
