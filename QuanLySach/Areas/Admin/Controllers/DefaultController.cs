using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLySach.Models;

namespace QuanLySach.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Admin/Default
        QuanLySachEntities db = new QuanLySachEntities();
        public ActionResult Index()
        {
            var sanpham = db.SanPhams.ToList();
            return View(sanpham);
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string TenDangNhap, string MatKhau)
        {
            MaHoa mh = new MaHoa();
            string mk = mh.GetMD5_low(MatKhau);
            NguoiDung nd = db.NguoiDungs.Where(x => x.TenDangNhap == TenDangNhap && x.MatKhau == mk).FirstOrDefault();
            if (nd != null)
            {
                Session["NguoiDung"] = nd;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ms = "Tài khoản mật khẩu không chính xác";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["nguoidung"] = null;
            return RedirectToAction("Login");
        }
    }
}