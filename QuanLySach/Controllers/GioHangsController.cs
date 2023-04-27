using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLySach.Models;

namespace QuanLySach.Controllers
{
    public class GioHangsController : Controller
    {
        // GET: GioHangs
        QuanLySachEntities db = new QuanLySachEntities();
        string giohang = "GioHang";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ThemGioHang(int masp, int sl)
        {

            var gh = (List<GioHang>)Session[giohang];
            var sp = db.SanPhams.Find(masp);

            // kiem tra 

            if (gh == null)
            {
                var gio_hang_mois = new List<GioHang>();

                GioHang gh_moi = new GioHang();
                gh_moi.sp = sp;
                gh_moi.Soluong = sl;
                gio_hang_mois.Add(gh_moi);
                Session[giohang] = gio_hang_mois;
            }
            else
            {
                if (gh.Exists(x => x.sp.Ma == masp))
                {
                    var sanpham = gh.Where(s => s.sp.Ma == masp).FirstOrDefault();
                    sanpham.Soluong = sanpham.Soluong + sl;


                }
                else
                {
                    GioHang gh_moi = new GioHang();
                    gh_moi.sp = sp;
                    gh_moi.Soluong = sl;
                    gh.Add(gh_moi);

                }
                Session[giohang] = gh;
            }

            return Redirect(Request.UrlReferrer.ToString());

        }
        public ActionResult XemGioHang()
        {
            if (Session[giohang] == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                List<GioHang> gh = (List<GioHang>)Session[giohang];
                return View(gh);
            }
        }
        public ActionResult XoaSP(int masp)
        {

            var gh = (List<GioHang>)Session[giohang];
            var sp = db.SanPhams.Find(masp);
            if (gh.Exists(x => x.sp.Ma == masp))
            {
                var sanpham = gh.Where(s => s.sp.Ma == masp).FirstOrDefault();
                gh.Remove(sanpham);
                Session[giohang] = gh;
            }
            return Redirect(Request.UrlReferrer.ToString());

        }
        public ActionResult TangSl(int masp)
        {
            var gh = (List<GioHang>)Session[giohang];
            var sp = db.SanPhams.Find(masp);
            if (gh.Exists(x => x.sp.Ma == masp))
            {
                var sanpham = gh.Where(s => s.sp.Ma == masp).FirstOrDefault();
                sanpham.Soluong = sanpham.Soluong + 1;
                Session[giohang] = gh;

            }
            return Redirect(Request.UrlReferrer.ToString());

        }
        public ActionResult GiamSl(int masp)
        {
            var gh = (List<GioHang>)Session[giohang];
            var sp = db.SanPhams.Find(masp);
            if (gh.Exists(x => x.sp.Ma == masp))
            {
                var sanpham = gh.Where(s => s.sp.Ma == masp).FirstOrDefault();
                if (sanpham.Soluong > 0)
                {
                    sanpham.Soluong = sanpham.Soluong - 1;
                    Session[giohang] = gh;
                }

            }
            return Redirect(Request.UrlReferrer.ToString());

        }
        public ActionResult CheckOut()
        {
            List<GioHang> gh = (List<GioHang>)Session[giohang];
            if (gh == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return View(gh);
            }
        }
        public ActionResult CheckOutProduct(int makh, string tenkh, string diachi, string SoDienThoai, string email, string diachigiaohang, string checkGiaoHang)
        {
            List<GioHang> gh = (List<GioHang>)Session[giohang];


            KhachHang kh = db.KhachHangs.Find(makh);

            kh.Ten = tenkh;
            kh.DiaChi = diachi;
            kh.DienThoai = SoDienThoai;
            kh.Email = email;
            db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            DonHang dh = new DonHang();
            dh.MaKhachHang = makh;
            dh.NgayDatHang = DateTime.Now;
            dh.PhiGiao = 0;
            dh.TenNguoiNhan = tenkh;
            if (checkGiaoHang == "0")
            {
                dh.DiaChi = diachigiaohang;
            }
            else
            {
                dh.DiaChi = diachi;
            }
            dh.DienThoai = SoDienThoai;
            dh.Email = email;
            dh.TrangThai = false;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            int mdh = db.DonHangs.OrderByDescending(x => x.Ma).FirstOrDefault().Ma;


            foreach (var item in gh)
            {
                ChiTietDonHang ctd = new ChiTietDonHang();
                ctd.MaDH = mdh;
                ctd.MaSP = item.sp.Ma;
                ctd.SoLuong = item.Soluong;
                ctd.DonGia = item.sp.Gia;
                db.ChiTietDonHangs.Add(ctd);
                db.SaveChanges();
            }
            DonHang ttdh = db.DonHangs.Find(mdh);
            Session["TTDonHang"] = ttdh;
            return RedirectToAction("DatHangTC");

        }


