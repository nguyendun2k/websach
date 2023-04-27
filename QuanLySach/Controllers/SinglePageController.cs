using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLySach.Models;



namespace QuanLySach.Controllers
{
    public class SinglePageController : Controller
    {
        QuanLySachEntities db = new QuanLySachEntities();
        // GET: SinglePage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SingleProduct(int ? ma)
        {

            ViewBag.sanpham = db.SanPhams.Where(x => x.Ma == ma).FirstOrDefault();
            ViewBag.LoaiSP = db.LoaiSPs.ToList();
            var mal = db.SanPhams.Where(x => x.Ma == ma).FirstOrDefault().MaLoai;
            ViewBag.SPCL = db.SanPhams.Where(x => x.MaLoai == mal).ToList();
            ViewBag.BESTSELL = db.SanPhams.OrderByDescending(x => x.Ma).Take(5).ToList();
            return View();
        }
    }
}