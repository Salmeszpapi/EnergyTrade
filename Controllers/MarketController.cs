using Csaba.Entity;
using EnergyTrade.Models;
using SSM.Common.Services.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using EnergyTrade.Services;

namespace EnergyTrade.Controllers
{
    public class MarketController : Controller
    {
        // GET: Market
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext();
            List<ProductWithUser> products = new List<ProductWithUser>();
            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .ToList();
            var products1 = db.Products
                .Include("Brand")
                .ToList();
            foreach (var si in stockItems)
            {
                var base64 = Convert.ToBase64String(si.Product.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                var brand = products1.Where(x => x.Id == si.Product.Id).FirstOrDefault();


                ProductWithUser productWithUser = new ProductWithUser()
                {
                    Brand = brand.Brand,
                    Coffein = si.Product.Coffein,
                    Image = imgSrc,
                    Name = si.Product.Name,
                    Size = si.Product.Size,
                    Sugar = si.Product.Size,
                    UserID = si.Stock.Id, // ide meg hozza tenni a User ID-t,  akie a product 
                    ProductID = si.Product.Id,

                };
                products.Add(productWithUser);

                //save object 
            }
            return View(products);
        }
        public ActionResult SearchResoult(string Brands, string Size, string Sugar)
        {
            EnergyContext db = new EnergyContext();
            List<Product> products = new List<Product>();

            int sugarOption = Sugar == "Withoutsugar" ? 0 : 1;
            string request= "";
            if (Size != "allSizes")
            {
                if (request.Length > 0)
                {
                    request += "&& ";
                }
                request += $"Size == {Size} ";
            }
            if (Sugar != "allSugar")
            {
                if (request.Length > 0)
                {
                    request += "&& ";
                }
                if (sugarOption == 0)
                {
                    request += $"Sugar == {sugarOption} ";
                }
                else
                {
                    request += $"Sugar > 0 ";
                }
            }
            if(request.Length == 0)
            {
                var products1 = db.Products
                    .ToList();
                return View(products1);
            }
            else
            {
                var products1 = db.Products
                    .Include("Brand")
                    .Where(request).ToList();
                return View(products1);
            }
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Item(int Id)
        {
            return Content(Id.ToString());
        }
        public ActionResult Logout()
        {
            Session["logged_in"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult UserProfile(int id)
        {

            EnergyContext db = new EnergyContext();
            List<ProductWithUser> products = new List<ProductWithUser>();
            ///////////////////////////////////////// get user products
            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .ToList();
            var products1 = db.Products
                .Include("Brand")
                .ToList();
            foreach (var si in stockItems)
            {
                var base64 = Convert.ToBase64String(si.Product.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                var brand = products1.Where(x => x.Id == si.Product.Id).FirstOrDefault();


                ProductWithUser productWithUser = new ProductWithUser()
                {
                    Brand = brand.Brand,
                    Coffein = si.Product.Coffein,
                    Image = imgSrc,
                    Name = si.Product.Name,
                    Size = si.Product.Size,
                    Sugar = si.Product.Size,
                    UserID = si.Stock.Id,
                    ProductID = si.Product.Id,

                };
                
                products.Add(productWithUser);
            }
            ViewBag.Products = products;
            //////////////////////////////////////// user account properties
            bool myprofile;
            myprofile = (Convert.ToInt32(Session["logged_Id"]) == id) ? true : false;
            try{
                var person = db.Users.Where(x => x.Id == id).ToList().LastOrDefault();
                Profile neprofile = new Profile()
                {
                    Name = person.Name,
                    DataJoined = person.DateJoined,
                    LastLoginDate = person.LastLoginDate,
                    OwnProfile = false,
                };
                return View(neprofile);
            }
            catch(Exception e)
            {
                return Content("Profile is not found please relog to the web application" + e);
            }
        }


        [HttpPost]
        public ActionResult Add(string Brand, string Name, string Size, string Coffein, string Sugar, HttpPostedFileBase ImageFile)
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ImageFile != null)
            {
                EnergyContext db = new EnergyContext(); //set db
                StockItem newStockitem = new StockItem(); //new stockitem
                var filename = Path.GetFileName(ImageFile.FileName);
                System.Drawing.Image sourceimage =
                    System.Drawing.Image.FromStream(ImageFile.InputStream);
                byte[] byteImg = MyMethods.ResizeImage(sourceimage, 540, 540);

                var brandid = db.Brands.Where(x => x.Name == Brand).ToList().FirstOrDefault();
                Product newProduct = new Product();
                if (brandid == null)
                {
                    Brand newBrand = new Brand()
                    {
                        Name = Brand,
                        Checked = false,
                        Image = byteImg,
                    };  // if needed new brand
                    newProduct.Brand = newBrand;
                    db.Brands.Add(newBrand);
                }
                else
                {
                    newProduct.Brand = brandid;
                }
                newProduct.Name = Name;
                newProduct.Size = Convert.ToInt32(Size);
                newProduct.Coffein = Convert.ToInt32(Coffein);
                newProduct.Sugar = Convert.ToInt32(Sugar);
                newProduct.Image = byteImg;

                newStockitem.Product = newProduct;
                newStockitem.Count = 1;

                var username = Convert.ToString(Session["logged_in"]);
                var userid = db.Users.Where(x => x.Name == username).ToList().FirstOrDefault();
                var foundStock = db.Stocks.Where(x => x.User.Id == userid.Id).ToList().FirstOrDefault();

                newStockitem.Stock = foundStock;
                db.Products.Add(newProduct);
                db.StockItems.Add(newStockitem);
                db.SaveChanges();
                return View();

            }
            else
            {
                // img = null
            }

            return View();

        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("UploadDocument");
        
        }
        public ActionResult MyItems()
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext();
            List<ProductWithUser> products = new List<ProductWithUser>();
            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .ToList();
            var products1 = db.Products
                .Include("Brand")
                .ToList();
            foreach (var si in stockItems)
            {
                var base64 = Convert.ToBase64String(si.Product.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                var brand = products1.Where(x => x.Id == si.Product.Id).FirstOrDefault();


                ProductWithUser productWithUser = new ProductWithUser()
                {
                    Brand = brand.Brand,
                    Coffein = si.Product.Coffein,
                    Image = imgSrc,
                    Name = si.Product.Name,
                    Size = si.Product.Size,
                    Sugar = si.Product.Size,
                    UserID = si.Stock.Id, // ide meg hozza tenni a User ID-t,  akie a product 
                    ProductID = si.Product.Id,

                };
                products.Add(productWithUser);
            }
        return View(products);
        }
        public ActionResult Kepfel(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            return View();
        }
        [HttpGet]
        public ActionResult Product(int Id)
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext();
            var productInfo = db.Products
                .Include("Brand")
                .FirstOrDefault(x => x.Id == Id);

            var base64 = Convert.ToBase64String(productInfo.Image);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

            ViewBag.ImgSrc = imgSrc;
            productInfo.Seen++;
            db.SaveChanges();
            return View(productInfo);
        }
        public ActionResult EditProfile()
        {
            return View();
        }
    }
}
