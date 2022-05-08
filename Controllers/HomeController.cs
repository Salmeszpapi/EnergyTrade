using Csaba.Entity;
using EnergyTrade.Services;
using SSM.Common.Services.DataContext;
using SSM.Common.Services.Security;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
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
            if (password != password2)
            {
                ViewData["Exist"] = "The passwords doesn't match!";
                return View("register");
            }
            if (!string.IsNullOrEmpty(Name)) {
                var a = db.Users.Where(x => x.Name == Name).ToList();
                if (a.Any()) {
                    //Existing user name
                    ViewData["Exist"] = Name + "Is already taken";
                    return View("register");
                } else {
                    DateTime localDate = DateTime.Now;
                    Image image = Image.FromFile(Path.Combine(Server.MapPath("/images"), "profile.png"));
                    User newUser = new User();
                    Stock newStock = new Stock();
                    newUser.Name = Name;
                    newUser.Password = hashedPassword;
                    newUser.LastLoginDate = localDate;
                    newUser.DateJoined = localDate;
                    newUser.Image = MyMethods.ResizeImage(image, 100, 100);
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
        public ActionResult Login(string Name, string password) 
        {
            if (!string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                RedirectToAction("Index", "Market");
            }
            EnergyContext db = new EnergyContext();
            ViewData["Exist"] = null;

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

                    ViewData["Exist"] = Name + "is already taken";
                    return View("Login");
                }
            }
            return View("login");
        }
        public ActionResult Proba() 
        {
            return View();
        }
        public ActionResult Settings(string oldPassword,string newPassword1, string newPassword2, HttpPostedFileBase ImageFile)
        {
            if (string.IsNullOrEmpty((string)Session["logged_in"]))
            {
                return RedirectToAction("Login", "Home");
            }
            EnergyContext db = new EnergyContext();
            var Name = (string)Session["logged_in"];
            var a = db.Users
                    .Where(x => x.Name == Name).FirstOrDefault();
            if (oldPassword == null && newPassword1 == null && newPassword2 == null && ImageFile == null)
            {
                return View(a);
            }
            if (ImageFile != null)
            {
                var filename = Path.GetFileName(ImageFile.FileName);
                System.Drawing.Image sourceimage =
                    System.Drawing.Image.FromStream(ImageFile.InputStream);
                a.Image = MyMethods.ResizeImage(sourceimage, 100, 100);
                db.SaveChanges();
                return View(a);
            }
            if (oldPassword == null)
            {
                ViewData["error"] = "The password has not match with the old password or the new password has not match";
                return View(a);


            }
            else if (HashService.HashData(oldPassword) != a.Password)
            {
                ViewData["error"] = "The password has not match with the old password or the new password has not match";
                return View(a);
            }
            else if (newPassword1 != newPassword2)
            {
                ViewData["error"] = "The passwords has not match";
                return View(a);
            }
            else
            {
                string hashedPassword = HashService.HashData(oldPassword);

                if (a != null)
                {
                    if (ImageFile != null)
                    {
                        var filename = Path.GetFileName(ImageFile.FileName);
                        System.Drawing.Image sourceimage =
                            System.Drawing.Image.FromStream(ImageFile.InputStream);
                        a.Image = MyMethods.ResizeImage(sourceimage, 100, 100);
                    }
                    a.Password = HashService.HashData(newPassword1);
                    db.SaveChanges();

                }
                else
                {
                    ViewData["error"] = "Account not found relogin please";
                    return View(a);
                }
                return View(a);
            }
            
        }
        public ActionResult TemplateRender()
        {
            return View();
        }
    }
        
}