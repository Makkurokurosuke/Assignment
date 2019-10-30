using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Website
{
    public class WETUtility
    {
        public static string GetString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }



    }

    public class WebsiteUtility
    {


        public static int GetWebAPITimeoutmillisecond()
        {
            int timeoutTime = 20000;

            string strMilliSec = Common.Utility.AppSettingsConfigurationManager.AppSetting["WebAPITimeout:Millisecond"];
            int.TryParse(strMilliSec, out timeoutTime);

            return timeoutTime;
        }


        public static async Task HandleWebAPIError(HttpResponseMessage response)
        {
            var errorMessage = "";
            try
            {
                errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception("User Review API Response Error:" + errorMessage);


            }
            catch (Exception ex)
            {
                throw new Exception("User Review API Response Error: " + response.ReasonPhrase + " " + ex.ToString());
            }
        }
    }



}
