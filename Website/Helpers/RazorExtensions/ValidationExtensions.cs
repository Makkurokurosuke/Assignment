using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Helpers.RazorExtensions
{
    public static class ValidationExtensions
    {
        public static IHtmlContent CustomValidationSummary(
            this IHtmlHelper htmlHelper, string formId, bool isEnglish)
        {

            var errorItems = (from key in htmlHelper.ViewData.ModelState.Keys
                              from state in htmlHelper.ViewData.ModelState
                              from error in state.Value.Errors
                              where state.Key == key
                              where !string.IsNullOrWhiteSpace(error.ErrorMessage)
                              select new { Id = key, Error = error.ErrorMessage }).ToList();

            var listBuilder = new TagBuilder("ul");

            int errorIndex = 0;
            foreach (var item in errorItems)
            {
                errorIndex++;
                TagBuilder html = null;

                if (item.Id != null)
                {
                    var anchorBuilder = new TagBuilder("a");
                    anchorBuilder.MergeAttribute("href", string.Format("#{0}", item.Id));
                    anchorBuilder.InnerHtml.AppendHtml("<span class='prefix'>Error&nbsp;"
                        + errorIndex.ToString() + ": </span>" +
                        item.Error);
                    html = anchorBuilder;
                }

                var li = new TagBuilder("li");
                li.InnerHtml.AppendHtml(html);
                listBuilder.InnerHtml.AppendHtml(li);
            }


            var section = new TagBuilder("section");
            section.Attributes.Add("id", "errors-" + formId);
            section.AddCssClass("alert alert-danger");
            section.Attributes.Add("tabindex", "-1");

            string errorMsg = "";

            if (errorIndex == 1)
            {
                errorMsg = isEnglish ? "error was" : "";
            }
            else if (errorIndex > 1)
            {
                errorMsg = isEnglish ? "errors were" : "";
            }

            section.InnerHtml.AppendHtml("<h2>" + (isEnglish ?
                "The form could not be submitted because " +
                errorIndex.ToString()
                + " " + errorMsg + " found." : "The form could not be submitted because " +
                errorIndex.ToString()
                + " " + errorMsg + " found. [Fr]") + "</h2>");

            section.InnerHtml.AppendHtml(listBuilder);

            string retValue = string.Empty;

            if (errorIndex != 0)
            {
                using (var sw = new System.IO.StringWriter())
                {
                    section.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
                    retValue = sw.ToString();
                }
            };

            return new HtmlString(retValue);
        }
    }
}
