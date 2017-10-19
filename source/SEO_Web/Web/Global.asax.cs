using Business.Business;
using Business.CommonBusiness;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Common;
using Web.FwCore.Factory;
using StackExchange.Profiling;
using log4net;
using StackExchange.Profiling.EntityFramework6;


namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            MiniProfilerEF6.Initialize();
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new VtControllerFactory());
            log4net.Config.XmlConfigurator.Configure();
            StoreData();
        }
        private void Application_Error(object sender, EventArgs e)
        {
            if (HandleHelper.IsMaxRequestExceededException(this.Server.GetLastError()))
            {
                this.Server.ClearError();
                this.Response.Redirect("~/UploadError.html");
            }

            Exception ex = Server.GetLastError();

            if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                this.Server.ClearError();
                this.Response.Redirect("~/UnAuthor.html");
            }

            log.Error("App_Error", ex);
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        private void application_EndRequest(object sender, EventArgs e)
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;

            if ((request.HttpMethod == "POST") &&
                (response.StatusCode == 404 && response.SubStatusCode == 13))
            {
                // Clear the response header but do not clear errors and
                // transfer back to requesting page to handle error
                response.ClearHeaders();
                request.Abort();
                this.Server.Transfer("~/UploadError.html");
            }

            MiniProfiler.Stop();
        }
        /// <summary>
        /// Store data to static class
        /// </summary>
        public void StoreData()
        {
            
        }
    }
}