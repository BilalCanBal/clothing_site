using GiyimSitesi.Context;
using GiyimSitesi.Context.Entity;
using GiyimSitesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace GiyimSitesi.Controllers
{
    public class User : Controller
    {
        public IActionResult Index()
        {
            using (var db = new Baglanti())
            {
                var ozelUrunler = db.Products.ToList();

                Random random = new Random();
                List<Product> rastgeleVeriler = ozelUrunler.OrderBy(x => random.Next()).Take(6).ToList();
                ViewBag.OzelUrunler = rastgeleVeriler;



                ViewBag.Kategoriler = db.Categories.ToList();
                return View();

            }
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Contact model)
        {
            using (var db = new Baglanti())
            {
                var iletisim = new Contact()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Description = model.Description
                };
                db.Contacts.Add(iletisim);
                int affectedRows = db.SaveChanges();
                if (affectedRows > 0)
                {
                    TempData["Message"] = "Mesajınız başarılı bir şekilde iletilmiştir.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Mesaj Gönderilemedi!";
                }
                return RedirectToAction("Contact");

            }
        }
        public IActionResult Shop(int page = 1)
        {
            using (var db = new Baglanti())
            {
                ViewBag.Kategoriler = db.Categories.ToList();


                int pageSize = 15; // Sayfa başına öğe sayısı
                int skipCount = (page - 1) * pageSize; // Atlanacak öğe sayısı

                var products = db.Products.OrderBy(p => p.Id).Skip(skipCount).Take(pageSize).ToList();

                int totalProducts = db.Products.Count();
                int pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                var viewModel = new ProductViewModel
                {
                    Products = products,
                    PageNumber = page,
                    PageCount = pageCount
                };

                return View(viewModel);

            }

        }
        public IActionResult FilterProducts(ProductViewModel model, int page = 1)
        {
            using (var db = new Baglanti())
            {
                ViewBag.Kategoriler = db.Categories.ToList();

                int pageSize = 15; // Sayfa başına öğe sayısı
                int skipCount = (page - 1) * pageSize; // Atlanacak öğse sayısı
                var filteredProducts = db.Products.ToList();
                var pageCount = 0;
                var products = new List<Product>();
                if (model.categoryId == null || model.categoryId == 0)
                {
                    if (model.minPrice == null || model.minPrice == 0)
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            products = filteredProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filteredProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p =>
                        p.Price <= model.maxPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                    else
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            var filterProducts = db.Products.Where(p =>
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);
                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p =>
                        p.Price <= model.maxPrice &&
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                }
                else
                {

                    if (model.minPrice == null || model.minPrice == 0)
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            var filterProducts = db.Products.Where(p => p.CategoryId == model.categoryId).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p =>
                        p.CategoryId == model.categoryId &&
                        p.Price <= model.maxPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                    else
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            var filterProducts = db.Products.Where(p =>
                        p.CategoryId == model.categoryId &&
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);
                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p =>
                        p.CategoryId == model.categoryId &&
                        p.Price <= model.maxPrice &&
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                }


                var viewModel = new ProductViewModel
                {
                    Products = products,
                    PageNumber = page,
                    PageCount = pageCount,
                    categoryId = model.categoryId,
                    minPrice = model.minPrice,
                    maxPrice = model.maxPrice

                };



                return View(viewModel);
            }
        }
        public IActionResult Search(string searchTerm, ProductViewModel model, int page = 1)
        {
            using (var db = new Baglanti())
            {
                ViewBag.Kategoriler = db.Categories.ToList();

                int pageSize = 10; // Sayfa başına öğe sayısı
                int skipCount = (page - 1) * pageSize; // Atlanacak öğe sayısı

                var filteredProducts = db.Products.ToList();
                var pageCount = 0;
                var products = new List<Product>();

                if (model.categoryId == null || model.categoryId == 0)
                {
                    if (model.minPrice == null || model.minPrice == 0)
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            var filterProducts = filteredProducts.Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower()))).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower())) &&
                        p.Price <= model.maxPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                    else
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            var filterProducts = db.Products.Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower())) &&
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);
                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower())) &&
                        p.Price <= model.maxPrice &&
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                }
                else
                {

                    if (model.minPrice == null || model.minPrice == 0)
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            var filterProducts = db.Products.Where(p => p.CategoryId == model.categoryId && (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower()))).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower())) &&
                        p.CategoryId == model.categoryId &&
                        p.Price <= model.maxPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                    else
                    {
                        if (model.maxPrice == null || model.maxPrice == 0)
                        {
                            var filterProducts = db.Products.Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower())) &&
                        p.CategoryId == model.categoryId &&
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);
                        }
                        else
                        {
                            var filterProducts = db.Products.Where(p => (p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Category.Name.ToLower().Contains(searchTerm.ToLower())) &&
                        p.CategoryId == model.categoryId &&
                        p.Price <= model.maxPrice &&
                        p.Price >= model.minPrice).ToList();
                            products = filterProducts.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                            int totalProducts = filterProducts.Count();
                            pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                        }
                    }
                }




                //var products = Searchproducts.OrderBy(p => p.Id).Skip(skipCount).Take(pageSize).ToList();

                //int totalProducts = Searchproducts.Count();
                //int pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);

                var viewModel = new ProductViewModel
                {
                    Products = products,
                    PageNumber = page,
                    PageCount = pageCount,
                    categoryId = model.categoryId,
                    minPrice = model.minPrice,
                    maxPrice = model.maxPrice,
                    searchTerm = searchTerm
                };
                return View(viewModel);

            }

        }
        public IActionResult Categories(int collectionType, int page = 1)
        {
            using (var db = new Baglanti())
            {
                ViewBag.Kategoriler = db.Categories.ToList();

                var result = db.Products.Where(x => x.CategoryId == collectionType).ToList();
                int pageSize = 10;
                int skipCount = (page - 1) * pageSize;

                var products = result.OrderBy(p => p.Price).Skip(skipCount).Take(pageSize).ToList();

                int totalProducts = result.Count();
                int pageCount = (int)Math.Ceiling((double)totalProducts / pageSize);
                var viewModel = new ProductViewModel
                {
                    Products = products,
                    PageNumber = page,
                    PageCount = pageCount,
                    categoryId = collectionType
                };

                return View(viewModel);
            }
        }
        public IActionResult DetailsProduct(int id)
        {
            using (var db = new Baglanti())
            {
                var ozelUrunler = db.Products.ToList();

                var product = db.Products.Where(i => i.Id == id).FirstOrDefault();

                Random random = new Random();

                List<Product> benzerUrunler1 = ozelUrunler
        .Where(predicate: p => p.Id != id && (p.Name.ToLower().Contains(product.Name.ToLower())))
        .OrderBy(x => random.Next())
        .Take(8)
        .ToList();

                if (benzerUrunler1.Count() == 0 || benzerUrunler1 == null)
                {
                    List<Product> benzerUrunler2 = ozelUrunler.Where(predicate: p => p.Id != id || p.CategoryId==product.CategoryId || (p.Name.ToLower().Contains(product.Name.ToLower()))).OrderBy(x => random.Next()).Take(8).ToList();
                    ViewBag.BenzerUrunler = benzerUrunler2;
                }
                else
                {
                    ViewBag.BenzerUrunler = benzerUrunler1;
                }

                return View(product);
            }

        }





    }
}
