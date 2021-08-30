using PatientChecking.ServiceModels;
using PatientChecking.Views.ViewModels;
using System.Threading.Tasks;

namespace PatientChecking.Services.Patient
{
    public interface IPatientService
    {
        Task<int> GetPatientsSummary();
        Task<PatientList> GetListPatientPaging(PagingRequest request);
        Task<PatientCheckIn.DataAccess.Models.Patient> GetPatientInDetail(int patientId);
        Task<int> UpdatePatientDetail(PatientCheckIn.DataAccess.Models.Patient patientDetails);
        Task<int> UploadPatientImage(int patientId, string avatarLink);
    }
}
