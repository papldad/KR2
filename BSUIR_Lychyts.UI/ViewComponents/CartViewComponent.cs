using Microsoft.AspNetCore.Mvc;

namespace BSUIR_Lychyts.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            ViewBag.Total = 0.00m;
            ViewBag.Count = 0;
            return View();
        }
    }
}