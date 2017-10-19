using System.Web.Mvc;

namespace Web.Areas.BaiVietArea
{
    public class BaiVietAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BaiVietArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BaiVietArea_default",
                "BaiVietArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
