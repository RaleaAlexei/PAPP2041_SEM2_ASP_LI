using Boutique.Models;
using Boutique.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.Controllers
{
    [Authorize(Roles = ShopConstants.Admin)]
    public class CategorieController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategorieController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Categorie> objList = _db.Categorie;
            return View(objList);
        }
        //get-create
        public IActionResult Creeaza()
        {
            
            return View("Upsert", new Categorie());
        }

        //post-create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Creeaza(Categorie obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categorie.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //get-edit
        public IActionResult Editeaza(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Categorie.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View("Upsert", obj);
        }

        //post-edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editeaza(Categorie obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categorie.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //post-delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sterge(int? id)
        {
            var obj = _db.Categorie.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categorie.Remove(obj);
            _db.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
