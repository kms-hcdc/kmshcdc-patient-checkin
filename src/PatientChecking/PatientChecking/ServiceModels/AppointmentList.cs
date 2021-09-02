using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.ServiceModels
{
    public class AppointmentList
    {
        public List<Appointment> Appointments { get; set; }
        public int TotalCount { get; set; }
    }
}
