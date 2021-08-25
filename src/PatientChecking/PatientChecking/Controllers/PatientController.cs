using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientChecking.Feature.Patient.Queries;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.AppConfiguration;
using PatientChecking.Services.Patient;
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
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return await Index((int)PatientSortSelection.ID, 10, 1);
        }

        [Route("[Controller]/Index/{sortOption}")]
        public async Task<IActionResult> Index(int sortOption)
        {
            return await Index(sortOption, 10, 1);
        }

        [Route("[Controller]/Index/{sortOption}-{pagingOption}")]
        public async Task<IActionResult> Index(int sortOption, int pagingOption)
        {
            return await Index(sortOption, pagingOption, 1);
        }

        [Route("[Controller]/Index/{sortOption}-{pagingOption}/{currentPage}")]
        public async Task<IActionResult> Index(int sortOption, int pagingOption, int currentPage)
        {
            var request = new PagingRequest
            {
                PageIndex = currentPage,
                PageSize = pagingOption,
                SortSelection = sortOption
            };

            return View(await _mediator.Send(new GetAllPatientsPagingQuery() { requestPaging = request }));
        }

        public async Task<IActionResult> Detail(int patientId)
        {
            return View(await _mediator.Send(new GetPatientInDetailByIdQuery() { PatientId = patientId }));
        }
    }
}
