using System;
using System.Collections.Generic;

#nullable disable

namespace PatientCheckIn.DataAccess.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
            Contacts = new HashSet<Contact>();
        }

        public int PatientId { get; set; }
        public string PatientIdentifier { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public int Gender { get; set; }
        public bool MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string EthnicRace { get; set; }
        public string HomeTown { get; set; }
        public string BirthplaceCity { get; set; }
        public string IdcardNo { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string IssuedPlace { get; set; }
        public string InsuranceNo { get; set; }
        public string AvatarLink { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
