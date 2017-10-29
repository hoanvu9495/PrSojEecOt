using System.Web.Mvc;

namespace Web.Areas.FacebookArea
{
    public class FacebookAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FacebookArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FacebookArea_default",
                "FacebookArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
