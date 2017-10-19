using System.Web.Mvc;

namespace Web.Areas.CoCauToChucArea
{
    public class CoCauToChucAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CoCauToChucArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CoCauToChucArea_default",
                "CoCauToChucArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
