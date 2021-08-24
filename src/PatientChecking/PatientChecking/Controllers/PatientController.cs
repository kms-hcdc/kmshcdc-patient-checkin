using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatientChecking.Services.Repository;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Services.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class PatientController : BaseController
    {

        private readonly IPatientService _patientService;
        private readonly IAppConfigurationService _provinceCityService;
        private readonly INotyfService _notyf;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PatientController(IPatientService patientService, IAppConfigurationService provinceCityService, INotyfService notyf, IHostingEnvironment hostingEnvironment)
        {
            _patientService = patientService;
            _provinceCityService = provinceCityService;
            _notyf = notyf;
            _hostingEnvironment = hostingEnvironment;
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
            var cityResult = _provinceCityService.GetProvinceCities();
            var cityList = new List<string>();

            foreach (ProvinceCity p in cityResult)
            {
                cityList.Add(p.ProvinceCityName);
            }

            if (patientId < 0)
            {
                var emptyModel = new PatientDetailViewModel
                {
                    PatientId = -1,
                    PatientIdentifier = "",
                    Nationality = "Vietnamese",
                    ProvinceCities = cityList
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

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(PatientDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Unable to save changes. Please try again");
            }

            var patientDetails = new PatientDetails
            {
                PatientId = model.PatientId,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                FullName = string.IsNullOrEmpty(model.MiddleName) ? model.FirstName + " " + model.LastName : model.FirstName + " " + model.MiddleName + " " + model.LastName,
                Nationality = model.Nationality,
                DoB = DateTime.Parse(model.DoB),
                MaritalStatus = model.MaritalStatus == (int)PatientMaritalStatus.Married,
                Gender = (PatientGender)model.Gender,
                EthnicRace = model.Nationality == "Vietnamese" ? model.EthnicRace : null,
                HomeTown = model.HomeTown,
                BirthplaceCity = model.BirthplaceCity,
                InsuranceNo = model.InsuranceNo,
                IdcardNo = model.IdcardNo,
                IssuedDate = !string.IsNullOrEmpty(model.IssuedDate) ? DateTime.Parse(model.IssuedDate) : null,
                IssuedPlace = model.IssuedPlace
            };

            var result = _patientService.UpdatePatientDetail(patientDetails);

            if (result > 0)
            {
                var message = new ViewMessage
                {
                    MsgType = MessageType.Success,
                    MsgText = "Update patient information successfully!",
                    MsgTitle = "Success updated"
                };
                TempData["Message"] = JsonConvert.SerializeObject(message);
            }
            else
            {
                var message = new ViewMessage
                {
                    MsgType = MessageType.Error,
                    MsgText = "Update patient information failed!",
                    MsgTitle = "Success updated"
                };
                TempData["Message"] = JsonConvert.SerializeObject(message);
            }

            return RedirectToAction("Detail", new { patientId = patientDetails.PatientId });
        }

        [HttpPost]
        public IActionResult UploadImage(int patientId, IFormFile formFile)
        {
            try
            {
                if (formFile != null)
                {
                    var fileExtension = Path.GetExtension(formFile.FileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != "jpeg")
                    {
                        _notyf.Error("Upload patient image Failed! Uploaded file must be image extensions");
                        return RedirectToAction("Detail", new { patientId = patientId });
                    }

                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Image");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(formFile.FileName);

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    formFile.CopyTo(new FileStream(filePath, FileMode.Create));

                    var avatarLink = "/Image/" + uniqueFileName;

                    var result = _patientService.UploadPatientImage(patientId, avatarLink);
                    if (result > 0)
                    {
                        _notyf.Success("Update patient detail successfully!");
                    }
                    else
                    {
                        _notyf.Error("Upload patient image Failed!");
                    }
                }
            }
            catch (IOException)
            {
                _notyf.Error("Upload patient image Failed!");
            }

            return RedirectToAction("Detail", new { patientId = patientId });
        }
    }
}
