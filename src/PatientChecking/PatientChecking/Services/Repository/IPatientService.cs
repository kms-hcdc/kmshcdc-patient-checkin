using PatientChecking.Services.ServiceModels;
using PatientChecking.Views.ViewModels;
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
        Task<PagedResult<PatientListViewModel>> GetListPatientPaging(PagingRequest request);
    }
}
