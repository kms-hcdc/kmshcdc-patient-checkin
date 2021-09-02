using PatientChecking.ServiceModels;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Appointment
{
    public interface IAppointmentService
    {
        Task<AppointmentDashboard> GetAppointmentSummaryAsync();
        Task<AppointmentList> GetListAppoinmentsPagingAsync(PagingRequest request);
        Task<int> UpdateAppointmentAsync(PatientCheckIn.DataAccess.Models.Appointment appointment);
        Task<PatientCheckIn.DataAccess.Models.Appointment> GetAppointmentByIdAsync(int appointmentId);
    }
}