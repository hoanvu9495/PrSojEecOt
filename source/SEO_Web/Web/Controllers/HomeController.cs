using Core.SAML.Library;
using Core.SAML.Library.Schema;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using Web.Common;
using Web.Custom;
using Web.FwCore;
using Business.Business;
using Model.DBTool;
using System.Security.Cryptography;
using Business.CommonBusiness;
using Business;
using Web.FwCore.Factory;
using System.Runtime.InteropServices;
using System.IO;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        //SysCauhinhhienthiBusiness SysCauhinhhienthiBusiness;
        //SysCauhinhtrangchuBusiness SysCauhinhtrangchuBusiness;
        //SysCauhinhHanghienthiBusiness SysCauhinhHanghienthiBusiness;
        //SysChucnangtrangchuBusiness SysChucnangtrangchuBusiness;
        private DmNguoidungBusiness DmNguoidungBusiness;
        private NguoiDungVaiTroBusiness NguoiDungVaiTroBusiness;
        #region Base Action
        public ActionResult Index1()
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            if (user != null)
            {
                var defaultMenu = user.ListCN.Where(o => o.MAC_DINH == 1).FirstOrDefault();
                if (defaultMenu != null)
                {
                    Response.Redirect(defaultMenu.URL, true);
                }
            }
            return View();
        }
        [HttpPost]
        public JsonResult SettingRoleDefault(string RoleDefault)
        {
            if (!string.IsNullOrEmpty(RoleDefault))
            {
                DmNguoidungBusiness = Get<DmNguoidungBusiness>();
                NguoiDungVaiTroBusiness = Get<NguoiDungVaiTroBusiness>();
                UserInfoBO user = (UserInfoBO)GetUserInfo();
                int roledefault = int.Parse(RoleDefault);
                if (roledefault == -1)//lựa chọn vai trò mặc định
                {
                    NguoiDungVaiTroBusiness.ResetRoleDefault(user.UserID);
                    return Json("success");
                }
                else
                {

                    var roleID = int.Parse(RoleDefault);
                    var NGUOIDUNG_VAITRO = NguoiDungVaiTroBusiness.GetRole((int)user.UserID, roleID);
                    if (NGUOIDUNG_VAITRO != null)
                    {
                        NGUOIDUNG_VAITRO.ROLE_DEFAULT = true;
                        NguoiDungVaiTroBusiness.Save(NGUOIDUNG_VAITRO);
                        var ListRole = NguoiDungVaiTroBusiness.All.Where(x => x.NGUOIDUNG_ID == user.UserID).ToList();
                        if (ListRole != null && ListRole.Count > 0)
                        {
                            user.ListNguoiDung_Vaitro = ListRole;
                        }
                        return Json("success");
                    }
                    else
                    {
                        throw new BusinessException("Đã xảy ra lỗi");
                    }

                }
            }
            else
            {
                throw new BusinessException("Đã xảy ra lỗi");
            }
        }
        [HttpPost]
        public JsonResult SettingRole(string OptionRole = "", string ReturnUrl = "")
        {
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            NguoiDungVaiTroBusiness = Get<NguoiDungVaiTroBusiness>();
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            if (!string.IsNullOrEmpty(OptionRole))
            {
                var getUrl = "";
                var roleID = int.Parse(OptionRole);
                user.RoleID = roleID;
                var userInfo = DmNguoidungBusiness.GetUserRole(user);
                user.ListThaoTac = userInfo.ListThaoTac;
                user.ListCN = userInfo.ListCN;
                user.ListCNFull = userInfo.ListCNFull;

                string decodedUrl = "";
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    decodedUrl = Server.UrlDecode(ReturnUrl);
                }
                if (Url.IsLocalUrl(decodedUrl))
                {
                    getUrl = decodedUrl;
                }
                else
                {
                    getUrl = Url.Action("Index", "Home");
                }

                return Json(getUrl);
            }
            else
            {
                throw new BusinessException("Đã xảy ra lỗi");
            }
        }
        public ActionResult NotFound()
        {
            //string decodedUrl = "";
            //if (!string.IsNullOrEmpty(ReturnUrl))
            //{
            //    decodedUrl = Server.UrlDecode(ReturnUrl);
            //}
            //if (Url.IsLocalUrl(decodedUrl))
            //{
            //    return Redirect(decodedUrl);
            //}
            //string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ////return RedirectToAction("Notfound", "Home", new { area = "", ReturnUrl = url });
            //return Redirect("/home/Notfound?ReturnUrl=" + url);
            return View();
        }

        public ActionResult Index()
        {
            DrashboardViewModel model = new DrashboardViewModel();
            UserInfoBO user = (UserInfoBO)GetUserInfo();

            if (user != null)
            {
                //SysChucnangMacdinhBusiness SysChucnangMacdinhBusiness = Get<SysChucnangMacdinhBusiness>();

                //var currentDefault = SysChucnangMacdinhBusiness.GetByUser(user.UserID);
                //if (currentDefault != null)
                //{
                //    var defaultMenu = user.ListCN.Where(o => o.DM_CHUCNANG_ID == currentDefault.CHUCNANG_ID && o.VAITROID == user.RoleID).FirstOrDefault();

                //    if (defaultMenu != null)
                //    {
                //        var ThaoTac = user.ListThaoTac.Where(x => x.DM_CHUCNANG_ID == defaultMenu.DM_CHUCNANG_ID).FirstOrDefault();// Redirect thao tác đầu tiên của chức năng mặc định
                //        if (ThaoTac != null)
                //        {
                //            Response.Redirect(ThaoTac.MENU_LINK, true);
                //            //Response.Redirect(defaultMenu.URL, true);
                //        }
                //    }
                //}
                //SysCauhinhHanghienthiBusiness = Get<SysCauhinhHanghienthiBusiness>();
                //SysCauhinhhienthiBusiness = Get<SysCauhinhhienthiBusiness>();
                //SysCauhinhtrangchuBusiness = Get<SysCauhinhtrangchuBusiness>();
                //var configRowUser = SysCauhinhHanghienthiBusiness.GetByUser(user.UserID);
                //List<DrashboardRowViewModel> ListDrashboardRowViewModel = new List<DrashboardRowViewModel>();
                //Nếu chưa tồn tại cấu hình theo user thì tạo mới
                //if (configRowUser == null)
                //{
                //    SYS_CAUHINH_HANGHIENTHI rowConfig = new SYS_CAUHINH_HANGHIENTHI();
                //    rowConfig.User_ID = (long)user.UserID;
                //    rowConfig.SOHANGHIENTHI = 1;
                //    SysCauhinhHanghienthiBusiness.Save(rowConfig);
                //    configRowUser = rowConfig;
                //}
                //if (configRowUser != null)
                //{
                //    //Lấy danh sách chức năng và cấu hình cột hiển thị trên Từng Hàng hiển thị
                //    if (configRowUser.SOHANGHIENTHI > 0)
                //    {
                //        for (int i = 1; i <= configRowUser.SOHANGHIENTHI; i++)
                //        {
                //            DrashboardRowViewModel _rowModel = new DrashboardRowViewModel();
                //            _rowModel.Width = 49;
                //            var configUser = SysCauhinhhienthiBusiness.GetByUser(user.UserID, i);
                //            //Nếu chưa tồn tại layout cho hàng thì thêm giá trị mặc định là 50-50
                //            if (configUser == null)
                //            {
                //                configUser = new SYS_CAUHINHHIENTHI();
                //                configUser.User_ID = (long)user.UserID;
                //                configUser.LayOutTypeID = 2;//50%-50%
                //                configUser.HANG_HIENTHI = i;
                //                SysCauhinhhienthiBusiness.Save(configUser);
                //            }
                //            if (configUser != null)
                //            {
                //                _rowModel.CauHinhHienThi = configUser;
                //                switch (_rowModel.CauHinhHienThi.LayOutTypeID)
                //                {
                //                    case 1://100%
                //                        _rowModel.Width = 99;
                //                        break;
                //                    case 2://50-50
                //                        _rowModel.Width = (float)48.5;
                //                        break;
                //                    case 3://25-25-25-25
                //                        _rowModel.Width = (float)23.5;
                //                        break;
                //                    case 4://30-70
                //                        _rowModel.Width = 37;
                //                        break;
                //                    case 5://30-40-30
                //                        _rowModel.Width = 343;
                //                        break;
                //                    case 6://70-30
                //                        _rowModel.Width = 73;
                //                        break;
                //                    default:
                //                        break;
                //                }
                //            }
                //            _rowModel.ListChucNang = SysCauhinhtrangchuBusiness.GetListChucNang(user.UserID, i);
                //            ListDrashboardRowViewModel.Add(_rowModel);
                //        }
                //    }
                //}

                //model.CauHinhHangHienThi = configRowUser;
                ////model.ListDrashboardRowViewModel = new List<DrashboardRowViewModel>();
                //model.ListDrashboardRowViewModel = ListDrashboardRowViewModel;
            }

            return View(model);
        }

        public ActionResult OptionRole(string ReturnUrl = "")
        {


            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult SaveConfigRowHomePage(FormCollection col)
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            if (user != null)
            {
                var SOHANGHIENTHI = col["SOHANGHIENTHI"].ToIntOrZero();
                if (SOHANGHIENTHI > 0)
                {
                    //SysCauhinhHanghienthiBusiness = Get<SysCauhinhHanghienthiBusiness>();
                    //var configRowUser = SysCauhinhHanghienthiBusiness.GetByUser(user.UserID);
                    //if (configRowUser != null)
                    //{
                    //    configRowUser.SOHANGHIENTHI = SOHANGHIENTHI;
                    //}
                    //else
                    //{
                    //    configRowUser = new SYS_CAUHINH_HANGHIENTHI();
                    //    configRowUser.User_ID = (long)user.UserID;
                    //    configRowUser.SOHANGHIENTHI = SOHANGHIENTHI;
                    //}
                    //SysCauhinhHanghienthiBusiness.Save(configRowUser);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SaveConfigHomePage(FormCollection col)
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            if (user != null)
            {
                var LayOutTypeID = col["LayOutTypeID"].ToIntOrZero();
                var HANG_HIENTHI = col["HANG_HIENTHI"].ToIntOrZero();
                if (LayOutTypeID > 0)
                {
                    //SysCauhinhhienthiBusiness = Get<SysCauhinhhienthiBusiness>();
                    //var configUser = SysCauhinhhienthiBusiness.GetByUser(user.UserID, HANG_HIENTHI);
                    //if (configUser != null)
                    //{
                    //    configUser.LayOutTypeID = LayOutTypeID;
                    //    configUser.HANG_HIENTHI = HANG_HIENTHI;
                    //}
                    //else
                    //{
                    //    configUser = new SYS_CAUHINHHIENTHI();
                    //    configUser.User_ID = (long)user.UserID;
                    //    configUser.LayOutTypeID = LayOutTypeID;
                    //    configUser.HANG_HIENTHI = HANG_HIENTHI;
                    //}
                    //SysCauhinhhienthiBusiness.Save(configUser);
                }
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult FunctionListConfig(int row)
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            DrashboardFunctionViewModel model = new DrashboardFunctionViewModel();
            model.ROW = row;
            if (user != null)
            {
                //SysChucnangtrangchuBusiness = Get<SysChucnangtrangchuBusiness>();
                //SysCauhinhtrangchuBusiness = Get<SysCauhinhtrangchuBusiness>();
                //model.ListAllChucNang = SysChucnangtrangchuBusiness.GetListChucNang(user.CoSoID.Value, user.RoleID);
                //model.ListChucNangInRow = SysCauhinhtrangchuBusiness.GetListChucNang(user.UserID, row).Select(o => o.CHUCNANG_ID.Value).ToList();
            }
            return PartialView("_FunctionListConfig", model);
        }

        //SaveFunctionListConfig
        [HttpPost]
        public ActionResult SaveFunctionListConfig(FormCollection col)
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            if (user != null)
            {
                var ALL_VAL_SELECT_TT = col["ALL_VAL_SELECT_TT"];
                var HANG_HIENTHI = col["HANG_HIENTHI"].ToIntOrZero();
                if (HANG_HIENTHI > 0)
                {
                    //SysCauhinhtrangchuBusiness = Get<SysCauhinhtrangchuBusiness>();
                    //var configHomePage = SysCauhinhtrangchuBusiness.GetListChucNang(user.UserID, HANG_HIENTHI);
                    //if (configHomePage != null && configHomePage.Count > 0)
                    //{
                    //    foreach (var item in configHomePage)
                    //    {
                    //        SysCauhinhtrangchuBusiness.Delete(item.ID);
                    //        SysCauhinhtrangchuBusiness.Save();
                    //    }
                    //}
                    //if (!string.IsNullOrEmpty(ALL_VAL_SELECT_TT))
                    //{
                    //    var list_chucnang = ALL_VAL_SELECT_TT.ToListInt(',');
                    //    if (list_chucnang != null && list_chucnang.Count > 0)
                    //    {
                    //        var i = 1;
                    //        foreach (var item in list_chucnang)
                    //        {
                    //            SYS_CAUHINHTRANGCHU SysCauhinhtrangchu = new SYS_CAUHINHTRANGCHU();
                    //            SysCauhinhtrangchu.User_ID = (long)user.UserID;
                    //            SysCauhinhtrangchu.CHUCNANG_ID = item;
                    //            SysCauhinhtrangchu.SOTHUTU = i;
                    //            SysCauhinhtrangchu.HANGHIENTHI = HANG_HIENTHI;
                    //            i++;
                    //            SysCauhinhtrangchuBusiness.Save(SysCauhinhtrangchu);
                    //        }
                    //    }
                    //}
                }
            }
            return RedirectToAction("Index");
        }
        //SetFunctionListConfig
        //[HttpPost]
        //public ActionResult SetFunctionListConfig(string sortorder)
        //{
        //    UserInfoBO user = (UserInfoBO)GetUserInfo();
        //    if (user != null && !string.IsNullOrEmpty(sortorder))
        //    {
        //        var list_row = sortorder.ToListStringLower('|');
        //        SysCauhinhtrangchuBusiness = Get<SysCauhinhtrangchuBusiness>();
        //        var configHomePage = SysCauhinhtrangchuBusiness.GetListChucNang(user.UserID);
        //        if (configHomePage != null && configHomePage.Count > 0)
        //        {
        //            foreach (var item in configHomePage)
        //            {
        //                SysCauhinhtrangchuBusiness.Delete(item.ID);
        //                SysCauhinhtrangchuBusiness.Save();
        //            }
        //        }
        //        if (list_row != null && list_row.Count > 0)
        //        {
        //            var row = 1;
        //            foreach (var item in list_row)
        //            {
        //                if (!string.IsNullOrEmpty(item))
        //                {
        //                    var list_chucnang = item.ToListInt(',');
        //                    var i = 1;
        //                    foreach (var chucnang in list_chucnang)
        //                    {
        //                        SYS_CAUHINHTRANGCHU SysCauhinhtrangchu = new SYS_CAUHINHTRANGCHU();
        //                        SysCauhinhtrangchu.User_ID = (long)user.UserID;
        //                        SysCauhinhtrangchu.CHUCNANG_ID = chucnang;
        //                        SysCauhinhtrangchu.SOTHUTU = i;
        //                        SysCauhinhtrangchu.HANGHIENTHI = row;
        //                        i++;
        //                        SysCauhinhtrangchuBusiness.Save(SysCauhinhtrangchu);
        //                    }
        //                    row++;
        //                }
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}
        [AllowAnonymous]
        public ActionResult InstallBrower()
        {
            return View();
        }
        //[AllowAnonymous]
        //public ActionResult ServerMaintenance()
        //{
        //    CAUHINH_HETHONG sc2 = Get<CauhinhHethongBusiness>().All.Where(o => o.MA_CAU_HINH == "MAINTAIN_TIME_QL").FirstOrDefault();
        //    if (sc2 != null && sc2.GIA_TRI == null)
        //    {
        //        return Redirect("/Account/Login");
        //    }
        //    else if (sc2 != null)
        //    {
        //        ViewData["MAINTAIN_TIME"] = sc2.GIA_TRI;
        //    }
        //    return PartialView("_ServerMaintenance");
        //}
        [AllowAnonymous]
        public ActionResult FireFox()
        {
            return PartialView("_FireFox");
        }
        [AllowAnonymous]
        public ActionResult BError()
        {
            Response.StatusCode = 450;
            return (ActionResult)SessionManager.GetValue("LAST_ERROR");
        }

        public ActionResult UnAuthor()
        {
            return View();
        }

        public ActionResult NotFound404()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckLoginStatus(string SAMLResponse, string returnUserUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(SAMLResponse))
                {
                    return RedirectToAction("Error");
                }

                XmlDocument SAMLXML = new XmlDocument();
                SAMLXML.LoadXml(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Server.UrlDecode(SAMLResponse))));
                string certificateLocation = ConfigurationManager.AppSettings["certificateLocation"];
                X509Certificate2 signingCert = CertificateUtility.GetCertificateForSigning(@certificateLocation, ConfigurationManager.AppSettings["CertPass"]);

                if (!CertificateUtility.ValidateX509CertificateSignature(SAMLXML, signingCert))
                {
                    return RedirectToAction("Error");
                }

                XmlReader xmlReader = new XmlNodeReader(SAMLXML);
                XmlSerializer serializer = new XmlSerializer(typeof(ResponseType));
                ResponseType reponseType = (ResponseType)serializer.Deserialize(xmlReader);
                string issuer = reponseType.Issuer.Value;
                string statusMessage = reponseType.Status.StatusMessage;
                string iddentityProvider = ConfigurationManager.AppSettings["serviceProviderIdentity"];
                AssertionType assertionType = Helper.GetAssertionFromXMLDoc(SAMLXML);
                ResponseAttribute responseAttribute = Helper.AssertionData(assertionType);

                if (string.IsNullOrEmpty(iddentityProvider) || string.IsNullOrEmpty(responseAttribute.UserName))
                {
                    return RedirectToAction("Error");
                }

                string userName = responseAttribute.UserName;

                Authenticate.LoadUserInfo(userName);
                FormsAuthentication.SetAuthCookie(userName, false);

                return string.IsNullOrWhiteSpace(returnUserUrl) ? RedirectPermanent("/Home/Index") : RedirectPermanent(Server.UrlDecode(returnUserUrl));
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult ClearSession(string SAMLResponse, string returnMainUrl)
        {
            bool isRedirected = false;
            try
            {
                if (!string.IsNullOrEmpty(SAMLResponse))
                {
                    XmlDocument SAMLXML = new XmlDocument();
                    SAMLXML.LoadXml(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Server.UrlDecode(SAMLResponse))));
                    string certificateLocation = (ConfigurationManager.AppSettings["certificateLocation"] ?? "").ToString();
                    X509Certificate2 signingCert = CertificateUtility.GetCertificateForSigning(@certificateLocation, ConfigurationManager.AppSettings["CertPass"]);
                    if (!CertificateUtility.ValidateX509CertificateSignature(SAMLXML, signingCert))
                    {
                        isRedirected = true;
                        return RedirectToAction("Error");
                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        SessionManager.Clear();

                        string mainUrl = Server.UrlDecode(returnMainUrl);
                        if (!string.IsNullOrEmpty(mainUrl))
                        {
                            isRedirected = true;
                            Response.Clear();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<html>");
                            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                            sb.AppendFormat("<form name='form' action='{0}' method='post'>", mainUrl);
                            sb.Append("</form>");
                            sb.Append("</body>");
                            sb.Append("</html>");
                            Response.Write(sb.ToString());
                            Response.End();
                        }
                    }
                }
                else
                {
                    isRedirected = true;
                    string identityProvider = (ConfigurationManager.AppSettings["identityProvider"].ToString().Replace("/SSOServices/CheckLoginStatus", ""));
                    Response.Clear();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                    sb.AppendFormat("<form name='form' action='{0}' method='post'>", identityProvider);
                    sb.Append("</form>");
                    sb.Append("</body>");
                    sb.Append("</html>");
                    Response.Write(sb.ToString());
                    Response.End();
                }
            }
            catch
            {
                // Do nothing
            }

            if (!isRedirected)
            {
                string identityProvider = (ConfigurationManager.AppSettings["identityProvider"].ToString().Replace("/SSOServices/CheckLoginStatus", ""));
                return Redirect(identityProvider);
            }
            return null;
        }
        #endregion

        [AllowAnonymous]
        public PartialViewResult ConfigFunctionDefault()
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            if (user != null)
            {
                //SysChucnangMacdinhBusiness SysChucnangMacdinhBusiness = Get<SysChucnangMacdinhBusiness>();
                //var currentDefault = SysChucnangMacdinhBusiness.GetByUser(user.UserID);
                //if (currentDefault != null)
                //{
                //    ViewBag.CurrentDefaultFunction = currentDefault.CHUCNANG_ID;
                //}
                //else
                //{
                //    ViewBag.CurrentDefaultFunction = -100;
                //}
                return PartialView("_ConfigFunctionDefault", user);
            }
            return PartialView("_ConfigFunctionDefault", new UserInfoBO());
        }
        //rdoDefaultFunction
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SaveConfigFunctionDefault(FormCollection col)
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            if (user != null)
            {
                var ChucnangID = col["rdoDefaultFunction"];
                if (!string.IsNullOrEmpty(ChucnangID))
                {
                    //SysChucnangMacdinhBusiness SysChucnangMacdinhBusiness = Get<SysChucnangMacdinhBusiness>();
                    //var currentDefault = SysChucnangMacdinhBusiness.GetByUser(user.UserID);
                    //if (currentDefault != null)
                    //{
                    //    if (ChucnangID.ToIntOrNULL() == -100)
                    //    {
                    //        SysChucnangMacdinhBusiness.Delete(currentDefault.ID);
                    //        SysChucnangMacdinhBusiness.Save();
                    //    }
                    //    else
                    //    {
                    //        currentDefault.CHUCNANG_ID = ChucnangID.ToIntOrNULL();
                    //        SysChucnangMacdinhBusiness.Save(currentDefault);
                    //    }
                    //}
                    //else
                    //{
                    //    SYS_CHUCNANG_MACDINH chucnang_macdinh = new SYS_CHUCNANG_MACDINH();
                    //    chucnang_macdinh.USER_ID = (long)user.UserID;
                    //    chucnang_macdinh.CHUCNANG_ID = ChucnangID.ToIntOrNULL();
                    //    SysChucnangMacdinhBusiness.Save(chucnang_macdinh);
                    //}
                }
            }
            return Json("");
        }
        #region for ckeditor
        public ActionResult uploadPartial()
        {
            var user = (UserInfoBO)SessionManager.GetUserInfo();
            var appData = Server.MapPath("~/Uploads/Editor/" + user.UserID);
            if (!System.IO.Directory.Exists(appData))
            {
                Directory.CreateDirectory(appData);
            }
            var images = Directory.GetFiles(appData).Select(x => new imageviewmodel
            {
                Url = Url.Content("/Uploads/Editor/" + user.UserID + "/" + Path.GetFileName(x))
            });
            return View(images);
        }
        public void uploadnow(HttpPostedFileWrapper upload)
        {
            var user = (UserInfoBO)SessionManager.GetUserInfo();

            if (upload != null)
            {
                string folder = Server.MapPath("~/Uploads/Editor/" + user.UserID);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string ImageName = upload.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Uploads/Editor/" + user.UserID), ImageName);
                upload.SaveAs(path);
            }
        }
        #endregion
    }
}
