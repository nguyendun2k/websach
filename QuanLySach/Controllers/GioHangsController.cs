using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using PagedList;
using PayPal.Api;
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
            var listThanhtien = new List<decimal>();
            foreach (var item in gh)
            {
                decimal giaGoc = item.sp.Gia ?? 0;
                var phanTRam = decimal.Parse(item.sp.GiamGia);
                var giagiam = giaGoc * item.Soluong * phanTRam / 100;
                var thanhtien = giaGoc * item.Soluong - giagiam;
                listThanhtien.Add(thanhtien);
            }
            Session["thanhtien"] = listThanhtien.Sum(x => x);

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
                var product = db.SanPhams.FirstOrDefault(x => x.Ma == item.sp.Ma);
                product.SoLuongBanRa = product.SoLuongBanRa + item.Soluong;
                db.Entry(product).State = EntityState.Modified;
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
                if (kh != null)
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
            MaHoa mh = new MaHoa();
            string mk = mh.GetMD5_low(MatKhau);
            var isKH = db.KhachHangs.Where(x => x.DienThoai == SoDienThoai && x.Password == mk).FirstOrDefault();
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
            MaHoa mh = new MaHoa();
            string mk = mh.GetMD5_low(password);
            kh.Password = mk;
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
            //Session[giohang] = null;
            return View(ctdh);

        }

        private Payment payment;
        private Payment CreatePayment(APIContext context, string redirectUrl)
        {
            const decimal UsdToVnd = 23470.00m;
            var listItems = new ItemList() { items = new List<Item>() };
            List<GioHang> gh = (List<GioHang>)Session[giohang];
            //gh = gh.Select(x => x.sp).ToList();
            foreach (GioHang item in gh)
            {
                listItems.items.Add(new Item
                {
                    name = item.sp.Ten,
                    currency = "USD",
                    price = $"{item.sp.Gia / UsdToVnd:n}",
                    quantity = item.Soluong.ToString(),
                    sku = "sku"
                });
            }

            var payer = new Payer() { payment_method = "Paypal" };
            var redirectUrls = new RedirectUrls
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl,
            };
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = $"{listItems.items.Sum(x => Convert.ToDecimal(x.price) * Convert.ToDecimal(x.quantity)):n}",
            };
            var tax = Convert.ToDecimal(details.tax);
            var shipping = Convert.ToDecimal(details.shipping);
            var subtotal = listItems.items.Sum(x => Convert.ToDecimal(x.price) * Convert.ToDecimal(x.quantity));
            var amounts = new Amount
            {
                currency = "USD",
                total = $"{(tax + shipping + subtotal):n}",
                details = details,
            };
            var transactions = new List<Transaction>()
            {
                new Transaction
                {
                    description = "Websach transaction",
                    invoice_number = Convert.ToString(new Random().Next(100000)),
                    amount = amounts,
                    item_list = listItems
                }
            };

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactions,
                redirect_urls = redirectUrls
            };

            return payment.Create(context);
        }
        private Payment ExecutePayment(APIContext context, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution
            {
                payer_id = payerId,
            };

            payment = new Payment
            {
                id = paymentId,
            };
            return payment.Execute(context, paymentExecution);
        }

        public ActionResult PaymentWithPaypal(string makh, string tenkh, string diachi, string SoDienThoai, string email)
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            List<GioHang> gh = (List<GioHang>)Session[giohang];
            string payerId = Request.Params["PayerID"];
            var _makh = Convert.ToInt32(makh);
            if (string.IsNullOrEmpty(payerId))
            {
                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHangs/PaymentWithPayPal?";
                var guid = Convert.ToString((new Random()).Next(100000));
                var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
                var links = createdPayment.links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    Links lnk = links.Current;
                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalRedirectUrl = lnk.href;
                    }
                }
                Session.Add(guid, createdPayment.id);

                #region save order to Db
                KhachHang kh = db.KhachHangs.Find(_makh);

                kh.Ten = tenkh;
                kh.DiaChi = diachi;
                kh.DienThoai = SoDienThoai;
                kh.Email = email;
                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                DonHang dh = new DonHang();
                dh.MaKhachHang = _makh;
                dh.NgayDatHang = DateTime.Now;
                dh.PhiGiao = 0;
                dh.TenNguoiNhan = tenkh;
                dh.DiaChi = diachi;
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
                    var product = db.SanPhams.FirstOrDefault(x => x.Ma == item.sp.Ma);
                    product.SoLuongBanRa = product.SoLuongBanRa + item.Soluong;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }

                DonHang ttdh = db.DonHangs.Find(mdh);
                Session["TTDonHang"] = ttdh;

                #endregion
                return Redirect(paypalRedirectUrl);
            }
            else
            { 
                var guid = Request.Params["guid"];
                var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                //If executed payment failed then we will show payment failure message to user  
                if (executedPayment.state.ToLower() != "approved")
                {
                    return View("Failure");
                }
                Session[guid] = null;
                
                return RedirectToAction("DatHangTC", "GioHangs");
            }

        }
    }
}