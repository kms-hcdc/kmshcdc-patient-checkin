using PatientChecking.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class AppointmentListViewModel
    {
        public int AppointmentId { get; set; }
        public string CheckInDate { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public string PatientIdentifier { get; set; }
        public string DoB { get; set; }

    }
}
