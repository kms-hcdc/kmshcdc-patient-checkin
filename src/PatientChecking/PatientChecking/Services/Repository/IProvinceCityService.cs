using PatientCheckIn.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IProvinceCityService
    {
        List<ProvinceCity> GetProvinceCities();
    }
}
