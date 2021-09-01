using PatientChecking.ServiceModels;
using PatientChecking.Views.ViewModels;
using System.Threading.Tasks;

namespace PatientChecking.Services.Patient
{
    public interface IPatientService
    {
        Task<int> GetPatientsSummaryAsync();
        Task<PatientList> GetListPatientPagingAsync(PagingRequest request);
        Task<PatientCheckIn.DataAccess.Models.Patient> GetPatientInDetailAsync(int patientId);
        Task<int> UpdatePatientDetailAsync(PatientCheckIn.DataAccess.Models.Patient patientDetails);
        Task<int> UploadPatientImageAsync(int patientId, string avatarLink);
    }
}
