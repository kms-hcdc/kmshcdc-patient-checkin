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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public int Gender { get; set; }
        
        public Address Address { get; set; }
        public string AvatarLink { get; set; }
        public Contact Contact { get; set; }
    }
}
