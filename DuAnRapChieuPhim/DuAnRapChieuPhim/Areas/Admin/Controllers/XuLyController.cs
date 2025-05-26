using DuAnRapChieuPhim.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DuAnRapChieuPhim.Areas.Admin.Controllers
{
    public class XuLyController : Controller
    {
        DbDataContext db = new DbDataContext();
        // GET: Admin/XuLy
        public JsonResult EditFood(int id)
        {
            Session["MaDoAn"] = id;
            var food = db.DoAns.SingleOrDefault(u => u.MaDoAn == id);
            var foodData = new
            {
                MaDoAn = food.MaDoAn,
                TenDoAn = food.TenDoAn,
                Gia = food.Gia,
                Loai = food.Loai,
                TrangThai = food.TrangThai
            };
            return Json(new { success = true, food = foodData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditTL(int id)
        {
            Session["MaTL"] = id;
            var food = db.TheLoais.SingleOrDefault(u => u.MaTheLoai == id);
            var foodData = new
            {
                MaTheLoai = food.MaTheLoai,
                TenTheLoai = food.TenTheLoai
            };
            return Json(new { success = true, TL = foodData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditNuoc(int id)
        {
            Session["MaNuoc"] = id;
            var food = db.Nuocs.SingleOrDefault(u => u.MaNuoc == id);
            var foodData = new
            {
                MaNuoc = food.MaNuoc,
                TenNuoc = food.TenNuoc,
                Gia = food.Gia,
                Loai = food.Loai
            };
            return Json(new { success = true, Nuoc = foodData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditCB(int id)
        {
            Session["MaCB"] = id;
            var food = db.Combos.SingleOrDefault(u => u.MaCB == id);
            var foodData = new
            {
                TenCombo = food.TenCB,
                Gia = food.Gia,
                MoTa = food.MoTa,
                MaDoAn = food.MaDoAn,
                MaNuoc =  food.MaNuoc,
                SoLuongDA = food.SLDoAn,
                SoLuongNuoc = food.SLNuoc,
                GhiChu = food.GhiChu,
            };
            return Json(new { success = true, CB = foodData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditRoom(int id)
        {
            Session["MaPhong"] = id;

            var phong = db.Phongs.SingleOrDefault(u => u.MaPhong == id);

            if (phong == null)
            {
                return Json(new { success = false, message = "Phòng không tồn tại." });
            }

            var phongData = new
            {
                MaPhong = phong.MaPhong,
                TenPhong = phong.TenPhong,
                SoChoNgoi = phong.SoChoNgoi
            };

            return Json(new { success = true, phong = phongData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditShowTime(int id)
        {
            Session["MaLichChieu"] = id;
            var food = db.LichChieus.SingleOrDefault(u => u.MaChieuPhim == id);
            var foodData = new
            {
                NgonNgu = food.NgonNgu,
                GioChieu = food.GioChieu.ToString(), // Convert TimeSpan to DateTime and then format
                GioKetThuc = food.GioKetThuc.ToString(),
                Phong = food.Phong,
                MaPhim = food.MaPhim,
                NgayChieu = food.NgayChieu.Value.Date.ToString("yyyy-MM-dd"),
            };
            return Json(new { success = true, lc = foodData }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult EditInfor(int id)
        {
            var infor = db.ThongTinCaNhans.SingleOrDefault(u => u.MaKH == id);
            Session["MaKH"] = id;
            if (infor == null)
            {
                return Json(new { success = false, message = "Khách hàng không tồn tại." });
            }

            var inforData = new
            {
                MaKH = infor.MaKH,
                Ten = infor.Ten,
                SDT = infor.SDT,
                Email = infor.Email,
                ThongTinChiTieu = infor.ThongTinChiTieu,
                CapDoThe = infor.CapDoThe,
                MaThe = infor.MaThe
            };

            return Json(new { success = true, infor = inforData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditAccount(string id)
        {
            var tk = db.TaiKhoans.SingleOrDefault(u => u.Username == id);

            if (tk == null)
            {
                return Json(new { success = false, message = "Khách hàng không tồn tại." });
            }

            var tkData = new
            {
                Username = tk.Username,
                Password = tk.Password,
                ChucDanh = tk.ChucDanh
            };

            return Json(new { success = true, tk = tkData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditVC(int id)
        {
            var tk = db.LoaiVouchers.SingleOrDefault(u => u.id == id);
            Session["VC"] = id;
            if (tk == null)
            {
                return Json(new { success = false, message = "Voucher không tồn tại." });
            }

            var tkData = new
            {
                LoaiGiamGia = tk.LoaiGiamGia,
                GiamGia = tk.GiamGia,
                Diem = tk.Diem,
                sl = tk.sl
            };

            return Json(new { success = true, tk = tkData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditChair(int id)
        {
            Session["MaGhe"] = id;

            var ghe = db.Ghes.SingleOrDefault(u => u.MaGhe == id);

            if (ghe == null)
            {
                return Json(new { success = false, message = "Khách hàng không tồn tại." });
            }

            var tkData = new
            {
                MaGhe = ghe.MaGhe,
                TrangThai = ghe.TrangThai,
                MaPhong = ghe.MaPhong,
                TenGhe = ghe.TenGhe,
                LoaiGhe = ghe.LoaiGhe,
                Gia = ghe.GiaTien
            };

            return Json(new { success = true, ghe = tkData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditMovie(int id)
        {
            Session["MaPhim"] = id;
            var phim = db.Phims.SingleOrDefault(u => u.MaPhim == id);

            if (phim == null)
            {
                return Json(new { success = false, message = "Khách hàng không tồn tại." });
            }

            var phimData = new
            {
                MaPhim = phim.MaPhim,
                TenPhim = phim.TenPhim,
                Ngay = phim.NgayRaMat,
                TrangThai = phim.TrangThai,
                TheLoai = phim.TheLoai,
                ThoiLuong = phim.ThoiLuong,
                DienVien = phim.DienVien,
                DaoDien = phim.DaoDien,
                NgonNgu = phim.NgonNgu,
                Video = phim.Video,
            };

            return Json(new { success = true, phim = phimData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditBill(int id)
        {
            Session["MaHoaDon"] = id;

            var hoaDon = db.HoaDons.SingleOrDefault(u => u.MaHoaDon == id);

            if (hoaDon == null)
            {
                return Json(new { success = false, message = "Khách hàng không tồn tại." });
            }

            var hoaDonData = new
            {
                MaHoaDon = hoaDon.MaHoaDon,
                MaKH = hoaDon.MaKH,
                HoTen = hoaDon.HoTen,
                SoDienThoai = hoaDon.SoDienThoai,
                Email = hoaDon.Email,

            };

            return Json(new { success = true, hoaDon = hoaDonData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Editnew(int id)
        {
            Session["Matin"] = id;

            var hoaDon = db.Tintucs.SingleOrDefault(u => u.Matin == id);

            if (hoaDon == null)
            {
                return Json(new { success = false, message = "Khách hàng không tồn tại." });
            }

            var hoaDonData = new
            {
                Ten = hoaDon.Chudetin,
                batdau = hoaDon.Ngaybatdau,
                kt = hoaDon.Ngayketthuc,
                mt = hoaDon.Mota,
                dk = hoaDon.DieuKien,
                time = hoaDon.ThoiHan
            };

            return Json(new { success = true, tk = hoaDonData }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDoAn(FormCollection formCollection)
        {
            if (Session["admin"] != null )
            {
                decimal Gia = Convert.ToDecimal(formCollection["Gia"]);
                string TenDoAn = formCollection["TenDoAn"];
                string Loai = formCollection["Loai"];
                
				var existingTheLoai = db.DoAns.FirstOrDefault(tl => tl.TenDoAn == TenDoAn);
                if (existingTheLoai != null)
                {
					TempData["ErrorMessage"] = "Tên đồ ăn " + TenDoAn + " này đã có trong CSDL";
				}
                else
                {
					
						DoAn d = new DoAn
						{
							TenDoAn = TenDoAn,
							Gia = Gia,
							Loai = Loai,
							TrangThai = "Đang bán"
						};

						db.DoAns.InsertOnSubmit(d);
						db.SubmitChanges();
					// Có thể xử lý hình đại diện ở đây nếu cần.
					TempData["SuccessMessage"] = "đồ ăn " + TenDoAn + " đã được thêm thành công!";

				}
				return RedirectToAction("Food", "Admin", new { area = "Admin" });
			}

            else
            {
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}
			
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVC(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string u = formCollection["LoaiGiamGia"];
                int p = Convert.ToInt32(formCollection["GiamGia"]);
                int cv = Convert.ToInt32(formCollection["Diem"]);
                int sl = Convert.ToInt32(formCollection["sl"]);
                string img;
                if (u == "Phim")
                {
                    img = "phim.jpg";
                }
                else
                {
                    img = "combo.png";
                }
                LoaiVoucher tk = new LoaiVoucher
                {
                    LoaiGiamGia = u,
                    GiamGia = p,
                    Diem = cv,
                    sl = sl,
                    img = img
                };
                db.LoaiVouchers.InsertOnSubmit(tk);
                db.SubmitChanges();
                // Có thể xử lý hình đại diện ở đây nếu cần.


                return RedirectToAction("Voucher", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccount(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string u = formCollection["UserName"];
                string p = formCollection["Password"];
                string cv = formCollection["ChucDanh"];
                TaiKhoan tk = new TaiKhoan
                {
                    Username = u,
                    Password = HashSHA256(p),
                    ChucDanh = cv
                };
                db.TaiKhoans.InsertOnSubmit(tk);
                db.SubmitChanges();
                // Có thể xử lý hình đại diện ở đây nếu cần.


                return RedirectToAction("Account", "Admin", new { area = "Admin" });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNuoc(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string TenNuoc = formCollection["TenNuoc"];
                decimal Gia = Convert.ToDecimal(formCollection["Gia"]);
                string Loai = formCollection["Loai"];
				var existingTheLoai = db.Nuocs.FirstOrDefault(tl => tl.TenNuoc == TenNuoc);
                if (existingTheLoai != null)
                {
					TempData["ErrorMessage"] = "Tên nước " + TenNuoc + " này đã có trong CSDL";
				}
                else
                {
					Nuoc d = new Nuoc
					{
						TenNuoc = TenNuoc,
						Gia = Gia,
						Loai = Loai,
						TrangThai = "Đang bán"
					};

					db.Nuocs.InsertOnSubmit(d);
					db.SubmitChanges();
					TempData["SuccessMessage"] = "nước " + TenNuoc + " đã được thêm thành công!";
				}
				return RedirectToAction("Nuoc", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTL(FormCollection formCollection)
        {
			
			if (Session["admin"] != null)
			{
				string TenTheLoai = formCollection["TenTheLoai"];

				// Kiểm tra xem tên thể loại đã tồn tại trong CSDL chưa
				var existingTheLoai = db.TheLoais.FirstOrDefault(tl => tl.TenTheLoai == TenTheLoai);

				if (existingTheLoai != null)
				{
					// Nếu thể loại đã tồn tại, đưa ra thông báo và không thêm vào CSDL
					TempData["ErrorMessage"] = "Tên thể loại " + TenTheLoai + " này đã có trong CSDL";
					
				}
				else
				{
					// Nếu thể loại chưa tồn tại, thêm vào CSDL
					TheLoai d = new TheLoai
					{
						TenTheLoai = TenTheLoai
					};

					db.TheLoais.InsertOnSubmit(d);
					db.SubmitChanges();
					// Có thể xử lý hình đại diện ở đây nếu cần.

					TempData["SuccessMessage"] = "Thể loại " + TenTheLoai + " đã được thêm thành công!";
				}

				return RedirectToAction("TheLoai", "Admin", new { area = "Admin" });
			}
			else
			{
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCB(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                int MaDoAn = Convert.ToInt32(formCollection["MaDoAn"]);
                int MaNuoc = Convert.ToInt32(formCollection["MaNuoc"]);
                int SoLuongDA = Convert.ToInt32(formCollection["SoLuongDA"]);
                int SoLuongNuoc = Convert.ToInt32(formCollection["SoLuongNuoc"]);
                string TenCombo = formCollection["TenCombo"];
                string GhiChu = formCollection["GhiChu"];
                decimal Gia = Convert.ToDecimal(formCollection["Gia"]);
                string Mota = formCollection["Loai"];
                var anhBia = Request.Files["HinhAnh"];
                string imagePath = SaveImage(anhBia);
				var existingTheLoai = db.Combos.FirstOrDefault(tl => tl.TenCB == TenCombo);
                if(existingTheLoai != null)
                {
					TempData["ErrorMessage"] = "Tên combo " + TenCombo + " này đã có trong CSDL";
				}
                else
                {
					Combo d = new Combo
					{
						TenCB = TenCombo,
						MoTa = Mota,
						Gia = Gia,
						MaDoAn = MaDoAn,
						MaNuoc = MaNuoc,
						SLDoAn = SoLuongDA,
						SLNuoc = SoLuongNuoc,
						HinhAnh = imagePath,
						GhiChu = GhiChu,
						TrangThai = "Đang bán"
					};

					db.Combos.InsertOnSubmit(d);
					db.SubmitChanges();
					// Có thể xử lý hình đại diện ở đây nếu cần.
					TempData["SuccessMessage"] = "Ten combo " + TenCombo + " đã được thêm thành công!";

				}
				return RedirectToAction("Combo", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSlider(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string Ten = formCollection["Name"];
                var anhBia = Request.Files["HinhAnh"];
                string imagePath = SaveImage(anhBia);
				var existingTheLoai = db.Sliders.FirstOrDefault(tl => tl.TenHinh == Ten);
				if (existingTheLoai != null)
				{
					TempData["ErrorMessage"] = "Tên slider " + Ten + " này đã có trong CSDL";
				}
				else
                {
					Slider d = new Slider
					{
						TenHinh = Ten,
						HinhAnh = imagePath,
					};

					db.Sliders.InsertOnSubmit(d);
					db.SubmitChanges();
					// Có thể xử lý hình đại diện ở đây nếu cần.
					TempData["SuccessMessage"] = "Ten slider " + Ten + " đã được thêm thành công!";

				}
				return RedirectToAction("Slider", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addnew(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {

                string Ten = formCollection["Ten"];
                DateTime batdau = DateTime.Parse(formCollection["batdau"]);
                DateTime kt = DateTime.Parse(formCollection["kt"]);
                string mt = formCollection["mt"];
                string dk = formCollection["dk"];
                string time = formCollection["time"];
                var anhBia = Request.Files["HinhAnh"];
                string imagePath = SaveImage(anhBia);
				var existingTheLoai = db.Tintucs.FirstOrDefault(tl => tl.Chudetin == Ten);
				if (existingTheLoai != null)
				{
					TempData["ErrorMessage"] = "Tên tin " + Ten + " này đã có trong CSDL";
				}
                else
                {
					Tintuc d = new Tintuc
					{
						Chudetin = Ten,
						Ngaybatdau = batdau,
						Ngayketthuc = kt,
						Mota = mt,
						DieuKien = dk,
						ThoiHan = time,
						Anhbia = imagePath,
					};

					db.Tintucs.InsertOnSubmit(d);
					db.SubmitChanges();
					// Có thể xử lý hình đại diện ở đây nếu cần.
					TempData["SuccessMessage"] = "Ten tin " + Ten + " đã được thêm thành công!";

				}
				return RedirectToAction("News", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editnew(FormCollection formCollection)
        {
            if (Session["admin"]!= null)
            {
                string Ten = formCollection["Ten"];
                DateTime batdau = DateTime.Parse(formCollection["batdau"]);
                DateTime kt = DateTime.Parse(formCollection["kt"]);
                string mt = formCollection["mt"];
                string dk = formCollection["dk"];
                string time = formCollection["time"];
                var anhBia = Request.Files["HinhAnh"];
                string imagePath = SaveImage(anhBia);
                int id = (int)Session["Matin"];
                Tintuc d = db.Tintucs.SingleOrDefault(u => u.Matin == id);


                d.Chudetin = Ten;
                d.Ngaybatdau = batdau;
                d.Ngayketthuc = kt;
                d.Mota = mt;
                d.DieuKien = dk;
                d.ThoiHan = time;
                if (anhBia != null && anhBia.ContentLength > 0)
                {
                    d.Anhbia = imagePath;

                }
                db.SubmitChanges();
                // Có thể xử lý hình đại diện ở đây nếu cần.


                return RedirectToAction("News", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCB(FormCollection formCollection)
        {

            if (Session["admin"] != null)
            {
                int MaDoAn = Convert.ToInt32(formCollection["MaDoAn"]);
                int MaNuoc = Convert.ToInt32(formCollection["MaNuoc"]);
                int SoLuongDA = Convert.ToInt32(formCollection["SoLuongDA"]);
                int SoLuongNuoc = Convert.ToInt32(formCollection["SoLuongNuoc"]);
                string TenCombo = formCollection["TenCombo"];
                decimal Gia = Convert.ToDecimal(formCollection["Gia"]);
                string Mota = formCollection["Loai"];
                var anhBia = Request.Files["HinhAnh"];
                string GhiChu = formCollection["GhiChu"];
                string imagePath = SaveImage(anhBia);

                // Xử lý các bước cập nhật thông tin đối tượng
                int id = 0; // Giá trị mặc định nếu Session["MaDoAn"] là null
                object maDoAnObj = Session["MaCB"];
                if (maDoAnObj != null && maDoAnObj is int)
                {
                    id = (int)maDoAnObj;
                }
                Combo existingDoAn = db.Combos.SingleOrDefault(d => d.MaCB == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }
                existingDoAn.GhiChu = GhiChu;
                existingDoAn.TenCB = TenCombo;
                existingDoAn.MoTa = Mota;
                existingDoAn.Gia = Gia;
                existingDoAn.MaDoAn = MaDoAn;
                existingDoAn.MaNuoc = MaNuoc;
                existingDoAn.SLDoAn = SoLuongDA;
                existingDoAn.SLNuoc = SoLuongNuoc;
                if (anhBia != null && anhBia.ContentLength > 0)
                {
                    existingDoAn.HinhAnh = imagePath;
                }

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Combo", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
           
        }

        private string ConvertToEmbedLink(string youtubeLink)
        {
            var regex = new Regex(@"(?:https?://)?(?:www\.)?(?:youtube\.com/(?:[^/\n\s]+/\S+/|(?:v|e(?:mbed)?)/|\S*?[?&]v=)|youtu\.be/)([a-zA-Z0-9_-]{11})");

            var match = regex.Match(youtubeLink);

            if (match.Success)
            {
                // Chuyển đổi thành định dạng embed link
                var videoId = match.Groups[1].Value;
                return "https://www.youtube.com/embed/" + videoId;
            }

            // Trả về chuỗi rỗng nếu không thành công
            return string.Empty;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMovie(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string TenPhim = formCollection["TenPhim"];
                DateTime Ngay = DateTime.Parse(formCollection["Ngay"]);
                string TrangThai = formCollection["TrangThai"];
                string[] TheLoai = formCollection.GetValues("TheLoai");
                string theLoai = string.Join(",", TheLoai);
                int ThoiLuong = Convert.ToInt32(formCollection["ThoiLuong"]);
                string DienVien = formCollection["DienVien"];
                string DaoDien = formCollection["DaoDien"];
                string NgonNgu = formCollection["NgonNgu"];
                var anhBia = Request.Files["AnhBia"];
                string Video = ConvertToEmbedLink(formCollection["Video"]);
                string imagePath = SaveImage(anhBia);
				var existingTheLoai = db.Phims.FirstOrDefault(tl => tl.TenPhim == TenPhim);
				if (existingTheLoai != null)
				{
					TempData["ErrorMessage"] = "Tên phim " + TenPhim + " này đã có trong CSDL";
				}
				else
                {
					Phim d = new Phim
					{
						TenPhim = TenPhim,
						NgayRaMat = Ngay,
						TrangThai = TrangThai,
						TheLoai = theLoai,
						DaoDien = DaoDien,
						DienVien = DienVien,
						ThoiLuong = ThoiLuong,
						NgonNgu = NgonNgu,
						Video = Video,
						AnhBia = imagePath
					};

					db.Phims.InsertOnSubmit(d);
					db.SubmitChanges();
					// Có thể xử lý hình đại diện ở đây nếu cần.
					TempData["SuccessMessage"] = "Ten phim " + TenPhim + " đã được thêm thành công!";

				}
				return RedirectToAction("Movie", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMovie(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string TenPhim = formCollection["TenPhim"];
                DateTime Ngay = DateTime.Parse(formCollection["Ngay"]);
                string TrangThai = formCollection["TrangThai"];
                string[] TheLoai = formCollection.GetValues("TheLoai");
                string theLoai = string.Join(",", TheLoai);
                int ThoiLuong = Convert.ToInt32(formCollection["ThoiLuong"]);
                string DienVien = formCollection["DienVien"];
                string DaoDien = formCollection["DaoDien"];
                string NgonNgu = formCollection["NgonNgu"];
                var anhBia = Request.Files["AnhBia"];
                string Video = ConvertToEmbedLink(formCollection["Video"]);
                string imagePath = SaveImage(anhBia);

                // Xử lý các bước cập nhật thông tin đối tượng
                int id = 0; // Giá trị mặc định nếu Session["MaDoAn"] là null
                object maDoAnObj = Session["MaPhim"];
                if (maDoAnObj != null && maDoAnObj is int)
                {
                    id = (int)maDoAnObj;
                }
                Phim existingDoAn = db.Phims.SingleOrDefault(d => d.MaPhim == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }
                existingDoAn.TenPhim = TenPhim;
                existingDoAn.NgayRaMat = Ngay;
                existingDoAn.TrangThai = TrangThai;
                existingDoAn.TheLoai = theLoai;
                existingDoAn.ThoiLuong = ThoiLuong;
                existingDoAn.DienVien = DienVien;
                existingDoAn.DaoDien = DaoDien;
                existingDoAn.NgonNgu = NgonNgu;
                existingDoAn.Video = Video;
                if (anhBia != null && anhBia.ContentLength > 0)
                {
                    existingDoAn.AnhBia = imagePath;
                }

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Movie", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFood(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string tenDoAn = formCollection["TenDoAn"];
                decimal gia = Convert.ToDecimal(formCollection["Gia"]);
                string loai = formCollection["Loai"];
                string trangthai = formCollection["TrangThai"];

                // Xử lý các bước cập nhật thông tin đối tượng
                int id = 0; // Giá trị mặc định nếu Session["MaDoAn"] là null
                object maDoAnObj = Session["MaDoAn"];
                if (maDoAnObj != null && maDoAnObj is int)
                {
                    id = (int)maDoAnObj;
                }
				Debug.WriteLine(id);
				Debug.WriteLine(tenDoAn);
				Debug.WriteLine(gia);
				Debug.WriteLine(loai);
				Debug.WriteLine(trangthai);

				DoAn existingDoAn = db.DoAns.SingleOrDefault(d => d.MaDoAn == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }

                existingDoAn.TenDoAn = tenDoAn;
                existingDoAn.Gia = gia;
                existingDoAn.Loai = loai;
                existingDoAn.TrangThai = trangthai;

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Food", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
            // Lấy giá trị từ form
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTL(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string tenDoAn = formCollection["TenTheLoai"];

                // Xử lý các bước cập nhật thông tin đối tượng
                int id = 0; // Giá trị mặc định nếu Session["MaDoAn"] là null
                object maDoAnObj = Session["MaTL"];
                if (maDoAnObj != null && maDoAnObj is int)
                {
                    id = (int)maDoAnObj;
                }
                TheLoai existingDoAn = db.TheLoais.SingleOrDefault(d => d.MaTheLoai == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }

                existingDoAn.TenTheLoai = tenDoAn;

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("TheLoai", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
            // Lấy giá trị từ form
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string cv = formCollection["ChucDanh"];
                string u = formCollection["UserName"] != null ? formCollection["UserName"].ToString() : "";
                // Xử lý các bước cập nhật thông tin đối tượng
                TaiKhoan existingDoAn = db.TaiKhoans.SingleOrDefault(tk => tk.Username.Contains(u));

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }

                existingDoAn.ChucDanh = cv;

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Account", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
            // Lấy giá trị từ form
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVC(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string u = formCollection["LoaiGiamGia"];
                int p = Convert.ToInt32(formCollection["GiamGia"]);
                int cv = Convert.ToInt32(formCollection["Diem"]);
                int s = Convert.ToInt32(formCollection["sl"]);
                // Xử lý các bước cập nhật thông tin đối tượng
                int id = 0; // Giá trị mặc định nếu Session["MaDoAn"] là null
                object maDoAnObj = Session["VC"];
                if (maDoAnObj != null && maDoAnObj is int)
                {
                    id = (int)maDoAnObj;
                }
                Debug.WriteLine(id);
                Debug.WriteLine(u);
                Debug.WriteLine(p);
                Debug.WriteLine(cv);
                Debug.WriteLine(s);
                LoaiVoucher existingDoAn = db.LoaiVouchers.SingleOrDefault(tk => tk.id == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }

                existingDoAn.LoaiGiamGia = u;
                existingDoAn.GiamGia = p;
                existingDoAn.Diem = cv;
                existingDoAn.sl = s;

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Voucher", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
            // Lấy giá trị từ form

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKH(FormCollection formCollection)
        {

            if (Session["admin"] != null)
            {
                string t = formCollection["Ten"];
                string sdt = formCollection["SDT"];
                string e = formCollection["Email"];
                string ct = formCollection["ThongTinChiTieu"];
                int cd = int.TryParse(formCollection["CapDoThe"], out cd) ? cd : 0;
                string mt = formCollection["MaThe"];

                int id = (int)Session["MaKH"];
                // Xử lý các bước cập nhật thông tin đối tượng
                ThongTinCaNhan existingDoAn = db.ThongTinCaNhans.SingleOrDefault(tk => tk.MaKH == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }
                existingDoAn.Ten = t;
                existingDoAn.SDT = sdt;
                existingDoAn.Email = e;

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Infor", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
            // Lấy giá trị từ form
         
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNuoc(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string tenNuoc = formCollection["TenNuoc"];
                decimal gia = Convert.ToDecimal(formCollection["Gia"]);
                string loai = formCollection["Loai"];

                // Xử lý các bước cập nhật thông tin đối tượng
                int id = 0; // Giá trị mặc định nếu Session["MaDoAn"] là null
                object maDoAnObj = Session["MaNuoc"];
                if (maDoAnObj != null && maDoAnObj is int)
                {
                    id = (int)maDoAnObj;
                }
                Nuoc existingDoAn = db.Nuocs.SingleOrDefault(d => d.MaNuoc == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }

                existingDoAn.TenNuoc = tenNuoc;
                existingDoAn.Gia = gia;
                existingDoAn.Loai = loai;

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Nuoc", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
            // Lấy giá trị từ form
           
        }
        public ActionResult DLNuoc (int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.Nuocs.SingleOrDefault(u => u.MaNuoc == itemId);
                nuoc.TrangThai = "Ngưng bán";
                db.SubmitChanges();
                return RedirectToAction("Nuoc", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
 
        }
        public ActionResult DLS(int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.Sliders.SingleOrDefault(u => u.ma == itemId);
                db.Sliders.DeleteOnSubmit(nuoc);
                db.SubmitChanges();
                return RedirectToAction("Slider", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        public ActionResult DLVC(int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.LoaiVouchers.SingleOrDefault(u => u.id == itemId);
                db.LoaiVouchers.DeleteOnSubmit(nuoc);
                db.SubmitChanges();
                return RedirectToAction("Voucher", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        public ActionResult DLN(int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.Tintucs.SingleOrDefault(u => u.Matin == itemId);
                db.Tintucs.DeleteOnSubmit(nuoc);
                db.SubmitChanges();
                return RedirectToAction("News", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }


        }
        public ActionResult DLTK(string itemId)
        {
            if (Session["admin"] != null)
            {
                var tk = db.TaiKhoans.SingleOrDefault(u => u.Username.Contains(itemId));
                db.TaiKhoans.DeleteOnSubmit(tk);
                db.SubmitChanges();
                return RedirectToAction("Account", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }


        }
        public ActionResult DLCB(int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.Combos.SingleOrDefault(u => u.MaCB == itemId);
                nuoc.TrangThai = "Ngưng bán";
                db.SubmitChanges();
                return RedirectToAction("Combo", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        public ActionResult DLMovie(int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.Phims.SingleOrDefault(u => u.MaPhim == itemId);
                nuoc.TrangThai = "Ngưng chiếu";
                db.SubmitChanges();
                return RedirectToAction("Movie", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        public ActionResult DLPhong(int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.Phongs.SingleOrDefault(u => u.MaPhong == itemId);
                nuoc.TrangThai = "Ngưng sử dụng";
                db.SubmitChanges();
                return RedirectToAction("Room", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        public ActionResult DLFood(int itemId)
        {
            if (Session["admin"] != null)
            {
                var nuoc = db.DoAns.SingleOrDefault(u => u.MaDoAn == itemId);
                nuoc.TrangThai = "Ngưng bán";
                db.SubmitChanges();
                return RedirectToAction("Nuoc", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        private string SaveImage(HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var directoryPath = Server.MapPath("~/IMG/");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Path.GetFileName(image.FileName);
                var imagePath = Path.Combine(directoryPath, fileName);
                image.SaveAs(imagePath);

                return fileName;
            }

            return null;
        }
        public ActionResult AddLichChieu(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string NgonNgu = formCollection["NgonNgu"];
                string gioChieuString = formCollection["GioChieu"].ToString();
                TimeSpan gioChieu = TimeSpan.TryParse(gioChieuString, out gioChieu) ? gioChieu : TimeSpan.Zero;
                string gioKetthucString = formCollection["GioKetThuc"].ToString();
                TimeSpan gioKetthuc = TimeSpan.TryParse(gioKetthucString, out gioKetthuc) ? gioKetthuc : TimeSpan.Zero;
                string phongString = formCollection["Phong"].ToString();
                int phong = int.TryParse(phongString, out phong) ? phong : 0;
                string MaPhim = formCollection["MaPhim"].ToString();
                int phim = int.TryParse(MaPhim, out phim) ? phim : 0;
                DateTime Ngay = DateTime.Parse(formCollection["NgayChieu"]);

                LichChieu d = new LichChieu
                {
                    NgonNgu = NgonNgu,
                    GioChieu = gioChieu,
                    GioKetThuc = gioKetthuc,
                    Phong = phong,
                    MaPhim = phim,
                    NgayChieu = Ngay
                };

                db.LichChieus.InsertOnSubmit(d);
                db.SubmitChanges();


                return RedirectToAction("Showtimes", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }

        }
        [HttpPost]
        public JsonResult CheckScheduleConflict(LichChieu model)
        {
            var isConflict = IsScheduleConflict(model);

            return Json(new { isConflict });
        }

        private bool IsScheduleConflict(LichChieu model)
        {
            var existingSchedule = db.LichChieus
                .FirstOrDefault(s =>
                    s.NgayChieu == model.NgayChieu &&
                    s.Phong == model.Phong &&
                    (
                        (model.GioChieu >= s.GioChieu && model.GioChieu < s.GioKetThuc) ||
                        (model.GioKetThuc > s.GioChieu && model.GioKetThuc <= s.GioKetThuc)
                    )
                );

            return existingSchedule != null;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLC(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string NgonNgu = formCollection["NgonNgu"];
                string gioChieuString = formCollection["GioChieu"].ToString();
                TimeSpan gioChieu = TimeSpan.TryParse(gioChieuString, out gioChieu) ? gioChieu : TimeSpan.Zero;
                string gioKetthucString = formCollection["GioKetThuc"].ToString();
                TimeSpan gioKetthuc = TimeSpan.TryParse(gioKetthucString, out gioKetthuc) ? gioKetthuc : TimeSpan.Zero;
                string phongString = formCollection["Phong"].ToString();
                int phong = int.TryParse(phongString, out phong) ? phong : 0;
                string MaPhim = formCollection["MaPhim"].ToString();
                int phim = int.TryParse(MaPhim, out phim) ? phim : 0;
                int id = (int)Session["MaLichChieu"];
                DateTime Ngay = DateTime.Parse(formCollection["NgayChieu"]);

                LichChieu existingDoAn = db.LichChieus.SingleOrDefault(d => d.MaChieuPhim == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }
                existingDoAn.NgonNgu = NgonNgu;
                existingDoAn.GioChieu = gioChieu;
                existingDoAn.GioKetThuc = gioKetthuc;
                existingDoAn.Phong = phong;
                existingDoAn.MaPhim = phim;
                existingDoAn.NgayChieu = Ngay;


                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Showtimes", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhong(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string TenPhong = formCollection["TenPhong"];
                string SoChoNgoi = formCollection["SoChoNgoi"].ToString();
                int sochongoi = int.TryParse(SoChoNgoi, out sochongoi) ? sochongoi : 0;


                Phong d = new Phong
                {
                    TenPhong = TenPhong,
                    SoChoNgoi = sochongoi,
                    TrangThai = "Hoạt động"
                };

                db.Phongs.InsertOnSubmit(d);
                db.SubmitChanges();


                return RedirectToAction("Room", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhong(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string TenPhong = formCollection["TenPhong"];
                string SoChoNgoi = formCollection["SoChoNgoi"].ToString();
                int sochongoi = int.TryParse(SoChoNgoi, out sochongoi) ? sochongoi : 0;

                int id = (int)Session["MaPhong"];
                Phong existingDoAn = db.Phongs.SingleOrDefault(d => d.MaPhong == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }
                existingDoAn.TenPhong = TenPhong;
                existingDoAn.SoChoNgoi = sochongoi;


                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Room", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGhe(FormCollection formCollection)
        {
            if (Session["admin"] != null)
            {
                string TenPhong = formCollection["TenGhe"];
                string SoChoNgoi = formCollection["LoaiGhe"];
                decimal gia = Decimal.TryParse(formCollection["Gia"].ToString(), out gia) ? gia : 0;
                int id = (int)Session["MaGhe"];
                Ghe existingDoAn = db.Ghes.SingleOrDefault(d => d.MaGhe == id);

                if (existingDoAn == null)
                {
                    return HttpNotFound();
                }
                existingDoAn.TenGhe = TenPhong;
                existingDoAn.LoaiGhe = SoChoNgoi;
                existingDoAn.GiaTien = gia;

                db.SubmitChanges();

                // Chuyển hướng đến trang chi tiết hoặc danh sách đối tượng
                return RedirectToAction("Chair", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { area = "Admin" });
            }
           
        }
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddGhe(FormCollection formCollection)
		{
			if (Session["admin"] != null)
			{
				int gia = int.Parse(formCollection["gia"]);
				string TenGhe = formCollection["TenGhe"];
				string LoaiGhe = formCollection["LoaiGhe"];
				string MaPhong = formCollection["MaPhong"].ToString();
				int phong = int.TryParse(MaPhong, out phong) ? phong : 0;
				var existingTheLoai = db.Ghes.FirstOrDefault(tl => tl.TenGhe == TenGhe);
				if (existingTheLoai != null)
				{
					TempData["ErrorMessage"] = "Tên ghế " + TenGhe + " này đã có";
				}
                else
                {
					Ghe d = new Ghe
					{
						GiaTien = gia,
						TrangThai = "Trống",
						MaPhong = phong,
						LoaiGhe = LoaiGhe,
						TenGhe = TenGhe,
					};

					db.Ghes.InsertOnSubmit(d);
					db.SubmitChanges();

					TempData["SuccessMessage"] = "Tên ghế " + TenGhe + " này đã được thêm";

				}




				return RedirectToAction("Chair", "Admin", new { area = "Admin" });
			}
			else
			{
				return RedirectToAction("Login", "Admin", new { area = "Admin" });
			}

		}

		[HttpPost]
		public string SearchCombo(string searchString)
		{
			try
			{
				// Lọc danh sách combo dựa trên tên combo
				var combos = db.Combos
								.Where(c => c.TenCB.Contains(searchString))
								.Select(c => new {
									c.MaCB,
									c.TenCB,
									c.Gia
								})
								.ToList();

				// Chuyển danh sách combo thành chuỗi JSON
				string jsonCombos = JsonConvert.SerializeObject(combos);

				// Trả về chuỗi JSON
				return jsonCombos;
			}
			catch (Exception ex)
			{
				// Xử lý khi có lỗi xảy ra trong quá trình tìm kiếm combo
				Console.WriteLine("Lỗi tìm kiếm combo: " + ex.Message);
				return "{\"error\": \"Lỗi tìm kiếm combo: " + ex.Message + "\"}";
			}
		}


	}

}