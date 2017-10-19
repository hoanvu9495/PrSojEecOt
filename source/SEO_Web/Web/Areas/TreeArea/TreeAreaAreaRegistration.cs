using System.Web.Mvc;

namespace Web.Areas.TreeArea
{
    public class TreeAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TreeArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TreeArea_default",
                "TreeArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
