﻿using PatientChecking.Services.ServiceModels;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Repository
{
    public interface IAppointmentService
    {
        Task<AppointmentDashboard> GetAppointmentSummary();
        Task<AppointmentList> GetListAppoinmentsPaging(PagingRequest request);
        Task<Appointment> GetAppointmentById(int appointmentId);
        int UpdateAppointment(Appointment appointment);
    }
}
