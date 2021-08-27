using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using IndividualCollectionsWeb.Models;
using System.Net;

namespace IndividualCollectionsWeb.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            IQueryable<Collection> collections = null;
            if (user != null)
            {
                collections = db.Collections.Include(c => c.Theme).Where(c => c.ApplicationUserId == user.Id);
            }
            return View(collections.ToList());
        }

        // GET: User/DetailsCollection/5
        public ActionResult DetailsCollection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // GET: User/CreateCollection
        public ActionResult CreateCollection()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && user.Blocked)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title");
            return View();
        }

        // POST: User/CreateCollection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCollection([Bind(Include = "Id,ApplicationUserId,Title,Description,ThemeId,PictureUrl,IsNumerical1Enabled,Numerical1Title,IsNumerical2Enabled,Numerical2Title,IsNumerical3Enabled,Numerical3Title,IsString1Enabled,String1Title,IsString2Enabled,String2Title,IsString3Enabled,String3Title,IsText1Enabled,Text1Title,IsText2Enabled,Text2Title,IsText3Enabled,Text3Title,IsDate1Enabled,Date1Title,IsDate2Enabled,Date2Title,IsDate3Enabled,Date3Title,IsBoolean1Enabled,Boolean1Title,IsBoolean2Enabled,Boolean2Title,IsBoolean3Enabled,Boolean3Title")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    if (!user.Blocked)
                    {
                        collection.ApplicationUserId = user.Id;
                        db.Collections.Add(collection);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь заблокирован");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
            }

            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title", collection.ThemeId);
            return View(collection);
        }

        // GET: User/EditCollection/5
        public ActionResult EditCollection(int? id)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && user.Blocked)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title", collection.ThemeId);
            return View(collection);
        }

        // POST: User/EditCollection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCollection([Bind(Include = "Id,ApplicationUserId,Title,Description,ThemeId,PictureUrl,IsNumerical1Enabled,Numerical1Title,IsNumerical2Enabled,Numerical2Title,IsNumerical3Enabled,Numerical3Title,IsString1Enabled,String1Title,IsString2Enabled,String2Title,IsString3Enabled,String3Title,IsText1Enabled,Text1Title,IsText2Enabled,Text2Title,IsText3Enabled,Text3Title,IsDate1Enabled,Date1Title,IsDate2Enabled,Date2Title,IsDate3Enabled,Date3Title,IsBoolean1Enabled,Boolean1Title,IsBoolean2Enabled,Boolean2Title,IsBoolean3Enabled,Boolean3Title")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    if (!user.Blocked)
                    {
                        if (collection.ApplicationUserId == user.Id)
                        {
                            db.Entry(collection).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь заблокирован");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
            }
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title", collection.ThemeId);
            return View(collection);
        }

        // GET: User/DeleteCollection/5
        public ActionResult DeleteCollection(int? id)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && user.Blocked)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: User/DeleteCollection/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCollection(int id)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && user.Blocked)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            Collection collection = db.Collections.Find(id);
            if (user != null && collection.ApplicationUserId == user.Id)
            {
                db.Collections.Remove(collection);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: User/Collection/5
        public ActionResult Collection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            ViewBag.Collection = collection;
            var items = db.Items.Include(i => i.Collection).Where(i => i.CollectionId == id);
            return View(items.ToList());
        }

        // GET: User/CreateItem
        public ActionResult CreateItem()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                ViewBag.CollectionId = new SelectList(db.Collections.Include(c => c.Theme).Where(c => c.ApplicationUserId == user.Id), "Id", "Title");
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View();
        }

        // POST: User/CreateItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem([Bind(Include = "Id,CollectionId,Title,Tags,AddingTime,Numerical1,Numerical2,Numerical3,String1,String2,String3,Text1,Text2,Text3,Date1,Date2,Date3,Boolean1,Boolean2,Boolean3")] Item item)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (!user.Blocked)
                    {
                        item.AddingTime = DateTime.Now;
                        db.Items.Add(item);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь заблокирован");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
            }
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Title", item.CollectionId);
            return View(item);
        }

        // GET: User/DetailsItem/5
        public ActionResult DetailsItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: User/EditItem/5
        public ActionResult EditItem(int? id)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionId = new SelectList(db.Collections.Where(c => c.ApplicationUserId == user.Id), "Id", "Title", item.CollectionId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem([Bind(Include = "Id,CollectionId,Title,Tags,AddingTime,Numerical1,Numerical2,Numerical3,String1,String2,String3,Text1,Text2,Text3,Date1,Date2,Date3,Boolean1,Boolean2,Boolean3")] Item item)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (!user.Blocked)
                    {
                        Collection collection = db.Collections.Find(item.CollectionId);
                        if (user != null && collection.ApplicationUserId == user.Id)
                        {
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь заблокирован");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
            }
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Title", item.CollectionId);
            return View(item);
        }

        // GET: User/DeleteItem/5
        public ActionResult DeleteItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: User/DeleteItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && user.Blocked)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            Item item = db.Items.Find(id);
            Collection collection = db.Collections.Find(item.CollectionId);
            if (user != null && collection.ApplicationUserId == user.Id)
            {
                db.Items.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
    }
}