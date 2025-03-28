using doan1.Models;
using System;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Management;

namespace doan1.Controllers
{
    public class HomeController : Controller
    {
        DataBaseDataContext db = new DataBaseDataContext("");
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky(string succes, string err)
        {
            ViewData["succes"] = null;
            ViewData["err"] = null;
            if (succes != null){
                ViewData["succes"] = succes;
            }
            if(err != null)
            {
                ViewData["err"] = err;
            }
            return View();
        }
       
        public ActionResult Login(string succes, string err)
        {
            ViewData["succes"] = null;
            ViewData["err"] = null;
            if (succes != null)
            {
                ViewData["succes"] = succes;
            }
            if (err != null)
            {
                ViewData["err"] = err;
            }
            return View();
           
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
        public ActionResult Login(FormCollection formCollection)
        {
            string emailOrPhone = formCollection["loginInput"];
            string password = HashSHA256(formCollection["passwordInput"]);
            // Kiểm tra thông tin đăng nhập trong cơ sở dữ liệu
            var user = db.ThongTinCaNhans
                .SingleOrDefault(u => (u.Email == emailOrPhone || u.SDT == emailOrPhone) && u.Matkhau == password);

            if (user != null)
            {
                Session["KH"] = user;
                if (Session["PreviousPage"] != null)
                {
                    var previousPage = Session["PreviousPage"].ToString();
                    Session.Remove("PreviousPage"); // Xóa thông tin trang trước đó sau khi sử dụng
                    return Redirect(previousPage);
                }

                // Nếu không có trang trước đó, chuyển hướng về trang mặc định
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Đăng nhập không thành công, trả về thông báo lỗi
                return RedirectToAction("Login", new { err = "Thông tin đăng nhập không đúng" });
            }
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection form)
        {
            string hoTen = form["HoTen"];
            string soDienThoai = form["SoDienThoai"];
            string email = form["Email"];
            string matKhau = form["MatKhau"];

            if (db.ThongTinCaNhans.Any(x => x.SDT == soDienThoai))
            {
                return RedirectToAction("Dangky", new { err = "Số điện thoại đã tồn tại" });
            }

            // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu hay chưa
            if (db.ThongTinCaNhans.Any(x => x.Email == email))
            {
                return RedirectToAction("Dangky", new { err = "Email đã tồn tại" });
            }
            // Kiểm tra và xử lý dữ liệu nếu cần
            if (!string.IsNullOrEmpty(hoTen) && !string.IsNullOrEmpty(soDienThoai) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(matKhau))
            {
                // Mã hóa mật khẩu bằng SHA-256
                string hashedPassword = HashSHA256(matKhau);

                var thongTinCaNhan = new ThongTinCaNhan
                {
                    Ten = hoTen,
                    SDT = soDienThoai,
                    Email = email,
                    Matkhau = hashedPassword,
                    ThongTinChiTieu = 0,
                    DiemThuong = 0
                };

                db.ThongTinCaNhans.InsertOnSubmit(thongTinCaNhan);
                db.SubmitChanges();

                return RedirectToAction("Login", new { succes = "Đăng ký thành công" }); // Chuyển hướng sau khi đăng ký thành công
            }
            return RedirectToAction("Dangky", new { err = "Vui lòng nhập đầy đủ thông tin" });
        }
        [HttpPost]
        public JsonResult CheckUserExistence(string inputType, string inputInfo)
        {
            // Kiểm tra sự tồn tại của người dùng trong bảng ThongTinCaNhan
            bool userExists = CheckUserExists(inputType, inputInfo);

            return Json(new { exist = userExists });
        }

        // Hàm kiểm tra sự tồn tại của người dùng
        private bool CheckUserExists(string inputType, string inputInfo)
        {

            // Kiểm tra sự tồn tại của người dùng với số điện thoại hoặc email
            if (inputType.ToLower() == "phone")
            {
                // Kiểm tra số điện thoại
                return db.ThongTinCaNhans.Any(u => u.SDT == inputInfo);
            }
            else if (inputType.ToLower() == "email")
            {
                // Kiểm tra email
                return db.ThongTinCaNhans.Any(u => u.Email == inputInfo);
            }

            // Trường hợp không xác định loại thông tin, trả về false
            return false;

        }
    }
}