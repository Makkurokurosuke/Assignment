using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Filter
{
    public class WebsiteAuthorizationFilter : IAuthorizationFilter
    {
        //private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;


        public WebsiteAuthorizationFilter()
        {
            // _tempDataDictionaryFactory = tempDataDictionary;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var rd = filterContext.RouteData;
            string currentControllerName = rd.Values["controller"].ToString().ToUpper();

            if (!currentControllerName.Equals("DEFAULT") && !currentControllerName.Equals("ERROR"))
            {
                if (String.IsNullOrEmpty(JWTHelper.GetJWTUserToken()))
                {
                    //var tempData = _tempDataDictionaryFactory.GetTempData(filterContext.HttpContext);
                    // tempData["SessionExpired"] = true;
                    filterContext.Result = new RedirectResult("/en-CA/Default/LogIn");
                }
            }
        }
    }
}
