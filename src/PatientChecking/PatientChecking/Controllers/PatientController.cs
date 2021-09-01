using MediatR;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientChecking.Feature.Patient.Queries;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PatientChecking.ServiceModels;
using PatientChecking.Feature.Patient.Commands;

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

        [HttpPost]
        public async Task<IActionResult> Update(PatientDetailViewModel model)
        {
            var result = await _mediator.Send(new UpdatePatientInformationCommand() { Demographic = model});

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

            return RedirectToAction("Detail", new { patientId = model.PatientId });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(int patientId, IFormFile formFile)
        {
            try
            {
                var result = await _mediator.Send(new UploadPatientImageCommand() { FormFile = formFile, PatientId = patientId });

                if(result == ImageUploadingStatus.IsNotImageFailed)
                {
                    var message = new ViewMessage
                    {
                        MsgType = MessageType.Error,
                        MsgText = "Uploaded File must be Image Format! Please Try Again",
                        MsgTitle = "Upload Failed"
                    };
                    TempData["Message"] = JsonConvert.SerializeObject(message);
                }
                else if(result == ImageUploadingStatus.UploadImageFailed)
                {
                    var message = new ViewMessage
                    {
                        MsgType = MessageType.Error,
                        MsgText = "Upload Patient Image Failed!",
                        MsgTitle = "Upload Failed"
                    };
                    TempData["Message"] = JsonConvert.SerializeObject(message);
                }
                else
                {
                    var message = new ViewMessage
                    {
                        MsgType = MessageType.Success,
                        MsgText = "Upload Patient Image Successfully!",
                        MsgTitle = "Upload Successfully"
                    };
                    TempData["Message"] = JsonConvert.SerializeObject(message);
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
