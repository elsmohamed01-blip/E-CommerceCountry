using E_CommerceStoreCountry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace E_CommerceStoreCountry.Controllers
{
    public class HomeController : Controller
    {
        ECommerceCountryContext db = new ECommerceCountryContext();
        public IActionResult Index(string country)
        {
            if (!string.IsNullOrEmpty(country))
            {
                HttpContext.Session.SetString("selectedCountry", country);
            }
            else
            {
                country = HttpContext.Session.GetString("selectedCountry") ?? "مصر";
            }

            IndexVM result = new IndexVM
            {
                SelectedCountry = country,
                Categories = db.Catoegries.Where(c => c.Country == country).ToList(),
                Products = db.Products.Where(p => p.Country == country).ToList(),
                Reviews = db.Reviews.ToList(),
                LatestProducts = db.Products.OrderByDescending(x => x.Price).Take(8).ToList()
            };


            return View(result);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int Id, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("يجب أن تكون الكمية أكبر من صفر");
            }

            var product = db.Products.Find(Id);
            if (product == null)
            {
                return NotFound("المنتج غير موجود.");
            }

            var selectedCountry = HttpContext.Session.GetString("selectedCountry");

            // التحقق من أن المنتج ينتمي لنفس الدولة المختارة
            if (product.Country != selectedCountry)
            {
                TempData["Error"] = "لا يمكنك إضافة منتجات من دولة مختلفة، قم بتغيير الدولة أو إفراغ السلة.";
                return RedirectToAction("Cart");
            }

            var item = db.Carts.FirstOrDefault(x => x.ProductId == Id && x.UserId == User.Identity.Name);
            if (item != null)
            {
                item.Qty += quantity;
            }
            else
            {
                db.Carts.Add(new Cart
                {
                    ProductId = Id,
                    UserId = User.Identity.Name,
                    Qty = quantity,
                    Price = product.Price,
                    Country = selectedCountry
                });
            }

            db.SaveChanges();
            return RedirectToAction("Cart");
        }

        [Authorize]
        public IActionResult Cart()
        {
            var selectedCountry = HttpContext.Session.GetString("selectedCountry");

            var result = db.Carts
                .Include(x => x.Product)
                .Where(x => x.UserId == User.Identity.Name && x.Country == selectedCountry)
                .ToList();

            return View(result);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var item = db.Carts.FirstOrDefault(x => x.ProductId == id && x.UserId == User.Identity.Name);

            if (item != null)
            {
                db.Carts.Remove(item);
                db.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddOrder(Order model)
        {
            var selectedCountry = HttpContext.Session.GetString("selectedCountry");

            Order order = new Order
            {
                Name = model.Name,
                Aderss = model.Aderss,
                Email = model.Email,
                Mobile = model.Mobile,
                IsonlineParid = model.IsonlineParid,
                UserId = User.Identity.Name,
                Country = selectedCountry
            };

            var Cartitems = db.Carts.Where(x => x.UserId == User.Identity.Name && x.Country == selectedCountry).ToList();
            foreach (var item in Cartitems)
            {
                var total = item.Qty * item.Price;
                order.AddOrdersDetails.Add(new AddOrdersDetail
                {
                    Qty = item.Qty,
                    Price = item.Price,
                    Productid = item.ProductId,
                    Totalprice = total
                });
            }

            db.Carts.RemoveRange(Cartitems);
            db.Orders.Add(order);
            db.SaveChanges();

            return RedirectToAction("Index", new { country = selectedCountry });
        }

        [Authorize]
        public IActionResult Orders()
        {
            // جلب الدولة المختارة من الـ Session
            var selectedCountry = HttpContext.Session.GetString("selectedCountry");

            if (string.IsNullOrEmpty(selectedCountry))
            {
                // لو المستخدم ما اختارش دولة، رجعه لصفحة اختيار الدولة
                return RedirectToAction("Index");
            }

            // جلب الطلبات الخاصة بالمستخدم والدولة المختارة
            var ordersCountries = db.Orders
                .Include(x => x.AddOrdersDetails)
                .ThenInclude(x => x.Product)
                .Where(x => x.UserId == User.Identity.Name && x.Country == selectedCountry) // الفلترة بالدولة
                .ToList();

            return View(ordersCountries);
        }

        public IActionResult EndOrders()
        {
            return View();
        }

        public IActionResult SetCountry(string country)
        {
            HttpContext.Session.SetString("selectedCountry", country);
            return RedirectToAction("Orders");
        }

        public IActionResult ProductShow(int id)
        {
            // قراءة الدولة المختارة من السشن
            var selectedCountry = HttpContext.Session.GetString("selectedCountry");

            // لو السشن فاضي وهذا أول دخول للمستخدم → اجعله مصر أو الدولة الافتراضية
            if (string.IsNullOrEmpty(selectedCountry))
                selectedCountry = "مصر";

            // جلب المنتج مع الصور حسب الدولة
            var product = db.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id && p.Country == selectedCountry);

            if (product == null)
            {
                TempData["Error"] = "هذا المنتج غير متوفر في الدولة المختارة.";
                return RedirectToAction("Index", new { country = selectedCountry });
            }

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
