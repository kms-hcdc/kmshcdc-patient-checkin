using Microsoft.AspNetCore.Mvc;
using PatientChecking.Services.Repository;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
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
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        [HttpGet("GetListPatient")]
        public async Task<IActionResult> GetListPatientPaging([FromQuery] PagingRequest request)
        {
            var pagedResult = await _patientService.GetListPatientPaging(request);
            return new JsonResult(pagedResult);
        }
    }
}
