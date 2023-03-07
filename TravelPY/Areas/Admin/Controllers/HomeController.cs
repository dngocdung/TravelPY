using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize()]
    public class HomeController : Controller
    {
        //[AllowAnonymous]
        //[Route("admin.html", Name = "Home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
