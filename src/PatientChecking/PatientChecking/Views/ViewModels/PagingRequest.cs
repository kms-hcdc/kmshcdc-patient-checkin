using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.Views.ViewModels
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int SortSelection { get; set; }
    }
}
