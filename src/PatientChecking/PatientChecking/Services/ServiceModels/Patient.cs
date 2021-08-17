using PatientChecking.Services.ServiceModels.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PatientChecking.Services.ServiceModels
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string PatientIdentifier { get; set; } 
        public string FullName { get; set; }
        public DateTime DoB { get; set; }
        public PatientGender Gender { get; set; }
        public string AvatarLink { get; set; }
        [NotMapped]
        public Address PrimaryAddress { get; set; }
        [NotMapped]
        public Contact PrimaryContact { get; set; }
    }
}
