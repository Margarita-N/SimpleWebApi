using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace SimpleWebApi.Logic.Helpers
{
    public class RequireRoleAttribute : Attribute, IAuthorizationFilter
    {
        private string Role { get; }
        public RequireRoleAttribute(string role) 
        {
            Role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = context.HttpContext.User.Claims;
            if (claims.Where(x => x.Type == "Roles").Any())
            {
                var roleClaimValue = claims.FirstOrDefault(x => x.Type == "Roles").Value;
                var rolesDeserialized = JsonConvert.DeserializeObject<List<string>>(roleClaimValue);

                if (!rolesDeserialized.Contains(Role))
                    context.Result = new ForbidResult();
            }
        }
    }
}
