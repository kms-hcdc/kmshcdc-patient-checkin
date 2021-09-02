using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.ServiceModels
{
    public class AppointmentDashboard
    {
        public int NumOfAppointments { get; set; }
        public int NumOfAppointmentsInMonth { get; set; }
        public int NumOfAppointmentsInToday { get; set; }
        public int NumOfPatientsInMonth { get; set; }
    }
}
