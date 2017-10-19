using Business.CommonBusiness;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.FwCore.Factory
{
    public class VtExceptionAttribute : FilterAttribute, IExceptionFilter
    {        
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
            filterContext.Exception is BusinessException)
            {
                BusinessException ex = (BusinessException)filterContext.Exception;
                filterContext.Result = ex.Result != null ? (ActionResult)ex.Result :
                new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { Type = "ERROR", Message = ex.Message }
                };
                filterContext.ExceptionHandled = true;
                SessionManager.SetValue("LAST_ERROR", filterContext.Result);
                filterContext.HttpContext.Response.StatusCode = 450;
            }
            else if (!filterContext.ExceptionHandled &&
            filterContext.Exception is HttpRequestValidationException)
            {
                BusinessException ex = new BusinessException("Bạn không được phép nhập các ký tự dạng thẻ html");
                filterContext.Result = 
                new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { Type = "ERROR", Message = ex.Message }
                };
                SessionManager.SetValue("LAST_ERROR", filterContext.Result);
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = 450;                
            }
            else if (!filterContext.ExceptionHandled &&
                filterContext.Exception is UnauthorizedAccessException)
            {
                BusinessException ex = new BusinessException("Bạn không có quyền thực hiện chức năng này hoặc hiện tại đang ngoài khoảng thời gian cho phép thực hiện");
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result =
                    new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { Type = "ERROR", Message = ex.Message }
                    };
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.StatusCode = 401;
                }
                else
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Redirect("/Home/UnAuthor", true);
                }
            }
        } 
    }
}