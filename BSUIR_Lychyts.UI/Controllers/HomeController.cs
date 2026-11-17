using Microsoft.AspNetCore.Mvc;

namespace BSUIR_Lychyts.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["TitleText"] = "Контрольная работа №1";
            return View();
        }
    }
}