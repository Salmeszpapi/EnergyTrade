using Csaba.Entity;
using SSM.Common.Services.DataContext;
using System;
using System.Collections.Generic;
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
            return View();
        }
        public ActionResult Add() 
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string Brand, string Name, string Size, string Coffein, string Sugar)
        {
            EnergyContext db = new EnergyContext();
            if (!string.IsNullOrEmpty(Name))
            {
                
            }
            var a = db.Users.Where(x => x.Name == Name).ToList();
            var username = Convert.ToString(Session["logged_in"]);
            //newStock.User.Name = username;
            var userid = db.Users.Where(x => x.Name == username).ToList();
            var foundStock = db.Stocks.Where(x => x.User.Id == userid[0].Id).ToList();
            Brand newBrand = new Brand();
            newBrand.Name = Brand;
            Product newProduct = new Product()
            {
                Brand = newBrand,
                Name = Name,
                Size = Convert.ToInt32(Size),
                Coffein = Convert.ToInt32(Coffein),
                sugar = Convert.ToInt32(Sugar),

            };

            
            StockItem newStockitem = new StockItem();
            newStockitem.Product = newProduct;
            newStockitem.Count = 1;
            newStockitem.Stock.Id =Convert.ToInt32(foundStock[0].Id);

            
            
            db.Products.Add(newProduct);
            db.StockItems.Add(newStockitem);
            
            db.SaveChanges();
            return Content(userid[0].Id.ToString());

        }
    }
}
