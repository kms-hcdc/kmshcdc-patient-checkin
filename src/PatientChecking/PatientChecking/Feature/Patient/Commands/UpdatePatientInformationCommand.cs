using MediatR;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.Patient;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Patient.Commands
{
    public class UpdatePatientInformationCommand : IRequest<int>
    {
        public PatientDetailViewModel PatientModel { get; set; }
    }

    public class UpdatePatientInformationCommandHandler : IRequestHandler<UpdatePatientInformationCommand, int>
    {
        private readonly IPatientService _patientService;
        public UpdatePatientInformationCommandHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }
        public async Task<int> Handle(UpdatePatientInformationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patientDetails = new PatientCheckIn.DataAccess.Models.Patient
                {
                    PatientId = request.PatientModel.PatientId,
                    FirstName = request.PatientModel.FirstName,
                    MiddleName = request.PatientModel.MiddleName,
                    LastName = request.PatientModel.LastName,
                    FullName = string.IsNullOrEmpty(request.PatientModel.MiddleName) ? request.PatientModel.FirstName + " " + request.PatientModel.LastName : request.PatientModel.FirstName + " " + request.PatientModel.MiddleName + " " + request.PatientModel.LastName,
                    Nationality = request.PatientModel.Nationality,
                    DoB = DateTime.Parse(request.PatientModel.DoB),
                    MaritalStatus = request.PatientModel.MaritalStatus == (int)PatientMaritalStatus.Married,
                    Gender = request.PatientModel.Gender,
                    EthnicRace = request.PatientModel.Nationality == PatientNationality.Vietnamese.ToString() ? request.PatientModel.EthnicRace : null,
                    HomeTown = request.PatientModel.HomeTown,
                    BirthplaceCity = request.PatientModel.BirthplaceCity,
                    InsuranceNo = request.PatientModel.InsuranceNo,
                    IdcardNo = request.PatientModel.IdcardNo,
                    IssuedDate = !string.IsNullOrEmpty(request.PatientModel.IssuedDate) ? DateTime.Parse(request.PatientModel.IssuedDate) : null,
                    IssuedPlace = request.PatientModel.IssuedPlace
                };

                var result = await _patientService.UpdatePatientDetail(patientDetails);

                return result;
            }
            catch (FormatException)
            {
                return -1;
            }
        }
    }
}
