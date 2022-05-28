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
            if (TempData["product"] != null)
            {
                ViewBag.Brand = db.Brands
                    .GroupBy(i => i.Name)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key).ToList();
                //save object 

                ViewBag.Size = db.Products
                    .GroupBy(i => i.Size)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key).ToList();
                //save object 

                var asd = TempData["product"];
                return View(asd);
            }
            
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
                    Rating = si.Product.Raiting

                };
                products.Add(productWithUser);

                
            }
            ViewBag.Brand = db.Brands
                    .GroupBy(i => i.Name)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key).ToList();
            //save object 
            ViewBag.Size = db.Products
                    .GroupBy(i => i.Size)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key).ToList();
            //save object 

            return View(products);
        }
        public ActionResult SearchResoult(string Brands, string Size, string Sugar)
        {
            EnergyContext db = new EnergyContext();
            List<Product> products = new List<Product>();
            List<Product> selectedProducts = new List<Product>();
            List<ProductWithUser> products12 = new List<ProductWithUser>();

            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .ToList();
            var products1 = db.Products
                .Include("Brand")
                .ToList();
            foreach (var si in stockItems)
            {
                var brand = products1.Where(x => x.Id == si.Product.Id).FirstOrDefault();
                var base64 = Convert.ToBase64String(si.Product.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
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
                    Rating = si.Product.Raiting

                };
                products12.Add(productWithUser);

                //save object 
            }


            if (Brands == "allBrands")
            {
                //return all items 
            }
            else
            {
                products12 = products12.Where(x => x.Brand.Name == Brands).ToList();
            }
            if(Size == "allSizes")
            {
                //return all items
            }
            else
            {
                int SizeInt = Convert.ToInt32(Size);
                products12 = products12.Where(x => x.Size == SizeInt).ToList();
            }
            if(Sugar == "allSugar")
            {

            }
            else
            {
                //products12 = products12.Where(x => x.Sugar == Sugar).ToList();
            }
            TempData["product"] = products12;
            
            return RedirectToAction("Index", "Market");
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
        public ActionResult MyProfile()
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext();
            List<ProductWithUser> products = new List<ProductWithUser>();
            ///////////////////////////////////////// get user products
            int id = Convert.ToInt32(Session["logged_Id"]);
            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .Where(x => x.Stock.Id == id)
                .ToList();
            var products1 = db.Products
                .Include("Brand")
                .ToList();
            foreach (var si in stockItems)
            {
                var base64 = Convert.ToBase64String(si.Product.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                var brand = products1.Where(x => x.Id == si.Product.Id).FirstOrDefault();
                var usr = db.Users.Where(x=>x.Id == id).FirstOrDefault();


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
                    AboutMe = usr.AboutMe,
                    


                };

                products.Add(productWithUser);
            }
            ViewBag.Products = products;
            //////////////////////////////////////// user account properties
            bool myprofile;
            myprofile = (Convert.ToInt32(Session["logged_Id"]) == id) ? true : false;
            try
            {
                var person = db.Users.Where(x => x.Id == id).ToList().LastOrDefault();
                Profile neprofile = new Profile()
                {
                    Name = person.Name,
                    DataJoined = person.DateJoined,
                    LastLoginDate = person.LastLoginDate,
                    OwnProfile = myprofile,
                    Image = person.Image,

                };
                return View(neprofile);
            }
            catch (Exception e)
            {
                return Content("Profile is not found please relog to the web application" + e);
            }
        }
        [HttpGet]
        public ActionResult UserProfile(int id)
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext();
            List<ProductWithUser> products = new List<ProductWithUser>();
            ///////////////////////////////////////// get user products
            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .Where(x => x.Stock.Id == id)
                .ToList();
            var products1 = db.Products
                .Include("Brand")
                .ToList();
            foreach (var si in stockItems)
            {
                var base64 = Convert.ToBase64String(si.Product.Image);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                var brand = products1.Where(x => x.Id == si.Product.Id).FirstOrDefault();
                var usr = db.Users.Where(x => x.Id == id).FirstOrDefault();

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
                    AboutMe = usr.AboutMe

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
                    OwnProfile = myprofile,
                    Image = person.Image,
                    
                };
                return View(neprofile);
            }
            catch(Exception e)
            {
                return Content("Profile is not found please relog to the web application" + e);
            }
        }


        [HttpPost]
        public ActionResult Add(string Brand, string Name, string Size, string Coffein, string Sugar,string Price, HttpPostedFileBase ImageFile)
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
                newProduct.Sugar = Sugar;
                newProduct.Image = byteImg;
                newProduct.Price = Convert.ToDouble(Price);

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
            int userId = (int)Session["logged_Id"];
            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .Where(x =>x.Stock.Id == userId)
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
                    Rating = si.Product.Raiting

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

            var user = db.StockItems
                .Include("Product")
                .Include("Stock")
                .Where(x => x.Product.Id == Id).FirstOrDefault();



            ViewBag.ImgSrc = imgSrc;
            ViewBag.User = user.Stock.Id;
            productInfo.Seen++;
            if(productInfo.Seen < 10)
            {
                productInfo.Raiting = 0;
            }
            switch(productInfo.Seen)
            {
                case 20:
                    productInfo.Raiting = 1;
                    break;
                case 40:
                    productInfo.Raiting = 2;
                    break;
                case 60:
                    productInfo.Raiting = 3;
                    break;
                case 80:
                    productInfo.Raiting = 4;
                    break;
                case 100:
                    productInfo.Raiting = 5;
                    break;
            }
            db.SaveChanges();
            return View(productInfo);
        }
        public ActionResult EditProfile()
        {
            return View();
        }
    }
}
