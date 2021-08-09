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
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            PagingRequest request = new PagingRequest()
            {
                PageIndex = 1,
                PageSize = 10,
                SortSelection = 0
            };
            dynamic mymodel = new ExpandoObject();  
            PagedResult<AppointmentListViewModel> pagedResult = await _appointmentService.GetListAppoinmentsPaging(request);
            mymodel.PagedResult = pagedResult;
            mymodel.PagingRequest = request;
            return View(mymodel);
        }
        public async Task<IActionResult> SortingAndPaging(int Option, int PageSize, int PageIndex)
        {
            PagingRequest request = new PagingRequest()
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                SortSelection = Option
            };
            dynamic mymodel = new ExpandoObject();
            PagedResult<AppointmentListViewModel> pagedResult = await _appointmentService.GetListAppoinmentsPaging(request);
            mymodel.PagedResult = pagedResult;
            mymodel.PagingRequest = request;
            return View("Index", mymodel);
        }
    }
}
