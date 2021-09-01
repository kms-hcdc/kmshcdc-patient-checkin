using PatientCheckIn.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.AppConfiguration
{
    public interface IAppConfigurationService
    {
        Task<List<ProvinceCity>> GetProvinceCitiesAsync();
    }
}
