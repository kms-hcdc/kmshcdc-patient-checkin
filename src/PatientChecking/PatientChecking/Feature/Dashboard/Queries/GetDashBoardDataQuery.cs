using MediatR;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatientChecking.Feature.Dashboard.Queries
{
    public class GetDashBoardDataQuery : IRequest<DashboardViewModel>
    {
        class GetDashBoardDataHandler : IRequestHandler<GetDashBoardDataQuery, DashboardViewModel>
        {
            public Task<DashboardViewModel> Handle(GetDashBoardDataQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
