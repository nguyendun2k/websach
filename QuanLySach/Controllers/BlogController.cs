using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLySach.Models;
using PagedList;
namespace QuanLySach.Controllers
{
    public class BlogController : Controller
    {
        QuanLySachEntities db = new QuanLySachEntities();
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult showBlog(int? ma , int ? page) {

            if (page == null)
            {
                page = 1;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var tintuc = db.TinTucs.Where(x => x.MaCM == ma).OrderByDescending(s => s.NgayDang).ToPagedList(pageNumber, pageSize);

            if (tintuc.Count() > 0)
            {
                ViewBag.Ma = ma;
                tintuc = db.TinTucs.Where(x => x.MaCM == ma).OrderByDescending(s => s.NgayDang).ToPagedList(pageNumber, pageSize);

            }
            else
            {
                tintuc = db.TinTucs.OrderByDescending(s => s.NgayDang).ToPagedList(pageNumber, pageSize);
            }
            ViewBag.ChuyenDe = db.ChuyenMucs.ToList();
            return View(tintuc);
        }
        public ActionResult BlogDetail(int? ma )
        {
            ViewBag.TieuDe = db.TinTucs.Where(x => x.Ma == ma).FirstOrDefault().TieuDe;
            ViewBag.Anh = db.TinTucs.Where(x => x.Ma == ma).FirstOrDefault().Anh;
            ViewBag.MoTa = db.TinTucs.Where(x => x.Ma == ma).FirstOrDefault().MoTa;
            ViewBag.MoTaNgan = db.TinTucs.Where(x => x.Ma == ma).FirstOrDefault().MoTaNgan;
            ViewBag.TinTuc = db.TinTucs.OrderByDescending(x=>x.NgayDang).Take(5).ToList();
            ViewBag.NgayDang = db.TinTucs.Where(x => x.Ma == ma).FirstOrDefault().NgayDang;
            ViewBag.TacGia = db.TinTucs.Where(x => x.Ma == ma).FirstOrDefault().TacGia;
            ViewBag.ChuyenDe = db.ChuyenMucs.ToList();
            ViewBag.Tag = db.TinTucs.Where(x =>x.Ma == ma).FirstOrDefault().ChuyenMuc.Ten;
            return View();
        }
       
    }
}