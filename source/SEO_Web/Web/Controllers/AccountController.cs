using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using Web.Custom;
using Web.FwCore;
using Business.Business;
using Model.DBTool;
using Business.CommonBusiness;
using Web.FwCore.Factory;
//using Web.Filter;
using Business.CommonHelper;

/*
 * Notice: Do not modify this file. For authentication please edit in Web.Custom.Authenticate
 */

namespace Web.Controllers
{
    public class AccountController : BaseController
    {
        DmNguoidungBusiness DmNguoidungBusiness;

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterLogin()
        {
            string Brower = Request.Browser.Browser.ToString();
            int Version = Request.Browser.MajorVersion;
            if (Brower == "IE")
            {
                if (Version >= 4 && Version < 8)
                {
                    return RedirectToAction("InstallBrower", "Home");
                }
            }
            if (Request.IsAuthenticated)
            {

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        //[ActionAudit]
        public ActionResult Login(string username, string password, string captcha, string ReturnUrl)
        {
            if (SessionManager.HasValue("Captcha"))
            {

                if (string.IsNullOrEmpty(captcha))
                {
                    ViewData["Message"] = "Bạn chưa nhập mã xác nhận";
                    ViewData["ShowCaptcha"] = 1;
                    return View();
                }
                string scaptcha = (string)SessionManager.GetValue("Captcha");
                if (captcha.Length != 7 || captcha != scaptcha)
                {
                    ViewData["Message"] = "Mã xác nhận không đúng";
                    ViewData["ShowCaptcha"] = 1;
                }
            }

            try
            {
                if (Authenticate.ValidateLogin(username, password))
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    SessionManager.Remove("LoginFailCount");
                    SessionManager.Remove("Captcha");
                    //Comment by namdv
                    //GetContext().Database.ExecuteSqlCommand("update dm_nguoidung set ngaysua = sysdate where TENDANGNHAP = :1 and trangthai = 1", username);


                    string decodedUrl = "";
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        decodedUrl = Server.UrlDecode(ReturnUrl);
                    }

                    UserInfoBO userInfo = ((UserInfoBO)SessionManager.GetUserInfo());
                    //if (userInfo.ListRole != null && userInfo.ListRole.Count > 1)// nếu người dùng có nhiều vai trò
                    //{
                    //    if (userInfo.ListNguoiDung_Vaitro != null)
                    //    {
                    //        var RoleDefault = userInfo.ListNguoiDung_Vaitro.Where(x => x.ROLE_DEFAULT == true).FirstOrDefault();
                    //        if (RoleDefault != null)
                    //        {
                    //            if (Url.IsLocalUrl(decodedUrl))
                    //            {
                    //                return Redirect(decodedUrl);
                    //            }
                    //            else
                    //            {
                    //                return RedirectToAction("Index", "Home");
                    //            }
                    //        }

                    //    }
                    //    return RedirectToAction("OptionRole", "Home", new { ReturnUrl = ReturnUrl });
                    //}
                    //if (userInfo.RoleID < 1 && userInfo.ListRoleId.Count == 0)
                    //{
                    //    return RedirectToAction("UnAuthor", "Home");
                    //}

                    //duynt comment to redirect index page
                    //if (Url.IsLocalUrl(decodedUrl))
                    //{
                    //    return Redirect(decodedUrl);
                    //}
                    return Redirect("/BaiVietArea/BaiViet/index");
                    //return RedirectToAction("Index", "Home");
                }

                else
                {
                    object LoginFailCount = SessionManager.GetValue("LoginFailCount");
                    int count = LoginFailCount == null ? 0 : (int)LoginFailCount;
                    count++;
                    SessionManager.SetValue("LoginFailCount", count);
                    if (count > 5)
                    {
                        ViewData["ShowCaptcha"] = 1;
                    }
                    ViewData["Message"] = "Sai tên đăng nhập hoặc mật khẩu";
                    if (SessionManager.GetUserInfo() != null)
                    {
                        FormsAuthentication.SignOut();
                        SessionManager.Clear();
                        Session.Abandon();
                        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                    }
                    return View();

                }
            }
            catch
            {
                ViewData["Message"] = "Sai tên đăng nhập hoặc mật khẩu";
                return View();
            }

        }



        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult Register(string SoCMND, string Email, string PhoneNum, string ConfirmCode)
        //{
        //    DmNguoidungBusiness dmNguoiDungBussiness = new DmNguoidungBusiness(GetContext());
        //    string scaptcha = (string)SessionManager.GetValue("Captcha");
        //    try
        //    {
        //        if ((string)SessionManager.GetValue("CMNDExist") != SoCMND && dmNguoiDungBussiness.isCmndExist(SoCMND))
        //        {
        //            ViewBag.cmnd = SoCMND;
        //            ViewBag.isCmndExist = true;
        //            SessionManager.SetValue("CMNDExist", SoCMND);
        //            return View();
        //        }
        //        else if (!scaptcha.Equals(ConfirmCode))
        //        {
        //            SessionManager.Remove("CMNDExist");
        //            ViewBag.captchaNotMatch = true;
        //            return View();
        //        }
        //        else
        //        {
        //            SessionManager.Remove("CMNDExist");
        //            DKNguoiDung dkNguoiDung = new DKNguoiDung();
        //            dkNguoiDung.cmnd = SoCMND;
        //            dkNguoiDung.email = Email;
        //            dkNguoiDung.phoneNum = PhoneNum;
        //            dkNguoiDung.soPhieuDK = String.Format("{0:000000000}", dmNguoiDungBussiness.getSequencePhieuDK());
        //            SessionManager.SetValue("DKNguoiDung", dkNguoiDung);
        //            return RedirectToAction("Register", "RegisterArea");
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }

