using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Role_Again.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Role_Again.Controllers
{
    public class ManageAccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        // GET: users
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                    ViewBag.Admin = (from a in db.Users select a).ToList();
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();
        }

        public ActionResult status(string id)
        {
            var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var userid = usermanager.FindById(id).Id;
            if (usermanager.GetLockoutEnabled(userid) == true)
            {
                usermanager.SetLockoutEnabled(userid, false);
            }
            else
            {
                usermanager.SetLockoutEnabled(userid, true);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var find = (from f in db.Users where f.Id == id select f).ToList();
            ViewBag.delete = find;

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var deleted = db.Users.Find(id);
            db.Users.Remove(deleted);
            db.SaveChanges();
            TempData["delete"] = "Data has been Deleted";

            return RedirectToAction("Index");
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}