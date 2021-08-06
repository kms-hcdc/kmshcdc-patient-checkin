using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IAppointmentService
    {
        Task<int> GetNumberOfAppointments();
        Task<int> GetNumberOfAppointmentsInCurrentMonth();
        Task<int> GetNumberOfAppointmentsInToday();
    }
}
