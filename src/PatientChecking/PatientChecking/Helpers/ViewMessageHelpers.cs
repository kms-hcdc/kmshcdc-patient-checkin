using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using PatientChecking.Services.ServiceModels;
using PatientChecking.Services.ServiceModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PatientChecking.Helpers
{
    public static class ViewMessageHelpers
    {
        public static IHtmlContent MessageAlert(this IHtmlHelper helper, ViewMessage msg)
        {
            return MessageAlert(helper, msg, null);
        }
        public static IHtmlContent MessageAlert(this IHtmlHelper helper, ViewMessage msg,
                                           object htmlAttributes)
        {
            if (msg == null || String.IsNullOrEmpty(msg.MsgText))
            {
                return HtmlString.Empty;
            }
            return BuildMessageHelper(helper, msg, htmlAttributes);
        }
        private static IHtmlContent BuildMessageHelper(this IHtmlHelper helper, ViewMessage msg,
                                                 object attributes)
        {
            // Create the container
            var ulMsg = new System.Web.Mvc.TagBuilder("div");
            ulMsg.MergeAttribute("role", "alert");
            ulMsg.MergeAttribute("id", "alert-message");
            switch (msg.MsgType)
            {
                case MessageType.Information:
                    ulMsg.MergeAttribute("class", "alert alert-info");
                    break;
                case MessageType.Error:
                    ulMsg.MergeAttribute("class", "alert alert-danger");
                    break;
                case MessageType.Warning:
                    ulMsg.MergeAttribute("class", "alert alert-warning");
                    break;
                case MessageType.Success:
                    ulMsg.MergeAttribute("class", "alert alert-success");
                    break;
            }

            ulMsg.MergeAttributes(new RouteValueDictionary(attributes));

            var sb = new StringBuilder();
            sb.AppendFormat("<p>{0}</p>", msg.MsgText);
            sb.Append("<a class=\"close\" data-dismiss=\"alert\" href=\"#\">×</a>");
            ulMsg.InnerHtml = sb.ToString();

            return new HtmlString(ulMsg.ToString(System.Web.Mvc.TagRenderMode.Normal));
        }
    }
}
