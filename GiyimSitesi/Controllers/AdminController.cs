using GiyimSitesi.Context;
using GiyimSitesi.Context.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace GiyimSitesi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Index()
	{
            using (var db = new Baglanti())
            {
                ViewBag.Urunler=db.Products.ToList();
                ViewBag.Kategoriler=db.Categories.ToList();
                ViewBag.Iletisim = db.Contacts.ToList();

            }
            return View();
        }
        public IActionResult CategoryList()
        {
            using (var db = new Baglanti())
            {
                var kategori = db.Categories.ToList();
                return View(kategori);
            }

        }
        public IActionResult ProductList()
        {
            using (var db = new Baglanti())
            {
                var urun = db.Products.Include(i => i.Category).ToList();
                return View(urun);
            }
        }




        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category model, IFormFile file)
        {
            using (var db = new Baglanti())
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var kategori = new Category()
                    {
                        Name = model.Name,
                        ImgUrl = fileName,
                    };
                    db.Categories.Add(kategori);
                    db.SaveChanges();

                    // SweetAlert success notification
                    TempData["CategoryAdd"] = "Kategoriniz eklenmiştir.";
                }
                else
                {
                    var kategori = new Category()
                    {
                        Name = model.Name,
                        ImgUrl = "men.jpg",
                    };
                    db.Categories.Add(kategori);
                    db.SaveChanges();

                    // SweetAlert success notification
                    TempData["CategoryAdd"] = "Kategoriniz eklenmiştir.";
                }

                return RedirectToAction("CategoryList");
            }
        }

        public IActionResult EditCategory(int id)
        {
            using (var db = new Baglanti())
            {
                var kategori = db.Categories.Where(i => i.Id == id).FirstOrDefault(); ;
                return View(kategori);
            }
        }
        [HttpPost]
        public IActionResult EditCategory(Category model, IFormFile file)
        {
            using (var db = new Baglanti())
            {
                var kategori = db.Categories.Where(i => i.Id == model.Id).FirstOrDefault();
                kategori.Name = model.Name;

                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var eskiDosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", kategori.ImgUrl);
                    if (System.IO.File.Exists(eskiDosyaYolu))
                    {
                        System.IO.File.Delete(eskiDosyaYolu);
                    }
                    kategori.ImgUrl = fileName;
                    db.Categories.Update(kategori);
                    db.SaveChanges();
                    TempData["CategoryEdit"] = "Kategeoriniz başarılı bir şekilde güncellenmiştir.";

                }
                else
                {

                    db.Categories.Update(kategori);
                    db.SaveChanges();
                    TempData["CategoryEdit"] = "Kategeoriniz başarılı bir şekilde güncellenmiştir.";

                }

                return RedirectToAction("CategoryList");

            }
        }
        
        public IActionResult RemoveCategory(int Id)
        {
            using (var db = new Baglanti())
            {
                var kategori = db.Categories.Where(i => i.Id == Id).FirstOrDefault();
                var urun = db.Products.Where(i => i.CategoryId == kategori.Id).ToList();
                if (urun != null && urun.Count==0)
                {
                    db.Categories.Remove(kategori);
                    db.SaveChanges();
                    TempData["CategoryRemove"] = "Kategoriniz başarılı bir şekilde silinmiştir.";

                }
                else
                {
                    TempData["ErrorMessage1"] = "Bu kategoride ürün bulunmaktadır. Kategoriyi silemezsiniz.";
                }
                return RedirectToAction("CategoryList");


            }
        }




        public IActionResult AddProduct()
        {
            using (var db = new Baglanti())
            {
                var kategoriler = db.Categories.ToList();
                ViewBag.Kategoriler = kategoriler;


                return View();
            }
        }
        [HttpPost]
        public IActionResult AddProduct(Product model, IFormFile file)
        {
            using (var db = new Baglanti())
            {

                model.Created_at = DateTime.Now;

                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    model.ImgUrl = fileName;
                    TempData["ProductAdd"] = "Ürününüz başarılı bir şekilde eklenmiştir.";

                }
                else
                {
                    model.ImgUrl = "men.jpg";
                    TempData["ProductAdd"] = "Ürününüz başarılı bir şekilde eklenmiştir.";

                }

                db.Products.Add(model);
                db.SaveChanges();

                return RedirectToAction("ProductList");
            }
        }
        public IActionResult EditProduct(int id)
        {

            using (var db = new Baglanti())
            {
                var kategoriler = db.Categories.ToList();
                ViewBag.urunler = kategoriler;

                var urunler = db.Products.Where(i => i.Id == id).FirstOrDefault(); ;
                return View(urunler);
            }
        }
        [HttpPost]
        public IActionResult EditProduct(Product model, IFormFile file)
        {
            using (var db = new Baglanti())
            {
                var urunler = db.Products.Where(i => i.Id == model.Id).FirstOrDefault();

                urunler.Name = model.Name;
                urunler.Description = model.Description;
                urunler.Price = model.Price;
                urunler.CategoryId = model.CategoryId;
                urunler.Created_at = DateTime.Now;

                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Eski dosyayı silme (opsiyonel, ihtiyaca göre)
                    var eskiDosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", urunler.ImgUrl);
                    if (System.IO.File.Exists(eskiDosyaYolu))
                    {
                        System.IO.File.Delete(eskiDosyaYolu);
                    }

                    urunler.ImgUrl = fileName;
                    db.Products.Update(urunler);
                    db.SaveChanges();
                    TempData["ProductEdit"] = "Ürününüz güncellenmiştir.";

                }
                else
                {
                    db.Products.Update(urunler);
                    db.SaveChanges();
                    TempData["ProductEdit"] = "Ürününüz güncellenmiştir.";

                }
                return RedirectToAction("ProductList");


            }
        }
        public IActionResult RemoveProduct(int Id)
        {
            using (var db = new Baglanti())
            {
                var urunler = db.Products.Where(i => i.Id == Id).FirstOrDefault();
                if (urunler != null)
                {
                    db.Products.Remove(urunler);
                    db.SaveChanges();
                    TempData["ProductRemove"] = "Ürününüz başarılı bir şekilde silinmiştir.";
                    return RedirectToAction("ProductList");
                }
                else
                {
                    TempData["ErrorMesssage2"] = "Bu kategoride ürün bulunmaktadır. Kategoriyi silemezsiniz.";

                }
                return RedirectToAction("ProductList");

            }
        }
    }
}

