using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Common;
using Business.CommonBusiness;
using System.Configuration;
using Web.Custom;
using System.Web.Configuration;


namespace Web.Areas.BaiVietArea.Controllers
{
    public class ThuThapContentController : BaseController
    {
        //
        // GET: /BaiVietArea/ThuThapContent/
        private string HOST_WEB = WebConfigurationManager.AppSettings["HOST_WEB"];
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Thuthap()
        {
            return View();
        }

        public JsonResult GetHTML(string url)
        {

            Uri myUri = new Uri(url);
            var host = myUri.Host;
            string urlAddress = url;
            var model = new JsonResultBO(true);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();
                data = data.Replace(HOST_WEB, host);
                model.Message = data;

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
