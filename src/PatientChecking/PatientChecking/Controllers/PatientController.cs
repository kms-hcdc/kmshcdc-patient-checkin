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

        public IActionResult Index(int sortOption = (int)PatientSortSelection.ID, int pagingOption = 10, int currentPage = 1)
        {
            PagingRequest request = new PagingRequest()
            {
                PageIndex = currentPage,
                PageSize = pagingOption,
                SortSelection = sortOption
            };

            PatientList result = _patientService.GetListPatientPaging(request);

            List<PatientViewModel> patientsVm = new List<PatientViewModel>();

            foreach (Patient p in result.Patients)
            {
                patientsVm.Add(new PatientViewModel()
                {
                    PatientIdentifier = p.PatientIdentifier,
                    FullName = p.FullName,
                    Gender = p.Gender.ToString(),
                    DoB = p.DoB.ToString("dd-MM-yyyy"),
                    AvatarLink = p.AvatarLink,
                    Address = p.PrimaryAddress?.StreetLine,
                    Email = p.PrimaryContact?.Email,
                    PhoneNumber = p.PrimaryContact?.PhoneNumber
                });
            }

            PatientListViewModel model = new PatientListViewModel()
            {
                Patients = patientsVm,
                SortSelection = request.SortSelection,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = result.TotalCount
            };

            return View(model);
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
