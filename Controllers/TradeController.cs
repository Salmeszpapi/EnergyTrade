using Csaba.Entity;
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
            EnergyContext db = new EnergyContext();
            List<string[]> list = new List<string[]>();
            List<string[]> list2 = new List<string[]>();
            string sender = "";
            string receiver = "";
            int senderId = Convert.ToInt32(Ids[0].Split(';')[0]);
            int receiverId = Convert.ToInt32(Ids[Ids.Length-1].Split(';')[0]);
            foreach (var i in Ids)
            {
                if(senderId == Convert.ToInt32(i.Split(';')[0]))
                {
                    list.Add(i.Split(';'));
                }
                else
                {
                    list2.Add(i.Split(';'));
                }
            }
            for(int i = 0; i < list.Count; i++)
            {
                for(int j = 0; j < list[i].Length; j++)
                {
                    if(j == 0)
                    {

                    }
                    else
                    {
                        sender += list[i][j].ToString() + ";";
                    }
                    
                }
            }
            for (int i = 0; i < list2.Count; i++)
            {
                for (int j = 0; j < list2[i].Length; j++)
                {
                    
                    if (j == 0)
                    {

                    }
                    else
                    {
                        receiver += list2[i][j].ToString() + ";";
                    }
                }
            }
            Csaba.Entity.Trade trade = new Csaba.Entity.Trade()
            {
                Date = DateTime.Now,
                IsCompleted = 0,
                Receiver = receiverId,
                Sender = senderId,
                ReceiverItems = receiver,
                SenderItems = sender,
            };
            db.Trade.Add(trade);
            db.SaveChanges();
            return Json(new { Name = "Arrived!", DateTime = DateTime.Now.ToShortDateString() });
        }
        public ActionResult TradeRequests(string tradeStatus = "sended")
        {
            EnergyContext db = new EnergyContext();
            List<Product> myProducts = new List<Product>();
            List<Product> hisProducts = new List<Product>();
            List<TradeAgent> agents = new List<TradeAgent>();
            int myId = Convert.ToInt32(Session["Logged_Id"]);
            if (tradeStatus == "received")
            {
                var received = db.Trade
                    .Where(x => x.Receiver == myId)
                    .ToList();
                return View(received);
            }
            else if (tradeStatus == "sended")
            {
                var sended = db.Trade
                    .Where(x => x.Sender == myId)
                    .Where(x => x.IsCompleted == 0)
                    .ToList();
                return View(sended);

            }else if(tradeStatus == "Hystori")
            {
                var history = db.Trade.Where(x => x.Receiver == myId || x.Sender == myId).ToList();
                return View(history);
            }
            else
            {
                Session["Error"] = "Internal error";
            }
            
            return View();
        }
        public ActionResult CompleteTrade(int tradeId, int status)
        {
            EnergyContext db = new EnergyContext();
            var tradeRow = db.Trade.Where(x => x.Id == tradeId).FirstOrDefault();
            var receiverItems = tradeRow.ReceiverItems.Split(';');
            var senderItems = tradeRow.SenderItems.Split(';');
            int counter = 0;
            foreach(var item in receiverItems)
            {
                var stockItems = db.StockItems
                .Include("Product")
                .Include("Stock")
                .Where(x => x.Product.Id == Convert.ToInt32(item)).FirstOrDefault();
                stockItems.Product.Id = 5;
                //db.StockItems.Remove();
            }
            foreach (var item in senderItems)
            {

            }
            

            return RedirectToAction("TradeRequests", "trade");
        }
    }
}