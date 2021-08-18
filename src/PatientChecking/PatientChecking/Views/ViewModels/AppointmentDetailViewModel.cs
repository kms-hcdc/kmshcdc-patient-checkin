using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class AppointmentDetailViewModel
    {
        public int AppointmentId { get; set; }
        public string CheckInDate { get; set; }
        public string MedicalConcerns { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
    }
}
