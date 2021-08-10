using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PatientChecking.Services.ServiceModels
{
    public partial class Patient
    {
        public Patient()
        {
            
        }
        public int PatientId { get; set; }
        public string PatientIdentifier { get; set; } 
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public int Gender { get; set; }
        public string AvatarLink { get; set; }
        [NotMapped]
        public Address PrimaryAddress { get; set; }
        [NotMapped]
        public Contact PrimaryContact { get; set; }
    }

    public class PatientDetails : Patient
    {
        public PatientDetails() : base()
        {
            Appointments = new HashSet<Appointment>();
            Contacts = new HashSet<Contact>();
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string EthnicRace { get; set; }
        public string HomeTown { get; set; }
        public string BirthplaceCity { get; set; }
        public string IdcardNo { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string IssuedPlace { get; set; }
        public string InsuranceNo { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }

    public class PatientList
    {
        public List<Patient> Patients { get; set; }
        public int TotalCount { get; set; }
    }
}
