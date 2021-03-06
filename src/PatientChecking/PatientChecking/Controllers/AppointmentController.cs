using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MediatR;
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
using PatientChecking.Feature.Appointment.Commands;
using System.Globalization;

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
            var request = new PagingRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SortSelection = option,
            };
            return View(await _mediator.Send(new GetAppointmentPagingQuery { pagingRequest = request }));
        }

        public async Task<IActionResult> Detail(int appointmentId)
        {
            var result = await _mediator.Send(new GetAppointmentByIdQuery() { Id = appointmentId });
            if(result != null){
                return View(result);
            }
            var message = new ViewMessage
            {
                MsgType = MessageType.Error,
                MsgText = "Appointment Not Found!",
                MsgTitle = "Not Found"
            };
            TempData["Message"] = JsonConvert.SerializeObject(message);
            return RedirectToAction("Index", new { option = 1, pageSize = 10, pageIndex = 1});
        }

        public async Task<IActionResult> Update(AppointmentDetailViewModel appointmentDetailViewModel)
        {
            var result = await _mediator.Send(new UpdateAppointmentCommand() { appointmentDetailViewModel = appointmentDetailViewModel });

            if (result > 0)
            {
                var message = new ViewMessage
                {
                    MsgType = MessageType.Success,
                    MsgText = "Update Appointment Successfully!",
                    MsgTitle = "Update Successfully"
                };
                TempData["Message"] = JsonConvert.SerializeObject(message);
            }
            else
            {
                var message = new ViewMessage
                {
                    MsgType = MessageType.Error,
                    MsgText = "Update Appointment Failed!",
                    MsgTitle = "Update Failed"
                };
                TempData["Message"] = JsonConvert.SerializeObject(message);
            }

            return RedirectToAction("Detail",new { appointmentId = appointmentDetailViewModel.AppointmentId });
        }
    }
}
