using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.Business;
using Model.eAita;
using Web.FwCore;
using Web.Custom;
using Web.Common;
using Business.CommonBusiness;
using Web.Areas.VanBanDenArea.Models;
using System.IO;
using System.Web.Configuration;
using Web.ServiceReference1;

namespace Web.Areas.VanBanDenArea.Controllers
{
    public class VanBanDenController : BaseController
    {
        //
        // GET: /VanBanDenArea/VanBanDen/
        private HscvDoKhanBusiness DoKhanBs;
        private HscvDonViBanHanhBusiness DonViBanHanhBs;
        private DmDonViBusiness DonViNhanHoSoBs;
        private HscvSoVanBanBusiness SoVanBanDenBs;
        private HscvLoaivanbanBusiness LoaiVanBanBs;
        private HscvVanbandenBusiness VanBanDenBs;
        private HscvFileuploadBusiness TaiLieuDinhKemBs;
        private DmNguoidungBusiness NguoiDungBs;
        private CosoBusiness CoSoBs;
        private DmDonViBusiness DonViBs;
        private DmPhongbanBusiness PhongBanBs;
        private HscvFileuploadBusiness FilesBs;
        private HscvNhomSoVanBanBusiness NhomSoBs;
        private HscvDoMatBusiness DoMatBs;
        private WfBuocchuyentrangthaiBusiness BuocChuyenTrangThaiBs;
        HnHelper helper = new HnHelper();
        private WfEntityRelationBusiness WfEntityRelationBusiness;
        private WfTrangthaiBusiness WfTrangthaiBusiness;
        private SysEntityMarkBusiness MarkBs;
        private WfLichsuchuyendoitrangthaiBusiness WfLichsuchuyendoitrangthaiBusiness;
        private HscvVanbandenDonviphoihopBusiness HscvVanbandenDonviphoihopBusiness;
        private HscvVanbandenNguoixulyBusiness HscvVanbandenNguoixulyBusiness;
        private HscvVanbandenPhongbanxulyBusiness HscvVanbandenPhongbanxulyBusiness;
        private HscvVanBanLienQuanBusiness HscvVanBanLienQuanBusiness;

        private TaiLieuDinhKemBusiness TaiLieuDinhKemBusiness;
        private string URLPath = WebConfigurationManager.AppSettings["FileUpload"];
        private string FileAllowUpload = WebConfigurationManager.AppSettings["VANBANDEN_FileAllowUpload"];
        private string MaxFileSizeUpload = WebConfigurationManager.AppSettings["VANBANDEN_MaxSizeUpload"];
        private bool isTruongDV = false, isTruongCQ = false;

