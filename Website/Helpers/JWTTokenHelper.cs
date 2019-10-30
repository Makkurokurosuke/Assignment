using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Website
{
    public static class JWTHelper
    {
        public static void SetTokenExpiryDateTime(DateTime expiryDateTime)
        {
            AppHttpContext.Current.Session.SetString("JWTUserTokenExpiryDateTime", expiryDateTime.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

        }

        public static string GetJWTUserToken()
        {
            return Website.AppHttpContext.Current.Session.GetString("JWTUserToken");
        }

        public static void SetJWTUserToken(string token)
        {
            Website.AppHttpContext.Current.Session.SetString("JWTUserToken", token);
        }

        public static int GetJWTUserId()
        {
            int intUserId = 0;

            string strUserId = Website.AppHttpContext.Current.Session.GetString("JWTUserId");

            if ((!String.IsNullOrEmpty(strUserId)) && int.TryParse(strUserId, out intUserId))
            {
                intUserId = int.Parse(strUserId);
            }

            return intUserId;
        }

        public static void SetJWTUserId(int userId)
        {
            Website.AppHttpContext.Current.Session.SetString("JWTUserId", userId.ToString());
        }


        //public static bool IsTokenExpiryRenewRequired(DateTime expiryDateTime)
        //{
        //    bool doesExpireSoon = false;
        //    string expiryTimeStr = AppHttpContext.Current.Session.GetString("JWTUserTokenExpiryDateTime");
        //    DateTime expiryTime = DateTime.ParseExact(expiryTimeStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        //    doesExpireSoon = DateTime.Now.AddMinutes(5) > expiryTime;
        //    return doesExpireSoon;

        //}

    }




}
