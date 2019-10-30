using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Helpers.RazorExtensions
{
    public static class LabelExtensions
    {

        public static IHtmlContent CustomLabelFor<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool isRequired, string extraInfo = "")
        {


            var memberExpression = expression.Body as MemberExpression;
            string fieldName = memberExpression.Member.Name;


            IHtmlContent labelHtmlContent = null;
            // Get the Label
            if (htmlAttributes != null)
            {
                labelHtmlContent = htmlHelper.LabelFor(expression, htmlAttributes);
            }
            else
            {
                labelHtmlContent = htmlHelper.LabelFor(expression);
            }

            string labelHtml = "";
            using (var sw = new System.IO.StringWriter())
            {
                labelHtmlContent.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
                labelHtml = sw.ToString();
            }

            // Get the metadata
            //            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            //var htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            // Extract the label content
            var r = new Regex("(<label.*>)(.*)(</label>)", RegexOptions.IgnoreCase);
            var m = r.Match(labelHtml);

            if (m.Success)
            {
                var openingTag = m.Groups[1].Value.Substring(0, m.Groups[1].Value.Length - 2) + "\"> ";
                var content = m.Groups[2].Value;

                if (isRequired)
                {
                    openingTag += "<span class=\"field-name\" >" + content + "</span>";
                }

                if (!String.IsNullOrEmpty(extraInfo))
                {
                    openingTag += " " + extraInfo;
                }

                if (isRequired)
                { // if required, append the required text after the the label.
                    openingTag += "<strong class=\"required\" > (required)</strong>";
                }

                var errorItems = (from key in htmlHelper.ViewData.ModelState.Keys
                                  from state in htmlHelper.ViewData.ModelState
                                  from error in state.Value.Errors
                                  where state.Key == key
                                  where !string.IsNullOrWhiteSpace(error.ErrorMessage)
                                  select new { Id = key, Error = error.ErrorMessage }).ToList();

                int errorIndex = 0;
                StringBuilder errorMsg = new StringBuilder();
                foreach (var item in errorItems)
                {
                    errorIndex++;
                    if (item.Id.Equals(fieldName))
                    {
                        string errorItemMsg = "<span class=\"label label-danger wb-server-error\"" +
                                "id = " + "error_id_" + fieldName + "_" + errorIndex.ToString() + " >" +
                                "<strong><span class=\"prefix\" >Error " +
                                errorIndex.ToString() + "&nbsp;:</span>" + item.Error +
                                "</strong>" +
                                "</span>";

                        errorMsg.Append(errorItemMsg);
                    }
                }

                var closingTag = m.Groups[3].Value;

                labelHtml = string.Format("{0}{1}{2}", openingTag, errorMsg.ToString(), closingTag);

            }

            return new HtmlString(labelHtml);
        }

    }
}
