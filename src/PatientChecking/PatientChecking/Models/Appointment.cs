using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PatientChecking.Services.ServiceModels
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime CheckInDate { get; set; }
        public string MedicalConcerns { get; set; }
        public string Status { get; set; }
        public virtual Patient Patient { get; set; }
        //public virtual PatientDetails PatientDetails { get; set; }
    }
    public class AppointmentList
    {
        public List<Appointment> Appointments { get; set; }
        public int TotalCount { get; set; }
    }
}
