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
                var patientDetails = await _patientService.GetPatientInDetailAsync(request.PatientModel.PatientId);

                if (patientDetails != null)
                {
                    patientDetails.FirstName = request.PatientModel.FirstName;
                    patientDetails.MiddleName = request.PatientModel.MiddleName;
                    patientDetails.LastName = request.PatientModel.LastName;
                    patientDetails.FullName = string.IsNullOrEmpty(request.PatientModel.MiddleName) ? request.PatientModel.FirstName + " " + request.PatientModel.LastName : request.PatientModel.FirstName + " " + request.PatientModel.MiddleName + " " + request.PatientModel.LastName;
                    patientDetails.Nationality = request.PatientModel.Nationality;
                    patientDetails.DoB = DateTime.Parse(request.PatientModel.DoB);
                    patientDetails.MaritalStatus = request.PatientModel.MaritalStatus == (int)PatientMaritalStatus.Married;
                    patientDetails.Gender = request.PatientModel.Gender;
                    patientDetails.EthnicRace = request.PatientModel.Nationality == PatientNationality.Vietnamese.ToString() ? request.PatientModel.EthnicRace : null;
                    patientDetails.HomeTown = request.PatientModel.HomeTown;
                    patientDetails.BirthplaceCity = request.PatientModel.BirthplaceCity;
                    patientDetails.InsuranceNo = request.PatientModel.InsuranceNo;
                    patientDetails.IdcardNo = request.PatientModel.IdcardNo;
                    patientDetails.IssuedDate = !string.IsNullOrEmpty(request.PatientModel.IssuedDate) ? DateTime.Parse(request.PatientModel.IssuedDate) : null;
                    patientDetails.IssuedPlace = request.PatientModel.IssuedPlace;

                    var result = await _patientService.UpdatePatientDetailAsync(patientDetails);

                    return result;
                }
                return -1;
            }
            catch (FormatException)
            {
                return -1;
            }
        }
    }
}
