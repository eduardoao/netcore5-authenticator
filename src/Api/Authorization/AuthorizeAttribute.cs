using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using WebApi.Entities;

namespace WebApi.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // pule a autorização se a ação for decorada com o atributo [AllowAnonymous]
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // autorização
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
                context.Result = new JsonResult(
                    new { message = "Unauthorized" }) 
                    { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}