﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.ServiceModels
{
    public class PatientList
    {
        public List<Patient> Patients { get; set; }
        public int TotalCount { get; set; }
    }
}
