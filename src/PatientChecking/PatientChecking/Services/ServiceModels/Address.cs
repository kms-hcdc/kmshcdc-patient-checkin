using System;
using System.Collections.Generic;

#nullable disable

namespace PatientChecking.Services.ServiceModels
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int TypeAddress { get; set; }
        public string Address1 { get; set; }
        public bool IsPrimary { get; set; }
        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
