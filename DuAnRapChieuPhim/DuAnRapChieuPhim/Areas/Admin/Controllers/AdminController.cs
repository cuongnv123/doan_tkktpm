using DuAnRapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Ajax.Utilities;

namespace DuAnRapChieuPhim.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        DbDataContext db = new DbDataContext();

        // GET: Admin/Admin
        public ActionResult Login()
        {
				return View();
        }
		
		public ActionResult Index()
        {
			if (Session["admin"] != null)
            {
				var model = GetDoanhThuData();
				return View(model);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}



        private DoanhThuViewModel GetDoanhThuData()
        {
            var doanhThuData = new DoanhThuViewModel
            {
                Labels = new List<string>(),
                TicketData = new List<decimal>(),
                ComboData = new List<decimal>()
            };

            // Lấy dữ liệu từ cơ sở dữ liệu và xử lý để điền vào model
            var doanhThuList = db.HoaDons
                                .Where(hd => hd.NgayDat != null && hd.NgayDat.Value.Month == DateTime.Now.Month && hd.NgayDat.Value.Year == DateTime.Now.Year)
                                .GroupBy(hd => hd.NgayDat.Value.Day)
                                .Select(group => new
                                {
                                    Ngay = group.Key,
                                    DoanhThuVe = group.Sum(hd => hd.TienVe),
                                    DoanhThuCombo = group.Sum(hd => hd.TienCombo)
                                })
                                .ToList();

            foreach (var doanhThu in doanhThuList)
            {
                doanhThuData.Labels.Add(doanhThu.Ngay.ToString()+"/"+DateTime.Now.Month);
                doanhThuData.TicketData.Add(doanhThu.DoanhThuVe.Value);
                doanhThuData.ComboData.Add(doanhThu.DoanhThuCombo.Value);
            }

            return doanhThuData;
        }
        public ActionResult Voucher(int? page)
        {
			if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var vc = db.LoaiVouchers.ToPagedList(pageNumber, pageSize);
				return View(vc);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}

		}
        public ActionResult Movie(int? page)
        {
			if (Session["admin"] != null)
            {
				ViewBag.TL = db.TheLoais.ToList();
				int pageSize = 5; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var phim = db.Phims.Where(u => !u.TrangThai.Contains("Ngưng chiếu")).ToPagedList(pageNumber, pageSize);

				return View(phim);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}

		}
        public ActionResult Slider(int? page)
        {
			if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var phim = db.Sliders.ToPagedList(pageNumber, pageSize);

				return View(phim);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}

		}
        public ActionResult News(int? page)
        {
            if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var phim = db.Tintucs.ToPagedList(pageNumber, pageSize);

				return View(phim);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}

		}
        public ActionResult TheLoai(int? page)
        {
			if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var phim = db.TheLoais.ToPagedList(pageNumber, pageSize);

				return View(phim);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}
        public ActionResult Food(int? page)
        {
            if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var doan = db.DoAns.Where(u => u.TrangThai.Contains("Đang bán")).ToPagedList(pageNumber, pageSize);

				return View(doan);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}
		}
		
		public ActionResult Chair(int? page)
		{
			if (Session["admin"] != null)
			{
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var ghe = db.Ghes.ToPagedList(pageNumber, pageSize);
				var phongs = db.Phongs.Select(p => p.MaPhong).ToList();
				ViewBag.MaPhong = phongs;

				return View(ghe);
			}
			else
			{
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}
		}
		public ActionResult Combo(int? page)
        {
            if (Session["admin"] != null)
            {
                ViewBag.DoAn = db.DoAns.ToList();
                ViewBag.Nuoc = db.Nuocs.ToList();
                int pageSize = 5; // Số sản phẩm trên mỗi trang
                int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
                var ghe = db.Combos.Where(u => u.TrangThai.Contains("Đang bán")).ToPagedList(pageNumber, pageSize);

                return View(ghe);
            }
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}
        public ActionResult Nuoc(int? page)
        {
            if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var ghe = db.Nuocs.Where(u => u.TrangThai.Contains("Đang bán")).ToPagedList(pageNumber, pageSize);

				return View(ghe);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}
        public ActionResult Bill(int? page)
        {
            if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var hoadon = db.HoaDons.Where(u => u.TrangThai.Contains("Đã thanh toán")).ToPagedList(pageNumber, pageSize);

				return View(hoadon);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}

		}
        private string HashSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public ActionResult DetailsHD(long id)
        {
            if (Session["admin"] != null)
            {
				var cthd = from chiTiet in db.ChiTietHoaDons
						   join lichChieu in db.LichChieus on chiTiet.MaLichChieu equals lichChieu.MaChieuPhim
						   join ghe in db.Ghes on chiTiet.MaGhe equals ghe.MaGhe
						   join phong in db.Phongs on ghe.MaPhong equals phong.MaPhong
						   join phim in db.Phims on lichChieu.MaPhim equals phim.MaPhim
						   where chiTiet.MaHoaDon == id
						   select new CTHD
						   {
							   chiTiet = chiTiet,
							   lichChieu = lichChieu,
							   ghe = ghe,
							   phong = phong,
							   phim = phim
						   };
				var cthdcb = from hdcb in db.CTHDComBos
							 join cb in db.Combos on hdcb.MaCB equals cb.MaCB
							 join nuoc in db.Nuocs on cb.MaNuoc equals nuoc.MaNuoc
							 join doan in db.DoAns on cb.MaDoAn equals doan.MaDoAn
							 where hdcb.MaHoaDon == id
							 select new CTHDCB
							 {
								 cthd = hdcb,
								 cb = cb,
								 nuoc = nuoc,
								 doan = doan
							 };
				var hd = db.HoaDons.SingleOrDefault(u => u.MaHoaDon == id);
				ViewBag.hd = hd;
				ViewBag.cb = cthdcb.ToList();
				ViewBag.ghe = cthd.ToList();
				string orderCode = $"HD{id}"; // Mã đơn hàng

				ViewBag.QRCode = QRCodeGenerator.GenerateQRCode(orderCode);
				return View();
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}
        [HttpPost]
        public ActionResult Login(string taikhoan, string password)
        {
            if (string.IsNullOrEmpty(taikhoan) || string.IsNullOrEmpty(password))
            {
				TempData["ErrorMessage"] = "Vui lòng nhập tên tài khoản và mật khẩu";
				return RedirectToAction("Login");
            }

            string hashedPassword = HashSHA256(password);

            var user = db.TaiKhoans
                .FirstOrDefault(u => u.Username == taikhoan && u.Password == hashedPassword);

            if (user != null)
            {
                Session["admin"] = user;
                return RedirectToAction("Index");
            }
            else
            {
				TempData["ErrorMessage"] = "Tên tài khoản hoặc mật khẩu không chính xác";
				return RedirectToAction("Login");
            }
        }
		public ActionResult Logout()
		{
			Session.Remove("admin");

			return RedirectToAction("Login", "Admin", new { area = "Admin" });

		}
		public ActionResult Showtimes (int? page)
        {
            if (Session["admin"] != null)
            {
				ViewBag.Phim = db.Phims.ToList();
				ViewBag.Phong = db.Phongs.ToList();
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var lichchieu = db.LichChieus.ToPagedList(pageNumber, pageSize);

				return View(lichchieu);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}
		}
        public ActionResult Room(int? page)
        {
            if (Session["admin"] != null)
            {
				int pageSize = 10; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var phong = db.Phongs.Where(u => u.TrangThai.Contains("Hoạt động")).ToPagedList(pageNumber, pageSize);

				return View(phong);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}
        public ActionResult Account (int? page)
        {
            if (Session["admin"] != null)
            {
				int pageSize = 5; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var taikhoan = db.TaiKhoans.ToPagedList(pageNumber, pageSize);

				return View(taikhoan);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}

		}
        public ActionResult Infor(int? page)
        {
            if (Session["admin"] != null)
            {
				int pageSize = 5; // Số sản phẩm trên mỗi trang
				int pageNumber = (page ?? 1); // Trang mặc định là trang 1 nếu không có tham số page
				var thongtin = db.ThongTinCaNhans.ToPagedList(pageNumber, pageSize);

				return View(thongtin);
			}
            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });

			}

		}

		[HttpPost]
		public ActionResult SearchCombo(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Combo> lstCus = db.Combos.Where(n => n.TenCB.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
            ViewBag.DoAn = db.DoAns.ToList();
			ViewBag.Nuoc = db.Nuocs.ToList();

			return View("Combo", lstCus.OrderByDescending(n => n.MaCB).ToPagedList(pageNumber, pageSize));
		}

		[HttpPost]
		public ActionResult SearchFood(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<DoAn> lstCus = db.DoAns.Where(n => n.TenDoAn.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Food", lstCus.OrderByDescending(n => n.MaDoAn).ToPagedList(pageNumber, pageSize));
		}

		[HttpPost]
		public ActionResult SearchNuoc(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Nuoc> lstCus = db.Nuocs.Where(n => n.TenNuoc.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Nuoc", lstCus.OrderByDescending(n => n.MaNuoc).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchMovie(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Phim> lstCus = db.Phims
						.Where(n => n.TenPhim.ToLower().Contains(searchString.ToLower()) &&
									(n.TrangThai == "Đang chiếu" || n.TrangThai == "Sắp chiếu")).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Movie", lstCus.OrderByDescending(n => n.MaPhim).ToPagedList(pageNumber, pageSize));
		}

		[HttpPost]
		public ActionResult SearchTheLoai(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<TheLoai> lstCus = db.TheLoais.Where(n => n.TenTheLoai.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("TheLoai", lstCus.OrderByDescending(n => n.MaTheLoai).ToPagedList(pageNumber, pageSize));
		}

		[HttpPost]
		public ActionResult SearchLC(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<LichChieu> lstCus = db.LichChieus.Where(n => n.NgonNgu.ToLower().Contains(searchString.ToLower())).ToList();

			ViewBag.Phong = db.Phongs.ToList();
			ViewBag.Phim = db.Phims.ToList();
			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Showtimes", lstCus.OrderByDescending(n => n.MaChieuPhim).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchVoucher(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Voucher> lstCus = db.Vouchers.Where(n => n.LoaiGiamGia.ToLower().Contains(searchString.ToLower())).ToList();

			ViewBag.Phong = db.Phongs.ToList();
			ViewBag.Phim = db.Phims.ToList();
			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Voucher", lstCus.OrderByDescending(n => n.MaVoucher).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchBill(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List <HoaDon> lstCus = db.HoaDons.Where(n => n.HoTen.ToLower().Contains(searchString.ToLower())).ToList();

			ViewBag.Phong = db.Phongs.ToList();
			ViewBag.Phim = db.Phims.ToList();
			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Bill", lstCus.OrderByDescending(n => n.MaHoaDon).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchChair(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Ghe> lstCus = db.Ghes.Where(n => n.TenGhe.ToLower().Contains(searchString.ToLower())).ToList();

			ViewBag.MaPhong = db.Phongs.ToList();
		
			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Chair", lstCus.OrderByDescending(n => n.MaGhe).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchRoom(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Phong> lstCus = db.Phongs.Where(n => n.TenPhong.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Room", lstCus.OrderByDescending(n => n.MaPhong).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchSlider(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Slider> lstCus = db.Sliders.Where(n => n.TenHinh.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Slider", lstCus.OrderByDescending(n => n.ma).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchAcc(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<TaiKhoan> lstCus = db.TaiKhoans.Where(n => n.Username.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Account", lstCus.OrderByDescending(n => n.Username).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchNew(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<Tintuc> lstCus = db.Tintucs.Where(n => n.Chudetin.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("News", lstCus.OrderByDescending(n => n.Matin).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult SearchKH(string searchString, int? page)
		{
			int pageSize = 5;
			int pageNumber = (page ?? 1);

			TempData["kwd"] = searchString;
			List<ThongTinCaNhan> lstCus = db.ThongTinCaNhans.Where(n => n.Ten.ToLower().Contains(searchString.ToLower())).ToList();

			// Tạo một danh sách các đối tượng Combo từ các mảng object[]
			return View("Infor", lstCus.OrderByDescending(n => n.MaKH).ToPagedList(pageNumber, pageSize));
		}
	}
}