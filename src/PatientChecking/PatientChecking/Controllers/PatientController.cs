using Microsoft.AspNetCore.Mvc;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Services.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class PatientController : BaseController
    {

        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IActionResult Index()
        {
            return Index((int)PatientSortSelection.ID, 10, 1);
        }

        [Route("[Controller]/Index/{sortOption}")]
        public IActionResult Index(int sortOption)
        {
            return Index(sortOption, 10, 1);
        }

        [Route("[Controller]/Index/{sortOption}-{pagingOption}")]
        public IActionResult Index(int sortOption, int pagingOption)
        {
            return Index(sortOption, pagingOption, 1);
        }

        [Route("[Controller]/Index/{sortOption}-{pagingOption}/{currentPage}")]
        public IActionResult Index(int sortOption, int pagingOption, int currentPage)
        {
            var request = new PagingRequest
            {
                PageIndex = currentPage,
                PageSize = pagingOption,
                SortSelection = sortOption
            };

            var result = _patientService.GetListPatientPaging(request);

            var patientsVm = new List<PatientViewModel>();

            foreach (Patient p in result.Patients)
            {
                patientsVm.Add(new PatientViewModel
                {
                    PatientId = p.PatientId,
                    PatientIdentifier = p.PatientIdentifier,
                    FullName = p.FullName,
                    Gender = p.Gender.ToString(),
                    DoB = p.DoB.ToString("MM-dd-yyyy"),
                    AvatarLink = p.AvatarLink,
                    Address = p.PrimaryAddress?.StreetLine,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber
                });
            }

            var model = new PatientListViewModel
            {
                Patients = patientsVm,
                SortSelection = request.SortSelection,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = result.TotalCount
            };

            return View(model);
        }

        public IActionResult Detail(int patientId)
        {
            if(patientId < 0){
                var emptyModel = new PatientDetailViewModel
                {
                    PatientId = -1,
                    PatientIdentifier = "",
                    Nationality = "Vietnamese",
                    InitData = new PatientDetailsInitData()
                };
                return View(emptyModel);
            }

            var result = _patientService.GetPatientInDetail(patientId);

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
                Gender = (int) result.Gender,
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
                InitData = new PatientDetailsInitData()
            };

            return View(model);
        }
    }
}
