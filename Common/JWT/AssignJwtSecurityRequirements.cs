using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class AssignJwtSecurityRequirements : IOperationFilter
    {
        /// <summary>
        /// Swagger UI Filter
        /// Swagger API JWT token authorization
        /// </summary>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();

            //if AllowAnonymous, access code is not required
            var allowAnonymousAccess = context.MethodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(AllowAnonymousAttribute));

            if (allowAnonymousAccess == false)
            {
                var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
            {
                { "JWToken", new List<string>() }
            };

                operation.Security.Add(oAuthRequirements);
            }
        }
    }
}
