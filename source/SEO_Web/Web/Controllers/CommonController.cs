using Business.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Custom;
using Business.CommonBusiness;
using Model.DBTool;
using Web.FwCore;
using Web.Models;
using Web.Common;
using System.Web.Configuration;
using System.Collections;
using Business.CommonHelper;
using System.Net.Mail;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Web.Controllers
{
    public class CommonController : BaseController
    {

        DmNguoidungBusiness NguoiDungBs;


        //HscbChuyenNgachBusiness HscbChuyenNgachBusiness;



        DmChucnangBusiness DmChucnangBusiness;

        private string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
        private List<string> arrFolder = new List<string>();

        private UserInfoBO currentUser;
        private long userID;
        private bool isCucTruong;
        private bool isTruongDonVi;
        private NguoiDungVaiTroBusiness NguoiDungVaiTroBusiness;


        HnHelper hn = new HnHelper();
        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetListPlaces(int provinceId, int districtId, int wardId, int type = 1)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            switch (type)
            {
                case 1:
                    HuyenBusiness huyenBusiness = Get<HuyenBusiness>();
                    result = huyenBusiness.All.Where(x => x.TINH_ID == provinceId)
                        .Select(x => new SelectListItem()
                    {
                        Value = x.HUYEN_ID.ToString(),
                        Text = x.TENHUYEN
                    }).ToList();
                    break;
                case 2:
                    XaBusiness xaBusiness = Get<XaBusiness>();
                    result = xaBusiness.All.Where(x => x.HUYEN_ID == districtId)
                        .Select(x => new SelectListItem()
                        {
                            Value = x.XA_ID.ToString(),
                            Text = x.TENXA
                        }).ToList();
                    break;
                default:
                    break;
            }
            return Json(result);
        }
        //[AllowAnonymous]
        //[HttpPost]
        //public JsonResult LoadDonVi(int TINH_ID, int HUYEN_ID, int XA_ID)
        //{
        //    CosoBusiness cosoBusiness = Get<CosoBusiness>();
        //    var ds_coso = cosoBusiness.All
        //        .Where(c => c.TINH_ID == TINH_ID && c.HUYEN_ID == HUYEN_ID && c.XA_ID == XA_ID);
        //    if (ds_coso.Count() > 0)
        //    {
        //        var result = ds_coso.Select(c => new
        //        {
        //            c.COSO_ID,
        //            c.TENCOSO
        //        });
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(false);
        //    }
        //}
        /// <summary>
        /// Load danh sách đơn vị trung ương hoặc bv trung ương
        /// </summary>
        /// <param name="HINHTHUC"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost]
        //public JsonResult LoadDonViTW(short? HINHTHUC)
        //{
        //    CosoBusiness cosoBusiness = Get<CosoBusiness>();
        //    var ds_coso = cosoBusiness.All.Where(x => x.HINHTHUC == HINHTHUC && x.IS_ACTIVE == 0);
        //    if (ds_coso.Count() > 0)
        //    {
        //        var result = ds_coso.Select(c => new
        //        {
        //            c.COSO_ID,
        //            c.TENCOSO
        //        });
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(false);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">TRA VE DS CHUCNANG CON HOAC CHUC NANG CHA</param>
        /// <returns></returns>
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[HttpGet]
        [HttpPost]
        public JsonResult LoadChucNang(int? parent, decimal? role)
        {
            if (parent.HasValue && role.HasValue)
            {
                DmChucnangBusiness DmChucnangBusiness = Get<DmChucnangBusiness>();
                // LOAD CAC CHUC NANG CON
                var CN_result = Get<VaitroChucnangBusiness>().All.Where(x => x.DM_CHUCNANG.CHUCNANG_CHA == parent && x.TRANGTHAI == 1 && x.DM_VAITRO_ID == role && x.DM_CHUCNANG.TRANGTHAI == 1).Select(o => new ChucNangBO
                {
                    DM_CHUCNANG_ID = o.DM_CHUCNANG_ID.Value,
                    TEN_CHUCNANG = o.DM_CHUCNANG.TEN_CHUCNANG,
                    URL = o.DM_CHUCNANG.URL,
                    TT_HIENTHI = o.DM_CHUCNANG.TT_HIENTHI,
                    CHUCNANG_CHA = o.DM_CHUCNANG.CHUCNANG_CHA,
                }).OrderBy(o => o.TT_HIENTHI).ThenBy(o => o.DM_CHUCNANG_ID).ToList();
                if (CN_result.Count > 0)
                {
                    return Json(CN_result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // LAY CHUC NANG CHA
                    var result = DmChucnangBusiness.All.Where(x => x.DM_CHUCNANG_ID == parent && x.TRANGTHAI == 1).Select(x => new
                    {
                        x.DM_CHUCNANG_ID,
                        x.CHUCNANG_CHA,
                        x.TEN_CHUCNANG,
                        x.URL
                    });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false);
        }
        public JsonResult LoadDiaDiemTinh(int TINH_ID, int HUYEN_ID, int TYPE)
        {
            //Load Huyện theo Tỉnh
            if (TYPE == 1)
            {
                HuyenBusiness HuyenBusiness = Get<HuyenBusiness>();
                // lấy danh sách tỉnh
                var huyenItems = HuyenBusiness.All.Where(h => h.TINH_ID == TINH_ID);
                if (huyenItems.Count() > 0) // kiểm tra dữ liệu
                {
                    // lấy dữ liệu ra và trả về dưới dạng Json
                    var result = huyenItems.Select(h => new
                    {
                        h.HUYEN_ID,
                        h.TENHUYEN
                    });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // không có dữ liệu, trả về lỗi
                    return Json(false);
                }
            }
            return Json(true);
        }



        public PartialViewResult GetTopMenu()
        {
            return PartialView("GetTopMenu");
        }


        public JsonResult DeleteFile(int id)
        {
            try
            {
                string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
                var TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
                if (id > 0 && id.GetType() == typeof(int))
                {
                    TAILIEUDINHKEM taikieu = TaiLieuDinhKemBusiness.Find(id);
                    if (taikieu != null)
                    {
                        string contentType = taikieu.DINHDANG_FILE;
                        string path = URLPath + taikieu.DUONGDAN_FILE;
                        System.IO.File.Delete(path);
                    }
                    TaiLieuDinhKemBusiness.Delete(id);
                    TaiLieuDinhKemBusiness.Save();
                }
                return Json("1");
            }
            catch
            {
                return Json("0");
            }
        }
        public FileResult DownloadFile(int id)
        {
            string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
            var TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            if (id > 0 && id.GetType() == typeof(int))
            {
                TAILIEUDINHKEM taikieu = TaiLieuDinhKemBusiness.Find(id);
                if (taikieu != null)
                {
                    string contentType = taikieu.DINHDANG_FILE;
                    string path = URLPath + taikieu.DUONGDAN_FILE;
                    var filename = path.Split('\\');
                    string fileSave = filename[filename.Count() - 1];
                    return File(path, contentType, fileSave);
                }
            }
            return null;
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult LayNguoiDungByEmail(string email)
        {
            var dmNguoidungBussiness = Get<DmNguoidungBusiness>();
            DM_NGUOIDUNG nd = dmNguoidungBussiness.All.Where(ng => ng.EMAIL.ToLower() == email.ToLower()).FirstOrDefault();
            if (nd != null)
            {
                var key = MaHoaMatKhau.Encode_Data(nd.DM_NGUOIDUNG_ID + CreateStringRandom() + "BTNQUOCGIA");
                return Json(new { key = key }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        private string CreateStringRandom()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SendMail(string to, string subject, string captcha, bool isHtml = true)
        {
            if (SessionManager.HasValue("ForgetCaptcha"))
            {
                if (string.IsNullOrEmpty(captcha))
                {
                    return Json(new { successful = false, chapchar = true, message = Web.Common.UIConstant.MESSAGE_CONFIMATION_EMPTY }, JsonRequestBehavior.AllowGet);
                }
                string scaptcha = (string)SessionManager.GetValue("ForgetCaptcha");
                if (captcha.Length != 7 || captcha != scaptcha)
                {
                    return Json(new { successful = false, chapchar = true, message = Web.Common.UIConstant.MESSAGE_CONFIMATION_NOT_CORRECT }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { successful = false, chapchar = true, message = Web.Common.UIConstant.MESSAGE_CONFIMATION_EMPTY }, JsonRequestBehavior.AllowGet);
            }
            var fromAddress = new MailAddress(Constant.FROMEMAIL, Constant.NAMEEMAIL);
            var toAddress = new MailAddress(to);
            const string fromPassword = Constant.PASSEMAIL;
            DmNguoidungBusiness BNguoiDung = Get<DmNguoidungBusiness>();
            var objNguoiDung = BNguoiDung.All.Where(o => o.EMAIL.ToLower() == to.ToLower()).FirstOrDefault();
            if (objNguoiDung == null)
                return Json(new { successful = false, chapchar = false, Web.Common.UIConstant.MESSAGE_EMAIL_NOT_EXIST }, JsonRequestBehavior.AllowGet);
            var key = MaHoaMatKhau.Encode_Data(objNguoiDung.DM_NGUOIDUNG_ID + CreateStringRandom() + "CUCTINHOCHOAVN");
            objNguoiDung.CODERESETPASS = key;
            BNguoiDung.Save(objNguoiDung);
            var smtp = new SmtpClient
            {
                Host = Constant.HOST,
                Port = Constant.PORT,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = "Bạn vui lòng ấn vào đường link sau để thay đổi lại mật khẩu của mình: <a href='" + GetSiteRoot() + "/Account/ResetPassword/" + key + "'>Nhấp vào đây</a>",
                IsBodyHtml = isHtml
            })
            {
                smtp.Send(message);
            }
            return Json(new { successful = true }, JsonRequestBehavior.AllowGet);
        }
        public static string GetSiteRoot()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;
            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";
            string sOut = protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + System.Web.HttpContext.Current.Request.ApplicationPath;
            if (sOut.EndsWith("/"))
            {
                sOut = sOut.Substring(0, sOut.Length - 1);
            }
            return sOut;
        }








        public PartialViewResult GetLeftMenu(string url)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (user.ListCN != null && user.ListCN.Count > 0)
            {
                var cn = user.ListCN.Where(o => o.URL.ToLower() == url.ToLower()).FirstOrDefault();
                if (cn != null)
                {
                    var list_cn = user.ListCN.Where(o => o.CHUCNANG_CHA == cn.CHUCNANG_CHA).OrderBy(o => o.TT_HIENTHI).ToList();
                    return PartialView("_GetLeftMenu", list_cn);
                }
            }
            return PartialView("_GetLeftMenu", new List<ChucNangBO>());
        }
        public PartialViewResult GetMenuChild()
        {
            MenuListChild model = new MenuListChild();
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            string currentArea = Request.RequestContext.RouteData.DataTokens["area"].ToString();
            List<ChucNangBO> ListChucNang = new List<ChucNangBO>();
            int? _ChucNangChaID = 0;
            string TenChucNangCha = string.Empty;
            if (!string.IsNullOrEmpty(currentArea))
            {
                _ChucNangChaID = UserInfo.ListCN.Where(o => o.URL.ToLower().Contains(currentArea.ToLower())).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                TenChucNangCha = UserInfo.ListCNFull.Where(o => o.DM_CHUCNANG_ID == _ChucNangChaID).Select(o => o.TEN_CHUCNANG).FirstOrDefault();
            }
            ListChucNang = ListMenuChucNang(_ChucNangChaID);
            model.TenChucNangCha = TenChucNangCha;
            model.ListChucNang = ListChucNang;
            return PartialView("_MenuListChild", model);
        }
        public List<ChucNangBO> ListMenuChucNang(int? ChucNangCha = 0)
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            List<ChucNangBO> ListChucNang = new List<ChucNangBO>();
            ListChucNang = UserInfo.ListCN.Where(o => o.CHUCNANG_CHA == ChucNangCha).ToList();
            for (int i = 0; i < ListChucNang.Count; i++)
            {
                ListChucNang.AddRange(ListMenuChucNang(ListChucNang[i].DM_CHUCNANG_ID));
            }
            return ListChucNang;
        }
        public PartialViewResult NhanSuMenuChild()
        {
            UserInfoBO UserInfo = (UserInfoBO)SessionManager.GetUserInfo();
            string currentArea = Request.RequestContext.RouteData.DataTokens["area"].ToString();
            List<ChucNangBO> ListChucNang = new List<ChucNangBO>();
            if (!string.IsNullOrEmpty(currentArea))
            {
                var lst_tt = UserInfo.ListThaoTac.Where(x => x.THAOTAC.ToLower().Contains(currentArea.ToLower())).Select(x => x.DM_CHUCNANG_ID).ToList();
                var _ChucNangChaID = UserInfo.ListCN.Where(o => lst_tt.Contains(o.DM_CHUCNANG_ID)).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                //var _ChucNangChaID = UserInfo.ListCN.Where(o => o.URL.ToLower().Contains(currentArea.ToLower())).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                if (_ChucNangChaID != null)
                    ListChucNang = UserInfo.ListCN.Where(o => o.CHUCNANG_CHA == _ChucNangChaID).OrderBy(o => o.TT_HIENTHI).ToList();
            }
            if (UserInfo.RoleID > 0)
            {
                ListChucNang = ListChucNang.Where(x => x.VAITROID == UserInfo.RoleID).OrderBy(o => o.TT_HIENTHI).ToList();
            }
            return PartialView("_NhanSuMenu", ListChucNang);
        }
        public PartialViewResult GetSearchForm(string ActionFindName, string NameTextSearch, string btnSearch = "btnSearch")
        {
            string currentArea = Request.RequestContext.RouteData.DataTokens["area"].ToString();
            string currentController = Request.RequestContext.RouteData.Values["controller"].ToString();
            string currentAction = Request.RequestContext.RouteData.Values["action"].ToString();
            ViewBag.ActionFindName = ActionFindName;
            ViewBag.NameTextSearch = NameTextSearch;
            ViewBag.btnSearch = btnSearch;
            ViewBag.currentController = currentController;
            ViewBag.currentAction = currentAction;
            ViewBag.currentArea = currentArea;
            return PartialView("_GetSearchForm");
        }


        [AllowAnonymous]
        [ValidateInput(false)]
        public PartialViewResult GetUserName(int COSO_ID, int DONVI_ID, long USER_ID, string ListRole, bool Is_Multi, string TIEUDE, string SELECTED, string EXISTED)
        {
            TuyChonGuiModel model = new TuyChonGuiModel();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            var lstRole = ListRole.Split(',').ToList();
            var lstSelected = SELECTED.ToListLong(',');
            var lstExisted = EXISTED.ToListLong(',');
            var ListNguoiDung = NguoiDungBs.GetListNguoiDung(COSO_ID, DONVI_ID, lstRole, lstSelected, lstExisted);
            model.IS_MULTI = Is_Multi;
            model.COSO_ID = COSO_ID;
            model.DONVI_ID = DONVI_ID;
            model.USER_ID = USER_ID;
            model.LISTROLE = lstRole;
            model.TIEUDE = TIEUDE;
            model.EXISTED = EXISTED;
            model.lstNguoiDung = ListNguoiDung;
            return PartialView("_ChonNguoiDung", model);
        }

        [AllowAnonymous]
        [ValidateInput(false)]
        public PartialViewResult GetUserNameVB(int COSO_ID, int DONVI_ID, long USER_ID, string ListRole, bool Is_Multi, string TIEUDE, string SELECTED, string EXISTED, int SPECIFY_ROLE)
        {
            TuyChonGuiModel model = new TuyChonGuiModel();
            NguoiDungBs = Get<DmNguoidungBusiness>();
            var lstRole = ListRole.Split(',').ToList();
            var lstSelected = SELECTED.ToListLong(',');
            var lstExisted = EXISTED.ToListLong(',');
            var ListNguoiDung = NguoiDungBs.GetListNguoiDung(COSO_ID, DONVI_ID, lstRole, lstSelected, lstExisted)
                .Where(x => x.DM_VAITRO_ID == SPECIFY_ROLE).ToList();
            model.IS_MULTI = Is_Multi;
            model.COSO_ID = COSO_ID;
            model.DONVI_ID = DONVI_ID;
            model.USER_ID = USER_ID;
            model.LISTROLE = lstRole;
            model.TIEUDE = TIEUDE;
            model.EXISTED = EXISTED;
            model.lstNguoiDung = ListNguoiDung;
            return PartialView("_ChonNguoiDung", model);
        }




        public JsonResult CheckkingFile(long ID)
        {
            var TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            TAILIEUDINHKEM taikieu = TaiLieuDinhKemBusiness.Find(ID);
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            if (taikieu != null)
            {
                //if (taikieu.USER_ID != (long)user.UserID)
                //{
                //    return Json("Khong");
                //}
                string path = URLPath + taikieu.DUONGDAN_FILE;
                if (System.IO.File.Exists(path))
                {
                    return Json("Co");
                }
                else
                {
                    return Json("Khong");
                }
            }
            return Json("Khong");
        }






        public JsonResult PreviewExistedFile(long ID)
        {
            string url = WebConfigurationManager.AppSettings["URLNavigation"];
            var TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            TAILIEUDINHKEM tailieu = TaiLieuDinhKemBusiness.Find(ID);
            string fExtension = tailieu.DINHDANG_FILE;
            string serverPath = url + "/Uploads/" + tailieu.DUONGDAN_FILE;
            //string serverPath = URLPath + tailieu.DUONGDAN_FILE;

            string htmlResult = string.Empty;
            switch (fExtension)
            {
                case "video/mp4":
                case ".mp4":
                    htmlResult += "<video style='width:100%;height:400px;display:block;' controls autoplay loop='loop'>";
                    htmlResult += "<source type='video/mp4' src='" + serverPath + "' />";
                    htmlResult += "</video>";
                    break;
                case "audio/mpeg":
                case ".mp3":
                case "audio/mp3":
                    htmlResult += "<audio id='PLAY_MUSIC' controls autoplay loop='loop' style='width:100%;height:400px;display:block;'>";
                    htmlResult += "<source type='audio/mp3' src='" + serverPath + "' />";
                    htmlResult += "</audio>";
                    break;
                case ".png":
                case ".jpg":
                case "image/jpeg":
                case "image/png":
                    htmlResult += "<img class='imgDetail' src='" + serverPath + "' style='display:block;'/>";
                    break;
                case "application/pdf":
                case ".pdf":
                case "application/download":
                    htmlResult += "<object style='width:100%;height:400px;display:block;' data='" + serverPath + "' type='application/pdf'></object>";
                    break;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                case "application/vnd.ms-word.document.12":
                case "application/msword":
                case ".docx":
                case ".doc":
                    serverPath = "http://docs.google.com/gview?url=" + serverPath + "&embedded=true";
                    htmlResult += "<iframe style='width:100%;height:400px;display:block;' src='" + serverPath + "'></iframe>";
                    break;
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                case "application/vnd.ms-excel":
                case ".xls":
                case ".xlsx":
                    serverPath = "http://docs.google.com/gview?url=" + serverPath + "&embedded=true";
                    htmlResult += "<iframe style='width:100%;height:400px;display:block;' src='" + serverPath + "'></iframe>";
                    break;
            }
            return Json(new { html = htmlResult });
        }
        /// <summary>
        /// preview file
        /// author duynn
        /// modify date 11/6/2017
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PreviewFile(HttpPostedFileBase file)
        {
            string url = WebConfigurationManager.AppSettings["URLNavigation"];
            string fExtension = Path.GetExtension(file.FileName).Trim().ToLower();
            Guid guidName = Guid.NewGuid();
            string fName = guidName + fExtension;
            string folder = Path.Combine(URLPath + "\\Temp");
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            string fTempPath = folder + "\\" + fName;
            file.SaveAs(fTempPath);
            string serverPath = url + "/Uploads/Temp/" + fName;
            string htmlResult = string.Empty;
            switch (fExtension)
            {
                case "video/mp4":
                case ".mp4":
                    htmlResult += "<video style='width:100%;height:400px;display:block;' controls autoplay loop='loop'>";
                    htmlResult += "<source type='video/mp4' src='" + serverPath + "' />";
                    htmlResult += "</video>";
                    break;
                case "audio/mpeg":
                case ".mp3":
                case "audio/mp3":
                    htmlResult += "<audio id='PLAY_MUSIC' controls autoplay loop='loop' style='width:100%;height:400px;display:block;'>";
                    htmlResult += "<source type='audio/mp3' src='" + serverPath + "' />";
                    htmlResult += "</audio>";
                    break;
                case ".png":
                case ".jpg":
                case "image/jpeg":
                case "image/png":
                    htmlResult += "<img class='imgDetail' src='" + serverPath + "' style='display:block;'/>";
                    break;
                case "application/pdf":
                case ".pdf":
                case "application/download":
                    htmlResult += "<object style='width:100%;height:400px;display:block;' data='" + serverPath + "' type='application/pdf'></object>";
                    break;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                case "application/vnd.ms-word.document.12":
                case "application/msword":
                case ".docx":
                case ".doc":
                    serverPath = "http://docs.google.com/gview?url=" + serverPath + "&embedded=true";
                    htmlResult += "<iframe style='width:100%;height:400px;display:block;' src='" + serverPath + "'></iframe>";
                    break;
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                case "application/vnd.ms-excel":
                case ".xls":
                case ".xlsx":
                    serverPath = "http://docs.google.com/gview?url=" + serverPath + "&embedded=true";
                    htmlResult += "<iframe style='width:100%;height:400px;display:block;' src='" + serverPath + "'></iframe>";
                    break;
            }
            return Json(new { html = htmlResult });
        }


    }
}
