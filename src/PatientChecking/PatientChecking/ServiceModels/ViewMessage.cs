using PatientChecking.ServiceModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientChecking.ServiceModels
{
    public class ViewMessage
    {
        public ViewMessage()
        {
            MsgType = MessageType.Information;
            MsgText = string.Empty;
            MsgTitle = string.Empty;
        }
        public ViewMessage(MessageType msgType, string msg, string title)
        {
            MsgType = msgType;
            MsgText = msg;
            MsgTitle = title;
        }
        public MessageType MsgType { get; set; }
        public string MsgText { get; set; }
        public string MsgTitle { get; set; }
    }
}