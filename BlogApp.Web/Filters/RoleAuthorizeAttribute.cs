using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Domain.Enums;

namespace BlogApp.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserRole[] _allowedRoles;

        public RoleAuthorizeAttribute(params UserRole[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? Array.Empty<UserRole>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            UserRole userRole = UserRole.Guest;
            if (_allowedRoles.Length == 0)
            {
                return;
            }

            if (context.HttpContext.Items["UserRole"] is string roleIdValue && Enum.IsDefined(typeof(UserRole), int.Parse(roleIdValue)))
            {
                userRole = (UserRole)int.Parse(roleIdValue);
            }

            if (!_allowedRoles.Contains(userRole))
            {
                context.Result = new ForbidResult();
            }
            return;
        }
    }
}
