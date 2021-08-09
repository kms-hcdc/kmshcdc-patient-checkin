using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IAppointmentService
    {
        Task<AppointmentDashboard> GetAppointmentSummary();
        Task<PagedResult<AppointmentListViewModel>> GetListAppoinmentsPaging(PagingRequest request);
    }
}
