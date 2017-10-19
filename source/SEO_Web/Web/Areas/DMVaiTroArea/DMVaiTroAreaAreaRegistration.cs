using System.Web.Mvc;

namespace Web.Areas.DMVaiTroArea
{
    public class DMVaiTroAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DMVaiTroArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DMVaiTroArea_default",
                "DMVaiTroArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
