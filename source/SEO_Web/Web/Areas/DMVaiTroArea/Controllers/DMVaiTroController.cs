using Business.Business;
using Business.CommonBusiness;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Web.Areas.DMVaiTroArea.Models;
using Web.Common;
using Web.Custom;
using Web.FwCore;

namespace Web.Areas.DMVaiTroArea.Controllers
{
    public class DMVaiTroController : BaseController
    {
        private DmVaitroBusiness DmVaitroBusiness;
        private DmChucnangBusiness DmChucnangBusiness;
        private DmThaotacBusiness DmThaotacBusiness;
        private VaitroChucnangBusiness VaitroChucnangBusiness;
        private VaitroThaotacBusiness VaitroThaotacBusiness;
        private NguoiDungVaiTroBusiness NguoiDungVaiTroBusiness;
        /// <summary>
        /// màn hình danh sách Vai trò
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            //if (!Ultilities.IsInActivities(user.ListThaoTac, "/DMVaiTroArea/DMVaiTro/Index"))
            //{
            //    return RedirectToAction("UnAuthor", "Home", new { area = "" });
            //}

            SessionManager.Remove("ListVaiTro");
            SessionManager.Remove("ListVaiTroSearch");
            List<DmVaiTroBO> result = FillDataToGrid();
            ViewData["Search"] = "0";
            VaiTroViewModel model = new VaiTroViewModel();
            
            model.listVaitro = result;
            //SessionManager.SetValue("ListVaiTro", result);
            return View(model);
        }

        /// <summary>
        /// cập nhập hoặc thêm mới một vai trò
        /// </summary>
        /// <param name="vaitro"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public JsonResult SaveVaiTro(DM_VAITRO vaitro)
        {
           
            UserInfoBO user = ((UserInfoBO)SessionManager.GetUserInfo());
        

            if (vaitro.DM_VAITRO_ID == 0)
            {
                vaitro.NGAYTAO = DateTime.Now;
                vaitro.NGUOITAO = user.Username;
            }
            else
            {
                vaitro.NGAYSUA = DateTime.Now;
                vaitro.NGUOISUA = user.Username;

            }
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            vaitro.MA_VAITRO = vaitro.MA_VAITRO.ToUpper();
            vaitro.IS_DELETE = false;
            DmVaitroBusiness.Save(vaitro);
            return Json("Đã thêm mới vai trò !");
        }

