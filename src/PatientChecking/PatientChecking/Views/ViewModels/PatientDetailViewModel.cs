using PatientChecking.Services.ServiceModels;
using PatientChecking.Services.ServiceModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class PatientDetailViewModel
    {
        public int PatientId { get; set; }
        public string PatientIdentifier { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DoB { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string EthnicRace { get; set; }
        public string HomeTown { get; set; }
        public string BirthplaceCity { get; set; }
        public string IdcardNo { get; set; }
        public string IssuedDate { get; set; }
        public string IssuedPlace { get; set; }
        public string InsuranceNo { get; set; }
        public string AvatarLink { get; set; }
        public PatientDetailsInitData InitData { get; set; }
    }
}
