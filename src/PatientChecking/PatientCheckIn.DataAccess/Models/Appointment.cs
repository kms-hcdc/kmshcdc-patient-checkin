using System;
using System.Collections.Generic;

#nullable disable

namespace PatientCheckIn.DataAccess.Models
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime CheckInDate { get; set; }
        public string MedicalConcerns { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
