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
    public class PatientService : IPatientService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public PatientService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }

        public PatientList GetListPatientPaging(PagingRequest request)
        {
            var query = from patient in _patientCheckInContext.Patients
                        join contact in _patientCheckInContext.Contacts on patient.PatientId equals contact.PatientId
                        join address in _patientCheckInContext.Addresses on contact.ContactId equals address.ContactId
                        where address.IsPrimary == true
                        select new { patient, contact, address };

            if (request.SortSelection == (int)PatientSortSelection.ID)
            {
                query = query.OrderBy(i => i.patient.PatientIdentifier);
            }
            else if (request.SortSelection == (int)PatientSortSelection.Name)
            {
                query = query.OrderBy(i => i.patient.FullName);
            }
            else
            {
                query = query.OrderBy(i => i.patient.DoB);
            }

            var totalRow = query.Count();

            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new Patient()
                {
                    PatientId = x.patient.PatientId,
                    PatientIdentifier = x.patient.PatientIdentifier,
                    FullName = x.patient.FullName,
                    DoB = x.patient.DoB,
                    Gender = x.patient.Gender,
                    AvatarLink = x.patient.AvatarLink != null ? x.patient.AvatarLink : "",
                    PrimaryAddress = x.address,
                    PrimaryContact = x.contact,
                }).ToList();

            var result = new PatientList()
            {
                Patients = data,
                TotalCount = totalRow
            };

            return result;
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
