using MediatR;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.AppConfiguration;
using PatientChecking.Services.Patient;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Patient.Queries
{
    public class GetPatientInDetailByIdQuery : IRequest<PatientDetailViewModel>
    {
        public int PatientId { get; set; }      
    }
    public class GetPatientInDetailByIdQueryHandler : IRequestHandler<GetPatientInDetailByIdQuery, PatientDetailViewModel>
    {
        private readonly IPatientService _patientService;
        private readonly IAppConfigurationService _appConfigurationService;
        public GetPatientInDetailByIdQueryHandler(IPatientService patientService, IAppConfigurationService appConfigurationService)
        {
            _patientService = patientService;
            _appConfigurationService = appConfigurationService;
        }
        public async Task<PatientDetailViewModel> Handle(GetPatientInDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var cities = await _appConfigurationService.GetProvinceCitiesAsync();

            var result = await _patientService.GetPatientInDetailAsync(request.PatientId);

            if (request.PatientId < 0 || result == null)
            {
                var emptyModel = new PatientDetailViewModel
                {
                    PatientId = -1,
                    PatientIdentifier = "",
                    Nationality = "Vietnamese",
                    ProvinceCities = cities
                };
                return emptyModel;
            }

            var model = new PatientDetailViewModel
            {
                PatientId = result.PatientId,
                PatientIdentifier = result.PatientIdentifier,
                FirstName = result.FirstName,
                LastName = result.LastName,
                MiddleName = result.MiddleName,
                FullName = result.FullName,
                Nationality = result.Nationality,
                DoB = result.DoB.ToString("yyyy-MM-dd"),
                MaritalStatus = (int)(result.MaritalStatus == true ? PatientMaritalStatus.Married : PatientMaritalStatus.Unmarried),
                Gender = (int)result.Gender,
                AvatarLink = result.AvatarLink,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                EthnicRace = result.EthnicRace,
                HomeTown = result.HomeTown,
                BirthplaceCity = result.BirthplaceCity,
                IdcardNo = result.IdcardNo,
                IssuedDate = result.IssuedDate?.ToString("yyyy-MM-dd"),
                IssuedPlace = result.IssuedPlace,
                InsuranceNo = result.InsuranceNo,
                ProvinceCities = cities
            };

            return model;
        }
    }
}
