using PatientChecking.ServiceModels;
using PatientChecking.Views.ViewModels;
using System.Threading.Tasks;

namespace PatientChecking.Services.Patient
{
    public interface IPatientService
    {
        Task<int> GetPatientsSummary();
        PatientList GetListPatientPaging(PagingRequest request);
        PatientDetails GetPatientInDetail(int patientId);
    }
}
