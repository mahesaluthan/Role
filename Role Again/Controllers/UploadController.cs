using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Role_Again.DAL;
using Role_Again.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Role_Again.Controllers
{
    public class UploadController : Controller
    {
        ApplicationUserManager _userManager;
        ApplicationDbContext context = new ApplicationDbContext();
        AttendanceDB contexts = new AttendanceDB();
        [Authorize]
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file == null)
                {
                    ModelState.AddModelError("File", "You haven't choice any file.");
                }
                else if (file.ContentLength > 0)
                {
                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }

                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        //TO:DO
                        var fileName = Path.GetFileName(file.FileName);
                        var userid = User.Identity.GetUserId();
                        var pathDB = Path.Combine(("~/Content/Upload"), fileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                        var filesize = file.ContentLength;
                        FileUpload uploads = new FileUpload { Filename = fileName, Filepath = pathDB, Userid = userid };
                        contexts.FileUploads.Add(uploads);
                        contexts.SaveChanges();
                        file.SaveAs(path);
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";
                    }
                }
            }
            return View();
        }

        public ActionResult Download(int id)
        {
            var download = (from d in contexts.FileUploads where d.ID == id select new { d.Filename, d.Filepath }).SingleOrDefault();
            
            if (download != null)
            {
                Response.AddHeader("content-disposition", "inline; Filename : " + download.Filename);
                return File(download.Filepath, "apllication/octet-stream");
            } 
            else
            {
                return null;
            }
        }

        public ActionResult Approve(int Id)
        {
            FileUpload find = contexts.FileUploads.Find(Id);
            if (find.Approve == true)
            {
                find.Approve = false;
            }
            else
            {
                find.Approve = true;
            }

            contexts.Entry(find).State = EntityState.Modified;
            contexts.SaveChanges();
            return RedirectToAction("DetailManage");
        }

        public ActionResult DetailManage(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            //}

            var userid = User.Identity.GetUserId();
            var manage = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = manage.GetRoles(userid);

            if (s[0].ToString() == "Admin")
            {
                var show = (from item in contexts.FileUploads select item).ToList();
                ViewBag.see = show;
                ViewBag.approve = true;

            }
            else
            {
                var show = (from item in contexts.FileUploads select item).ToList();
                var see = show.FirstOrDefault(); 
                if (see.Approve == true)
                {
                    ViewBag.error = "Your file has been Approve by Admin";
                }
                else
                {
                    ViewBag.error = "Your file hasn't Approve by Admin";
                }
                ViewBag.see = show;
                
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}