using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Business.Business;
using Business.CommonBusiness;
using Web.Custom;
using Web.FwCore;
using Model.eAita;
using System.Net.Mail;
using System.Text;
using System.Net;
using Web.Areas.QuanLyEmailArea.Models;
namespace Web.Areas.QuanLyEmailArea.Controllers
{
    public class QuanLyEmailController : BaseController
    {
        private decimal userID;
        private DmNguoidungBusiness userBusiness;
        private QLEmailBusinesss emailBusiness;
        private UserInfoBO currentUser;
        private DmDonViBusiness DmDonViBusiness;
        private DmNguoidungBusiness DmNguoidungBusiness;
        [HttpGet]
        public ActionResult Index()
        {
            if (!Request.IsAjaxRequest())
            {
                emailBusiness = Get<QLEmailBusinesss>();
                currentUser = GetUserInfo();
                var listInbox = currentUser.LstEmail;
                if (listInbox == null || listInbox.Count == 0)
                    ViewBag.EmptyMailBox = 1;
                else
                {
                    string email = currentUser.Email;
                    string emailPass = currentUser.EmailPass;
                    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(emailPass))
                    {
                        listInbox = emailBusiness.GetListEmail(currentUser.UserImapClient, (long)currentUser.UserID, listInbox).OrderByDescending(x => x.NGAYGUI).ToList();
                    }
                }
                //SessionManager.SetValue("ListEmailTemplate", this.Get<QLMailTemplateBusiness>().DsTemplateDropdown());
                SessionManager.SetValue("ListInbox", listInbox);
            }
            return View();
        }

