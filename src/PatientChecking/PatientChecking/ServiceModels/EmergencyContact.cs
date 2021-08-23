using System;
using System.Collections.Generic;

#nullable disable

namespace PatientChecking.ServiceModels
{
    public class EmergencyContact
    {
        public int EmergencyId { get; set; }
        public string Relationship { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
