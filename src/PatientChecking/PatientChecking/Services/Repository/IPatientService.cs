
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IPatientService
    {

        Task<PagedResult<PatientListViewModel>> GetListPatientPaging(PagingRequest request);

        Task<PatientDashboard> GetPatientsSummary();

    }
}