        //}

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            SessionManager.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public FileContentResult GetCaptcha()
        {
            FileContentResult captchaImg = null;
            string[] fonts = { "Arial", "Verdana", "Times New Roman", "Tahoma" };
            const int LENGTH = 7;
            // chuỗi để lấy các kí tự sẽ sử dụng cho captcha
            const string chars = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            using (MemoryStream memoryStream = new MemoryStream())
            {

                using (Bitmap bmp = new Bitmap(220, 38))
                {

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        // Tạo nền cho ảnh dạng sóng
                        HatchBrush brush = new HatchBrush(HatchStyle.DashedDownwardDiagonal, Color.Wheat, Color.Silver);
                        g.FillRegion(brush, g.Clip);
                        // Lưu chuỗi captcha trong quá trình tạo
                        StringBuilder strCaptcha = new StringBuilder();
                        Random rand = new Random();
                        float f = 0;
                        for (int i = 0; i < LENGTH; i++)
                        {
                            // Lấy kí tự ngẫu nhiên từ mảng chars
                            string str = chars[rand.Next(chars.Length)].ToString();
                            strCaptcha.Append(str);
                            // Tạo font với tên font ngẫu nhiên chọn từ mảng fonts
                            Font font = new Font(fonts[rand.Next(fonts.Length)], 16, FontStyle.Italic | FontStyle.Bold);
                            // Lấy kích thước của kí tự
                            SizeF size = g.MeasureString(str, font);
                            f = f + size.Width;
                            // Vẽ kí tự đó ra ảnh tại vị trí tăng dần theo i, vị trí top ngẫu nhiên                            
                            g.DrawString(str, font,
                            Brushes.Blue, f + i * 3, rand.Next(2, 10), StringFormat.GenericDefault);
                            font.Dispose();
                        }
                        // Lưu captcha vào session
                        SessionManager.SetValue("Captcha", strCaptcha.ToString());
                        // Ghi ảnh trực tiếp ra luồng xuất theo định dạng gif                      
                        bmp.Save(memoryStream, ImageFormat.Gif);
                        captchaImg = this.File(memoryStream.GetBuffer(), "image/Jpeg");
                    }
                }
            }
            return captchaImg;
        }

        //[AllowAnonymous]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return PartialView("Password");
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult ChangePass(string NewPassword, string key)
        {
            string str = "success";
            DmNguoidungBusiness DmNguoidungBusiness = Get<DmNguoidungBusiness>();

            DM_NGUOIDUNG user = DmNguoidungBusiness.All.Where(p => p.CODERESETPASS == key).FirstOrDefault();

            if (user != null)
            {
                var mahoa = MaHoaMatKhau.GenerateRandomString(5);
                user.MAHOA_MK = mahoa;
                user.MATKHAU = MaHoaMatKhau.Encode_Data(NewPassword + mahoa);
                //user.CODERESETPASS = string.Empty;

                DmNguoidungBusiness.Update(user, NewPassword);
                DmNguoidungBusiness.Save();
                FormsAuthentication.SignOut();
                SessionManager.Clear();
                Session.Abandon();
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = "Fail" }, JsonRequestBehavior.AllowGet);
        }
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult ExecuteChangePass(string OldPassword, string NewPassword, string ConfirmPassWord)
        {
            string str = "success";
            if (string.IsNullOrEmpty(OldPassword))
            {
                str = "Bạn chưa nhập mật khẩu cũ";
                return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                str = "Bạn chưa nhập mật khẩu mới";
                return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(ConfirmPassWord))
            {
                str = "Bạn chưa nhập ô xác nhận mật khẩu mật khẩu";
                return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
            }
            if (OldPassword.Equals(NewPassword))
            {
                str = "Mật khẩu mới không được trùng mật khẩu cũ";
                return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
            }
            if (!ConfirmPassWord.Equals(NewPassword))
            {
                str = "Mật khẩu mới và xác nhận mật khẩu không trùng nhau";
                return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
            }

            UserInfoBO userInfo = ((UserInfoBO)SessionManager.GetUserInfo());
            DmNguoidungBusiness DmNguoidungBusiness = new Business.Business.DmNguoidungBusiness(this.GetContext());
            DM_NGUOIDUNG user = DmNguoidungBusiness.Find(userInfo.UserID);

            if (!VtEncodeData.Encode_Data(OldPassword + userInfo.PasswordSalt).Equals(userInfo.Password))
            {
                str = "Mật khẩu cũ không đúng";
                return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
            }
            user.MATKHAU = VtEncodeData.Encode_Data(NewPassword + userInfo.PasswordSalt);
            DmNguoidungBusiness.Update(user, NewPassword);
            DmNguoidungBusiness.Save();
            FormsAuthentication.SignOut();
            SessionManager.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            return Json(new { Message = str }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public FileContentResult GetForgetCaptcha(string name)
        {
            FileContentResult captchaImg = null;
            string[] fonts = { "Arial", "Verdana", "Times New Roman", "Tahoma" };
            const int LENGTH = 7;
            // chuỗi để lấy các kí tự sẽ sử dụng cho captcha
            const string chars = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            using (MemoryStream memoryStream = new MemoryStream())
            {

                using (Bitmap bmp = new Bitmap(220, 38))
                {

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        // Tạo nền cho ảnh dạng sóng
                        HatchBrush brush = new HatchBrush(HatchStyle.DashedDownwardDiagonal, Color.Wheat, Color.Silver);
                        g.FillRegion(brush, g.Clip);
                        // Lưu chuỗi captcha trong quá trình tạo
                        StringBuilder strCaptcha = new StringBuilder();
                        Random rand = new Random();
                        float f = 0;
                        for (int i = 0; i < LENGTH; i++)
                        {
                            // Lấy kí tự ngẫu nhiên từ mảng chars
                            string str = chars[rand.Next(chars.Length)].ToString();
                            strCaptcha.Append(str);
                            // Tạo font với tên font ngẫu nhiên chọn từ mảng fonts
                            Font font = new Font(fonts[rand.Next(fonts.Length)], 16, FontStyle.Italic | FontStyle.Bold);
                            // Lấy kích thước của kí tự
                            SizeF size = g.MeasureString(str, font);
                            f = f + size.Width;
                            // Vẽ kí tự đó ra ảnh tại vị trí tăng dần theo i, vị trí top ngẫu nhiên                            
                            g.DrawString(str, font,
                            Brushes.Blue, f + i * 3, rand.Next(2, 10), StringFormat.GenericDefault);
                            font.Dispose();
                        }
                        // Lưu captcha vào session
                        if (string.IsNullOrEmpty(name))
                            SessionManager.SetValue("ForgetCaptcha", strCaptcha.ToString());
                        else
                            SessionManager.SetValue(name, strCaptcha.ToString());
                        // Ghi ảnh trực tiếp ra luồng xuất theo định dạng gif                      
                        bmp.Save(memoryStream, ImageFormat.Gif);
                        captchaImg = this.File(memoryStream.GetBuffer(), "image/Jpeg");
                    }
                }
            }
            return captchaImg;
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View("ResetPassword");
        }

    }
}
