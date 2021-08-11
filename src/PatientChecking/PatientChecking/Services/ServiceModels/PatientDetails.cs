using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.ServiceModels
{
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
}
