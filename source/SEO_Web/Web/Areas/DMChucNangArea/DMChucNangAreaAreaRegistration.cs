using System.Web.Mvc;

namespace Web.Areas.DMChucNangArea
{
    public class DMChucNangAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DMChucNangArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DMChucNangArea_default",
                "DMChucNangArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
