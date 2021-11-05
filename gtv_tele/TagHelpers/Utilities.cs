using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gtv_tele.TagHelpers
{
    public static class Utilities
    {
        public static string IsActiveSub(this HtmlHelper html,
                              string control,
                              string action, string sub ="")
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }

        public static string IsActiveMas(this HtmlHelper html,
                      string control)
        {
            var routeData = html.ViewContext.RouteData;
            
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl;

            return returnActive ? "active" : "";
        }

    }
}