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
        private readonly IAppConfigurationService _provinceCityService;
        public GetPatientInDetailByIdQueryHandler(IPatientService patientService, IAppConfigurationService provinceCityService)
        {
            _patientService = patientService;
            _provinceCityService = provinceCityService;
        }
        public async Task<PatientDetailViewModel> Handle(GetPatientInDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var cities = await _provinceCityService.GetProvinceCities();
            var cityList = new List<string>();

            foreach (PatientCheckIn.DataAccess.Models.ProvinceCity p in cities)
            {
                cityList.Add(p.ProvinceCityName);
            }

            if (request.PatientId < 0)
            {
                var emptyModel = new PatientDetailViewModel
                {
                    PatientId = -1,
                    PatientIdentifier = "",
                    Nationality = "Vietnamese",
                    ProvinceCities = cityList
                };
                return emptyModel;
            }

            var result = await _patientService.GetPatientInDetail(request.PatientId);

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
                ProvinceCities = cityList
            };

            return model;
        }
    }
}
