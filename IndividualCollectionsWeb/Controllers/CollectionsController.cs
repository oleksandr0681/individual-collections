using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IndividualCollectionsWeb.Models;

namespace IndividualCollectionsWeb.Controllers
{
    public class CollectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Collections
        public ActionResult Index()
        {
            var collections = db.Collections.Include(c => c.Theme);
            return View(collections.ToList());
        }

        // GET: Collections/Collection/5
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

        // GET: Collections/Details/5
        public ActionResult Details(int? id)
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

        // GET: Collections/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title");
            return View();
        }

        // POST: Collections/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ApplicationUserId,Title,Description,ThemeId,PictureUrl,IsNumerical1Enabled,Numerical1Title,IsNumerical2Enabled,Numerical2Title,IsNumerical3Enabled,Numerical3Title,IsString1Enabled,String1Title,IsString2Enabled,String2Title,IsString3Enabled,String3Title,IsText1Enabled,Text1Title,IsText2Enabled,Text2Title,IsText3Enabled,Text3Title,IsDate1Enabled,Date1Title,IsDate2Enabled,Date2Title,IsDate3Enabled,Date3Title,IsBoolean1Enabled,Boolean1Title,IsBoolean2Enabled,Boolean2Title,IsBoolean3Enabled,Boolean3Title")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                db.Collections.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title", collection.ThemeId);
            return View(collection);
        }

        // GET: Collections/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
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
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title", collection.ThemeId);
            return View(collection);
        }

        // POST: Collections/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationUserId,Title,Description,ThemeId,PictureUrl,IsNumerical1Enabled,Numerical1Title,IsNumerical2Enabled,Numerical2Title,IsNumerical3Enabled,Numerical3Title,IsString1Enabled,String1Title,IsString2Enabled,String2Title,IsString3Enabled,String3Title,IsText1Enabled,Text1Title,IsText2Enabled,Text2Title,IsText3Enabled,Text3Title,IsDate1Enabled,Date1Title,IsDate2Enabled,Date2Title,IsDate3Enabled,Date3Title,IsBoolean1Enabled,Boolean1Title,IsBoolean2Enabled,Boolean2Title,IsBoolean3Enabled,Boolean3Title")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ThemeId = new SelectList(db.Themes, "Id", "Title", collection.ThemeId);
            return View(collection);
        }

        // GET: Collections/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
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

        // POST: Collections/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Collection collection = db.Collections.Find(id);
            db.Collections.Remove(collection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
