using PatientChecking.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class PatientListViewModel : IPagination
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int SortSelection { get; set; }
        public List<PatientViewModel> Patients { get; set; }
    }

    public class PatientViewModel
    {
        public string PatientIdentifier { get; set; }
        public string FullName { get; set; }
        public string DoB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string AvatarLink { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
