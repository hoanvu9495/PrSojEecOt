using System.Web.Mvc;

namespace Web.Areas.TaiLieuKetXuatArea
{
    public class TaiLieuKetXuatAreaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TaiLieuKetXuatArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TaiLieuKetXuatArea_default",
                "TaiLieuKetXuatArea/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
