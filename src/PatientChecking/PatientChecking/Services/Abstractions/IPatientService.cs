using PatientChecking.Services.ServiceModels;
using PatientChecking.Views.ViewModels;
using System.Threading.Tasks;

namespace PatientChecking.Services.Abstractions
{
    public interface IPatientService
    {
        Task<int> GetPatientsSummary();
        PatientList GetListPatientPaging(PagingRequest request);
    }
}
