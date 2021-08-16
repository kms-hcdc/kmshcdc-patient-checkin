using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PatientChecking.Services.ServiceModels
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int TypeAddress { get; set; }
        [Column("Address")]
        public string StreetLine { get; set; }
        public bool IsPrimary { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