        /// màn hình thêm mới Vai trò
        /// </summary>
        /// <returns></returns>
        public PartialViewResult AddFormVaiTro()
        {

            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();

            VaiTroViewModel model = new VaiTroViewModel();
         
            return PartialView("_CreateRole", model);
        }
        public JsonResult ReloadData()
        {
            List<DmVaiTroBO> result = FillDataToGrid();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RoleAuthorities(int id, int? cs = 0)
        {
            
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            //DmCapCoSoBusiness = Get<DmCapCoSoBusiness>();
            //CosoBusiness = Get<CosoBusiness>();
            VaitroChucnangBusiness = Get<VaitroChucnangBusiness>();
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
             //cs = userInfo.CoSoID;
            PhanQuyenViewModel model = new PhanQuyenViewModel();
            //var ListChucNang = (List<dmChucNangBO>)SessionManager.GetValue("ListChucNangVaiTro");
            //if (ListChucNang == null || ListChucNang.Count <= 0)
            //{
            //    ListChucNang = DmChucnangBusiness.GetListChucNang(DM_VAITRO_ID);
            //    SessionManager.SetValue("ListChucNangVaiTro", ListChucNang);
            //}
            var VaiTro = DmVaitroBusiness.Find(id);
            if (VaiTro == null)
            {
                VaiTro = new DM_VAITRO();
            }
            //model.COSO_ID = cs;
            //model.TEN_COSO = CosoBusiness.GetName(cs);
            //model.TENCAP_COSO = DmCapCoSoBusiness.GetName(VaiTro.CAPCOSO_ID);
            model.VaiTro = VaiTro;
            model.ListChucNang = DmChucnangBusiness.GetListChucNang(id, cs);
            return View(model);
        }
        [HttpPost]
        public JsonResult GetThaoTacByChucNang(int id)
        {
            DmThaotacBusiness = Get<DmThaotacBusiness>();
            var lstThaoTac = DmThaotacBusiness.DSThaoTacByChucNang(id);
            return Json(lstThaoTac);
        }
        public ActionResult ThietLapVaiTro(int id)
        {
            var user = (UserInfoBO)SessionManager.GetUserInfo();
            
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            var tree = DmVaitroBusiness.GetTree(id, (int)user.CoSoID);
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            VaitroChucnangBusiness = Get<VaitroChucnangBusiness>();
            PhanQuyenViewModel model = new PhanQuyenViewModel();
            model.treeData = tree;
            //var ListChucNang = (List<dmChucNangBO>)SessionManager.GetValue("ListChucNangVaiTro");
            //if (ListChucNang == null || ListChucNang.Count <= 0)
            //{
            //    ListChucNang = DmChucnangBusiness.GetListChucNang(DM_VAITRO_ID);
            //    SessionManager.SetValue("ListChucNangVaiTro", ListChucNang);
            //}
            var VaiTro = DmVaitroBusiness.Find(id);
            if (VaiTro == null)
            {
                VaiTro = new DM_VAITRO();
            }
            model.COSO_ID = user.CoSoID;
            model.VaiTro = VaiTro;
            model.DSChucNang = DmChucnangBusiness.GetListChucNang();
            model.ListChucNang = DmChucnangBusiness.GetListChucNang(id, user.CoSoID);

            return View(model);
        }
        [HttpPost]
        public JsonResult GetTree(int id)
        {
            var user = (UserInfoBO)SessionManager.GetUserInfo();
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            var tree = DmVaitroBusiness.GetTree(id, (int)user.CoSoID);
            return Json(tree);
        }

        [HttpPost]
        public JsonResult AddPhanQuyen(FormCollection form)
        {
            var user = (UserInfoBO)SessionManager.GetUserInfo();
            JsonResultBO result = new JsonResultBO();
            var idVaitro = form["VAITRO_ID"].ToIntOrZero();
            var idChucnang = form["CHUCNANG"].ToIntOrZero();
            VaitroChucnangBusiness = Get<VaitroChucnangBusiness>();
            VAITRO_CHUCNANG chucnangvaitro = VaitroChucnangBusiness.getVaiTroChucNang(idVaitro, (int)user.CoSoID, idChucnang);
            if (chucnangvaitro == null)
            {
                chucnangvaitro = new VAITRO_CHUCNANG();
                chucnangvaitro.COSO_ID = user.CoSoID;
                chucnangvaitro.DM_CHUCNANG_ID = idChucnang;
                chucnangvaitro.DM_VAITRO_ID = idVaitro;
                chucnangvaitro.NGAYTAO = DateTime.Now;
                chucnangvaitro.NGUOITAO = user.Fullname;
                chucnangvaitro.TRANGTHAI = 1;
                VaitroChucnangBusiness.Save(chucnangvaitro);
            }
            var thaotac = form["THAOTAC"].ToListInt(',');
            VaitroThaotacBusiness = Get<VaitroThaotacBusiness>();
            foreach (var item in thaotac)
            {

                var vaitrothaotac = new VAITRO_THAOTAC();

                if (!VaitroThaotacBusiness.IsExist((int)chucnangvaitro.VAITRO_CHUCNANG_ID, (long)item, (int)user.CoSoID))
                {
                    vaitrothaotac.COSO_ID = user.CoSoID;
                    vaitrothaotac.DM_THAOTAC_ID = item;
                    vaitrothaotac.NGAYTAO = DateTime.Now;
                    vaitrothaotac.NGUOITAO = user.Fullname;
                    vaitrothaotac.TRANGTHAI = 1;
                    vaitrothaotac.VAITRO_CHUCNANG_ID = chucnangvaitro.VAITRO_CHUCNANG_ID;
                    VaitroThaotacBusiness.Save(vaitrothaotac);
                }
          
                result.Status = true;

            }
            
            
            return Json(result);
        }
      
       
        #region Danh sách phân cấp cơ sở
        private string genPreChar(int solan)
        {
            var str = new StringBuilder();
            if (solan > 0)
            {
                for (int i = 0; i < solan; i++)
                {
                    str = str.Append("--");
                }
                return str.ToString();
            }
            return string.Empty;
        }
      


        #endregion
        /// <summary>
        /// xóa vai trò
        /// </summary>
        /// <param name="DM_VAITRO_ID"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteVaiTro(int? DM_VAITRO_ID)
        {
            if (DM_VAITRO_ID > 0)
            {
                DmVaitroBusiness = Get<DmVaitroBusiness>();
                var VaiTro = DmVaitroBusiness.Find(DM_VAITRO_ID);
                if (VaiTro != null)
                {
                    VaiTro.IS_DELETE = true;
                    DmVaitroBusiness.Save(VaiTro);
                }
            }
            return Json(new { Type = "SUCCESS", Message = "Xóa thao tác thành công!" });
        }
       

        /// <summary>
        /// màn hình thiết lập chức năng mặc định cho vai trò
        /// </summary>
        /// <param name="DM_VAITRO_ID"></param>
        /// <returns></returns>
        public PartialViewResult VaiTroMacDinh(int DM_VAITRO_ID)
        {
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            DefaultChucNangViewMoel model = new DefaultChucNangViewMoel();
            model.ListChucNang = DmChucnangBusiness.GetListChucNangFull(DM_VAITRO_ID);
            model.VaiTro = DmVaitroBusiness.Find(DM_VAITRO_ID);
            return PartialView("_ChucNangDefault", model);
        }
        /// <summary>
        /// màn hình thiết lập phân quyền 
        /// </summary>
        /// <param name="DM_VAITRO_ID">mã tự tăng của bảng DM_VAITRO</param>
        /// <returns></returns>
        public PartialViewResult PhanQuyenVaiTro(int DM_VAITRO_ID, int? COSO = 0)
        {
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            VaitroChucnangBusiness = Get<VaitroChucnangBusiness>();
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            PhanQuyenViewModel model = new PhanQuyenViewModel();
            //var ListChucNang = (List<dmChucNangBO>)SessionManager.GetValue("ListChucNangVaiTro");
            //if (ListChucNang == null || ListChucNang.Count <= 0)
            //{
            //    ListChucNang = DmChucnangBusiness.GetListChucNang(DM_VAITRO_ID);
            //    SessionManager.SetValue("ListChucNangVaiTro", ListChucNang);
            //}
            var VaiTro = DmVaitroBusiness.Find(DM_VAITRO_ID);
            if (VaiTro == null)
            {
                VaiTro = new DM_VAITRO();
            }
            model.COSO_ID = COSO;
            model.VaiTro = VaiTro;
            model.ListChucNang = DmChucnangBusiness.GetListChucNang(DM_VAITRO_ID, COSO);


            return PartialView("_PhanQuyen", model);
        }

        [ValidateAntiForgeryToken]
        public PartialViewResult SaveChucNangDefault(int DM_VAITRO_ID, int DM_CHUCNANG_ID)
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            VaitroChucnangBusiness = Get<VaitroChucnangBusiness>();
            VaitroChucnangBusiness.SaveChucNangDefault(DM_VAITRO_ID, DM_CHUCNANG_ID);
            VaiTroViewModel model = new VaiTroViewModel();
            return PartialView("_VaiTroSearchResult", model);
        }
        /// <summary>
        /// lưu một hoặc nhiều vai trò chức năng
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        public ActionResult SaveVaiTroChucNang(FormCollection col)
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            //if (userInfo.CoSoID == null)
            //{
            //    throw new BusinessException("Bạn không có quyền truy cập chức năng này");
            //}
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            VaitroChucnangBusiness = Get<VaitroChucnangBusiness>();
            VaitroThaotacBusiness = Get<VaitroThaotacBusiness>();
            short DM_VAITRO_ID = short.Parse(col["DM_VAITRO_ID"]);
            //tìm kiếm các VAITRO_RESULT có DM_VAITRO_ID = DM_VAITRO_ID trả về
            var VaiTroChucNangResult = VaitroChucnangBusiness.All.Where(x => x.DM_VAITRO_ID == DM_VAITRO_ID).ToList();
            var listVaiTroChucNang_ID = VaiTroChucNangResult.Select(o => o.VAITRO_CHUCNANG_ID).ToList();
            var ListChucNang = DmChucnangBusiness.All.ToList();
            var chucNangDefault = VaiTroChucNangResult.Where(x => x.MAC_DINH == 1).FirstOrDefault();//CHUC NANG MAC DINH 
            VAITRO_CHUCNANG ChucNangMacDinh = new VAITRO_CHUCNANG();
            if (chucNangDefault != null && chucNangDefault.DM_VAITRO_ID.HasValue)
            {
                ChucNangMacDinh.DM_CHUCNANG_ID = chucNangDefault.DM_CHUCNANG_ID;
                ChucNangMacDinh.MAC_DINH = chucNangDefault.MAC_DINH;

            }
            // xóa hết các VaiTro_ThaoTac trả về có trong bảng VAITRO_THAOTAC có vai trò chức năng được tìm thấy theo DM_VAITRO_ID
            var VaiTroThaoTacResult = VaitroThaotacBusiness.All.Where(x => listVaiTroChucNang_ID.Contains(x.VAITRO_CHUCNANG_ID.Value)).ToList();
            if (VaiTroChucNangResult != null && VaiTroChucNangResult.Count > 0)
            {
                VaitroThaotacBusiness.DeleteAll(VaiTroThaoTacResult);
                VaitroThaotacBusiness.Save();
                VaitroChucnangBusiness.DeleteAll(VaiTroChucNangResult); // xóa hết các DM_VAITRO_ID trả về có trong bảng VAITRO_CHUCNANG
                VaitroChucnangBusiness.Save();

            }
            //bool check = false;
            foreach (var item in ListChucNang)
            {
                //short? macdinh = null;
                //Check những thao tác được chọn bên cột trái và cột phải
                var ALL_VAL_SELECT_TT_LEFT = col["ALL_VAL_SELECT_TT_LEFT_" + item.DM_CHUCNANG_ID];
                var ALL_VAL_SELECT_TT_RIGHT = col["ALL_VAL_SELECT_TT_RIGHT_" + item.DM_CHUCNANG_ID];
                var ListThaoTac_Left = ALL_VAL_SELECT_TT_LEFT.ToListInt(',');
                var ListThaoTac_Right = ALL_VAL_SELECT_TT_RIGHT.ToListInt(',');
                List<int> ListThaoTac = new List<int>();
                ListThaoTac.AddRange(ListThaoTac_Left);
                ListThaoTac.AddRange(ListThaoTac_Right);
                if (ListThaoTac != null && ListThaoTac.Count > 0)
                {
                    //lưu thông tin vai trò chức năng
                    //if (ChucNangMacDinh.DM_CHUCNANG_ID == item.DM_CHUCNANG_ID) { check = true; }
                    VAITRO_CHUCNANG _vaitroChucNang = new VAITRO_CHUCNANG();
                    _vaitroChucNang.DM_VAITRO_ID = DM_VAITRO_ID;
                    _vaitroChucNang.DM_CHUCNANG_ID = item.DM_CHUCNANG_ID;
                    if (ChucNangMacDinh != null && ChucNangMacDinh.DM_CHUCNANG_ID.HasValue && ChucNangMacDinh.DM_CHUCNANG_ID == item.DM_CHUCNANG_ID)
                    {
                        _vaitroChucNang.MAC_DINH = ChucNangMacDinh.MAC_DINH;
                    }
                    //vaitro_chucnang.MAC_DINH = (ChucNangMacDinh.DM_CHUCNANG_ID == item.DM_CHUCNANG_ID) ? ChucNangMacDinh.MAC_DINH : macdinh;
                    _vaitroChucNang.TRANGTHAI = 1;
                    //_vaitroChucNang.COSO_ID = COSO_ID;
                    _vaitroChucNang.NGAYTAO = DateTime.Now;
                    _vaitroChucNang.NGUOITAO = userInfo.Username;
                    _vaitroChucNang.NGAYSUA = DateTime.Now;
                    _vaitroChucNang.NGUOISUA = userInfo.Username;
                    VaitroChucnangBusiness.Save(_vaitroChucNang);

                    //lưu thông tin vai trò thao tác
                    foreach (var tt in ListThaoTac)
                    {
                        VAITRO_THAOTAC vaitro_thaotac = new VAITRO_THAOTAC();
                        vaitro_thaotac.DM_THAOTAC_ID = tt;
                        vaitro_thaotac.VAITRO_CHUCNANG_ID = _vaitroChucNang.VAITRO_CHUCNANG_ID;
                        vaitro_thaotac.TRANGTHAI = 1;
                        //vaitro_thaotac.COSO_ID = COSO_ID;
                        vaitro_thaotac.NGAYTAO = DateTime.Now;
                        vaitro_thaotac.NGUOITAO = userInfo.Username;
                        vaitro_thaotac.NGAYSUA = DateTime.Now;
                        vaitro_thaotac.NGUOISUA = userInfo.Username;
                        VaitroThaotacBusiness.Save(vaitro_thaotac);
                    }
                }


            }
            VaitroChucnangBusiness.Save();
            return Redirect("/DMVaiTroArea/DMVaiTro");
            //VaiTroViewModel model = new VaiTroViewModel();
            //return PartialView("_VaiTroSearchResult", model);
        }
        /// <summary>
        /// tìm và trả về một hoặc nhiều vai trò
        /// </summary>
        /// <param name="TEN_VAITRO">tên vai trò</param>
        /// <returns></returns>
        public PartialViewResult FindVaiTro(string TEN_VAITRO, string CAPCOSO)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            List<DmVaiTroBO> result = (List<DmVaiTroBO>)SessionManager.GetValue("ListVaiTro");
            if (result != null)
            {
                string tenvaitro = TEN_VAITRO.Trim().ToLower();
                if (!string.IsNullOrEmpty(tenvaitro))
                {
                    result = result.Where(x => x.TEN_VAITRO.ToLower().ConvertToFTS().Contains(tenvaitro.Trim().ToLower().ConvertToFTS())).ToList();
                }
                //if (!string.IsNullOrEmpty(COSO))
                //{
                //    result = result.Where(x => x.COSO_ID.HasValue ? x.COSO_ID == int.Parse(COSO) : false).ToList();
                //}
                if (!string.IsNullOrEmpty(CAPCOSO))
                {
                    int CAP_ID = int.Parse(CAPCOSO);
                    result = result.Where(x => x.CAPCOSO_ID.HasValue ? x.CAPCOSO_ID == CAP_ID : false).ToList();
                    //var lstCapCoSo = ListCapCoSo(CAP_ID).Select(x => int.Parse(x.Value)).ToList();
                    //if (lstCapCoSo != null && lstCapCoSo.Count > 0)
                    //{
                    //    //result = result.Where(x => x.CAPCOSO_ID.HasValue ? lstCapCoSo.Contains(x.CAPCOSO_ID.Value) : false || x.CAPCOSO_PARENT.HasValue ? lstCapCoSo.Contains(x.CAPCOSO_PARENT.Value) : false).ToList();
                    //}
                }
            }
            VaiTroViewModel model = new VaiTroViewModel();
            ViewData["TENVAITRO"] = TEN_VAITRO;
            ViewData["Search"] = "1";
            SessionManager.SetValue("ListVaiTroSearch", result);
            return PartialView("_VaiTroSearchResult", model);
        }
       
        /// <summary>
        /// gán dữ liệu cho danh sách vai trò
        /// Vai trò giờ xét trong cả hệ thống, không theo cơ sở gì nữa
        /// </summary>
        /// <param name="TENVAITRO"></param>
        /// <returns></returns>
        [HttpGet]
        public List<DmVaiTroBO> FillDataToGrid(string TENVAITRO = "")
        {
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            //DmCapCoSoBusiness = Get<DmCapCoSoBusiness>();
            //UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();

            return DmVaitroBusiness.GetListByParam();
            //var CheckLever = DmCapCoSoBusiness.GetLever(user.CAPCOSO_ID);
            //if (CheckLever)
            //{
            //    // NẾU LÀ CẤP CAO NHẤT( CẤP BỘ)
            //    var lstCapCoSo = ListCapCoSo(user.CAPCOSO_ID).Select(x => int.Parse(x.Value)).ToList();
            //    return DmVaitroBusiness.GetListByParam(lstCapCoSo);
            //}
            //else
            //{
            //    //NẾU LÀ CẤP CON
            //    return DmVaitroBusiness.GetListByLever(user.CAPCOSO_ID, user.CoSoID);

            //}
        }
        /// <summary>
        /// Load lại danh sách vai trò sau khi thêm, sửa, xóa
        /// </summary>
        /// <param name="TENVAITRO">Tên vai trò đang tìm kiếm</param>
        /// <returns></returns>
        public PartialViewResult ReloadGrid(string TENVAITRO)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            List<DmVaiTroBO> result = FillDataToGrid();
            ViewData["Search"] = "0";
            ViewData["TENVAITRO"] = TENVAITRO;
            SessionManager.SetValue("ListVaiTro", result);
            VaiTroViewModel model = new VaiTroViewModel();
            return PartialView("_VaiTroSearchResult", model);
        }
        /// <summary>
        /// khi tạo mới vai trò kiểm tra mã mgười dùng nhập đã tồn tại chưa
        /// </summary>
        /// <param name="MAVAITRO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult FindMa(string MAVAITRO)
        {
            DmVaitroBusiness = Get<DmVaitroBusiness>();

            var result = DmVaitroBusiness.All.Where(x => x.MA_VAITRO.ToLower() == MAVAITRO.ToLower()).Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// khi cập nhập vai trò kiểm tra mã mgười dùng nhập đã tồn tại chưa
        /// </summary>
        /// <param name="MAVAITRO">mã trước khi cập nhập</param>
        /// <param name="input">người dùng cập nhập</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult FindEditMa(string MAVAITRO, string input)
        {
            DmVaitroBusiness = Get<DmVaitroBusiness>();
            int count = 0;
            if (MAVAITRO.ToUpper() == input.ToUpper().Trim())
            {
                count = 0;
            }
            else
            {
                count = DmVaitroBusiness.All.Where(x => x.MA_VAITRO.ToLower() == input.ToLower()).Count();
            }
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetListChucNang(int DM_CHUCNANG_ID)
        {
            DmChucnangBusiness = Get<DmChucnangBusiness>();
            //var data = DmChucnangBusiness.All.Where(x => x.CHUCNANG_CHA == DM_CHUCNANG_ID);
            //if (data.Count() > 0)
            //{
            //    var result = data.Select(x => new
            //    {
            //        x.DM_CHUCNANG_ID,
            //        x.TEN_CHUCNANG
            //    }).ToList();
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}
            return Json(false);
        }
        public PartialViewResult GetUserInRole(int DM_VAITRO_ID)
        {
            UserInfoBO user = (UserInfoBO)GetUserInfo();
            NguoiDungVaiTroBusiness = Get<NguoiDungVaiTroBusiness>();

            ChucNangTrangChuViewModel model = new ChucNangTrangChuViewModel();
            if (user != null)
            {
                model.ListUserIDByRoleID = NguoiDungVaiTroBusiness.ListUserIDByRoleID(DM_VAITRO_ID);
            }
            model.DM_VAITRO_ID = DM_VAITRO_ID;
            return PartialView("_UserInRole", model);
        }
   

       
    }
}
