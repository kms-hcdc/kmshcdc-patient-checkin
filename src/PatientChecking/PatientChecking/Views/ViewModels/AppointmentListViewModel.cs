using PatientChecking.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class AppointmentListViewModel : IPagination
    {
        public int PageIndex { get; set ; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int SortSelection { get; set; }
        public List<AppointmentViewModel> AppointmentViewModels { get; set; }
    }
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string CheckInDate { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public string PatientIdentifier { get; set; }
        public string DoB { get; set; }
        public string AvatarLink { get; set; }
    }
}