        private void CheckRole(UserInfoBO user)
        {
            isTruongCQ = Ultilities.IsInActivities(user.ListThaoTac, "/VanBanDenArea/VanBanDen/IsCucTruong");
            isTruongDV = Ultilities.IsInActivities(user.ListThaoTac, "/VanBanDenArea/VanBanDen/IsTruongDonVi");
        }
        /// <summary>
        /// Hiển thị danh sách văn bản đến
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int ID = 0)
        {
            var TYPE = ID;
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            CheckRole(user);
            var VaiTroId = user.RoleID;
            #region 01. Lấy danh sách văn bản đến
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            WfEntityRelationBusiness = Get<WfEntityRelationBusiness>();
            WfTrangthaiBusiness = Get<WfTrangthaiBusiness>();
            VanBanDenSearchModel model = new VanBanDenSearchModel();
            model.TYPE = TYPE;
            int WF_ID = WfEntityRelationBusiness.GetWorkFlowId(user.CoSoID.Value, LOAITAILIEU.VANBANDEN);
            if (WF_ID > 0)
            {
                WF_TRANGTHAI tmp_obj = WfTrangthaiBusiness.GetWFTrangThai(WF_ID);
                if (tmp_obj != null)
                {
                    model.InitialState = tmp_obj.ID;
                }
            }
            else
            {
                model.InitialState = 0;
            }
            SessionManager.SetValue("ListBuocChuyenTrangThai", this.getTrangThai());
            var listVanBanDen = VanBanDenBs.GetListVanBanDen(user.CoSoID.Value, user.RoleID, user.DonViID, isTruongDV, isTruongCQ, LOAITAILIEU.VANBANDEN, (long)user.UserID, string.Empty, null, null, 0, 0, null, null, 0, false, 0, 0, TYPE);
            #endregion
            #region 02. Lưu danh sách văn bản đến vào session
            SessionManager.SetValue("ListVanBanDen", listVanBanDen);
            ViewData["Search"] = "0";
            #endregion
            #region 03. Khởi tạo các giá trị cho filter
            DoKhanBs = Get<HscvDoKhanBusiness>();
            DonViBanHanhBs = Get<HscvDonViBanHanhBusiness>();
            DonViNhanHoSoBs = Get<DmDonViBusiness>();
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            LoaiVanBanBs = Get<HscvLoaivanbanBusiness>();
            NhomSoBs = Get<HscvNhomSoVanBanBusiness>();
            //Initialize filter            
            model.ListDoKhan = DoKhanBs.All.Where(x => x.TRANGTHAI == true).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOKHAN,
                    Value = x.ID.ToString()
                }).ToList();
            model.ListCoQuanBanHanh = DonViBanHanhBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENDONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListDonViNhanHoSo = DonViNhanHoSoBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TEN_DONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            var LstNhomSoDen = NhomSoBs.All.Where(x => x.LOAIHOSO_ID == LOAIHOSO.VANBANDEN).ToList();
            List<int> LstNhomSoId = LstNhomSoDen.Select(x => x.ID).ToList();
            model.ListSoVanBanDen = SoVanBanDenBs.All.Where(x => LstNhomSoId.Contains((int)x.NHOMSOVANBAN_ID)).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENSO,
                    Value = x.ID.ToString()
                }
                ).ToList();

            model.ListLoaiVanBan = LoaiVanBanBs.All.Where(x => x.IS_DELETE == false).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENLOAIVANBAN,
                    Value = x.ID.ToString()
                }
                ).ToList();

            SessionManager.SetValue("InitialState", model.InitialState);
            #endregion
            return View(model);
        }
        public ActionResult DocumentMainProcess()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            var VaiTroId = user.RoleID;
            #region 01. Lấy danh sách văn bản đến
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            WfEntityRelationBusiness = Get<WfEntityRelationBusiness>();
            WfTrangthaiBusiness = Get<WfTrangthaiBusiness>();
            VanBanDenSearchModel model = new VanBanDenSearchModel();

            int WF_ID = WfEntityRelationBusiness.GetWorkFlowId(user.CoSoID.Value, LOAITAILIEU.VANBANDEN);
            if (WF_ID > 0)
            {
                WF_TRANGTHAI tmp_obj = WfTrangthaiBusiness.GetWFTrangThai(WF_ID);
                if (tmp_obj != null)
                {
                    model.InitialState = tmp_obj.ID;
                }
            }
            else
            {
                model.InitialState = 0;
            }
            SessionManager.SetValue("ListBuocChuyenTrangThai", this.getTrangThai());
            var listVanBanDen = VanBanDenBs.GetListVanBanDenMainProcess(LOAITAILIEU.VANBANDEN, (long)user.UserID, (long)user.UserID);
            #endregion
            #region 02. Lưu danh sách văn bản đến vào session
            SessionManager.SetValue("ListVanBanDen", listVanBanDen);
            ViewData["Search"] = "0";
            #endregion
            #region 03. Khởi tạo các giá trị cho filter
            DoKhanBs = Get<HscvDoKhanBusiness>();
            DonViBanHanhBs = Get<HscvDonViBanHanhBusiness>();
            DonViNhanHoSoBs = Get<DmDonViBusiness>();
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            LoaiVanBanBs = Get<HscvLoaivanbanBusiness>();
            NhomSoBs = Get<HscvNhomSoVanBanBusiness>();
            //Initialize filter            
            model.ListDoKhan = DoKhanBs.All.Where(x => x.TRANGTHAI == true).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOKHAN,
                    Value = x.ID.ToString()
                }).ToList();
            model.ListCoQuanBanHanh = DonViBanHanhBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENDONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListDonViNhanHoSo = DonViNhanHoSoBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TEN_DONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            var LstNhomSoDen = NhomSoBs.All.Where(x => x.LOAIHOSO_ID == LOAIHOSO.VANBANDEN).ToList();
            List<int> LstNhomSoId = LstNhomSoDen.Select(x => x.ID).ToList();
            model.ListSoVanBanDen = SoVanBanDenBs.All.Where(x => LstNhomSoId.Contains((int)x.NHOMSOVANBAN_ID)).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENSO,
                    Value = x.ID.ToString()
                }
                ).ToList();

            model.ListLoaiVanBan = LoaiVanBanBs.All.Where(x => x.IS_DELETE == false).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENLOAIVANBAN,
                    Value = x.ID.ToString()
                }
                ).ToList();

            SessionManager.SetValue("InitialState", model.InitialState);
            #endregion
            return View(model);
        }
        public ActionResult ListDelete()
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            CheckRole(user);
            SessionManager.SetValue("ListBuocChuyenTrangThai", this.getTrangThai());
            //Initialize business
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            //var listVanBanDen = VanBanDenBs.All.Where(x => x.IS_DELETE == true).ToList();
            var listVanBanDen = VanBanDenBs.GetListVanBanDen(user.CoSoID.Value, user.RoleID, user.DonViID, isTruongDV, isTruongCQ, LOAITAILIEU.VANBANDEN, (long)user.UserID, string.Empty, null, null, 0, 0, null, null, 0, true, 0);
            SessionManager.SetValue("ListDeleteVanBanDen", listVanBanDen);
            ViewData["Search"] = "0";

            DoKhanBs = Get<HscvDoKhanBusiness>();
            DonViBanHanhBs = Get<HscvDonViBanHanhBusiness>();
            DonViNhanHoSoBs = Get<DmDonViBusiness>();
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            LoaiVanBanBs = Get<HscvLoaivanbanBusiness>();

            //Initialize filter
            VanBanDenSearchModel model = new VanBanDenSearchModel();
            model.ListDoKhan = DoKhanBs.All.Where(x => x.TRANGTHAI == true).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOKHAN,
                    Value = x.ID.ToString()
                }).ToList();
            model.ListCoQuanBanHanh = DonViBanHanhBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENDONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListDonViNhanHoSo = DonViNhanHoSoBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TEN_DONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListSoVanBanDen = SoVanBanDenBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENSO,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListLoaiVanBan = LoaiVanBanBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENLOAIVANBAN,
                    Value = x.ID.ToString()
                }
                ).ToList();

            return View(model);
        }

        /// <summary>
        /// Tạo mới văn bản đến
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            DoKhanBs = Get<HscvDoKhanBusiness>();
            DonViBanHanhBs = Get<HscvDonViBanHanhBusiness>();
            DonViNhanHoSoBs = Get<DmDonViBusiness>();
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            LoaiVanBanBs = Get<HscvLoaivanbanBusiness>();
            NhomSoBs = Get<HscvNhomSoVanBanBusiness>();
            DoMatBs = Get<HscvDoMatBusiness>();
            SessionManager.SetValue("ListBuocChuyenTrangThai", this.getTrangThai());
            VanBanDenViewModel model = new VanBanDenViewModel();
            model.ListDoKhan = DoKhanBs.All.Where(x => x.TRANGTHAI == true).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOKHAN,
                    Value = x.ID.ToString()
                }).ToList();
            model.ListCoQuanBanHanh = DonViBanHanhBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENDONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();

            model.ListDonViNhanHoSo = DonViNhanHoSoBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TEN_DONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();

            model.ListSoVanBanDen = SoVanBanDenBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENSO,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListLoaiVanBan = LoaiVanBanBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENLOAIVANBAN,
                    Value = x.ID.ToString()
                }
                ).ToList();
            List<HSCV_NHOMSOVANBAN> LstNhomSoVanBan = NhomSoBs.All.Where(x => x.LOAIHOSO_ID == LOAIHOSO.VANBANDEN).ToList();
            model.ListNhomSoVanBan = LstNhomSoVanBan.Select(
                o => new SelectListItem()
                {
                    Text = o.NHOMSOVANBAN,
                    Value = o.ID.ToString(),
                    Selected = (LstNhomSoVanBan.Count() == 1 ? (o.ID == LstNhomSoVanBan.First().ID) : false)
                }
                ).ToList();
            model.ListDoMat = DoMatBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOMAT,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.UserInfo = userInfo;
            return View(model);
        }
        /// <summary>
        /// Xử lý tạo mới văn bản đến
        /// </summary>
        /// <returns></returns>
        /// 
        [ValidateInput(false)]
        public ActionResult CreateVanBanDen(IEnumerable<HttpPostedFileBase> filebase, string[] filename, FormCollection dataPost)
        {
            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            TaiLieuDinhKemBs = Get<HscvFileuploadBusiness>();
            WfLichsuchuyendoitrangthaiBusiness = Get<WfLichsuchuyendoitrangthaiBusiness>();
            HscvVanbandenDonviphoihopBusiness = Get<HscvVanbandenDonviphoihopBusiness>();
            HscvVanbandenNguoixulyBusiness = Get<HscvVanbandenNguoixulyBusiness>();
            HscvVanbandenPhongbanxulyBusiness = Get<HscvVanbandenPhongbanxulyBusiness>();
            WfEntityRelationBusiness = Get<WfEntityRelationBusiness>();
            WfTrangthaiBusiness = Get<WfTrangthaiBusiness>();
            HscvVanBanLienQuanBusiness = Get<HscvVanBanLienQuanBusiness>();

            //Create van ban den
            HSCV_VANBANDEN vbdModel = new HSCV_VANBANDEN();
            vbdModel.TIEUDE = dataPost["TIEUDE"];
            vbdModel.COQUANBANHANH_ID = dataPost["COQUANBANHANH_ID"].ToIntOrZero();
            vbdModel.COQUANBANHANHTEXT = dataPost["COQUANBANHANHTEXT"].Trim();
            vbdModel.SOKYHIEU = dataPost["SOKYHIEU"];
            vbdModel.LOAIVANBAN_ID = dataPost["LOAIVANBAN_ID"].ToIntOrZero();
            vbdModel.TRICHYEU = dataPost["TRICHYEU"];
            vbdModel.NGAYDEN = dataPost["NGAYDEN"].ToDateTimeNotNull();
            vbdModel.NGAYVANBAN = dataPost["NGAYVANBAN"].ToDateTimeNotNull();
            vbdModel.NHOMSOVANBAN_ID = dataPost["NHOMSOVANBANDEN_ID"].ToIntOrZero();
            vbdModel.SOVANBANDEN_ID = dataPost["SOVANBANDEN_ID"].ToIntOrZero();
            vbdModel.NOIDUNGVANBAN = dataPost["NOIDUNGVANBAN"].Trim();
            if (vbdModel.SOVANBANDEN_ID.HasValue)
            {
                vbdModel.SODEN = helper.GetAndUpdateSoDen(vbdModel.SOVANBANDEN_ID.Value);
            }

            int NGUOIXULYCHINH_ID = dataPost["ValueNguoiXuLyChinh"].Trim().ToIntOrZero();
            if (NGUOIXULYCHINH_ID > 0)
            {
                vbdModel.NGUOI_XU_LY_ID = NGUOIXULYCHINH_ID;
            }
            vbdModel.DOKHAN_ID = dataPost["DOKHAN_ID"].ToIntOrZero();
            vbdModel.SOTRANG = dataPost["SOTRANG"].ToIntOrZero();
            vbdModel.THOIHANXULY_BATDAU = dataPost["THOIHANXULY_BATDAU"].ToDateTime();
            vbdModel.THOIHANXULY_KETTHUC = dataPost["THOIHANXULY_KETTHUC"].ToDateTime();
            vbdModel.NGUOIKY = dataPost["NGUOIKY"];
            vbdModel.COSO_ID = userInfo.CoSoID;
            int DonViNhanId = dataPost["DONVINHANHOSO_ID"].ToIntOrZero();
            if (DonViNhanId > 0)
            {
                vbdModel.DONVINHANHOSO_ID = DonViNhanId;
            }
            int DoMatId = dataPost["DOMAT_ID"].ToIntOrZero();
            if (DoMatId > 0)
            {
                vbdModel.DOMAT_ID = DoMatId;
            }
            vbdModel.NGUOITAO = userInfo.UserID.ToString().ToLongOrZero();
            vbdModel.NGAYTAO = DateTime.Now;
            int WF_ID = WfEntityRelationBusiness.GetWorkFlowId(userInfo.CoSoID.Value, LOAITAILIEU.VANBANDEN);
            int WF_TRANGTHAI_ID = 0;
            if (WF_ID > 0)
            {
                vbdModel.WF_ID = WF_ID;
                WF_TRANGTHAI tmp_obj = WfTrangthaiBusiness.GetWFTrangThai(WF_ID);
                if (tmp_obj != null)
                {
                    vbdModel.MATRANGTHAI = tmp_obj.MATRANGTHAI;
                    vbdModel.TENTRANGTHAI = tmp_obj.TENTRANGTHAI;
                    vbdModel.WF_TRANGTHAI_ID = tmp_obj.ID;
                    WF_TRANGTHAI_ID = tmp_obj.ID;
                }
            }
            VanBanDenBs.Save(vbdModel);

            // save log into workflow log
            if (WF_ID > 0)
            {
                var WfHistoryBs = Get<WfLichsuchuyendoitrangthaiBusiness>();
                WF_LICHSUCHUYENDOITRANGTHAI model = new WF_LICHSUCHUYENDOITRANGTHAI();
                model.ENTITY_ID = LOAITAILIEU.VANBANDEN;

                model.ITEM_ID = vbdModel.ID;
                model.ENDSTATE = WF_TRANGTHAI_ID;
                model.NGUOITAO = (long)userInfo.UserID;
                model.NGAYTAO = DateTime.Now;
                WfHistoryBs.Save(model);
            }
            // end
            if (!string.IsNullOrEmpty(dataPost["DONVIPHOIHOPXULY_IDS"]))
            {
                string DONVIPHOIHOPXULY_IDS = dataPost["DONVIPHOIHOPXULY_IDS"].Trim();
                if (!string.IsNullOrEmpty(DONVIPHOIHOPXULY_IDS))
                {
                    string[] ARR_DONVIPHOIHOPXULY_IDS = DONVIPHOIHOPXULY_IDS.Split(',');
                    foreach (var dvxl_item in ARR_DONVIPHOIHOPXULY_IDS)
                    {
                        HSCV_VANBANDEN_DONVIPHOIHOP donviphoihop = new HSCV_VANBANDEN_DONVIPHOIHOP();
                        donviphoihop.VANBANDEN_ID = vbdModel.ID;
                        donviphoihop.DM_DONVI_ID = dvxl_item.ToIntOrZero();
                        HscvVanbandenDonviphoihopBusiness.Save(donviphoihop);
                    }
                }
            }
            if (!string.IsNullOrEmpty(dataPost["ADD_DEPARTMENT_ID"]))
            {
                string STR_ADD_DEPARTMENT_ID = dataPost["ADD_DEPARTMENT_ID"].Trim();
                if (!string.IsNullOrEmpty(STR_ADD_DEPARTMENT_ID))
                {
                    string[] ARR_DEPARTMENT_IDS = STR_ADD_DEPARTMENT_ID.Split(',');
                    if (ARR_DEPARTMENT_IDS.Count() > 0)
                    {
                        foreach (var pbxl_item in ARR_DEPARTMENT_IDS)
                        {
                            if (pbxl_item.ToIntOrZero() > 0)
                            {
                                HSCV_VANBANDEN_PHONGBANXULY phongbanxuly = new HSCV_VANBANDEN_PHONGBANXULY();
                                phongbanxuly.VANBANDEN_ID = vbdModel.ID;
                                phongbanxuly.PHONGBAN_ID = pbxl_item.ToIntOrZero();
                                HscvVanbandenPhongbanxulyBusiness.Save(phongbanxuly);
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(dataPost["ADD_USER_ID"]))
            {
                string STR_ADD_USER_ID = dataPost["ADD_USER_ID"].Trim();
                if (!string.IsNullOrEmpty(STR_ADD_USER_ID))
                {
                    string[] ARR_USER_ID = STR_ADD_USER_ID.Split(',');
                    if (ARR_USER_ID.Count() > 0)
                    {
                        foreach (var nguoixuly_item in ARR_USER_ID)
                        {
                            if (nguoixuly_item.ToIntOrZero() > 0)
                            {
                                HSCV_VANBANDEN_NGUOIXULY nguoixuly = new HSCV_VANBANDEN_NGUOIXULY();
                                nguoixuly.VANBANDEN_ID = vbdModel.ID;
                                nguoixuly.NGUOI_XU_LY_ID = nguoixuly_item.ToIntOrZero();
                                HscvVanbandenNguoixulyBusiness.Save(nguoixuly);
                            }
                        }
                    }
                }
            }

            #region Lưu thông tin tài liệu đính kèm
            UploadFileTool upload = new UploadFileTool();
            vbdModel.HAS_FILE = upload.UploadCustomFile(filebase, true, FileAllowUpload, URLPath, MaxFileSizeUpload.ToIntOrZero(), null, filename, vbdModel.ID, LOAITAILIEU.VANBANDEN, "Văn bản đến");
            VanBanDenBs.Save(vbdModel);

            #endregion
            #region Lưu thông tin văn bản liên quan

            if (!string.IsNullOrEmpty(dataPost["VanBanDenLienQuan"]))
            {
                var LstVanBanDenLienQuan = dataPost["VanBanDenLienQuan"].Trim().ToListLong(',');
                foreach (var item in LstVanBanDenLienQuan)
                {
                    HSCV_VANBANLIENQUAN vbLienQuan = new HSCV_VANBANLIENQUAN();
                    vbLienQuan.LOAI_VAN_BAN = LOAITAILIEU.VANBANDEN;
                    vbLienQuan.VAN_BAN_DEN_ID = vbdModel.ID;
                    vbLienQuan.VAN_BAN_LIEN_QUAN_ID = item;
                    HscvVanBanLienQuanBusiness.Save(vbLienQuan);
                }
            }
            if (!string.IsNullOrEmpty(dataPost["VanBanDiLienQuan"]))
            {
                var LstVanBanDiLienQuan = dataPost["VanBanDiLienQuan"].Trim().ToListLong(',');
                foreach (var item in LstVanBanDiLienQuan)
                {
                    HSCV_VANBANLIENQUAN vbLienQuan = new HSCV_VANBANLIENQUAN();
                    vbLienQuan.LOAI_VAN_BAN = LOAITAILIEU.VANBANDI;
                    vbLienQuan.VAN_BAN_DEN_ID = vbdModel.ID;
                    vbLienQuan.VAN_BAN_LIEN_QUAN_ID = item;
                    HscvVanBanLienQuanBusiness.Save(vbLienQuan);
                }
            }
            #endregion
            SessionManager.SetValue("Noti", 1);
            //end     
            return RedirectToAction("Index", "VanBanDen");
            //return View();
        }

        /// <summary>
        /// delete function
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult Delete(int ID, string SearchType)
        {
            if (ID > 0)
            {
                VanBanDenBs = Get<HscvVanbandenBusiness>();
                HscvVanbandenDonviphoihopBusiness = Get<HscvVanbandenDonviphoihopBusiness>();
                HscvVanbandenNguoixulyBusiness = Get<HscvVanbandenNguoixulyBusiness>();
                HscvVanbandenPhongbanxulyBusiness = Get<HscvVanbandenPhongbanxulyBusiness>();

                if (!string.IsNullOrEmpty(SearchType) && SearchType == "Raw")
                {
                    List<HSCV_VANBANDEN_DONVIPHOIHOP> LstHSCV_VANBANDEN_DONVIPHOIHOP = HscvVanbandenDonviphoihopBusiness.All.Where(x => x.VANBANDEN_ID == ID).ToList();
                    HscvVanbandenDonviphoihopBusiness.DeleteAll(LstHSCV_VANBANDEN_DONVIPHOIHOP);
                    HscvVanbandenDonviphoihopBusiness.Save();

                    List<HSCV_VANBANDEN_PHONGBANXULY> LstHSCV_VANBANDEN_PHONGBANXULY = HscvVanbandenPhongbanxulyBusiness.All.Where(x => x.VANBANDEN_ID == ID).ToList();
                    HscvVanbandenPhongbanxulyBusiness.DeleteAll(LstHSCV_VANBANDEN_PHONGBANXULY);
                    HscvVanbandenPhongbanxulyBusiness.Save();

                    List<HSCV_VANBANDEN_NGUOIXULY> LstHSCV_VANBANDEN_NGUOIXULY = HscvVanbandenNguoixulyBusiness.All.Where(x => x.VANBANDEN_ID == ID).ToList();
                    HscvVanbandenNguoixulyBusiness.DeleteAll(LstHSCV_VANBANDEN_NGUOIXULY);
                    HscvVanbandenNguoixulyBusiness.Save();

                    VanBanDenBs.Delete(ID, false);
                    VanBanDenBs.Save();
                }
                else
                {
                    var vbDen = VanBanDenBs.Find(ID);
                    vbDen.IS_DELETE = true;
                    VanBanDenBs.Save(vbDen);
                }
                return Json(new { Type = "SUCCESS", Message = "Xóa thành công loại văn bản" });
            }
            else
            {
                return Json(new { Type = "ERROR", Message = "Có lỗi xảy ra khi xóa loại văn bản" });
            }
        }
        public void UpdateTrangThaiRead(int ID, int TYPE_ID)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            MarkBs = Get<SysEntityMarkBusiness>();
            SYS_ENTITY_MARK tmp_obj = MarkBs.All.Where(x => x.ITEM_ID == ID && x.ITEM_TYPE == TYPE_ID && x.USER_ID == user.UserID).FirstOrDefault();
            if (tmp_obj == null)
            {
                tmp_obj = new SYS_ENTITY_MARK();
            }
            tmp_obj.ITEM_ID = ID;
            tmp_obj.ITEM_TYPE = TYPE_ID;
            tmp_obj.USER_ID = (long)user.UserID;
            tmp_obj.IS_READ = true;
            MarkBs.Save(tmp_obj);
        }


        public ActionResult ViewDetail(int ID, bool OnlyView = false)
        {
            UpdateTrangThaiRead(ID, LOAITAILIEU.VANBANDEN);
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            BuocChuyenTrangThaiBs = Get<WfBuocchuyentrangthaiBusiness>();
            WfLichsuchuyendoitrangthaiBusiness = Get<WfLichsuchuyendoitrangthaiBusiness>();
            TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            var vbDen = VanBanDenBs.Find(ID);
            SessionManager.SetValue("ListBuocChuyenTrangThai", this.getTrangThai());

            VanBanDenDetailModel model = new VanBanDenDetailModel();
            model.vbDen = vbDen;

            if (vbDen.DONVINHANHOSO_ID != null)
            {
                DonViBs = Get<DmDonViBusiness>();
                var dvObj = DonViBs.Find(vbDen.DONVINHANHOSO_ID);
                model.donvi = dvObj;
            }
            if (model.vbDen.VANBANDI_ID != null)
            {
                model.ListTaiLieuDinhKem = TaiLieuDinhKemBusiness.GetDataByItemID(model.vbDen.VANBANDI_ID.Value, LOAITAILIEU.VANBANDI);
            }
            else
            {
                model.ListTaiLieuDinhKem = TaiLieuDinhKemBusiness.GetDataByItemID(ID, LOAITAILIEU.VANBANDEN);
            }
            

            var WfLichsuchuyendoitrangthai = WfLichsuchuyendoitrangthaiBusiness.All.Where(x => x.ITEM_ID == ID && x.ENTITY_ID == LOAITAILIEU.VANBANDEN).OrderByDescending(x => x.LICHSUCHUYENDOI_ID).FirstOrDefault();
            int CurrentState;
            if (WfLichsuchuyendoitrangthai == null)
            {
                if (model.vbDen.WF_TRANGTHAI_ID != null)
                {
                    CurrentState = model.vbDen.WF_TRANGTHAI_ID.Value;
                    model.LstSteps = BuocChuyenTrangThaiBs.GetListByFromStateAndVaitro(CurrentState, user.RoleID, user.CoSoID.Value);
                }
            }
            else
            {
                CurrentState = WfLichsuchuyendoitrangthai.ENDSTATE.Value;
                model.LstSteps = BuocChuyenTrangThaiBs.GetListByFromStateAndVaitro(CurrentState, user.RoleID, user.CoSoID.Value);
            }
            model.OnlyView = OnlyView;
            return View(model);
        }

        /// <summary>
        /// View chi tiết văn bản đến để in
        /// </summary>
        /// <param name="ID">ID văn bản đến</param>
        /// <returns></returns>
        //public PartialViewResult ViewVanBanDen(int ID)
        //{            
        //    VanBanDenBs = Get<HscvVanbandenBusiness>();
        //    var vbDen = VanBanDenBs.Find(ID);
        //    VanBanDenDetailModel model = new VanBanDenDetailModel();
        //    model.vbDen = vbDen;

        //    if (vbDen.DONVINHANHOSO_ID != null)
        //    {
        //        DonViBs = Get<DmDonViBusiness>();
        //        var dvObj = DonViBs.Find(vbDen.DONVINHANHOSO_ID);
        //        model.donvi = dvObj;
        //    }

        //    FilesBs = Get<HscvFileuploadBusiness>();
        //    var fileObjs = FilesBs.All.Where(x => x.MODELNAME == "vanbanden" && x.ITEM_ID == ID).ToList();

        //    return PartialView("_ViewVanBanDen", model);
        //}
        /// <summary>
        /// Function edit van bản đến
        /// Hiển thị các thông tin của văn bản đến
        /// </summary>
        /// <param name="ID">Id của văn bản đến</param>
        /// <returns></returns>
        public ActionResult Edit(int? ID)
        {
            UpdateTrangThaiRead((int)ID, LOAITAILIEU.VANBANDEN);
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            HSCV_VANBANDEN VanBanDen;
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            if (ID > 0)
            {
                VanBanDen = VanBanDenBs.Find(ID);
                if (VanBanDen == null)
                {
                    return RedirectToAction("Index", "VanBanDen");
                }
            }
            else
            {
                return RedirectToAction("Index", "VanBanDen");
            }
            DoKhanBs = Get<HscvDoKhanBusiness>();
            DonViBanHanhBs = Get<HscvDonViBanHanhBusiness>();
            DonViNhanHoSoBs = Get<DmDonViBusiness>();
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            LoaiVanBanBs = Get<HscvLoaivanbanBusiness>();
            TaiLieuDinhKemBs = Get<HscvFileuploadBusiness>();
            NhomSoBs = Get<HscvNhomSoVanBanBusiness>();
            DoMatBs = Get<HscvDoMatBusiness>();
            TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            HscvVanBanLienQuanBusiness = Get<HscvVanBanLienQuanBusiness>();

            VanBanDenViewModel model = new VanBanDenViewModel();
            model.ListDoKhan = DoKhanBs.All.Where(x => x.TRANGTHAI == true).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOKHAN,
                    Value = x.ID.ToString(),
                    Selected = (x.ID == VanBanDen.DOKHAN_ID)
                }).ToList();
            model.ListCoQuanBanHanh = DonViBanHanhBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENDONVI,
                    Value = x.ID.ToString(),
                    Selected = (x.ID == VanBanDen.COQUANBANHANH_ID)
                }
                ).ToList();
            model.ListDonViNhanHoSo = DonViNhanHoSoBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TEN_DONVI,
                    Value = x.ID.ToString(),
                    Selected = (x.ID == VanBanDen.DONVINHANHOSO_ID)
                }
                ).ToList();
            model.ListSoVanBanDen = SoVanBanDenBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENSO,
                    Value = x.ID.ToString(),
                    Selected = (x.ID == VanBanDen.SOVANBANDEN_ID)
                }
                ).ToList();
            model.ListLoaiVanBan = LoaiVanBanBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENLOAIVANBAN,
                    Value = x.ID.ToString(),
                    Selected = (x.ID == VanBanDen.LOAIVANBAN_ID)
                }
                ).ToList();
            model.ListNhomSoVanBan = NhomSoBs.All.Where(x => x.LOAIHOSO_ID == LOAIHOSO.VANBANDEN).ToList().Select(
                o => new SelectListItem()
                {
                    Text = o.NHOMSOVANBAN,
                    Value = o.ID.ToString(),
                    Selected = (o.ID == VanBanDen.NHOMSOVANBAN_ID)
                }
                ).ToList();
            model.ListDoMat = DoMatBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOMAT,
                    Value = x.ID.ToString(),
                    Selected = (x.ID == VanBanDen.DOMAT_ID)
                }
                ).ToList();

            model.ListTaiLieuDinhKem = TaiLieuDinhKemBusiness.GetDataByItemID((long)ID, LOAITAILIEU.VANBANDEN);
            model.ListVanBanLienQuan = HscvVanBanLienQuanBusiness.getVanbanLienQuan((long)ID, 0, LOAITAILIEU.VANBANDEN, LOAITAILIEU.VANBANDI).ToList();

            //model.ListFiles = TaiLieuDinhKemBs.All.Where(x => x.ITEM_ID == VanBanDen.ID && x.MODELNAME == "vanbanden").ToList();
            model.VanBanDen = VanBanDen;
            model.UserInfo = user;
            // nguoi phoi hop
            ICollection<HSCV_VANBANDEN_NGUOIXULY> HSCV_VANBANDEN_NGUOIXULY = VanBanDen.HSCV_VANBANDEN_NGUOIXULY;
            if (HSCV_VANBANDEN_NGUOIXULY.Count > 0)
            {
                string NGUOIXULY_TEXT = "";
                string NGUOIXULY_VALUE = "";
                foreach (var item in HSCV_VANBANDEN_NGUOIXULY)
                {
                    NGUOIXULY_TEXT += item.DM_NGUOIDUNG.TENDANGNHAP + ",";
                    NGUOIXULY_VALUE += item.NGUOI_XU_LY_ID + ",";
                }
                model.NguoiPhoiHopText = NGUOIXULY_TEXT.Substring(0, NGUOIXULY_TEXT.Length - 1);
                model.NguoiPhoiHopValue = NGUOIXULY_VALUE.Substring(0, NGUOIXULY_VALUE.Length - 1);
            }
            // phong ban phoi hop
            ICollection<HSCV_VANBANDEN_PHONGBANXULY> HSCV_VANBANDEN_PHONGBANXULY = VanBanDen.HSCV_VANBANDEN_PHONGBANXULY;
            if (HSCV_VANBANDEN_PHONGBANXULY.Count > 0)
            {
                string PBXULY_TEXT = "";
                string PBXULY_VALUE = "";
                foreach (var item in HSCV_VANBANDEN_PHONGBANXULY)
                {
                    PBXULY_TEXT += item.DM_PHONGBAN.TENPHONGBAN + ",";
                    PBXULY_VALUE += item.PHONGBAN_ID + ",";
                }
                model.PhongBanPhoiHopText = PBXULY_TEXT.Substring(0, PBXULY_TEXT.Length - 1);
                model.PhongBanPhoiHopValue = PBXULY_VALUE.Substring(0, PBXULY_VALUE.Length - 1);
            }
            // đơn vị phối hợp
            ICollection<HSCV_VANBANDEN_DONVIPHOIHOP> HSCV_VANBANDEN_DONVIPHOIHOP = VanBanDen.HSCV_VANBANDEN_DONVIPHOIHOP;
            List<int> LST_DONVIPHOIHOP_IDS = HSCV_VANBANDEN_DONVIPHOIHOP.Select(x => x.DM_DONVI_ID).ToList();
            model.ListDonViPhoiHop = DonViNhanHoSoBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TEN_DONVI,
                    Value = x.ID.ToString(),
                    Selected = LST_DONVIPHOIHOP_IDS.Contains(x.ID)
                }
                ).ToList();
            SessionManager.SetValue("ListBuocChuyenTrangThai", this.getTrangThai());
            return View(model);
        }
        /// <summary>
        /// function update van ban den
        /// </summary>
        /// <returns></returns>
        /// 
        [ValidateInput(false)]
        public ActionResult UpdateVanBanDen(IEnumerable<HttpPostedFileBase> filebase, string[] filename, FormCollection dataPost)
        {
            HscvVanbandenDonviphoihopBusiness = Get<HscvVanbandenDonviphoihopBusiness>();
            HscvVanbandenNguoixulyBusiness = Get<HscvVanbandenNguoixulyBusiness>();
            HscvVanbandenPhongbanxulyBusiness = Get<HscvVanbandenPhongbanxulyBusiness>();
            TaiLieuDinhKemBusiness = Get<TaiLieuDinhKemBusiness>();
            HscvVanBanLienQuanBusiness = Get<HscvVanBanLienQuanBusiness>();

            UserInfoBO userInfo = (UserInfoBO)SessionManager.GetUserInfo();
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            TaiLieuDinhKemBs = Get<HscvFileuploadBusiness>();
            //var dataPost = Request.Form;

            int ID = dataPost["ID"].ToString().ToIntOrZero();
            if (ID <= 0)
            {
                return RedirectToAction("Index");
            }

            //Create van ban den
            HSCV_VANBANDEN vbdModel = VanBanDenBs.Find(ID);
            vbdModel.TIEUDE = dataPost["TIEUDE"];
            vbdModel.COQUANBANHANH_ID = dataPost["COQUANBANHANH_ID"].ToIntOrZero();
            vbdModel.SOKYHIEU = dataPost["SOKYHIEU"];
            vbdModel.LOAIVANBAN_ID = dataPost["LOAIVANBAN_ID"].ToIntOrZero();
            vbdModel.TRICHYEU = dataPost["TRICHYEU"];
            vbdModel.NGAYDEN = dataPost["NGAYDEN"].ToDateTimeNotNull();
            vbdModel.NGAYVANBAN = dataPost["NGAYVANBAN"].ToDateTimeNotNull();
            //vbdModel.SOVANBANDEN_ID = dataPost["SOVANBANDEN_ID"].ToIntOrZero();
            //vbdModel.SODEN = dataPost["SODEN"].ToIntOrZero();
            vbdModel.DOKHAN_ID = dataPost["DOKHAN_ID"].ToIntOrZero();
            vbdModel.SOTRANG = dataPost["SOTRANG"].ToIntOrZero();
            vbdModel.THOIHANXULY_BATDAU = dataPost["THOIHANXULY_BATDAU"].ToDateTime();
            vbdModel.THOIHANXULY_KETTHUC = dataPost["THOIHANXULY_KETTHUC"].ToDateTime();
            vbdModel.NOIDUNGVANBAN = dataPost["NOIDUNGVANBAN"].Trim();
            vbdModel.NGUOITAO = userInfo.UserID.ToString().ToLongOrZero();
            vbdModel.NGAYTAO = DateTime.Now;
            int DonViNhanId = dataPost["DONVINHANHOSO_ID"].ToIntOrZero();
            if (DonViNhanId > 0)
            {
                vbdModel.DONVINHANHOSO_ID = DonViNhanId;
            }

            if (!string.IsNullOrEmpty(dataPost["DONVIPHOIHOPXULY_IDS"]))
            {
                string DONVIPHOIHOPXULY_IDS = dataPost["DONVIPHOIHOPXULY_IDS"].Trim();
                string[] ARR_DONVIPHOIHOPXULY_IDS = DONVIPHOIHOPXULY_IDS.Split(',');
                foreach (var dvxl_item in ARR_DONVIPHOIHOPXULY_IDS)
                {
                    int dvxl_id = dvxl_item.ToIntOrZero();
                    if (dvxl_id > 0)
                    {
                        if (HscvVanbandenDonviphoihopBusiness.All.Where(x => x.VANBANDEN_ID == vbdModel.ID && x.DM_DONVI_ID == dvxl_id).FirstOrDefault() == null)
                        {
                            HSCV_VANBANDEN_DONVIPHOIHOP donviphoihop = new HSCV_VANBANDEN_DONVIPHOIHOP();
                            donviphoihop.VANBANDEN_ID = vbdModel.ID;
                            donviphoihop.DM_DONVI_ID = dvxl_id;
                            HscvVanbandenDonviphoihopBusiness.Save(donviphoihop);
                        }
                    }
                }
                List<HSCV_VANBANDEN_DONVIPHOIHOP> LstHSCV_VANBANDEN_DONVIPHOIHOP = vbdModel.HSCV_VANBANDEN_DONVIPHOIHOP.ToList().Where(x => x.VANBANDEN_ID == vbdModel.ID && !ARR_DONVIPHOIHOPXULY_IDS.Contains(x.DM_DONVI_ID.ToString())).ToList();
                HscvVanbandenDonviphoihopBusiness.DeleteAll(LstHSCV_VANBANDEN_DONVIPHOIHOP);
                HscvVanbandenDonviphoihopBusiness.Save();
            }
            else
            {
                List<HSCV_VANBANDEN_DONVIPHOIHOP> LstHSCV_VANBANDEN_DONVIPHOIHOP = vbdModel.HSCV_VANBANDEN_DONVIPHOIHOP.ToList().Where(x => x.VANBANDEN_ID == vbdModel.ID).ToList();
                HscvVanbandenDonviphoihopBusiness.DeleteAll(LstHSCV_VANBANDEN_DONVIPHOIHOP);
                HscvVanbandenDonviphoihopBusiness.Save();
            }

            if (!string.IsNullOrEmpty(dataPost["ADD_DEPARTMENT_ID"]))
            {
                string STR_ADD_DEPARTMENT_ID = dataPost["ADD_DEPARTMENT_ID"].Trim();
                string[] ARR_DEPARTMENT_IDS = STR_ADD_DEPARTMENT_ID.Split(',');
                if (ARR_DEPARTMENT_IDS.Count() > 0)
                {
                    foreach (var pbxl_item in ARR_DEPARTMENT_IDS)
                    {
                        int pbxl_id = pbxl_item.ToIntOrZero();
                        if (pbxl_id > 0)
                        {
                            if (HscvVanbandenPhongbanxulyBusiness.All.Where(x => x.VANBANDEN_ID == vbdModel.ID && x.PHONGBAN_ID == pbxl_id).FirstOrDefault() == null)
                            {
                                HSCV_VANBANDEN_PHONGBANXULY phongbanxuly = new HSCV_VANBANDEN_PHONGBANXULY();
                                phongbanxuly.VANBANDEN_ID = vbdModel.ID;
                                phongbanxuly.PHONGBAN_ID = pbxl_id;
                                HscvVanbandenPhongbanxulyBusiness.Save(phongbanxuly);
                            }
                        }
                    }
                    List<HSCV_VANBANDEN_PHONGBANXULY> LstHSCV_VANBANDEN_PHONGBANXULY = vbdModel.HSCV_VANBANDEN_PHONGBANXULY.ToList().Where(x => x.VANBANDEN_ID == vbdModel.ID && !ARR_DEPARTMENT_IDS.Contains(x.PHONGBAN_ID.ToString())).ToList();
                    HscvVanbandenPhongbanxulyBusiness.DeleteAll(LstHSCV_VANBANDEN_PHONGBANXULY);
                    HscvVanbandenPhongbanxulyBusiness.Save();
                }
            }
            else
            {
                List<HSCV_VANBANDEN_PHONGBANXULY> LstHSCV_VANBANDEN_PHONGBANXULY = vbdModel.HSCV_VANBANDEN_PHONGBANXULY.ToList().Where(x => x.VANBANDEN_ID == vbdModel.ID).ToList();
                HscvVanbandenPhongbanxulyBusiness.DeleteAll(LstHSCV_VANBANDEN_PHONGBANXULY);
                HscvVanbandenPhongbanxulyBusiness.Save();
            }
            if (!string.IsNullOrEmpty(dataPost["ADD_USER_ID"]))
            {
                string STR_ADD_USER_ID = dataPost["ADD_USER_ID"].Trim();
                string[] ARR_USER_ID = STR_ADD_USER_ID.Split(',');
                if (ARR_USER_ID.Count() > 0)
                {
                    foreach (var nguoixuly_item in ARR_USER_ID)
                    {
                        long nguoixuly_id = nguoixuly_item.ToIntOrZero();
                        if (nguoixuly_id > 0)
                        {
                            if (HscvVanbandenNguoixulyBusiness.All.Where(x => x.VANBANDEN_ID == vbdModel.ID && x.NGUOI_XU_LY_ID == nguoixuly_id).FirstOrDefault() == null)
                            {
                                HSCV_VANBANDEN_NGUOIXULY nguoixuly = new HSCV_VANBANDEN_NGUOIXULY();
                                nguoixuly.VANBANDEN_ID = vbdModel.ID;
                                nguoixuly.NGUOI_XU_LY_ID = nguoixuly_id;
                                HscvVanbandenNguoixulyBusiness.Save(nguoixuly);
                            }
                        }
                    }
                    List<HSCV_VANBANDEN_NGUOIXULY> LstHSCV_VANBANDEN_NGUOIXULY = vbdModel.HSCV_VANBANDEN_NGUOIXULY.ToList().Where(x => x.VANBANDEN_ID == vbdModel.ID && !ARR_USER_ID.Contains(x.NGUOI_XU_LY_ID.ToString())).ToList();
                    HscvVanbandenNguoixulyBusiness.DeleteAll(LstHSCV_VANBANDEN_NGUOIXULY);
                    HscvVanbandenNguoixulyBusiness.Save();
                }
            }
            else
            {
                List<HSCV_VANBANDEN_NGUOIXULY> LstHSCV_VANBANDEN_NGUOIXULY = vbdModel.HSCV_VANBANDEN_NGUOIXULY.ToList().Where(x => x.VANBANDEN_ID == vbdModel.ID).ToList();
                HscvVanbandenNguoixulyBusiness.DeleteAll(LstHSCV_VANBANDEN_NGUOIXULY);
                HscvVanbandenNguoixulyBusiness.Save();
            }

            #region Lưu thông tin tài liệu đính kèm
            UploadFileTool upload = new UploadFileTool();
            vbdModel.HAS_FILE = upload.UploadCustomFile(filebase, true, FileAllowUpload, URLPath, MaxFileSizeUpload.ToIntOrZero(), null, filename, vbdModel.ID, LOAITAILIEU.VANBANDEN, "Văn bản đến");
            var LstTaiLieu = TaiLieuDinhKemBusiness.GetDataByItemID((long)vbdModel.ID, LOAITAILIEU.VANBANDEN);
            if (LstTaiLieu.Count() > 0)
            {
                vbdModel.HAS_FILE = true;
            }
            else
            {
                vbdModel.HAS_FILE = false;
            }
            VanBanDenBs.Save(vbdModel);

            #endregion
            VanBanDenBs.Save(vbdModel);

            #region Update van ban lien quan
            if (!string.IsNullOrEmpty(dataPost["VanBanDenLienQuan"]))
            {
                var LstVanBanDenLienQuan = dataPost["VanBanDenLienQuan"].Trim().ToListLong(',');
                foreach (var item in LstVanBanDenLienQuan)
                {
                    if (HscvVanBanLienQuanBusiness.All.Where(x => x.ID == item).FirstOrDefault() == null)
                    {
                        HSCV_VANBANLIENQUAN vbLienQuan = new HSCV_VANBANLIENQUAN();
                        vbLienQuan.LOAI_VAN_BAN = LOAITAILIEU.VANBANDEN;
                        vbLienQuan.VAN_BAN_DEN_ID = vbdModel.ID;
                        vbLienQuan.VAN_BAN_LIEN_QUAN_ID = item;
                        HscvVanBanLienQuanBusiness.Save(vbLienQuan);
                    }
                }
            }
            if (!string.IsNullOrEmpty(dataPost["VanBanDiLienQuan"]))
            {
                var LstVanBanDiLienQuan = dataPost["VanBanDiLienQuan"].Trim().ToListLong(',');
                foreach (var item in LstVanBanDiLienQuan)
                {
                    if (HscvVanBanLienQuanBusiness.All.Where(x => x.ID == item).FirstOrDefault() == null)
                    {
                        HSCV_VANBANLIENQUAN vbLienQuan = new HSCV_VANBANLIENQUAN();
                        vbLienQuan.LOAI_VAN_BAN = LOAITAILIEU.VANBANDI;
                        vbLienQuan.VAN_BAN_DEN_ID = vbdModel.ID;
                        vbLienQuan.VAN_BAN_LIEN_QUAN_ID = item;
                        HscvVanBanLienQuanBusiness.Save(vbLienQuan);
                    }
                }
            }
            #endregion
            SessionManager.SetValue("Noti", 1);
            return RedirectToAction("Edit", new { ID = ID });
            //end            
            //return View();
        }

        /// <summary>
        /// Function call qua ajax khi thay đổi nhóm sổ văn bản đến
        /// </summary>
        /// <param name="ID">Id của nhóm sổ văn bản đến</param>
        /// <returns>Sổ văn bản đến tương ứng có trong nhóm sổ văn bản đến được chọn</returns>
        public PartialViewResult ChangeNhomSoVBDen(int ID)
        {
            VanBanDenViewModel model = new VanBanDenViewModel();
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            model.ListSoVanBanDen = SoVanBanDenBs.All.Where(x => x.NHOMSOVANBAN_ID == ID).ToList().Select(
                o => new SelectListItem()
                {
                    Text = o.TENSO,
                    Value = o.ID.ToString()
                }
                ).ToList();
            return PartialView("_DropDownNhomSoVanBanDen", model);
        }

        /// <summary>
        /// Chọn số văn bản đến trong sổ văn bản đến
        /// </summary>
        /// <param name="ID">Id của sổ văn bản đến</param>
        /// <returns>Số thứ tự cao nhất có trong sổ văn bản đến được chọn</returns>
        public int? ChangeSoVanBan(int ID)
        {
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            var SoVB = SoVanBanDenBs.Find(ID);
            if (SoVB != null)
            {
                return SoVB.SOVBTHEOSO + 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Function tìm kiếm văn bản đến theo các tiêu chí
        /// </summary>
        /// <param name="TRICHYEU"></param>
        /// <param name="SOKYHIEU"></param>
        /// <param name="LOAIVANBAN_ID"></param>
        /// <param name="SOVANBANDEN_ID"></param>
        /// <param name="SODEN"></param>
        /// <param name="COQUANBANHANH_ID"></param>
        /// <param name="DONVINHANHOSO_ID"></param>
        /// <param name="DOKHAN_ID"></param>
        /// <param name="fromdate_NGAYDEN"></param>
        /// <param name="todate_NGAYDEN"></param>
        /// <param name="fromdate_NGAYVANBAN"></param>
        /// <param name="todate_NGAYVANBAN"></param>
        /// <returns></returns>
        public PartialViewResult FindVanBanDen(string SearchType, FormCollection dataSearch)
        {
            //string TRICHYEU, string SOKYHIEU, int? LOAIVANBAN_ID, int? SOVANBANDEN_ID, int? SODEN, int? COQUANBANHANH_ID, int? DONVINHANHOSO_ID, int? DOKHAN_ID, string fromdate_NGAYDEN, string todate_NGAYDEN, string fromdate_NGAYVANBAN, string todate_NGAYVANBAN, string SearchType
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            string TRICHYEU = dataSearch["TRICHYEU"];
            string SOKYHIEU = dataSearch["SOKYHIEU"];
            //string SearchType = dataSearch["SearchType"];

            List<VanBanDenBO> result = new List<VanBanDenBO>();
            bool check_partial = true;
            if (!string.IsNullOrEmpty(SearchType) && SearchType == "Delete")
            {
                check_partial = false;
                result = (List<VanBanDenBO>)SessionManager.GetValue("ListDeleteVanBanDen");
            }
            else if (SearchType == "Trangthai")
            {
                check_partial = true;
                result = (List<VanBanDenBO>)SessionManager.GetValue("ListVanBanDenByTrangThai");
            }
            else
            {
                check_partial = true;
                result = (List<VanBanDenBO>)SessionManager.GetValue("ListVanBanDen");
            }
            if (!string.IsNullOrEmpty(TRICHYEU))
            {
                //result = result.Where(x => x.TRICHYEU.ToLower().Contains(TRICHYEU.ToLower())).ToList();
                TRICHYEU = Ultilities.RemoveSign4VietnameseString(TRICHYEU);
                result = result.Where(x => Ultilities.RemoveSign4VietnameseString(x.TRICHYEU.ToLower()).Contains(TRICHYEU.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(SOKYHIEU))
            {
                result = result.Where(x => x.SOKYHIEU.ToLower().Contains(SOKYHIEU.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["LOAIVANBAN_ID"]))
            {
                List<int> LOAIVANBAN_ID = dataSearch["LOAIVANBAN_ID"].ToListInt(',');
                result = result.Where(x => x.LOAIVANBAN_ID.HasValue == false || LOAIVANBAN_ID.Contains(x.LOAIVANBAN_ID.Value)).ToList();
            }

            if (!string.IsNullOrEmpty(dataSearch["SOVANBANDEN_ID"]))
            {
                List<int> SOVANBANDEN_ID = dataSearch["SOVANBANDEN_ID"].Trim().ToListInt(',');
                result = result.Where(x => x.SOVANBANDEN_ID.HasValue == false || SOVANBANDEN_ID.Contains(x.SOVANBANDEN_ID.Value)).ToList();
            }

            if (!string.IsNullOrEmpty(dataSearch["SODEN"]))
            {
                int SODEN = dataSearch["SODEN"].Trim().ToIntOrZero();
                result = result.Where(x => x.SODEN == SODEN).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["COQUANBANHANH_ID"]))
            {
                List<int> COQUANBANHANH_ID = dataSearch["COQUANBANHANH_ID"].ToListInt(',');
                result = result.Where(x => COQUANBANHANH_ID.Contains(x.COQUANBANHANH_ID.Value)).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["DONVINHANHOSO_ID"]))
            {
                List<int> DONVINHANHOSO_ID = dataSearch["DONVINHANHOSO_ID"].ToListInt(',');
                result = result.Where(x => DONVINHANHOSO_ID.Contains(x.DONVINHANHOSO_ID.Value)).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["DOKHAN_ID"]))
            {
                List<int> DOKHAN_ID = dataSearch["DOKHAN_ID"].ToListInt(',');
                result = result.Where(x => DOKHAN_ID.Contains(x.DOKHAN_ID.Value)).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["fromdate_NGAYDEN"]))
            {
                var FROMDATE = dataSearch["fromdate_NGAYDEN"].Trim().ToDateTime();
                result = result.Where(o => o.NGAYDEN >= FROMDATE).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["fromdate_NGAYDEN"]))
            {
                var FROMDATE_NGAYDEN = dataSearch["fromdate_NGAYDEN"].Trim().ToDateTime();
                result = result.Where(o => o.NGAYDEN >= FROMDATE_NGAYDEN).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["todate_NGAYDEN"]))
            {
                var TODATE_NGAYDEN = dataSearch["todate_NGAYDEN"].Trim().ToDateTime();
                result = result.Where(o => o.NGAYDEN <= TODATE_NGAYDEN).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["fromdate_NGAYVANBAN"]))
            {
                var FROMDATE_NGAYVB = dataSearch["fromdate_NGAYVANBAN"].Trim().ToDateTime();
                result = result.Where(o => o.NGAYVANBAN >= FROMDATE_NGAYVB).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["todate_NGAYVANBAN"]))
            {
                var TODATE_NGAYVB = dataSearch["todate_NGAYVANBAN"].Trim().ToDateTime();
                result = result.Where(o => o.NGAYVANBAN <= TODATE_NGAYVB).ToList();
            }
            if (!string.IsNullOrEmpty(dataSearch["MARKITEM"]))
            {
                List<int> MARKITEM = dataSearch["MARKITEM"].Trim().ToListInt(',');
                if (MARKITEM.Count != 6)
                {
                    if (!(MARKITEM.Contains(LOAITAILIEU.CONS_READ) && MARKITEM.Contains(LOAITAILIEU.CONS_UNREAD))
                        && !(MARKITEM.Contains(LOAITAILIEU.CONS_FAVORITE) && MARKITEM.Contains(LOAITAILIEU.CONS_UNFAVORITE))
                        && !(MARKITEM.Contains(LOAITAILIEU.CONS_IMPORTANT) && MARKITEM.Contains(LOAITAILIEU.CONS_UNIMPORTANT))
                        )
                    {
                        if (MARKITEM.Contains(LOAITAILIEU.CONS_READ))
                        {
                            result = result.Where(o => o.IS_READ == true).ToList();
                        }
                        if (MARKITEM.Contains(LOAITAILIEU.CONS_UNREAD))
                        {
                            result = result.Where(o => o.IS_READ != true).ToList();
                        }
                        if (MARKITEM.Contains(LOAITAILIEU.CONS_FAVORITE))
                        {
                            result = result.Where(o => o.IS_FAVORITE == true).ToList();
                        }
                        if (MARKITEM.Contains(LOAITAILIEU.CONS_UNFAVORITE))
                        {
                            result = result.Where(o => o.IS_FAVORITE != true).ToList();
                        }
                        if (MARKITEM.Contains(LOAITAILIEU.CONS_IMPORTANT))
                        {
                            result = result.Where(o => o.IS_IMPORTANT == true).ToList();
                        }
                        if (MARKITEM.Contains(LOAITAILIEU.CONS_UNIMPORTANT))
                        {
                            result = result.Where(o => o.IS_IMPORTANT != true).ToList();
                        }
                    }


                }

            }
            ViewData["Search"] = "1";
            if (check_partial == true)
            {
                if (SearchType == "Trangthai")
                {
                    SessionManager.SetValue("ListVanBanDenByTrangThaiSearch", result);
                    return PartialView("_DanhSachVanBan");
                }
                else
                {
                    SessionManager.SetValue("ListVanBanDenSearch", result);
                    return PartialView("_VanBanDenSearchResult");
                }
            }
            else
            {
                SessionManager.SetValue("ListDeleteVanBanDenSearch", result);
                return PartialView("_VanBanDenDeleteSearchResult");
            }
        }
        /// <summary>
        /// Function khôi phục lại văn bản đã xóa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Resolve(int ID)
        {
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            var vbDen = VanBanDenBs.Find(ID);
            vbDen.IS_DELETE = false;
            VanBanDenBs.Save(vbDen);
            return Json(new { Type = "SUCCESS", Message = "Khôi phục thành công loại văn bản" });
        }
        public PartialViewResult reloadGrid(string SearchType)
        {
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            CheckRole(user);
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            ViewData["Search"] = "0";


            if (!string.IsNullOrEmpty(SearchType) && SearchType == "Delete")
            {
                var listVanBanDen = VanBanDenBs.GetListVanBanDen(user.CoSoID.Value, user.RoleID, user.DonViID, isTruongDV, isTruongCQ, LOAITAILIEU.VANBANDEN, (long)user.UserID, string.Empty, null, null, 0, 0, null, null, 0, true, 0);
                SessionManager.SetValue("ListDeleteVanBanDen", listVanBanDen);
                return PartialView("_VanBanDenDeleteSearchResult");
            }
            else
            {
                var listVanBanDen = VanBanDenBs.GetListVanBanDen(user.CoSoID.Value, user.RoleID, user.DonViID, isTruongDV, isTruongCQ, LOAITAILIEU.VANBANDEN, (long)user.UserID, string.Empty, null, null, 0, 0, null, null, 0, false, 0);
                SessionManager.SetValue("ListVanBanDen", listVanBanDen);
                return PartialView("_VanBanDenSearchResult");
            }
        }

        public PartialViewResult reloadGridDanhSach(int ID)
        {
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            CheckRole(user);
            var listVanBanDen = VanBanDenBs.GetListVanBanDen(user.CoSoID.Value, user.RoleID, user.DonViID, isTruongDV, isTruongCQ, LOAITAILIEU.VANBANDEN, (long)user.UserID, string.Empty, null, null, 0, 0, null, null, 0, false, 0, ID);
            SessionManager.SetValue("ListVanBanDenByTrangThai", listVanBanDen);
            ViewData["Search"] = "0";
            return PartialView("_DanhSachVanBan");
        }

        public List<WF_TRANGTHAI> getTrangThai()
        {
            BuocChuyenTrangThaiBs = Get<WfBuocchuyentrangthaiBusiness>();
            WfTrangthaiBusiness = Get<WfTrangthaiBusiness>();
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            List<WF_BUOCCHUYENTRANGTHAI> LstBuocChuyenTrangThai = BuocChuyenTrangThaiBs.GetListActionByVaitro(user.RoleID, LOAITAILIEU.VANBANDEN, user.CoSoID.Value);
            List<int> TRANGTHAI_IDS = new List<int>();
            foreach (var item in LstBuocChuyenTrangThai)
            {
                TRANGTHAI_IDS.Add(item.TRANGTHAI_1.Value);
                TRANGTHAI_IDS.Add(item.TRANGTHAI_2.Value);
            }
            TRANGTHAI_IDS.Distinct();
            if (TRANGTHAI_IDS.Count > 0)
            {
                return WfTrangthaiBusiness.All.Where(x => TRANGTHAI_IDS.Contains(x.ID)).ToList();
            }
            else
            {
                return null;
            }
        }
        public ActionResult DanhSachVanBan(int ID)
        {
            WfTrangthaiBusiness = Get<WfTrangthaiBusiness>();
            WF_TRANGTHAI WfTrangThai = WfTrangthaiBusiness.Find(ID);
            int STATE_ID = ID;
            UserInfoBO user = (UserInfoBO)SessionManager.GetUserInfo();
            CheckRole(user);
            //WfLichsuchuyendoitrangthaiBusiness = Get<WfLichsuchuyendoitrangthaiBusiness>();
            //List<long> LstItemId = WfLichsuchuyendoitrangthaiBusiness.GetItemIDS(ID, LOAITAILIEU.VANBANDEN);

            var VaiTroId = user.RoleID;
            #region 01. Lấy danh sách văn bản đến
            VanBanDenBs = Get<HscvVanbandenBusiness>();
            WfEntityRelationBusiness = Get<WfEntityRelationBusiness>();
            WfTrangthaiBusiness = Get<WfTrangthaiBusiness>();
            VanBanDenSearchModel model = new VanBanDenSearchModel();
            model.State = STATE_ID;
            model.WfTrangThai = WfTrangThai;
            int WF_ID = WfEntityRelationBusiness.GetWorkFlowId(user.CoSoID.Value, LOAITAILIEU.VANBANDEN);
            if (WF_ID > 0)
            {
                WF_TRANGTHAI tmp_obj = WfTrangthaiBusiness.GetWFTrangThai(WF_ID);
                if (tmp_obj != null)
                {
                    model.InitialState = tmp_obj.ID;
                }
            }
            else
            {
                model.InitialState = 0;
            }
            SessionManager.SetValue("ListBuocChuyenTrangThai", this.getTrangThai());
            var listVanBanDen = VanBanDenBs.GetListVanBanDen(user.CoSoID.Value, user.RoleID, user.DonViID, isTruongDV, isTruongCQ, LOAITAILIEU.VANBANDEN, (long)user.UserID, string.Empty, null, null, 0, 0, null, null, 0, false, 0, ID);
            #endregion
            #region 02. Lưu danh sách văn bản đến vào session
            SessionManager.SetValue("ListVanBanDenByTrangThai", listVanBanDen);
            ViewData["Search"] = "0";
            #endregion
            #region 03. Khởi tạo các giá trị cho filter
            DoKhanBs = Get<HscvDoKhanBusiness>();
            DonViBanHanhBs = Get<HscvDonViBanHanhBusiness>();
            DonViNhanHoSoBs = Get<DmDonViBusiness>();
            SoVanBanDenBs = Get<HscvSoVanBanBusiness>();
            LoaiVanBanBs = Get<HscvLoaivanbanBusiness>();
            NhomSoBs = Get<HscvNhomSoVanBanBusiness>();
            //Initialize filter            
            model.ListDoKhan = DoKhanBs.All.Where(x => x.TRANGTHAI == true).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.DOKHAN,
                    Value = x.ID.ToString()
                }).ToList();
            model.ListCoQuanBanHanh = DonViBanHanhBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENDONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListDonViNhanHoSo = DonViNhanHoSoBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TEN_DONVI,
                    Value = x.ID.ToString()
                }
                ).ToList();
            var LstNhomSoDen = NhomSoBs.All.Where(x => x.LOAIHOSO_ID == LOAIHOSO.VANBANDEN).ToList();
            List<int> LstNhomSoId = LstNhomSoDen.Select(x => x.ID).ToList();
            model.ListSoVanBanDen = SoVanBanDenBs.All.Where(x => LstNhomSoId.Contains((int)x.NHOMSOVANBAN_ID)).ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENSO,
                    Value = x.ID.ToString()
                }
                ).ToList();
            model.ListLoaiVanBan = LoaiVanBanBs.All.ToList().Select(
                x => new SelectListItem()
                {
                    Text = x.TENLOAIVANBAN,
                    Value = x.ID.ToString()
                }
                ).ToList();

            SessionManager.SetValue("InitialState", model.InitialState);
            SessionManager.SetValue("TenTrangThaiDanhMuc", model.WfTrangThai.TENTRANGTHAI);
            #endregion
            return View(model);
        }
    }
}
