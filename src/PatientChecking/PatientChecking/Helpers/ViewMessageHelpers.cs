using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
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
            if (String.IsNullOrEmpty(msg?.MsgText))
            {
                return HtmlString.Empty;
            }

            return BuildMessageHelper(helper, msg, htmlAttributes);
        }

        private static IHtmlContent BuildMessageHelper(this IHtmlHelper helper, ViewMessage msg,
                                                 object attributes)
        {
            var ulMsg = new System.Web.Mvc.TagBuilder("div");

            ulMsg.MergeAttribute("role", "alert");
            ulMsg.MergeAttribute("id", "alert-message");

            switch (msg.MsgType)
            {
                case MessageType.Information:
                    ulMsg.MergeAttribute("class", "alert alert-info position-absolute d-flex justify-content-between align-items-center top-5 end-0");
                    break;

                case MessageType.Warning:
                    ulMsg.MergeAttribute("class", "alert alert-warning position-absolute d-flex justify-content-between align-items-center top-5 end-0");
                    break;

                case MessageType.Success:
                    ulMsg.MergeAttribute("class", "alert alert-success position-absolute d-flex justify-content-between align-items-center top-5 end-0");
                    break;

                default:
                    ulMsg.MergeAttribute("class", "alert alert-danger position-absolute d-flex justify-content-between align-items-center top-5 end-0");
                    break;
            }

            ulMsg.MergeAttributes(new RouteValueDictionary(attributes));

            var sb = new StringBuilder();

            sb.AppendFormat("<p>{0}</p>", msg.MsgText);
            sb.Append("<a class=\"close text-decoration-none\" data-dismiss=\"alert\" href=\"#\">×</a>");

            ulMsg.InnerHtml = sb.ToString();

            return new HtmlString(ulMsg.ToString(System.Web.Mvc.TagRenderMode.Normal));
        }
    }
}
