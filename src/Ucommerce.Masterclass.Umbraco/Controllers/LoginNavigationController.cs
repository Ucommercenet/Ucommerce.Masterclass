using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class LoginNavigationController : SurfaceController
    {
        public ActionResult Render()
        {
            var loginStatusModel = Members.GetCurrentLoginStatus();
            var loginViewModel = new LoginViewModel();

            loginViewModel.UserName = loginStatusModel.Username;
            loginViewModel.IsLoggedIn = loginStatusModel.IsLoggedIn;
            
            return View("/views/LoginNavigation/index.cshtml", loginViewModel);
        }

        [HttpPost]
        public ActionResult Post(LoginRequestViewModel request)
        {
            Members.Login(request.Username, request.Password);
            return RedirectToCurrentUmbracoPage();
        }
    }
}