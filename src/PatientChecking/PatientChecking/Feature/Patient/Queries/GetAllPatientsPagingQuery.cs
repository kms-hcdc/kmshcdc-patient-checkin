using MediatR;
using PatientChecking.Services.Patient;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Patient.Queries
{
    public class GetAllPatientsPagingQuery : IRequest<PatientListViewModel>
    {
        public PagingRequest requestPaging { get; set; }      
    }

    public class GetAllPatientsPagingQueryHandler : IRequestHandler<GetAllPatientsPagingQuery, PatientListViewModel>
    {
        private readonly IPatientService _patientService;
        public GetAllPatientsPagingQueryHandler(IPatientService patientService)
        {
            _patientService = patientService;
        }
        public async Task<PatientListViewModel> Handle(GetAllPatientsPagingQuery request, CancellationToken cancellationToken)
        {
            var result = await _patientService.GetListPatientPagingAsync(request.requestPaging);
            var patientsVm = new List<PatientViewModel>();

            foreach (ServiceModels.Patient p in result.Patients)
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
                SortSelection = request.requestPaging.SortSelection,
                PageIndex = request.requestPaging.PageIndex,
                PageSize = request.requestPaging.PageSize,
                TotalCount = result.TotalCount
            };

            return model;
        }
    }
}
