using Csaba.Entity;
using SSM.Common.Services.DataContext;
using SSM.Common.Services.Security;
using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnergyTrade.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            if (!string.IsNullOrEmpty((string)Session["logged_in"])) {
                return RedirectToAction("Index", "Market");
            }
            //Session["logged_in"] = "k";
            //Session["Logged_Id"] = 1;
            //return RedirectToAction("Add", "Market");
            return View("login");

            //return RedirectToAction("Index", "Market");
        }
        public ActionResult Register() {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string Name, string password, string password2) {
            EnergyContext db = new EnergyContext();
            string hashedPassword = HashService.HashData(password);
            ViewData["Exist"] = null;
            if (!string.IsNullOrEmpty(Name)) {
                var a = db.Users.Where(x => x.Name == Name).ToList();
                if (a.Any()) {
                    //Existing user name
                    ViewData["Exist"] = Name;
                    return View("register");
                } else {
                    DateTime localDate = DateTime.Now;

                    User newUser = new User();
                    Stock newStock = new Stock();
                    newUser.Name = Name;
                    newUser.Password = hashedPassword;
                    newUser.LastLoginDate = localDate;
                    newUser.DateJoined = localDate;
                    db.Users.Add(newUser);
                    newStock.User = newUser;
                    db.Stocks.Add(newStock);
                    

                    db.SaveChanges();
                    Session["logged_in"] = Name;
                    Session["Logged_Id"] = newUser.Id;
                    return RedirectToAction("Index", "Market");

                }
            }
            return View();
        }
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Name, string password, string password2) 
        {
            
            EnergyContext db = new EnergyContext();
            ViewData["Exist"] = null;
            if (!string.IsNullOrEmpty((string)Session["logged_in"])) 
            {
                RedirectToAction("Index", "Market");
            }
            if (!string.IsNullOrEmpty(Name)) 
            {
                string hashedPassword = HashService.HashData(password);
                var a = db.Users.Where(x => x.Name == Name && x.Password == hashedPassword).ToList().FirstOrDefault();
                if (a != null) 
                {
                    DateTime localDate = DateTime.Now;
                    Session["logged_in"] = Name;
                    Session["logged_Id"] = a.Id;
                    Response.Write(Session["logged_in"]);
                    User result = (from p in db.Users
                                   where p.Id == a.Id
                                   select p).SingleOrDefault();

                    result.LastLoginDate = localDate;

                    db.SaveChanges();
                    return RedirectToAction("Index", "Market");
                } else 
                {
                    
                    ViewData["NotFound"] = "Username or password is incorrect";
                    return View("Login");
                }
            }
            return View("login");
        }
        public ActionResult Proba() 
        {
            return View();
        }
        public ActionResult Settings()
        {
            return View();
        }
    }
        
}