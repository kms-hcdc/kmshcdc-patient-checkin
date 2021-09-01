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
        public PatientDetailViewModel Demographic { get; set; }
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
            var patientDetails = await _patientService.GetPatientInDetailAsync(request.Demographic.PatientId);

            if (patientDetails == null)
            {
                return -1;
            }

            patientDetails.FirstName = request.Demographic.FirstName;
            patientDetails.MiddleName = request.Demographic.MiddleName;
            patientDetails.LastName = request.Demographic.LastName;
            patientDetails.FullName = string.IsNullOrEmpty(request.Demographic.MiddleName) ? String.Format("{0} {1}", request.Demographic.FirstName, request.Demographic.LastName) : String.Format("{0} {1} {2}", request.Demographic.FirstName, request.Demographic.MiddleName, request.Demographic.LastName);
            patientDetails.Nationality = request.Demographic.Nationality;
            patientDetails.DoB = DateTime.Parse(request.Demographic.DoB);
            patientDetails.MaritalStatus = request.Demographic.MaritalStatus == (int)PatientMaritalStatus.Married;
            patientDetails.Gender = request.Demographic.Gender;
            patientDetails.EthnicRace = request.Demographic.Nationality == PatientNationality.Vietnamese.ToString() ? request.Demographic.EthnicRace : null;
            patientDetails.HomeTown = request.Demographic.HomeTown;
            patientDetails.BirthplaceCity = request.Demographic.BirthplaceCity;
            patientDetails.InsuranceNo = request.Demographic.InsuranceNo;
            patientDetails.IdcardNo = request.Demographic.IdcardNo;
            patientDetails.IssuedDate = !string.IsNullOrEmpty(request.Demographic.IssuedDate) ? DateTime.Parse(request.Demographic.IssuedDate) : null;
            patientDetails.IssuedPlace = request.Demographic.IssuedPlace;

            var result = await _patientService.UpdatePatientDetailAsync(patientDetails);

            return result;
        }
    }
}
