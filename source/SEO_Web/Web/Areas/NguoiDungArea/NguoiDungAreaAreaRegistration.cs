using System.Web.Mvc;

namespace Web.Areas.NguoiDungArea
{
    public class NguoiDungAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NguoiDungArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "NguoiDungArea_default",
                "NguoiDungArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
