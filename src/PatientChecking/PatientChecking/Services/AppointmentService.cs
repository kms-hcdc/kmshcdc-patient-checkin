using Microsoft.EntityFrameworkCore;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Services.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public AppointmentService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }

        public async Task<ServiceModels.Appointment> GetAppointmentById(int appointmentId)
        {
            var appointment = await _patientCheckInContext.Appointments.SingleOrDefaultAsync(x => x.AppointmentId == appointmentId);
            return new ServiceModels.Appointment
            { 
                AppointmentId = appointment.AppointmentId,
                CheckInDate = appointment.CheckInDate,
                MedicalConcerns = appointment?.MedicalConcerns,
                Status  = appointment.Status,
                PatientId = appointment.PatientId
            };
        }

        public async Task<AppointmentDashboard> GetAppointmentSummary()
        {
            var numberOfAppointments = await _patientCheckInContext.Appointments.ToListAsync();
            var numberOfAppointmentsInMonth = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Date.Year == DateTime.Now.Year && x.CheckInDate.Date.Month == DateTime.Now.Month ).ToListAsync();
            var numberOfAppointmentsInToday = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Date == DateTime.Today && x.Status == AppointmentStatus.CheckIn.ToString()).ToListAsync();
            var numberOfPatientsInMonth = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Year == DateTime.Now.Year && x.CheckInDate.Month == DateTime.Now.Month)
                                                                 .GroupBy(p => p.PatientId)
                                                                 .Select(g => new { g.Key, count = g.Count() }).ToListAsync();
            return new AppointmentDashboard
            {
                NumOfAppointments = numberOfAppointments.Count(),
                NumOfAppointmentsInMonth = numberOfAppointmentsInMonth.Count(),
                NumOfAppointmentsInToday = numberOfAppointmentsInToday.Count(),
                NumOfPatientsInMonth = numberOfPatientsInMonth.Count()
            };
        }

        public async Task<AppointmentList> GetListAppoinmentsPaging(PagingRequest request)
        {
            var query = from appointment in _patientCheckInContext.Appointments
                        join patient in _patientCheckInContext.Patients on appointment.Patient.PatientId equals patient.PatientId
                        select new { appointment, patient };
             if(request.SortSelection == 0)
            {
                query = query.OrderBy(i => i.patient.FullName);
            }
            else if(request.SortSelection == 1)
            {
                query = query.OrderBy(i => i.patient.PatientIdentifier); 
            }
            else if(request.SortSelection == 2)
            {
                query = query.OrderBy(i => i.patient.DoB);
            }
            else if (request.SortSelection == 3)
            {
                query = query.OrderByDescending(i => i.appointment.CheckInDate);
            }
            else
            {
                query = query.OrderBy(i => i.appointment.Status == AppointmentStatus.CheckIn.ToString() ? 1 
                                         : i.appointment.Status == AppointmentStatus.Closed.ToString() ? 2
                                         : 3
                );
            }
            int totalRow = query.Count();
            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new ServiceModels.Appointment
                {
                    AppointmentId = x.appointment.AppointmentId,
                    CheckInDate = x.appointment.CheckInDate,
                    Status = x.appointment.Status,
                    Patient = new ServiceModels.Patient
                    {
                        AvatarLink = x.patient.AvatarLink,
                        DoB = x.patient.DoB,
                        FullName = x.patient.FullName,
                        PatientIdentifier = x.patient.PatientIdentifier
                    }
                }).ToList();
            AppointmentList appointmentList = new AppointmentList
            {
                TotalCount = totalRow,
                Appointments = data
            };
            return appointmentList;
        }

        public int UpdateAppointment(ServiceModels.Appointment appointment)
        {
            var appointmentDataAccess = _patientCheckInContext.Appointments.FirstOrDefault(x => x.AppointmentId == appointment.AppointmentId);
            appointmentDataAccess.CheckInDate = appointment.CheckInDate;
            appointmentDataAccess.MedicalConcerns = appointment.MedicalConcerns;
            appointmentDataAccess.Status = appointment.Status;
            _patientCheckInContext.Update(appointmentDataAccess);
            return _patientCheckInContext.SaveChanges();
        }
    }
}
