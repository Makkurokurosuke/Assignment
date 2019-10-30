using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Filter
{
    public class GlobalActionFilter : ActionFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;

        public GlobalActionFilter()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.Controller is Controller controller)
            {
                controller.ViewBag.AppTitle = Resources.SharedResource.UserReviewApplication;
                controller.ViewBag.HomeURL = controller.Url.Action("Index", "Home");
                controller.ViewBag.SignOutURL = controller.Url.Action("LogOut", "Default");

            }

        }
    }
}
