using Microsoft.EntityFrameworkCore;
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

        public async Task<AppointmentDashboard> GetAppointmentSummary()
        {
            var numberOfAppointments = await _patientCheckInContext.Appointments.ToListAsync();
            var numberOfAppointmentsInMonth = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Date.Year == DateTime.Now.Year && x.CheckInDate.Date.Month == DateTime.Now.Month ).ToListAsync();
            var numberOfAppointmentsInToday = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Date == DateTime.Today && x.Status == AppointmentStatus.CheckIn.ToString()).ToListAsync();
            return new AppointmentDashboard()
            {
                NumOfAppointments = numberOfAppointments.Count(),
                NumOfAppointmentsInMonth = numberOfAppointmentsInMonth.Count(),
                NumOfAppointmentsInToday = numberOfAppointmentsInToday.Count(),
            };
        }

        public async Task<PagedResult<AppointmentListViewModel>> GetListAppoinmentsPaging(PagingRequest request)
        {
            var query = from appointment in _patientCheckInContext.Appointments
                        join patient in _patientCheckInContext.Patients on appointment.PatientId equals patient.PatientId
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
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new AppointmentListViewModel()
                {
                    AppointmentId = x.appointment.AppointmentId,
                    CheckInDate = x.appointment.CheckInDate.ToString("dd-MM-yyyy"),
                    Status = x.appointment.Status,
                    DoB = x.patient.DoB.ToString("dd-MM-yyyy"),
                    FullName = x.patient.FullName,
                    PatientIdentifier = x.patient.PatientIdentifier
                }).ToListAsync();

            var pagedResult = new PagedResult<AppointmentListViewModel>()
            {
                Items = data,
                TotalCount = totalRow,
                pageIndex = request.PageIndex,
                pageSize = request.PageSize
            };

            return pagedResult;
        }
    }
}
