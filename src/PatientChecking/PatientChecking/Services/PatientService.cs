using Microsoft.EntityFrameworkCore;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services
{
    public class PatientService : IPatientService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public PatientService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }

        public async Task<PagedResult<PatientListViewModel>> GetListPatientPaging(PagingRequest request)
        {
            var query = from patient in _patientCheckInContext.Patients
                        join contact in _patientCheckInContext.Contacts on patient.PatientId equals contact.PatientId
                        join address in _patientCheckInContext.Addresses on contact.ContactId equals address.ContactId
                        where address.IsPrimary == true
                        select new { patient, contact, address };

            if(request.SortSelection == 0)
            {
                query = query.OrderBy(i => i.patient.PatientIdentifier);
            }
            else if(request.SortSelection == 1)
            {
                query = query.OrderBy(i => i.patient.FullName);
            }
            else
            {
                query = query.OrderBy(i => i.patient.DoB);
            }

            int totalRow = query.Count();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new PatientListViewModel()
                {
                    PatientIdentifier = x.patient.PatientIdentifier,
                    FullName = x.patient.FullName,
                    DoB = x.patient.DoB.ToString("dd-MM-yyyy"),
                    Gender = x.patient.Gender == 0 ? "Male" : x.patient.Gender == 1 ? "Female" : "Others",
                    Address = x.address != null ? x.address.Address1 : "",
                    PhoneNumber = x.contact != null ? x.contact.PhoneNumber : "",
                    Email = x.contact != null ? x.contact.Email : "",
                    AvatarLink = x.patient.AvatarLink != null ? x.patient.AvatarLink : ""
                }).ToListAsync();

            var pagedResult = new PagedResult<PatientListViewModel>()
            {
                Items = data,
                TotalCount = totalRow,
                pageIndex = request.PageIndex,
                pageSize = request.PageSize
            };

            return pagedResult;
        }

        public async Task<PatientDashboard> GetPatientsSummary()
        {
            var numberOfPatients = await _patientCheckInContext.Patients.ToListAsync();
            var numberOfPatientsInMonth = await _patientCheckInContext.Appointments.Where(x => x.CheckInDate.Year == DateTime.Now.Year && x.CheckInDate.Month == DateTime.Now.Month)
                                                                 .GroupBy(p => p.PatientId)
                                                                 .Select(g => new { g.Key, count = g.Count() }).ToListAsync();
            return new PatientDashboard()
            {
                NumOfPatients = numberOfPatients.Count(),
                NumOfPatientsInMonth = numberOfPatientsInMonth.Count()
            };
        }
    }
}
