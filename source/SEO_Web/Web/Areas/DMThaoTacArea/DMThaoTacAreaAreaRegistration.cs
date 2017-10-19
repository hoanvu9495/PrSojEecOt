using System.Web.Mvc;

namespace Web.Areas.DMThaoTacArea
{
    public class DMThaoTacAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DMThaoTacArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DMThaoTacArea_default",
                "DMThaoTacArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
