using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLySach.Models;

namespace QuanLySach.Areas.Admin.Controllers
{
    public class ChuyenMucsController : Controller
    {
        private QuanLySachEntities db = new QuanLySachEntities();

        // GET: Admin/ChuyenMucs
        public ActionResult Index()
        {
            return View(db.ChuyenMucs.ToList());
        }

        // GET: Admin/ChuyenMucs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMuc chuyenMuc = db.ChuyenMucs.Find(id);
            if (chuyenMuc == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMuc);
        }

        // GET: Admin/ChuyenMucs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ChuyenMucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Ma,Ten,Mota")] ChuyenMuc chuyenMuc)
        {
            if (ModelState.IsValid)
            {
                db.ChuyenMucs.Add(chuyenMuc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chuyenMuc);
        }

        // GET: Admin/ChuyenMucs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMuc chuyenMuc = db.ChuyenMucs.Find(id);
            if (chuyenMuc == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMuc);
        }

        // POST: Admin/ChuyenMucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Ma,Ten,Mota")] ChuyenMuc chuyenMuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenMuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chuyenMuc);
        }

        // GET: Admin/ChuyenMucs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenMuc chuyenMuc = db.ChuyenMucs.Find(id);
            if (chuyenMuc == null)
            {
                return HttpNotFound();
            }
            return View(chuyenMuc);
        }
        public ActionResult DeleteConfirmed(int id)
        {
            ChuyenMuc chuyenMuc = db.ChuyenMucs.Find(id);
            db.ChuyenMucs.Remove(chuyenMuc);
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