        public PartialViewResult ListInbox()
        {
            return PartialView("_ListInboxSearchResult");
        }
        [HttpPost]
        public PartialViewResult GetEmail(bool get)
        {
            userBusiness = Get<DmNguoidungBusiness>();
            emailBusiness = Get<QLEmailBusinesss>();
            var currentUser = GetUserInfo();
            try
            {
                userID = GetUserInfo().UserID;
                var user = userBusiness.Find(userID);
                if (!string.IsNullOrEmpty(user.EMAIL) && !string.IsNullOrEmpty(user.EMAILPASS))
                {
                    //var listInbox = null;
                    //if (currentUser.UserImapClient == null)
                    //{
                    //    currentUser.UserImapClient = userBusiness.getImapClient(user.EMAIL, user.EMAILPASS);
                    //}
                    //var listInbox = emailBusiness.GetListEmail(currentUser.UserImapClient, user.DM_NGUOIDUNG_ID, null);
                    if (listInbox != null && listInbox.Count > 0)
                    {
                        SessionManager.SetValue("ListInbox", listInbox);
                        ViewBag.EmptyMailBox = 0;
                    }
                }
                return PartialView("_ListEmailResult");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public PartialViewResult GetEmail()
        {
            return PartialView("_ListEmailResult");
        }

        [HttpGet]
        public ActionResult GetInboxContent(long emailID)
        {
            emailBusiness = Get<QLEmailBusinesss>();
            currentUser = GetUserInfo();
            QL_EMAIL email = currentUser.LstEmail.Where(x => x.ID == emailID).FirstOrDefault();
            if (email != null)
            {
                if (email.IS_READ != true)
                {
                    email.IS_READ = true;
                    emailBusiness.Save(email);
                }
                //byte[] data = Convert.FromBase64String(currentUser.EmailPass);
                //string pass = Encoding.UTF8.GetString(data);
                var attachments = emailBusiness.GetMailAttachs(currentUser.UserImapClient,email.UID);
                if (attachments != null && attachments.Count > 0)
                {
                    ViewBag.Attachments = attachments;
                    SessionManager.SetValue("ListInboxAttchments", attachments);
                }
                return PartialView("_InboxContent", email);
            }
            return RedirectToAction("ErrorPages", "Home", new { @area = "" }); ;
        }

        public ViewResult SendEmailView()
        {
             UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            QuanLyMailModel model = new QuanLyMailModel();
            DmDonViBusiness = Get<DmDonViBusiness>();
            DmNguoidungBusiness = Get<DmNguoidungBusiness>();
            var ListDonVi = DmDonViBusiness.All.ToList();
            ListDonVi = ListDonVi.Where(x => x.COSO_ID == user.CoSoID.Value && x.TRANGTHAI.HasValue && x.TRANGTHAI.Value).ToList();
            var lstNguoiDung = DmNguoidungBusiness.All.Where(x => x.COSO_ID == user.CoSoID.Value && !string.IsNullOrEmpty(x.EMAIL)).ToList();
            lstNguoiDung = lstNguoiDung.GroupBy(x => x.EMAIL).Select(u => u.First()).ToList();
            model.ListDonVi = ListDonVi;
            model.ListNguoiDung = lstNguoiDung;
            var currentUser = GetUserInfo();
            ViewBag.Sender = currentUser.Email;
            ViewData["ListTemp"]= this.Get<QlMailTemplateBusiness>().DsTemplateDropdown();
            return View(model);
        }

        [HttpGet]
        public JsonResult SetStarredEmail(long emailID)
        {
            emailBusiness = Get<QLEmailBusinesss>();
            currentUser = GetUserInfo();
            QL_EMAIL email = currentUser.LstEmail.Where(x => x.ID == emailID).FirstOrDefault();
            if (email != null)
            {
                if (email.IS_STARRED != null)
                {
                    if (email.IS_STARRED != true)
                        email.IS_STARRED = true;
                    else
                        email.IS_STARRED = false;
                    emailBusiness.Save(email);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false, message = "Đánh dấu thất bại" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Không tìm thấy email" }, JsonRequestBehavior.AllowGet);
        }

        public FileResult FileDownload(int attIndex, string uid, string fileName)
        {
            FileResult result;
            List<AE.Net.Mail.Attachment> listAtts = (List<AE.Net.Mail.Attachment>)SessionManager.GetValue("ListInboxAttchments");
            AE.Net.Mail.Attachment[] atts = listAtts.ToArray();
            byte[] byteData = atts[attIndex].GetData();
            MemoryStream streamFile = GetFile(byteData);
            result = new FileContentResult(streamFile.ToArray(), "application/x-zip-compressed");
            result.FileDownloadName = fileName;
            return result;
        }
        private MemoryStream GetFile(byte[] byteArr)
        {
            byte[] buffer = byteArr;
            MemoryStream streamFile = new MemoryStream(byteArr);
            streamFile.Write(byteArr, 0, byteArr.Length);
            return streamFile;
        }
        [HttpPost]
        public JsonResult SendEmailCustomer(string to, string subject, string message, int template)
        {
            try
            {
                string mailBody = String.Empty;
                if (template != 0 && template != 1)
                {
                    mailBody = this.Get<QlMailTemplateBusiness>().Find(template).TEMPLATE_BODY;
                    mailBody = mailBody.Replace("@ViewBag.Title", subject);
                    mailBody = mailBody.Replace("@ViewBag.Body", message);
                    //mailBody = fc.ToString();
                }
                else
                    mailBody = message;
                if (sendEmail(mailBody, subject, to))
                    return Json(new { success = true });
                return Json(new { sucess = false, message = "Gửi email không thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, message = ex.Message });
            }
        }
        private bool sendEmail(string body, string subject, string address)
        {
            SmtpClient server = new SmtpClient();
            var currentUser = GetUserInfo();
            if (!string.IsNullOrEmpty(currentUser.Email) && !string.IsNullOrEmpty(currentUser.EmailPass))
            {
                try
                {
                    string from = currentUser.Email;
                    if (from.Contains("@yahoo.com") || from.Contains("@yahoo.com.vn"))
                    {
                        server.Host = "smtp.mail.yahoo.com";
                        server.Port = 465;
                    }

                    else if (from.Contains("@mail.com"))
                    {
                        server.Host = "smtp.mail.com";
                        server.Port = 587;
                    }
                    else if (from.Contains("@gmail.com"))
                    {
                        server.Host = "smtp.gmail.com";
                        server.Port = 587;
                    }
                    byte[] data = Convert.FromBase64String(currentUser.EmailPass);
                    string pass = Encoding.UTF8.GetString(data);
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(from, address);
                    //mail.From = new MailAddress(from);
                    //mail.To.Add(address);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    server.Credentials = new NetworkCredential(from, pass);
                    server.EnableSsl = true;
                    server.Send(mail);
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
