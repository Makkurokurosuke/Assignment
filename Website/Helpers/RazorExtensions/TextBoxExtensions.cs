using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Website.Helpers.RazorExtensions
{

    public static class TextBoxExtensions
    {
        public static IHtmlContent CustomTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {

            var attrs = GetErrorList(expression, htmlHelper, htmlAttributes);

            IHtmlContent htmlContentControl = null;

            // Get the TextBox
            if (attrs.Count > 0)
            {
                htmlContentControl = htmlHelper.TextBoxFor(expression, attrs);
            }
            else
            {
                htmlContentControl = htmlHelper.TextBoxFor(expression);
            }

            string textBoxString = GetStringFromHtmlContent(htmlContentControl);


            return new HtmlString(textBoxString);
        }

        private static string GetStringFromHtmlContent(IHtmlContent htmlContentControl)
        {

            string htmlContentStr = "";
            using (var sw = new System.IO.StringWriter())
            {
                htmlContentControl.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
                htmlContentStr = sw.ToString();
            }

            return htmlContentStr;
        }

        public static IHtmlContent CustomTextAreaFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            var attrs = GetErrorList(expression, htmlHelper, htmlAttributes);

            IHtmlContent htmlContentTextArea = null;

            if (attrs.Count > 0)
            {
                htmlContentTextArea = htmlHelper.TextAreaFor(expression, attrs);
            }
            else
            {
                htmlContentTextArea = htmlHelper.TextAreaFor(expression);
            }

            string textAreaString = GetStringFromHtmlContent(htmlContentTextArea);

            return new HtmlString(textAreaString);
        }

        private static IDictionary<string, object> GetErrorList<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression,
            IHtmlHelper<TModel> htmlHelper, object htmlAttributes)
        {
            var memberExpression = expression.Body as MemberExpression;
            string fieldName = memberExpression.Member.Name;

            var errorItems = (from key in htmlHelper.ViewData.ModelState.Keys.Where(x => x.Equals(fieldName))
                              from state in htmlHelper.ViewData.ModelState
                              from error in state.Value.Errors
                              where state.Key == key
                              where !string.IsNullOrWhiteSpace(error.ErrorMessage)
                              select new { Id = key, Error = error.ErrorMessage }).ToList();

            IDictionary<string, object> attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (errorItems.Count > 0)
            {
                if (attrs.ContainsKey("class"))
                {
                    attrs["class"] = attrs["class"] + " has-error ";
                }
                else
                {
                    attrs.Add("class", "has-error");
                }

            }

            return attrs;
        }
    }
}
