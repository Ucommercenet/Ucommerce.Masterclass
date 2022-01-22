using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class LoginNavigationController : SurfaceController
    {
        private readonly MembershipHelper _membershipHelper;

        public LoginNavigationController(MembershipHelper membershipHelper)
        {
            _membershipHelper = membershipHelper;
        }

        public ActionResult Render()
        {
            var loginStatusModel = _membershipHelper.GetCurrentLoginStatus();
            var loginViewModel = new LoginViewModel();

            loginViewModel.UserName = loginStatusModel.Username;
            loginViewModel.IsLoggedIn = loginStatusModel.IsLoggedIn;
            
            return View("/views/LoginNavigation/index.cshtml", loginViewModel);
        }

        [HttpPost]
        public ActionResult Post(LoginRequestViewModel request)
        {
            _membershipHelper.Login(request.Username, request.Password);
            
            return Redirect(System.Web.HttpContext.Current.Request?.UrlReferrer?.AbsoluteUri ?? "/");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            _membershipHelper.Logout();
            
            return Redirect(System.Web.HttpContext.Current.Request?.UrlReferrer?.AbsoluteUri ?? "/");

        }
    }
}