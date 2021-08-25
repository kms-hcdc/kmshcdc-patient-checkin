using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.ServiceModels.Enum
{
    public enum MessageType
    {
        [Description("Information")]
        Information,
        [Description("Warning")]
        Warning,
        [Description("Error")]
        Error,
        [Description("Success")]
        Success
    }
}
