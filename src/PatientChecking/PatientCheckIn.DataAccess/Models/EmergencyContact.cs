using System;
using System.Collections.Generic;

#nullable disable

namespace PatientCheckIn.DataAccess.Models
{
    public partial class EmergencyContact
    {
        public int EmergencyId { get; set; }
        public string Relationship { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
