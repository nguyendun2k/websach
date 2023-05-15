using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLySach.Models;
using PagedList;
namespace QuanLySach.Controllers
{
    public class ShopController : Controller
    {
        QuanLySachEntities db = new QuanLySachEntities();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Shop( int? page , string tensp , bool? asc)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            IPagedList<SanPham> LoaiSP;
            if (tensp != null)
            {
                LoaiSP = db.SanPhams.Where(x => x.Ten.Contains(tensp)).OrderBy(s => s.Ma).ToPagedList(pageNumber, pageSize);
            }
            ViewBag.asc = true;

            if (asc != null && asc == true)
            {
                LoaiSP = db.SanPhams.OrderBy(x => x.Gia).ToPagedList(pageNumber, pageSize);

            }
            else
            {
                LoaiSP = db.SanPhams.OrderByDescending(x => x.Gia).ToPagedList(pageNumber, pageSize);
                ViewBag.asc = false;
            }



            ViewBag.LoaiSP = db.LoaiSPs.ToList();
            return View(LoaiSP);


            //var  LoaiSP = db.SanPhams.OrderByDescending(s => s.Ma).ToPagedList(pageNumber, pageSize);

            //if(tensp != null)
            //{
            //    LoaiSP = db.SanPhams.Where(x=>x.Ten.Contains(tensp)).OrderByDescending(s => s.Ma).ToPagedList(pageNumber, pageSize);
            //}


            //if (gia > 0)
            //{
            //    LoaiSP = db.SanPhams.Where(x => x.Gia <= gia).OrderByDescending(s => s.Ma).ToPagedList(pageNumber, pageSize);
            //}

            //ViewBag.LoaiSP = db.LoaiSPs.ToList();
            //return View(LoaiSP);

        }
    }
}