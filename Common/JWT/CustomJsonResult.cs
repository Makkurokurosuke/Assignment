using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Return a response in Json
    /// </summary>
    public class CustomJsonResult : JsonResult
    {
        /// <summary>
        /// yyyy-MM-dd'T'HH:mm:ss.ffK(UTC => Z)
        /// </summary>
        private const string DateFormat = "yyyy-MM-dd'T'HH:mm:ss.ffK";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">Status Code</param>
        /// <param name="data">Data</param>
        public CustomJsonResult(HttpStatusCode code, object data)
            : base(data)
        {
            base.StatusCode = (int)code;
        }

        /// <summary>
        /// Result of Action Method of MVC
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new UnauthorizedAccessException("The context of this HTTP request is not defined.");
            }

            HttpResponse response = context.HttpContext.Response;
            await SerializeJsonAsync(response);
        }

        /// <summary>
        /// Serizlized JSON in response
        /// </summary>
        /// <param name="response">Response</param>
        public async Task SerializeJsonAsync(HttpResponse response)
        {
            if (!String.IsNullOrEmpty(ContentType))
            {
                //MIME
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json; charset=utf-8";
            }

            // Content Sniffering
            response.Headers.Add("X-Content-Type-Options", "nosniff");

            // Cache
            response.Headers.Add("Pragma", "no-cache");
            response.Headers.Add("Cache-Control", "no-store, no-cache");

            // Crosssitescripting
            response.Headers.Add("X-XSS-Protection", "1; mode=block");

            //CORS
            if (response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                response.Headers["Access-Control-Allow-Origin"] = "*";
            }
            else
            {
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            //response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            //response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");

            // HTTPs
            response.Headers.Add("Strict-Transport-Security", "max-age=15768000");

            response.StatusCode = StatusCode == null ? StatusCodes.Status200OK : StatusCode.Value;
            if (Value != null)
            {
                // Serialized Json.NET
                var converter = new IsoDateTimeConverter();
                converter.DateTimeStyles = System.Globalization.DateTimeStyles.AdjustToUniversal; //時刻はUTCで
                converter.DateTimeFormat = DateFormat;

                // Write a respose in body
                await response.WriteAsync(JsonConvert.SerializeObject(
                    Value, new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters = new List<JsonConverter>() { converter },
                        Formatting = Formatting.Indented,
                        StringEscapeHandling = StringEscapeHandling.Default,
                    }),
                    Encoding.UTF8
                );
                return;
            }
            else if (response.StatusCode != StatusCodes.Status204NoContent)
            {
                await response.WriteAsync("", Encoding.UTF8);
            }
            return;
        }
    }
}
