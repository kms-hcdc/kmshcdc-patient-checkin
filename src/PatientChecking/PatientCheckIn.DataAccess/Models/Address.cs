using System;
using System.Collections.Generic;

#nullable disable

namespace PatientCheckIn.DataAccess.Models
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int TypeAddress { get; set; }
        public string StreetLine { get; set; }
        public bool IsPrimary { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
