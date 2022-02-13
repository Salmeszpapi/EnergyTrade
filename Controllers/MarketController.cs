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
            int UserId = Convert.ToInt32(Session["logged_Id"]);
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
                };
                products.Add(productWithUser);
                //save object 
            }
            
            return View(products);
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
        [HttpGet]
        public ActionResult UserProfile(int id)
        {
            EnergyContext db = new EnergyContext();
            bool myprofile;
            myprofile = (Convert.ToInt32(Session["logged_Id"]) == id) ? true : false; 
            var person = db.Users.Where(x => x.Id == id).ToList().LastOrDefault();
            var stock = db.Stocks.Where(x => x.User.Id == id).ToList().FirstOrDefault();
            var StockItems = db.StockItems.Where(x => x.Stock.Id == stock.Id).ToList();

            Profile neprofile = new Profile()
            {
                Name = person.Name,
                DataJoined = person.DateJoined,
                LastLoginDate = person.LastLoginDate,
                OwnProfile=false,
            };
            return View(neprofile);
        }

        public ActionResult Add() 
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Add(string Brand, string Name, string Size, string Coffein, string Sugar, HttpPostedFileBase file)
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext(); //set db
            StockItem newStockitem = new StockItem(); //new stockitem
            byte[] byteImg = ConverToBytes(file);

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            if (path != null )
            {
                
                file.SaveAs(path);
                var brandid = db.Brands.Where(x => x.Name == Brand).ToList().FirstOrDefault();
                Product newProduct = new Product();
                if (brandid == null)
                {
                    Brand newBrand = new Brand()
                    {
                        Name = Brand,
                    };  // if needed new brand
                    newProduct.Name = Name;
                    newProduct.Brand = newBrand;
                    newProduct.Size = Convert.ToInt32(Size);
                    newProduct.Coffein = Convert.ToInt32(Coffein);
                    newProduct.Sugar = Convert.ToInt32(Sugar);
                    newProduct.Image = byteImg;
                } else
                {
                    newProduct.Name = Name;
                    newProduct.Brand = brandid;
                    newProduct.Size = Convert.ToInt32(Size);
                    newProduct.Coffein = Convert.ToInt32(Coffein);
                    newProduct.Sugar = Convert.ToInt32(Sugar);
                    newProduct.Image = byteImg;
                }
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

            } else
            {
                // img = null
            }
            
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
            List<StockItem> MyProducts = new List<StockItem>();
            //var brandid = db.Products.Where(x =>x.Brand.Id == 24).ToList().FirstOrDefault();
            //string imreBase64Data = Convert.ToBase64String(brandid.Image);
            //string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            //ViewBag.ImageData = imgDataURL;
            EnergyContext db = new EnergyContext();
            var username = Convert.ToString(Session["logged_in"]);
            var user = db.Users.Where(x => x.Name == username).ToList().FirstOrDefault();
            var stock = db.Stocks.Where(x => x.User.Id == user.Id).ToList().FirstOrDefault();
            var StockItems = db.StockItems.Where(x => x.Stock.Id == stock.Id).ToList();
            //var result = StockItems[0]
            List<int> ItemIds = new List<int>();
            //List<Product> ExistingProducts = new List<Product>();
            foreach(var item in StockItems)
            {
                var Product = db.StockItems.Find(item.Id);
                db.Entry(Product)
                    .Reference(u => u.Product)
                    .Load();

                MyProducts.Add(Product);
            }
            
            //here need to save products into a list 
            var ExistingProducts = db.Products.Where(x => x.Id == 11).ToList();
            //var myproducts = db.Products.Where(x => x.Id == StockItems[0].Product.Id).ToList();


            return View(MyProducts);
        }
        public byte[] ConverToBytes(HttpPostedFileBase file)
        {
            var length = file.InputStream.Length; //Length: 103050706
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }
    }
}
