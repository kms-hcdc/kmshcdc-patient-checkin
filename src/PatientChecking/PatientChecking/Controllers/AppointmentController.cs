using MediatR;
using Microsoft.AspNetCore.Mvc;
using PatientChecking.Feature.Appointment.Queries;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.Appointment;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return await Index((int)AppointmentSortSelection.ID, 10, 1).ConfigureAwait(false);
        }
       
        [Route("[Controller]/Index/{option}")]
        public async Task<IActionResult> Index(int option)
        {
            return await Index(option, 10, 1).ConfigureAwait(false);
        }

        [Route("[Controller]/Index/{option}-{pageSize}")]
        public async Task<IActionResult> Index(int option, int pageSize)
        {
            return await Index(option, pageSize, 1).ConfigureAwait(false);
        }

        [Route("[Controller]/Index/{option}-{pageSize}/{pageIndex}")]
        public async Task<IActionResult> Index(int option, int pageSize, int pageIndex)
        {
            return View(await _mediator.Send(new GetAppointmentPagingQuery { option = option, pageIndex = pageIndex, pageSize  = pageSize }));
        }
        public async Task<IActionResult> Detail(int appointmentId)
        {
            return View(await _mediator.Send(new GetAppointmentByIdQuery() { Id = appointmentId }));
        }
    }
}
