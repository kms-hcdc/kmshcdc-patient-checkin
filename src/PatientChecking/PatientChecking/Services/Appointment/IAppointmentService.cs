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
        Task<AppointmentDashboard> GetAppointmentSummary();
        Task<AppointmentList> GetListAppoinmentsPaging(PagingRequest request);
        Task<ServiceModels.Appointment> GetAppointmentById(int appointmentId);
    }
}
