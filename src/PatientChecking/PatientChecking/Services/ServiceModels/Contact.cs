using System;
using System.Collections.Generic;

#nullable disable

namespace PatientChecking.Services.ServiceModels
{
    public partial class Contact
    {
        public Contact()
        {
            Addresses = new HashSet<Address>();
            EmergencyContacts = new HashSet<EmergencyContact>();
        }

        public int ContactId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }
    }
}
