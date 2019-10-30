using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class MiddlewareErrorHandler
    {
        private readonly RequestDelegate next;
        public MiddlewareErrorHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (ex is Common.Extension.ModelValidationException)
            {
                code = HttpStatusCode.BadRequest;
            }
            else if (ex is Common.Extension.DataNotFoundException)
            {
                code = HttpStatusCode.BadRequest;
            }

            var errorObj = new API.Models.CustomErrorModel(ex);
            var result = JsonConvert.SerializeObject(errorObj);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }

        public string createErrorMessage(Exception ex)
        {

            var errorObj = new API.Models.CustomErrorModel(ex);
            var errorMessage = JsonConvert.SerializeObject(errorObj);

            return errorMessage;
        }

    }
}
