using Microsoft.AspNetCore.Mvc;
using PatientChecking.Services.Repository;
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

        public async Task<IActionResult> Index()
        {
            PagingRequest request = new PagingRequest()
            {
                PageIndex = 1,
                PageSize = 10,
                SortSelection = 0
            };
            PagedResult<PatientListViewModel> pagedResult = await _patientService.GetListPatientPaging(request);
            dynamic mymodel = new ExpandoObject();
            mymodel.PagedResult = pagedResult;
            mymodel.PagingRequest = request;
            return View(mymodel);
        }

        public async Task<IActionResult> SortAndPaging(int SortOption, int PagingOption, int CurrentPage)
        {
            PagingRequest request = new PagingRequest()
            {
                PageIndex = CurrentPage,
                PageSize = PagingOption,
                SortSelection = SortOption
            };
            PagedResult<PatientListViewModel> pagedResult = await _patientService.GetListPatientPaging(request);
            dynamic mymodel = new ExpandoObject();
            mymodel.PagedResult = pagedResult;
            mymodel.PagingRequest = request;
            return View("Index", mymodel);
        }

        public IActionResult Detail()
        {
            return View();
        }

        [HttpGet("[controller]/GetListPatient")]
        public async Task<IActionResult> GetListPatientPaging([FromQuery] PagingRequest request)
        {
            var pagedResult = await _patientService.GetListPatientPaging(request);
            return new JsonResult(pagedResult);
        }
    }
}
