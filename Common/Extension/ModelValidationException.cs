using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Extension
{
    public class ModelValidationException : Exception
    {

        public static ModelValidationException ErrorFactory(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            var errorItems = (from key in modelState.Keys
                              from state in modelState
                              from error in state.Value.Errors
                              where state.Key == key
                              where !string.IsNullOrWhiteSpace(error.ErrorMessage)
                              select new { Id = key, Error = error.ErrorMessage }).ToList();

            var errorMessage = JsonConvert.SerializeObject(errorItems);

            return new ModelValidationException(errorMessage);
        }

        public ModelValidationException(string message)
            : base(message)
        {

        }


    }
}
