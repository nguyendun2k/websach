using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLySach.Models;

namespace QuanLySach.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {

        QuanLySachEntities db = new QuanLySachEntities();
        // GET: Admin/DonHang
        public ActionResult Index()
        {
            var donhang = db.DonHangs.OrderByDescending(x => x.NgayDatHang).ToList();
            return View(donhang);
        }
        public ActionResult DanhSachDonHang() {

            var donhang = db.DonHangs.OrderByDescending(x=>x.NgayDatHang).ToList();
            return View(donhang);
        }
        public ActionResult XemChiTiet(int ? ma)
        {

            var ctd = db.ChiTietDonHangs.Where(x => x.MaDH == ma).OrderByDescending(x => x.DonGia).ToList();
            ViewBag.tenkh = db.DonHangs.Where(x => x.Ma == ma).FirstOrDefault().KhachHang.Ten;
            ViewBag.diachi = db.DonHangs.Where(x => x.Ma == ma).FirstOrDefault().KhachHang.DiaChi;
            ViewBag.sdt = db.DonHangs.Where(x => x.Ma == ma).FirstOrDefault().KhachHang.DienThoai;
            ViewBag.email = db.DonHangs.Where(x => x.Ma == ma).FirstOrDefault().KhachHang.Email;
            ViewBag.ngaymua = db.DonHangs.Where(x => x.Ma == ma).FirstOrDefault().NgayDatHang;
            ViewBag.TrangThai = db.DonHangs.Where(x => x.Ma == ma).FirstOrDefault().TrangThai;
            return View(ctd);
        }
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang sanPham = db.DonHangs.Find(id);
            db.DonHangs.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("DanhSachDonHang");
        }
        public ActionResult xoaChtdh(int ? ma , int ? masp)
        {
            var ctd = db.ChiTietDonHangs.Where(x => x.MaDH == ma && x.MaSP == masp).FirstOrDefault();
            db.ChiTietDonHangs.Remove(ctd);
            db.SaveChanges();
            return RedirectToAction("XemChiTiet" , new { ma = ma });
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MaLoai = new SelectList(db.LoaiSPs, "Ma", "Ten", donHang.MaLoai);
            return View(donHang);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Ma,MaKhachHang,NgayDatHang,PhiGiao,TenNguoiNhan,DiaChi, DienThoai, Email,TrangThai")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.MaLoai = new SelectList(db.LoaiSPs, "Ma", "Ten", donHang.MaLoai);
            return View(donHang);
        }
    }
}