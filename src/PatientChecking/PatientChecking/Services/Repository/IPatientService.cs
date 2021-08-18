using PatientChecking.Services.ServiceModels;
using PatientChecking.Views.ViewModels;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IPatientService
    {
        Task<int> GetPatientsSummary();
        PatientList GetListPatientPaging(PagingRequest request);
        PatientDetails GetPatientInDetail(int patientId);
    }
}
