using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IPatientService
    {
        Task<int> GetNumberOfPatients();
        Task<int> GetNumberOfPatientsInCurrentMonth();
    }
}
