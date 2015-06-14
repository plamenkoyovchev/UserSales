namespace UserSales.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Data.Entity;

    using UserSales.Data;
    using UserSales.Models;
    using System.Web.Helpers;

    public class HomeController : Controller
    {
        private UserSalesEntities dbContext;

        public HomeController()
        {
            this.dbContext = new UserSalesEntities();
        }

        public ActionResult Index()
        {
            var model = this.dbContext.Users
                                      .Include(x => x.Sales)
                                      .Select(u => new UserViewModel()
                                      {
                                          Id = u.ID,
                                          FirstName = u.FirstName,
                                          LastName = u.LastName,
                                          Email = u.Email
                                      })
                                      .OrderBy(x => x.FirstName)
                                      .ToList();
            return View(model);
        }

        public JsonResult GetSalesVolume()
        {
            var model = this.dbContext.Users
                                      .Include(u => u.Sales)
                                      .OrderByDescending(s => s.Sales.Sum(x => x.Volume))
                                      .Select(x => new SalesViewModel()
                                      {
                                          Email = x.Email,
                                          Volume = x.Sales.Sum(y => y.Volume)
                                      })
                                      .Take(10)
                                      .ToList();

            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserSalesVolume(int id)
        {
            var userSales = this.dbContext.Users
                                          .Include(x => x.Sales)
                                          .FirstOrDefault(x => x.ID == id);

            if(userSales == null)
            {
                throw new ArgumentNullException("Invalid user!");
            }

            var model = new SalesViewModel()
            {
                Email = userSales.Email,
                Volume = userSales.Sales.Sum(x => x.Volume)
            };

            return this.Json(model, JsonRequestBehavior.AllowGet);                                 
        }

        public ActionResult InitDb()
        {
            try
            {
                this.dbContext.Initialization();
                this.dbContext.Seed();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}