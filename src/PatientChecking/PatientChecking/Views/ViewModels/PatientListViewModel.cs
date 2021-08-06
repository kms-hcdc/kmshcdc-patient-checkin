using PatientChecking.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class PatientListViewModel
    {
        public string PatientIdentifier { get; set; }
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public string AvatarLink { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
