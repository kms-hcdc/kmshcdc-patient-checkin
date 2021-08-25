using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class DashboardViewModel
    {
        public int NumOfAppointments { get; set; }
        public int NumOfAppointmentsInMonth { get; set; }
        public int NumOfAppointmentsInToday { get; set; }
        public int NumOfPatients { get; set; }
        public int NumOfPatientsInMonth { get; set; }

    }
}
