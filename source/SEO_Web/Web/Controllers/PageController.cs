using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Custom;
using Web.Common;
using Web.FwCore;
using PagedList;

namespace Web.Controllers
{
    public class PageController : BaseController
    {
        //
        // GET: /Page/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNavigator()
        {
            var ipagelist = (IPagedList)SessionManager.GetValue("PageList");
            return PartialView("_PagingPartial", ipagelist);
        }

    }
}
