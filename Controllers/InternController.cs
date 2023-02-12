using Microsoft.AspNetCore.Mvc;

namespace warriorTime.Controllers
{
    public class InternController : Controller
    {
        public IActionResult DashBoard()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
            {
                return View();
/* Il faut que je m'identifie pour avoir acce a la page intern*/

            }

            TempData["PermissionDenied"] = 1;
            return RedirectToAction(actionName:"LoginPage",controllerName:"Login" );
        }
    }
}
