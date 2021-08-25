using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PatientChecking.Feature.Dashboard.Queries;
using PatientChecking.ServiceModels;
using PatientChecking.Services.Appointment;
using PatientChecking.Services.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpGet("[controller]/getDashBoard")]
        public async Task<IActionResult> GetDashBoardData()
        {
            return new JsonResult(await _mediator.Send(new GetDashBoardDataQuery()));
        }
    }

}
