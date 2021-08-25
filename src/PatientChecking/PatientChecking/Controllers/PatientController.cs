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
        private readonly IImageService _imageService;

        public PatientController(IPatientService patientService, IAppConfigurationService provinceCityService, IImageService imageService)
        {
            _patientService = patientService;
            _provinceCityService = provinceCityService;
            _imageService = imageService;
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
                    MsgText = "Update Patient Information Successfully!",
                    MsgTitle = "Update Successfully"
                };
                TempData["Message"] = JsonConvert.SerializeObject(message);
            }
            else
            {
                var message = new ViewMessage
                {
                    MsgType = MessageType.Error,
                    MsgText = "Update Patient Information Failed!",
                    MsgTitle = "Update Failed"
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
                    if (!_imageService.IsImageFile(formFile))
                    {
                        var message = new ViewMessage
                        {
                            MsgType = MessageType.Error,
                            MsgText = "Uploaded File Must Be In Image Format! Please Try Again",
                            MsgTitle = "Upload Failed"
                        };
                        TempData["Message"] = JsonConvert.SerializeObject(message);
                        return RedirectToAction("Detail", new { patientId = patientId });
                    }

                    var avatarLink = _imageService.SaveImage(formFile);

                    var result = _patientService.UploadPatientImage(patientId, avatarLink);

                    if (result > 0)
                    {
                        var message = new ViewMessage
                        {
                            MsgType = MessageType.Success,
                            MsgText = "Upload Patient Image Successfully!",
                            MsgTitle = "Upload Successfully"
                        };
                        TempData["Message"] = JsonConvert.SerializeObject(message);
                    }
                    else
                    {
                        var message = new ViewMessage
                        {
                            MsgType = MessageType.Error,
                            MsgText = "Upload Patient Image Failed!",
                            MsgTitle = "Upload Failed"
                        };
                        TempData["Message"] = JsonConvert.SerializeObject(message);
                    }
                }
            }
            catch (IOException)
            {
                var message = new ViewMessage
                {
                    MsgType = MessageType.Error,
                    MsgText = "Upload Patient Image Failed!",
                    MsgTitle = "Upload Failed"
                };
                TempData["Message"] = JsonConvert.SerializeObject(message);
            }

            return RedirectToAction("Detail", new { patientId = patientId });
        }
    }
}