        public ActionResult LoginAcc()
        {
            List<GioHang> gh = (List<GioHang>)Session[giohang];
            KhachHang kh = (KhachHang)Session["KhachHang"];
            if (gh == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                if(kh != null)
                {
                    return Redirect("CheckOut");
                }
                else
                {
                   return View(gh);
                }
            }
        }
        [HttpPost]
        public ActionResult LoginAcc(string SoDienThoai, string MatKhau)
        {
            List<GioHang> gh = (List<GioHang>)Session[giohang];
            var isKH = db.KhachHangs.Where(x => x.DienThoai == SoDienThoai && x.Password == MatKhau).FirstOrDefault();
            if (isKH != null)
            {
                Session["KhachHang"] = isKH;
                return Redirect("CheckOut");
            }
            else
            {
                ViewBag.mess = "Đăng nhập không thành công";
            }
            return View(gh);
        }
        public ActionResult CreateAcc()
        {
            List<GioHang> gh = (List<GioHang>)Session[giohang];
            if (gh == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return View(gh);
            }
        }

        [HttpPost]
        public ActionResult CreateAcc(string tenkh, string diachi, string sodienthoai, string password, string email, string diachigiaohang, string checkGiaoHang)
        {
            List<GioHang> gh = (List<GioHang>)Session[giohang];

            KhachHang kh = new KhachHang();

            kh.Ten = tenkh;
            kh.DiaChi = diachi;
            kh.DienThoai = sodienthoai;
            kh.Email = email;
            kh.Password = password;
            db.KhachHangs.Add(kh);
            db.SaveChanges();

            var makh = db.KhachHangs.OrderByDescending(x => x.Ma).FirstOrDefault().Ma;

            DonHang dh = new DonHang();
            dh.MaKhachHang = makh;
            dh.NgayDatHang = DateTime.Now;
            dh.PhiGiao = 0;
            dh.TenNguoiNhan = tenkh;
            dh.DienThoai = sodienthoai;
            dh.DiaChi = diachi;
            dh.Email = email;
            if (checkGiaoHang == "0")
            {
                dh.DiaChi = diachigiaohang;
            }
            else
            {
                dh.DiaChi = diachi;
            }
            dh.TrangThai = false;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            int mdh = db.DonHangs.OrderByDescending(x => x.Ma).FirstOrDefault().Ma;
            foreach (var item in gh)
            {
                ChiTietDonHang ctd = new ChiTietDonHang();
                ctd.MaDH = mdh;
                ctd.MaSP = item.sp.Ma;
                ctd.SoLuong = item.Soluong;
                ctd.DonGia = item.sp.Gia;
                db.ChiTietDonHangs.Add(ctd);
                db.SaveChanges();
            }

            DonHang ttdh = db.DonHangs.Find(mdh);
            Session["TTDonHang"] = ttdh;
            return RedirectToAction("DatHangTC");

        }
        public ActionResult DatHangTC()
        {

            var dh = (DonHang)Session["TTDonHang"];
            var ctdh = db.ChiTietDonHangs.Where(x => x.MaDH == dh.Ma).ToList();
            Session[giohang] = null;
            return View(ctdh);

        }

    }
}