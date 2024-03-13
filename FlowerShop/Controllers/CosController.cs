using Boutique.Models;
using Boutique.Models.ViewModels;
using Boutique.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Boutique.Controllers
{
    [Authorize]
    public class CosController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public ProdusUtilizatorViewModel productUserViewModel { get; set; }
        public CosController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Cos> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Produs> productsList = _db.Produs.Where(u => prodInCart.Contains(u.Id));
            return View(productsList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Sumar));
        }

        public IActionResult Sumar()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Cos> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Produs> productsList = _db.Produs.Where(u => prodInCart.Contains(u.Id));

            productUserViewModel = new()
            {
                Utilizator = _db.Utilizator.FirstOrDefault(u => u.Id == claim.Value),
                ListaProduse = productsList.ToList()

            };

            return View(productUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Sumar")]
        public async Task<IActionResult> SumarPost(ProdusUtilizatorViewModel productUserViewModel)
        {
            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                  + "template" + Path.DirectorySeparatorChar.ToString() +
                  "Inquiry.html";
            var subject = "New";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }
            StringBuilder productListSB = new StringBuilder();
            foreach (var prod in productUserViewModel.ListaProduse)
            {
                productListSB.Append($" - Name: {prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(HtmlBody,
                productUserViewModel.Utilizator.FullName,
                productUserViewModel.Utilizator.Email,
                productUserViewModel.Utilizator.PhoneNumber,
                productListSB.ToString());
            return RedirectToAction(nameof(ConfirmaTranzactia));
        }
        public IActionResult ConfirmaTranzactia()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Sterge(int id)
        {
            List<Cos> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<Cos>>(ShopConstants.SessionCart).ToList();
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(ShopConstants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
