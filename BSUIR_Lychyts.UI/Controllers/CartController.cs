using Microsoft.AspNetCore.Mvc;
using BSUIR_Lychyts.UI.Models;
using BSUIR_Lychyts.UI.Services.ProductService;

namespace BSUIR_Lychyts.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Add(int id, string? returnUrl)
        {
            // Получаем информацию о блюде
            var response = await _productService.GetProductByIdAsync(id);
            if (response.Successfull && response.Data != null)
            {
                // Получаем корзину из сессии
                var cart = GetCartFromSession();

                // Проверяем, есть ли уже такой товар в корзине
                var existingItem = cart.FirstOrDefault(c => c.DishId == id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        DishId = response.Data.Id,
                        DishName = response.Data.Name,
                        Calories = response.Data.Calories,
                        Quantity = 1
                    });
                }

                // Сохраняем корзину в сессию
                SaveCartToSession(cart);
            }

            // Возвращаемся на предыдущую страницу
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCartFromSession();
            cart.RemoveAll(c => c.DishId == id);
            SaveCartToSession(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            return System.Text.Json.JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = System.Text.Json.JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}