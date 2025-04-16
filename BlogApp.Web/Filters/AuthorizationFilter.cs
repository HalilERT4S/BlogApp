using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlogApp.Web.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public AuthorizationFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isUserLoggedIn = context.HttpContext.Items["IsUserLoggedIn"] as bool?;
            if (isUserLoggedIn.HasValue && isUserLoggedIn.Value)
            {
                return;
            }

            
            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            tempData["ErrorMessage"] = "Bu işlemi gerçekleştirebilmek için giriş yapmanız gerekmektedir.";
            tempData.Keep("ErrorMessage");

            
            string refererUrl = context.HttpContext.Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(refererUrl))
            {
                context.Result = new RedirectResult(refererUrl);
            }
            else
            {
                context.Result = new RedirectToActionResult("BlogList", "Blog", null);
            }
        }
    }
}
