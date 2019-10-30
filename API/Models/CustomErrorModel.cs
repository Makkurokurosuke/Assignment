using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CustomErrorModel
    {
        public CustomErrorModel(System.Exception exception)
        {
            this.Error = new CustomException(exception.GetType().Name, exception.Message);
            this.Error.Target = $"{exception.TargetSite.ReflectedType.FullName}.{exception.TargetSite.Name}";
            if (exception.InnerException != null)
            {
                this.Error.InnerError = new CustomInnerException(exception.InnerException);
            }
        }

        public CustomException Error { get; set; }




    }


    public class CustomException
    {
        public CustomException(String errorType, String message)
        {
            this.ErrorType = errorType;
            this.Message = message;
        }

        [Required]
        public String ErrorType { get; set; }

        [Required]
        public String Message { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Target { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<CustomException> Details { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomInnerException InnerError { get; set; }
    }

    public class CustomInnerException
    {
        public CustomInnerException(System.Exception exception)
        {
            this.Code = exception.GetType().Name;
            if (exception.InnerException != null)
            {
                this.InnerError = new CustomInnerException(exception.InnerException);
            }
        }

        [Required]
        public String Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CustomInnerException InnerError { get; set; }
    }

}