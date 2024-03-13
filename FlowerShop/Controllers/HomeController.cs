using Boutique.Models;
using Boutique.Models.ViewModels;
using Boutique.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Boutique.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                Produse = _db.Produs.Include(u => u.Categorie),
                Categorii = _db.Categorie,
            };

            return View(homeVM);
        }
        public IActionResult Detalii(int id)
        {
            List<Cos> shopingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).Count() > 0)
            {
                shopingCartList = HttpContext.Session.Get<List<Cos>>(ShopConstants.SessionCart);
            }
            DetaliiViewModel detailsVM = new DetaliiViewModel()
            {
                Produs = _db.Produs.Include(u => u.Categorie).Where(u => u.Id == id).FirstOrDefault(),
                ExistaInCos = false
            };
            foreach (var item in shopingCartList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistaInCos = true;
                }
            }
            return View(detailsVM);
        }
        [HttpPost, ActionName("Detalii")]
        public IActionResult DetaliiPost(int id)
        {
            List<Cos> shopingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).Count() > 0)
            {
                shopingCartList = HttpContext.Session.Get<List<Cos>>(ShopConstants.SessionCart);
            }
            shopingCartList.Add(new Cos { ProductId = id });
            HttpContext.Session.Set(ShopConstants.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            List<Cos> shopingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).Count() > 0)
            {
                shopingCartList = HttpContext.Session.Get<List<Cos>>(ShopConstants.SessionCart);
            }

            var itemToRemove = shopingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemToRemove != null)
            {
                shopingCartList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(ShopConstants.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
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
