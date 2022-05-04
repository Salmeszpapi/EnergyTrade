using EnergyTrade.Models;
using Microsoft.AspNetCore.Mvc;
using SSM.Common.Services.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EnergyTrade.Controllers
{
    
    public class TradeController : Controller
    {
        // GET: Trade
        public ActionResult Index(int Id)
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext();
            List<ProductWithUser> products = new List<ProductWithUser>();
            List<ProductWithUser> productstheir = new List<ProductWithUser>();
            int userId = (int)Session["logged_Id"];
            var stockItems = db.StockItems
                .Include("Stock")
                .Include("Product")
                .Where(x => x.Stock.Id == userId)
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
                // load the choosen items 
            }
            if (Id != userId)
            {
                var stockItemstheir = db.StockItems
               .Include("Stock")
               .Include("Product")
               .Where(x => x.Stock.Id == Id)
               .ToList();
                var products1their = db.Products
                    .Include("Brand")
                    .ToList();
                foreach (var si in stockItemstheir)
                {
                    var base64 = Convert.ToBase64String(si.Product.Image);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                    var brand = products1.Where(x => x.Id == si.Product.Id).FirstOrDefault();

                        ProductWithUser productwithuserofther = new ProductWithUser()
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
                        productstheir.Add(productwithuserofther);

                }
                ViewBag.theirProducts = productstheir;

            }
            return View(products);

        }
        [HttpPost]
        public JsonResult TradeAgent(JsonResult data)
        {

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoToConfirm(string[] Ids)
        {
            return Json(new { Name = "ye", DateTime = DateTime.Now.ToShortDateString() });
        }
    }
}