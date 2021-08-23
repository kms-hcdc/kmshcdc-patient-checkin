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

        public PatientList GetListPatientPaging(PagingRequest request)
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

            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize).Select(x => new ServiceModels.Patient
                {
                    PatientId = x.patient.PatientId,
                    PatientIdentifier = x.patient.PatientIdentifier,
                    FullName = x.patient.FullName,
                    DoB = x.patient.DoB,
                    Gender = (PatientGender)x.patient.Gender,
                    AvatarLink = x.patient.AvatarLink != null ? x.patient.AvatarLink : "",
                    Email = x.patient.Email,
                    PhoneNumber = x.patient.PhoneNumber,
                    PrimaryAddress = new ServiceModels.Address
                    {
                        StreetLine = x.patientAddress.StreetLine
                    }
                }).ToList();

            var result = new PatientList
            {
                Patients = data,
                TotalCount = totalRow
            };

            return result;
        }

        public PatientDetails GetPatientInDetail(int patientId)
        {
            var patient = _patientCheckInContext.Patients.Find(patientId);

            var patientDetail = new PatientDetails
            {
                PatientId = patient.PatientId,
                PatientIdentifier = patient.PatientIdentifier,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                FullName = patient.FullName,
                Nationality = patient.Nationality,
                DoB = patient.DoB,
                MaritalStatus = patient.MaritalStatus,
                Gender = (PatientGender)patient.Gender,
                AvatarLink = patient.AvatarLink,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                EthnicRace = patient.EthnicRace,
                HomeTown = patient.HomeTown,
                BirthplaceCity = patient.BirthplaceCity,
                IdcardNo = patient.IdcardNo,
                IssuedDate = patient.IssuedDate,
                IssuedPlace = patient.IssuedPlace,
                InsuranceNo = patient.InsuranceNo
            };

            return patientDetail;
        }

        public async Task<int> GetPatientsSummary()
        {
            var NumberOfPatients = await _patientCheckInContext.Patients.ToListAsync();
            return NumberOfPatients.Count();
        }
    }
}
