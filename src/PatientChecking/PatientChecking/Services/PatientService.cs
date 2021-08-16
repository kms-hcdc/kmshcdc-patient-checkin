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
                .Take(request.PageSize).Select(x => new ServiceModels.Patient
                {
                    PatientId = x.patient.PatientId,
                    PatientIdentifier = x.patient.PatientIdentifier,
                    FullName = x.patient.FullName,
                    DoB = x.patient.DoB,
                    Gender = (PatientGender) x.patient.Gender,
                    AvatarLink = x.patient.AvatarLink != null ? x.patient.AvatarLink : "",
                    PrimaryAddress = new ServiceModels.Address
                    { 
                        StreetLine = x.address.StreetLine
                    },
                    PrimaryContact = new ServiceModels.Contact
                    { 
                        Email = x.contact.Email,
                        PhoneNumber = x.contact.PhoneNumber
                    },
                }).ToList();

            var result = new PatientList
            {
                Patients = data,
                TotalCount = totalRow
            };

            return result;
        }

        public async Task<int> GetPatientsSummary()
        {
            var NumberOfPatients = await _patientCheckInContext.Patients.ToListAsync();
            return NumberOfPatients.Count();
        }
    }
}
