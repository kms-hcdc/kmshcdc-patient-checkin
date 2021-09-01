using Microsoft.EntityFrameworkCore;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Services.Patient
{
    public class PatientService : IPatientService
    {
        private readonly PatientCheckInContext _patientCheckInContext;

        public PatientService(PatientCheckInContext patientCheckInContext)
        {
            _patientCheckInContext = patientCheckInContext;
        }

        public async Task<PatientList> GetListPatientPagingAsync(PagingRequest request)
        {
            var query = from patient in _patientCheckInContext.Patients
                        join address in _patientCheckInContext.Addresses on patient.PatientId equals address.PatientId into pa
                        from patientAddress in pa.DefaultIfEmpty()
                        where patientAddress == null || patientAddress.IsPrimary == true
                        select new { patient, patientAddress };

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

            var data = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new PatientChecking.ServiceModels.Patient
                {
                    PatientId = x.patient.PatientId,
                    PatientIdentifier = x.patient.PatientIdentifier,
                    FullName = x.patient.FullName,
                    DoB = x.patient.DoB,
                    Gender = (PatientGender)x.patient.Gender,
                    AvatarLink = x.patient.AvatarLink != null ? x.patient.AvatarLink : "",
                    Email = x.patient.Email,
                    PhoneNumber = x.patient.PhoneNumber,
                    PrimaryAddress = new Address
                    {
                        StreetLine = x.patientAddress.StreetLine
                    }
                }).ToListAsync();

            var result = new PatientList
            {
                Patients = data,
                TotalCount = totalRow
            };

            return result;
        }

        public async Task<PatientCheckIn.DataAccess.Models.Patient> GetPatientInDetailAsync(int patientId)
        {
            var patient = await _patientCheckInContext.Patients.FindAsync(patientId);
            return patient;
        }

        public async Task<int> GetPatientsSummaryAsync()
        {
            var NumberOfPatients = await _patientCheckInContext.Patients.ToListAsync();
            return NumberOfPatients.Count();
        }

        public async Task<int> UpdatePatientDetailAsync(PatientCheckIn.DataAccess.Models.Patient patientDetails)
        {
            if(patientDetails != null)
            {
                _patientCheckInContext.Update(patientDetails);

                return await _patientCheckInContext.SaveChangesAsync();
            }
            return -1;
        }

        public async Task<int> UploadPatientImageAsync(int patientId, string avatarLink)
        {
            var patient = _patientCheckInContext.Patients.Find(patientId);

            if(patient != null)
            {
                patient.AvatarLink = avatarLink;

                _patientCheckInContext.Entry(patient).Property("AvatarLink").IsModified = true;

                return await _patientCheckInContext.SaveChangesAsync();
            }
            return -1;
        }
    }
}
