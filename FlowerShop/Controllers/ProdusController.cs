using Boutique.Models.ViewModels;
using Boutique.Models;
using Boutique.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Boutique.Controllers
{
    [Authorize(Roles = ShopConstants.Admin)]
    public class ProdusController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProdusController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Produs> objList = _db.Produs;
            foreach (var obj in objList)
            {
                obj.Categorie = _db.Categorie.FirstOrDefault(u => u.Id == obj.CategorieId);
            };
            return View(objList);
        }
        //get-upsert
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProdusViewModel productViewModel = new()
            {
                Produs = new Produs(),
                CategoriaListaSelectabila = _db.Categorie.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
            };


            if (id == null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Produs = _db.Produs.Find(id);
                if (productViewModel.Produs == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
        }

        //post-upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProdusViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = webHostEnvironment.WebRootPath;

                if (productViewModel.Produs.Id == -1)
                {
                    string upload = webRootPath + ShopConstants.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extention = Path.GetExtension(files[0].FileName);


                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productViewModel.Produs.Image = fileName + extention;

                    _db.Produs.Add(productViewModel.Produs);
                }
                else
                {
                    var objFromBb = _db.Produs.AsNoTracking().FirstOrDefault(u => u.Id == productViewModel.Produs.Id);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + ShopConstants.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extention = Path.GetExtension(files[0].FileName);
                        var oldFile = Path.Combine(upload, objFromBb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productViewModel.Produs.Image = fileName + extention;

                    }
                    else
                    {
                        productViewModel.Produs.Image = objFromBb.Image;
                    }
                    _db.Produs.Update(productViewModel.Produs);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productViewModel.CategoriaListaSelectabila = _db.Categorie.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            return View(productViewModel);
        }
        //post-delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sterge(int? id)
        {
            Produs? obj = _db.Produs.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            string upload = webHostEnvironment.WebRootPath + ShopConstants.ImagePath;


            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Produs.Remove(obj);
            _db.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
