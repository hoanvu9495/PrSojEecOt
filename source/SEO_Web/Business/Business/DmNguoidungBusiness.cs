/**
* The HiNet License
*
* Copyright 2015 Hinet JSC. All rights reserved.
* HINET PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
*/
using Business.CommonBusiness;
using Business.CommonHelper;
using DAL.Repository;
using log4net;
using Model.DBTool;
using SyncLDAP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AE.Net.Mail;
using System.Web.Mvc;
using PagedList;

namespace Business.Business
{
    public partial class DmNguoidungBusiness : GenericBussiness<DM_NGUOIDUNG>
    {
        private static readonly ILog log = LogManager.GetLogger("DmNguoidungBusiness");

        public DmNguoidungBusiness(Entities context = null)
            : base()
        {
            repository = new DmNguoidungRepository(context);
        }

        private bool? IsSyncLDAP = null;

        private bool isSyncLDAP()
        {
            if (IsSyncLDAP == null)
            {
                string SyncLDAP = ConfigurationManager.AppSettings["SyncLDAP"];
                IsSyncLDAP = SyncLDAP != null && SyncLDAP.ToLower() == "true";
            }
            return IsSyncLDAP.Value;
        }
        public bool isEmailExist(string Email, decimal userID = 0)
        {
            bool check = false;
            IEnumerable<DM_NGUOIDUNG> lstUser = null;
            if (userID != 0)
            {
                lstUser = this.All.Where(x => x.EMAIL.ToLower().Equals(Email.ToLower()) && x.DM_NGUOIDUNG_ID != userID);
            }
            else
            {
                lstUser = this.All.Where(x => x.EMAIL.ToLower().Equals(Email.ToLower()));
            }
            if (lstUser.Any())
            {
                check = true;
            }
            return check;
        }
        public DM_NGUOIDUNG Insert(DM_NGUOIDUNG user, string MatKhau)
        {
            if (isSyncLDAP())
            {
                LdapHelper ldap = new LdapHelper();
                var ad = ldap.createDirectoryEntry();
                bool insert = ldap.CreateUser(user.TENDANGNHAP, MatKhau, ad, true);
                if (!insert)
                {
                    throw new BusinessException("Không tạo được tài khoản VPN");
                }
            }
            return base.Insert(user);
        }

        public DM_NGUOIDUNG Update(DM_NGUOIDUNG user, string MatKhau)
        {
            if (isSyncLDAP())
            {
                LdapHelper ldap = new LdapHelper();
                var ad = ldap.createDirectoryEntry();
                bool updated = ldap.UpdateUser(user.TENDANGNHAP, MatKhau, user.TRANGTHAI == 1, ad);
                if (!updated)
                {
                    throw new BusinessException("Không cập nhật được tài khoản VPN");
                }
            }
            return base.Update(user);
        }
        //public List<TreeDataBO> GetDataTree(string keyWord)
        //{
        //    var lst = (from ngdung in this.context.DM_NGUOIDUNG
        //               join phongban in this.context.DM_DONVI
        //               on ngdung.DM_DONVI_ID equals phongban.ID into j1
        //               from tenPhongBan in j1.DefaultIfEmpty()
        //               orderby ngdung.DM_DONVI_ID
        //               group new { ngdung, tenPhongBan } by new { ngdung.DM_DONVI_ID } into g1
        //               select new TreeDataBO
        //               {
        //                   Type = g1.FirstOrDefault().tenPhongBan.TEN_DONVI,
        //                   Item = g1.Select(x => new SelectListItem()
        //                   {
        //                       Text = x.ngdung.HOTEN,
        //                       Value = x.ngdung.DM_NGUOIDUNG_ID.ToString(),
        //                   }).ToList()
        //               }).ToList();
        //    return lst;
        //}
        public override void Delete(object model)
        {
            DM_NGUOIDUNG user = null;
            if (!(model is DM_NGUOIDUNG))
            {
                user = this.Find(model);
            }
            else
            {
                user = (DM_NGUOIDUNG)model;
            }

            if (isSyncLDAP())
            {
                LdapHelper ldap = new LdapHelper();
                var ad = ldap.createDirectoryEntry();
                bool updated = ldap.DeleteUser(user.TENDANGNHAP, ad);
                if (!updated)
                {
                    throw new BusinessException("Không xóa được tài khoản VPN");
                }
            }
            repository.Delete(user);
        }

        public override void DeleteAll(List<DM_NGUOIDUNG> entities)
        {
            foreach (DM_NGUOIDUNG user in entities)
            {
                this.Delete(user);
            }
        }
        //Hoàn
        //Lấy danh sách tất cả người dùng thuộc phòng ban
        public List<SelectListItem> GetNguoiDungByDonVi(int idDonVi, int idUser = 0, int selectID = 0)
        {
            var lsr = this.context.DM_NGUOIDUNG.Where(x => x.DM_PHONGBAN_ID == idDonVi).Select(x => new SelectListItem()
            {
                Text = x.HOTEN,
                Value = x.DM_NGUOIDUNG_ID.ToString(),
                Selected = selectID > 0 && selectID == x.DM_NGUOIDUNG_ID
            });
            if (idUser > 0)
            {
                lsr = lsr.Where(x => x.Value != idUser.ToString());
            }
            return lsr.ToList();
        }
        //public DMNguoiDungBO GetNguoiDungBO(int CCVC_ID)
        //{
        //    var result = (from nguoiDung in this.context.DM_NGUOIDUNG
        //                  //join NguoiDungCanBo in this.context.HSCB_NGUOIDUNG_CANBO
        //                  //on nguoiDung.DM_NGUOIDUNG_ID equals NguoiDungCanBo.NGUOIDUNG_ID
        //                  //into g1
        //                  //from gNguoiDung in g1.DefaultIfEmpty()
        //                  join coso in this.context.COSO
        //                  on nguoiDung.COSO_ID equals coso.COSO_ID
        //                  join phongban in this.context.DM_PHONGBAN
        //                  on nguoiDung.DM_PHONGBAN_ID equals phongban.DM_PHONGBAN_ID
        //                  into g2
        //                  from gPhongBan in g2.DefaultIfEmpty()
        //                  //where gNguoiDung.HOSO_ID == CCVC_ID && nguoiDung.DM_NGUOIDUNG_ID == gNguoiDung.NGUOIDUNG_ID
        //                  select new DMNguoiDungBO
        //                  {
        //                      DIENTHOAI = nguoiDung.DIENTHOAI,
        //                      DM_NGUOIDUNG_ID = nguoiDung.DM_NGUOIDUNG_ID,
        //                      EMAIL = nguoiDung.EMAIL,
        //                      TEN_PHONGBAN = gPhongBan.TENPHONGBAN,
        //                      TEN_COSO = coso.TENCOSO,
        //                      HOTEN = nguoiDung.HOTEN,
        //                      TENDANGNHAP = nguoiDung.TENDANGNHAP,
        //                  });
        //    return result.FirstOrDefault();
        //}

        #region Khai bao
        //private DmDonviQlBusiness DmDonviQlBusiness;
        //private DmDiemTiepnhanBusiness DmDiemTiepnhanBusiness;

        #endregion
        public void Save(DM_NGUOIDUNG nguoidung)
        {
            try
            {
                if (nguoidung.DM_NGUOIDUNG_ID == 0)
                {
                    this.repository.Insert(nguoidung);
                }
                else
                    this.repository.Update(nguoidung);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }
        public bool AuthenticateUser(string Username, string Password)
        {
            DM_NGUOIDUNG user = this.All.Where(o => o.TENDANGNHAP.ToUpper().Equals(Username.ToUpper())).FirstOrDefault();
            if (user != null && user.MATKHAU == Password)
            {
                return true;
            }
            return false;
        }

        public UserInfoBO GetUserInfo(string Username)
        {
            try
            {
                DM_NGUOIDUNG user = this.All.Where(o => o.TENDANGNHAP.ToUpper().Equals(Username.ToUpper()) && o.TRANGTHAI == 1).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception(string.Format("UserAccountBusiness.GetUserInfo: Khong ton tai user {0}", Username));
                }
                UserInfoBO UserInfo = new UserInfoBO();
                UserInfo.UserID = user.DM_NGUOIDUNG_ID;
                UserInfo.Username = user.TENDANGNHAP;
                UserInfo.CoSoID = user.COSO_ID;
                UserInfo.CAPCOSO_ID = user.CAPCOSO_ID;
                UserInfo.OptionRole = user.OptionRole;
                UserInfo.Email = user.EMAIL;
                UserInfo.EmailPass = user.EMAILPASS;
                UserInfo.ListCN = new List<ChucNangBO>();
                UserInfo.ListCNFull = new List<ChucNangBO>();
                UserInfo.ListThaoTac = new List<ThaoTacBO>();
                UserInfo.ListRole = new List<DM_VAITRO>();
                #region Kiểm tra avatar có tồn tại
                UserInfo.ImagesUrl = user.ANH_DAIDIEN;

                #endregion

                var listUserBO = this.All.Where(o => o.DM_NGUOIDUNG_ID != UserInfo.UserID)
                        .Select(o => new { o.HOTEN, o.TENDANGNHAP, o.DM_NGUOIDUNG_ID }).OrderBy(o => o.DM_NGUOIDUNG_ID).ToList();
                if (listUserBO != null)
                {
                    var idx = 0;
                    string ListUserName = "[";
                    List<UserBO> lst_UserBO = new List<UserBO>();
                    foreach (var item in listUserBO)
                    {
                        UserBO bo = new UserBO();
                        bo.idx = idx;
                        bo.label = item.HOTEN;
                        lst_UserBO.Add(bo);
                        if (idx == listUserBO.Count - 1)
                        {
                            ListUserName += "\"" + item.TENDANGNHAP + "\"]";
                        }
                        else
                        {
                            ListUserName += "\"" + item.TENDANGNHAP + "\", ";
                        }
                        idx++;
                    }
                    UserInfo.ListUserBO = lst_UserBO;
                    UserInfo.ListUserName = ListUserName;
                }
                // gán fullname
                UserInfo.Fullname = user.HOTEN;
                UserInfo.PhongBanID = user.DM_PHONGBAN_ID;




                UserInfo.Password = user.MATKHAU;
                UserInfo.PasswordSalt = user.MAHOA_MK;


                #region lay ra toan bo vai tro cua nguoi dung
                var NguoiDungVaiTro = GetBusiness<NguoiDungVaiTroBusiness>();
                var LstRole = NguoiDungVaiTro.All.Where(x => x.NGUOIDUNG_ID == UserInfo.UserID).Select(x => x.VAITRO_ID).ToList();
                UserInfo.ListVaiTro = LstRole;
                #endregion
                #region Lay ra toan bo danh sach chuc nang cua nguoi dung theo vai tro
                //Buoc 1: Lay ra toan bo vai tro Chuc nang
                var VaitroChucnangBusiness = GetBusiness<VaitroChucnangBusiness>();
                var LstVaiTroChucNangObj = VaitroChucnangBusiness.All.Where(x => LstRole.Contains(x.DM_VAITRO_ID)).ToList();
                List<int> LstVaiTroChucNang = LstVaiTroChucNangObj.Select(x => x.VAITRO_CHUCNANG_ID).ToList();
                List<int> LstChucNang = LstVaiTroChucNangObj.Select(x => (int)x.DM_CHUCNANG_ID).ToList();
                var DmChucNangBusiness = GetBusiness<DmChucnangBusiness>();
                UserInfo.ListCN = DmChucNangBusiness.All.Where(x => LstChucNang.Contains(x.DM_CHUCNANG_ID)).Select(o => new ChucNangBO
                {
                    CSSCLASS = o.CSSCLASS,
                    DM_CHUCNANG_ID = o.DM_CHUCNANG_ID,
                    TEN_CHUCNANG = o.TEN_CHUCNANG,
                    MA_CHUCNANG = o.MA_CHUCNANG,
                    TT_HIENTHI = o.TT_HIENTHI
                }).OrderBy(x => x.TT_HIENTHI).ToList();
                //Buoc 2: Lay ra toan bo thao tac ung voi cac vai tro chuc nang do
                var VaiTroThaoTac = GetBusiness<VaitroThaotacBusiness>();
                var LstVaiTroThaoTac = VaiTroThaoTac.All.Where(x => LstVaiTroChucNang.Contains((int)x.VAITRO_CHUCNANG_ID)).Select(x => x.DM_THAOTAC_ID).Distinct().ToList();
                //Buoc 3: Lay ra cac thao tac nguoi dung duoc phep tac dong
                var ThaoTacBs = GetBusiness<DmThaotacBusiness>();
                UserInfo.ListThaoTac = ThaoTacBs.All.Where(x => LstVaiTroThaoTac.Contains(x.DM_THAOTAC_ID)).Select(o => new ThaoTacBO
                {
                    MENU_LINK = o.MENU_LINK,
                    DM_CHUCNANG_ID = o.DM_CHUCNANG_ID.Value,
                    DM_THAOTAC_ID = o.DM_THAOTAC_ID,
                    TEN_THAOTAC = o.TEN_THAOTAC,
                    THAOTAC = o.THAOTAC,
                    TT_HIENTHI = o.TT_HIENTHI
                }).ToList();
                #endregion

                return UserInfo;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }
        /// <summary>
        /// Thiết lập lại danh sách chức năng và vai trò sau khi chọn 1 role khác
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public UserInfoBO GetUserRole(UserInfoBO user)
        {
            UserInfoBO UserInfo = new UserInfoBO();


            var ds_ChucNangVaiTro = GetBusiness<VaitroChucnangBusiness>().All
   .Where(o => user.RoleID == o.DM_VAITRO_ID && o.DM_CHUCNANG.TRANGTHAI == 1 && o.TRANGTHAI == 1 && o.COSO_ID == user.CoSoID)
   .Select(o => new ChucNangBO
   {
       DM_CHUCNANG_ID = o.DM_CHUCNANG_ID.Value,
       TEN_CHUCNANG = o.DM_CHUCNANG.TEN_CHUCNANG,
       URL = o.DM_CHUCNANG.URL,
       TT_HIENTHI = o.DM_CHUCNANG.TT_HIENTHI,
       //CHUCNANG_CHA = o.DM_CHUCNANG.CHUCNANG_CHA,
       MAC_DINH = o.MAC_DINH,
       IS_HIDDEN = o.DM_CHUCNANG.IS_HIDDEN,
       VAITROID = o.DM_VAITRO_ID
   }).OrderBy(o => o.TT_HIENTHI).ThenBy(o => o.DM_CHUCNANG_ID).ToList();
            UserInfo.ListCN = ds_ChucNangVaiTro;//DANH SACH CHUNG NANG CUA VAI TRO
            //if (ds_ChucNangVaiTro != null)
            //{
            //    List<ChucNangBO> lstChucNangCha = GetBusiness<DmChucnangBusiness>().All.Where(x => x.CHUCNANG_CHA == null && x.TRANGTHAI == 1).ToList().Select(o => new ChucNangBO
            //    {
            //        DM_CHUCNANG_ID = o.DM_CHUCNANG_ID,
            //        TEN_CHUCNANG = o.TEN_CHUCNANG,
            //        URL = o.URL,
            //        TT_HIENTHI = o.TT_HIENTHI,
            //        CHUCNANG_CHA = o.CHUCNANG_CHA,
            //        IS_HIDDEN = o.IS_HIDDEN,
            //        ICONPATH = o.ICONPATH,
            //        CSSCLASS = o.CSSCLASS
            //    }).OrderBy(o => o.TT_HIENTHI).ThenBy(o => o.DM_CHUCNANG_ID).ToList();//DANH SACH CHUC NANG CHA CUA BANG DM_CHUCNANG
            //    List<int> ds_CHUCNANG_ID = new List<int>();
            //    if (lstChucNangCha != null)
            //    {
            //    foreach (var cn1 in lstChucNangCha)
            //    {
            //        foreach (var cn2 in ds_ChucNangVaiTro)
            //        {
            //            if (cn2.DM_CHUCNANG_ID == cn1.DM_CHUCNANG_ID || cn2.CHUCNANG_CHA == cn1.DM_CHUCNANG_ID)
            //            {
            //                ds_CHUCNANG_ID.Add(cn1.DM_CHUCNANG_ID);
            //            }
            //        }
            //    }
            //    if (ds_CHUCNANG_ID != null)
            //    {
            //        var CNRepeat = ds_CHUCNANG_ID.Distinct();
            //        ds_CHUCNANG_ID = CNRepeat.ToList<int>();// XOA CAC DM_CHUCNANG TRUNG` NHAU
            //        List<ChucNangBO> resultCN = new List<ChucNangBO>();
            //        foreach (var cn in lstChucNangCha)
            //        {
            //            foreach (var item in ds_CHUCNANG_ID)
            //            {
            //                if (cn.DM_CHUCNANG_ID == item)
            //                {
            //                    ChucNangBO chucNang = new ChucNangBO();
            //                    chucNang.DM_CHUCNANG_ID = cn.DM_CHUCNANG_ID;
            //                    chucNang.CHUCNANG_CHA = cn.CHUCNANG_CHA;
            //                    chucNang.TEN_CHUCNANG = cn.TEN_CHUCNANG;
            //                    if (cn.URL.ToLower().Contains("USE_THAOTAC".ToLower())) //nếu là chức năng quản lý công chức hoặc viên chức sẽ lấy url link của thao tác
            //                    {
            //                        var ChucNang_id = ds_ChucNangVaiTro.Where(x => x.CHUCNANG_CHA == item && x.CHUCNANG_CHA.HasValue).OrderBy(x => x.TT_HIENTHI).Select(x => x.DM_CHUCNANG_ID).FirstOrDefault();
            //                        var vaitro_cn_id = GetBusiness<VaitroChucnangBusiness>().All.Where(x => x.DM_CHUCNANG_ID == ChucNang_id && x.DM_VAITRO_ID == user.RoleID && x.COSO_ID == user.CoSoID).Select(x => x.VAITRO_CHUCNANG_ID).FirstOrDefault();
            //                        var ThaoTac = GetBusiness<VaitroThaotacBusiness>().All.Where(x => x.VAITRO_CHUCNANG_ID == vaitro_cn_id).OrderBy(x => x.DM_THAOTAC.TT_HIENTHI).Select(x => x.DM_THAOTAC).FirstOrDefault();
            //                        if (ThaoTac != null)
            //                        {
            //                            chucNang.URL = ThaoTac.MENU_LINK;
            //                        }
            //                        else
            //                        {
            //                            chucNang.URL = cn.URL;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        var function = ds_ChucNangVaiTro.Where(x => x.CHUCNANG_CHA == cn.DM_CHUCNANG_ID).OrderBy(x => x.TT_HIENTHI).FirstOrDefault();
            //                        if (function != null)
            //                        {
            //                            chucNang.URL = function.URL;

            //                        }
            //                        else
            //                        {
            //                            if (!string.IsNullOrEmpty(cn.URL) && cn.URL != "")
            //                            {
            //                                chucNang.URL = cn.URL;
            //                            }
            //                            else
            //                            {
            //                                chucNang.URL = string.Empty;
            //                            }
            //                        }

            //                    }
            //                    chucNang.IS_HIDDEN = cn.IS_HIDDEN;
            //                    chucNang.TT_HIENTHI = cn.TT_HIENTHI;
            //                    chucNang.ICONPATH = cn.ICONPATH;
            //                    chucNang.CSSCLASS = cn.CSSCLASS;
            //                    resultCN.Add(chucNang);
            //                    break;
            //                }
            //            }
            //        }
            //        UserInfo.ListCNFull = resultCN;
            //    }
            //}
            //UserInfo.ListThaoTac = GetBusiness<VaitroThaotacBusiness>().All
            //    .Where(o => user.RoleID == o.VAITRO_CHUCNANG.DM_VAITRO_ID &&
            //        o.VAITRO_CHUCNANG.DM_CHUCNANG.TRANGTHAI == 1 &&
            //        o.TRANGTHAI == 1 && o.VAITRO_CHUCNANG.TRANGTHAI == 1 &&
            //        o.DM_THAOTAC.TRANGTHAI == 1 && o.COSO_ID == user.CoSoID)
            //    .Select(o => new ThaoTacBO
            //    {
            //        DM_CHUCNANG_ID = o.VAITRO_CHUCNANG.DM_CHUCNANG_ID.Value,
            //        DM_THAOTAC_ID = (int)o.DM_THAOTAC_ID.Value,
            //        TEN_THAOTAC = o.DM_THAOTAC.TEN_THAOTAC,
            //        THAOTAC = o.DM_THAOTAC.THAOTAC,
            //        MENU_LINK = o.DM_THAOTAC.MENU_LINK,
            //        TT_HIENTHI = o.DM_THAOTAC.TT_HIENTHI,
            //        IS_HIENTHI = o.DM_THAOTAC.IS_HIENTHI
            //    }).OrderBy(x => x.TT_HIENTHI).ToList();
            //}
            return UserInfo;
        }
        public bool isUserExist(string UserName, int UserId = 0)
        {
            bool check = false;
            IEnumerable<DM_NGUOIDUNG> lstUser = null;
            if (UserId != 0)
            {
                lstUser = this.All.Where(o => o.TENDANGNHAP.ToLower().Equals(UserName.ToLower())
                            && o.DM_NGUOIDUNG_ID != UserId);
            }
            else
            {
                lstUser = this.All.Where(o => o.TENDANGNHAP.ToLower().Equals(UserName.ToLower()));
            }

            if (lstUser.Any())
            {
                check = true;
            }
            return check;
        }

        public long getSequencePhieuDK()
        {


            long sequence = context.Database.SqlQuery<long>("SELECT PHIEUDANGKY_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();

            return sequence;

        }

        public bool CheckExistByPhongBanId(int? id)
        {
            return repository.All.Any(n => n.DM_PHONGBAN_ID == id);
        }
        public bool CheckExistByDTNId(int? id)
        {
            return false;// repository.All.Any(n => n.DM_DIEM_TIEPNHAN_ID == id);
        }
        public bool CheckExistByCumThiId(int? id)
        {
            return false;//repository.All.Any(n => n.DM_CUMTHI_ID == id);
        }

        public void UpdateSTTByDVQLId(int? id, int Trangthai)
        {
            if (CheckExistByPhongBanId(id))
            {
                DM_NGUOIDUNG ng;
                foreach (var item in repository.All.Where(n => n.DM_PHONGBAN_ID == id))
                {
                    ng = this.Find(item.DM_NGUOIDUNG_ID);
                    ng.TRANGTHAI = Trangthai;
                    this.Update(ng);
                }
                repository.Save();
            }
        }



        public void DeleteByDVQLId(int? id)
        {
            if (CheckExistByPhongBanId(id))
            {
                this.DeleteAll(repository.All.Where(u => u.DM_PHONGBAN_ID == id).ToList());
                this.Save();
            }
        }

        //public List<NguoiDungBO> DataToGrid(UserInfoBO userInfo)
        //{
        //    //var cosoResult = this.context.COSO.Find(userInfo.CoSoID);
        //    List<NguoiDungBO> lstSource = new List<NguoiDungBO>();
        //    var lstDataTruong = (from u in context.DM_NGUOIDUNG
        //                         join cs in context.COSO
        //                         on u.COSO_ID equals cs.COSO_ID
        //                         into group2
        //                         from gCoSo in group2.DefaultIfEmpty()
        //                         //join capcs in context.DM_CAPCOSO
        //                         //on u.CAPCOSO_ID equals capcs.ID
        //                         //into group0
        //                         //from gCapCoSo in group0.DefaultIfEmpty()
        //                         join dv in context.DM_DONVI on u.DM_DONVI_ID equals dv.ID
        //                         into group1
        //                         from gDonVi in group1.DefaultIfEmpty()
        //                         //join nd_CanBo in context.HSCB_NGUOIDUNG_CANBO on u.DM_NGUOIDUNG_ID equals nd_CanBo.NGUOIDUNG_ID
        //                         //into groupNd_Canbo
        //                         //from gNd_CanBo in groupNd_Canbo.DefaultIfEmpty()
        //                         //join canbo in this.context.HSCB_CONGCHUC_VIENCHUC on gNd_CanBo.HOSO_ID equals canbo.ID
        //                         //into groupCanBo
        //                         //from gCanBo in groupCanBo.DefaultIfEmpty()
        //                         //join vaitro in context.DM_VAITRO on u.DM_VAITRO_ID equals vaitro.DM_VAITRO_ID
        //                         //into group5
        //                         //from gVaiTro in group5.DefaultIfEmpty()
        //                         join chucvu in this.context.DM_CHUCVU on u.CHUCVU_ID equals chucvu.ID
        //                         into g6
        //                         from gChucVu in g6.DefaultIfEmpty()
        //                         where gCoSo.IS_DELETE == false && gCapCoSo.IS_DELETE == false
        //                         //&& gVaiTro.IS_DELETE == false && gVaiTro.TRANGTHAI == 1
        //                         orderby u.DM_NGUOIDUNG_ID descending
        //                         select new NguoiDungBO
        //                         {
        //                             DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
        //                             TENDANGNHAP = u.TENDANGNHAP,
        //                             MATKHAU = u.MATKHAU,
        //                             MAHOA_MK = u.MAHOA_MK,
        //                             TRANGTHAI = u.TRANGTHAI.Value,
        //                             EMAIL = u.EMAIL,
        //                             DIENTHOAI = u.DIENTHOAI,
        //                             //DM_TRUONGDH_ID = u.DM_TRUONGDH_ID,
        //                             MADONVI = gCoSo.MACOSO,
        //                             TENDONVI = gDonVi.TEN_DONVI,
        //                             DM_DONVI_ID = gDonVi.ID,
        //                             TYPE = 3,
        //                             //TYPEID = g1.PHANLOAI,
        //                             //TYPENAME = "Trường ĐH, CĐ",
        //                             COSO_ID = gCoSo.COSO_ID,
        //                             TEN_COSO = gCoSo.TENCOSO,
        //                             //DM_VAITRO_ID = u.DM_VAITRO_ID,
        //                             //TENVAITRO = gVaiTro.TEN_VAITRO,
        //                             HOTEN = u.HOTEN,
        //                             CAPCOSO_ID = gCapCoSo.ID,
        //                             CAPCOSO_PARENT = gCapCoSo.PARENT_CAPCOSO,
        //                             CAPCOSO = gCapCoSo.CAPCOSO,
        //                             CHUCVU_ID = gChucVu.ID,
        //                             OptionRole = u.OptionRole,
        //                             CANBO = gCanBo.HO_VA_TEN,
        //                             HSCB_ID = gCanBo.ID,
        //                             LOAI_HD = gCanBo.DM_HOPDONG_ID,
        //                             FTS = u.FTS
        //                             //HINHTHUC = cs.HINHTHUC,
        //                             //TINH_ID = cs.TINH_ID,
        //                             //HUYEN_ID = cs.HUYEN_ID,
        //                             //XA_ID = cs.XA_ID
        //                             //HINHTHUC = g1.HINHTHUC,
        //                             //TINH_ID = g1.TINH_ID,
        //                             //HUYEN_ID = g1.HUYEN_ID,
        //                             //XA_ID = g1.XA_ID
        //                         }).ToList();
        //    //if (userInfo.CoSoID.HasValue)
        //    //{
        //    //    lstDataTruong = lstDataTruong.Where(x => x.COSO_ID == userInfo.CoSoID).ToList();
        //    //}
        //    //if (userInfo.DonViID > 0)
        //    //{
        //    //    lstDataTruong = lstDataTruong.Where(x => x.DM_DONVI_ID == userInfo.DonViID).ToList();
        //    //}
        //    if (userInfo.ListCoSoRemove != null && userInfo.ListCoSoRemove.Count > 0)
        //    {
        //        lstDataTruong = lstDataTruong.Where(x => x.COSO_ID.HasValue ? !userInfo.ListCoSoRemove.Contains(x.COSO_ID.Value) : false).ToList();
        //    }
        //    if (userInfo.ListCapCoSo != null && userInfo.ListCapCoSo.Count > 0)
        //    {
        //        lstDataTruong = lstDataTruong.Where(x => x.CAPCOSO_ID.HasValue ? userInfo.ListCapCoSo.Contains(x.CAPCOSO_ID.Value) : false).ToList();
        //    }
        //    if (userInfo.CAPCOSO_ID.HasValue)
        //    {
        //        var ListNguoiDungVaiTro = GetBusiness<NguoiDungVaiTroBusiness>().All.Where(x => x.NGUOIDUNG_ID.HasValue && x.VAITRO_ID.HasValue).ToList();
        //        var ListVaiTro = GetBusiness<DmVaitroBusiness>().GetListByCapCoSoID(userInfo.CAPCOSO_ID, 0, false);
        //        var ListUserRole = GetBusiness<NguoiDungVaiTroBusiness>().All.ToList();
        //        if (ListVaiTro != null && ListVaiTro.Count > 0 && ListNguoiDungVaiTro != null && ListNguoiDungVaiTro.Count > 0)
        //        {
        //            foreach (var item in lstDataTruong)
        //            {
        //                //hiển thị tên cán bộ công chức và viên chức
        //                if (item.HSCB_ID.HasValue)
        //                {
        //                    if (item.LOAI_HD == QLHoSoCanBoConstant.CONGCHUC)
        //                    {
        //                        item.CANBO = "<a href='/ProfileCongChucArea/ProfileCongChuc/Detail/" + item.HSCB_ID + "' title='" + item.CANBO + "'>" + item.CANBO + "</a>";
        //                    }
        //                    else
        //                    {
        //                        item.CANBO = "<a href='/ProfileVienChucArea/ProfileVienChuc/Detail/" + item.HSCB_ID + "' title='" + item.CANBO + "'>" + item.CANBO + "</a>";
        //                    }
        //                }
        //                // lấy danh sách vai trò của người dùng
        //                List<int> listVaiTroND = ListNguoiDungVaiTro.Where(x => x.NGUOIDUNG_ID == item.DM_NGUOIDUNG_ID && x.VAITRO_ID.HasValue).Select(x => x.VAITRO_ID.Value).ToList();
        //                if (listVaiTroND != null && listVaiTroND.Count > 0)
        //                {
        //                    item.ListVaiTro = listVaiTroND;
        //                }
        //                if (item.OptionRole == 1)// nếu thiết lập phân quyền riêng cho người dùng
        //                {
        //                    item.TENVAITRO = "<span style='color:red'>Sử dụng phân quyền riêng</span>";
        //                }
        //                else
        //                {
        //                    var userRole = ListUserRole.Where(x => x.NGUOIDUNG_ID == item.DM_NGUOIDUNG_ID).Select(x => x.VAITRO_ID).ToList();
        //                    var lstRole = ListVaiTro.Where(x => userRole.Contains(x.DM_VAITRO_ID)).ToList();
        //                    var str_sub = "";
        //                    var str_full = "";
        //                    var count = lstRole.Count;
        //                    var _count = 0;

        //                    if (count > 0)
        //                    {
        //                        foreach (var role in lstRole)
        //                        {
        //                            if (count > 2)
        //                            {
        //                                if (_count < 2)
        //                                {
        //                                    if (_count == 1)
        //                                    {
        //                                        str_sub += role.TEN_VAITRO + " ";

        //                                    }
        //                                    else
        //                                    {
        //                                        str_sub += role.TEN_VAITRO + ", ";
        //                                    }
        //                                }
        //                                else if (_count == 2)
        //                                {
        //                                    str_sub += " và " + (count - _count) + " vai trò";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (count == 2)
        //                                {
        //                                    if (_count == 0)
        //                                    {
        //                                        str_sub += role.TEN_VAITRO + " và ";
        //                                    }
        //                                    else
        //                                    {
        //                                        str_sub += role.TEN_VAITRO;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    str_sub += role.TEN_VAITRO + "";
        //                                }
        //                            }
        //                            str_full += "<p>" + role.TEN_VAITRO + "</p>";

        //                            _count++;
        //                        }
        //                        item.TENVAITRO = " <a href='javascript:void(0)' style='color:#016897 !important; font-weight: bold;' data-comment='" + str_full + "' class='show-pop' data-contrains='vertical'>" + str_sub + "</a>";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    lstSource.AddRange(lstDataTruong);

        //    //DmDonviQlBusiness = GetBusiness<DmDonviQlBusiness>();
        //    //DmDiemTiepnhanBusiness = GetBusiness<DmDiemTiepnhanBusiness>();
        //    //if (userInfo.DmDVQLID != null)
        //    //{
        //    //    int loaitruong = userInfo.RoleID;
        //    //    #region Đăng nhập với người dùng là cục
        //    //    if (loaitruong == 1)
        //    //    {
        //    //        //Lấy danh sách người dùng thuộc Cục
        //    //        var lstDatacuc = (from u in this.All.Where(o => o.DM_DONVI_QL_ID != null).AsEnumerable()
        //    //                          join d in DmDonviQlBusiness.All.Where(o => o.LOAI_TRUONG == 0).AsEnumerable()
        //    //                           on u.DM_DONVI_QL_ID equals d.DM_DONVI_QL_ID
        //    //                          group u by new
        //    //                          {
        //    //                              u.DM_NGUOIDUNG_ID,
        //    //                              u.TENDANGNHAP,
        //    //                              u.MATKHAU,
        //    //                              u.MAHOA_MK,
        //    //                              u.TRANGTHAI,
        //    //                              u.DIENTHOAI,
        //    //                              u.DM_DONVI_QL_ID,

        //    //                              u.DM_DONVI_QL,
        //    //                              u.DM_VAITRO_ID,
        //    //                              u.DM_VAITRO
        //    //                          } into empg
        //    //                          select new NguoiDungBO
        //    //                        {
        //    //                            DM_NGUOIDUNG_ID = empg.Key.DM_NGUOIDUNG_ID,
        //    //                            TENDANGNHAP = empg.Key.TENDANGNHAP,
        //    //                            MATKHAU = empg.Key.MATKHAU,
        //    //                            TRANGTHAI = empg.Key.TRANGTHAI,
        //    //                            DIENTHOAI = empg.Key.DIENTHOAI,
        //    //                            DM_DONVI_QL_ID = empg.Key.DM_DONVI_QL_ID,
        //    //                            TENDONVI = empg.Key.DM_DONVI_QL.TEN_DONVI_QL,
        //    //                            MADONVI = empg.Key.DM_DONVI_QL.MA_DONVI_QL,
        //    //                            TYPE = 0,
        //    //                            TYPEID = (int)empg.Key.DM_DONVI_QL_ID,
        //    //                            MAHOA_MK = empg.Key.MAHOA_MK,
        //    //                            TYPENAME = "Cục",
        //    //                            DM_VAITRO_ID = empg.Key.DM_VAITRO_ID,
        //    //                            TENVAITRO = empg.Key.DM_VAITRO.TEN_VAITRO
        //    //                        }).OrderBy(o => o.MADONVI);
        //    //        lstSource.AddRange(lstDatacuc);
        //    //        //Lấy danh sách người dùng thuộc sở
        //    //        var lstDataSo = (from u in this.All.Where(o => o.DM_DONVI_QL_ID != null).AsEnumerable()
        //    //                         join d in DmDonviQlBusiness.All.Where(o => o.LOAI_TRUONG == 1).AsEnumerable()
        //    //                          on u.DM_DONVI_QL_ID equals d.DM_DONVI_QL_ID
        //    //                         group u by new
        //    //                         {
        //    //                             u.DM_NGUOIDUNG_ID,
        //    //                             u.TENDANGNHAP,
        //    //                             u.MATKHAU,
        //    //                             u.MAHOA_MK,
        //    //                             u.TRANGTHAI,
        //    //                             u.DIENTHOAI,
        //    //                             u.DM_DONVI_QL_ID,
        //    //                             u.DM_DONVI_QL,
        //    //                             u.DM_VAITRO_ID,
        //    //                             u.DM_VAITRO
        //    //                         } into empg
        //    //                         select new NguoiDungBO
        //    //                         {
        //    //                             DM_NGUOIDUNG_ID = empg.Key.DM_NGUOIDUNG_ID,
        //    //                             TENDANGNHAP = empg.Key.TENDANGNHAP,
        //    //                             MATKHAU = empg.Key.MATKHAU,
        //    //                             MAHOA_MK = empg.Key.MAHOA_MK,
        //    //                             TRANGTHAI = empg.Key.TRANGTHAI,
        //    //                             DIENTHOAI = empg.Key.DIENTHOAI,
        //    //                             DM_DONVI_QL_ID = empg.Key.DM_DONVI_QL_ID,
        //    //                             MADONVI = empg.Key.DM_DONVI_QL.MA_DONVI_QL,
        //    //                             TENDONVI = empg.Key.DM_DONVI_QL.TEN_DONVI_QL,
        //    //                             TYPE = 1,
        //    //                             TYPEID = (int)empg.Key.DM_DONVI_QL_ID,
        //    //                             TYPENAME = "Sở",
        //    //                             DM_VAITRO_ID = empg.Key.DM_VAITRO_ID,
        //    //                             TENVAITRO = empg.Key.DM_VAITRO.TEN_VAITRO
        //    //                         }).OrderBy(o => o.MADONVI);
        //    //        lstSource.AddRange(lstDataSo);
        //    //        //var lstDataSo = (from u in repository.All.Where(o => o.DM_DONVI_QL_ID != null)
        //    //        //                 join d in GetBusiness<DmDonviQlBusiness>().All.Where(o => o.LOAI_TRUONG == 1) on u.DM_DONVI_QL_ID equals d.DM_DONVI_QL_ID

        //    //        //                 select new NguoiDungBO
        //    //        //                 {
        //    //        //                     DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
        //    //        //                     TENDANGNHAP = u.TENDANGNHAP,
        //    //        //                     MATKHAU = u.MATKHAU,
        //    //        //                     TRANGTHAI = u.TRANGTHAI,
        //    //        //                     DIENTHOAI = u.DIENTHOAI,
        //    //        //                     DM_DONVI_QL_ID = u.DM_DONVI_QL_ID,
        //    //        //                     TENDONVI = u.DM_DONVI_QL.TEN_DONVI_QL,
        //    //        //                     TYPE = 1,
        //    //        //                     TYPEID = (int)u.DM_DONVI_QL_ID,
        //    //        //                     TYPENAME = "Sở",
        //    //        //                     DM_VAITRO_ID = u.DM_VAITRO_ID,
        //    //        //                     TENVAITRO = u.DM_VAITRO.TEN_VAITRO
        //    //        //                 });
        //    //        //lstSource.AddRange(lstDataSo);
        //    //        //Lấy danh sách người dùng thuộc cụm
        //    //        var lstDataCum = (from u in this.All.Where(o => o.DM_CUMTHI_ID != null)
        //    //                          select new NguoiDungBO
        //    //                          {
        //    //                              DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
        //    //                              TENDANGNHAP = u.TENDANGNHAP,
        //    //                              MATKHAU = u.MATKHAU,
        //    //                              MAHOA_MK = u.MAHOA_MK,
        //    //                              TRANGTHAI = u.TRANGTHAI,
        //    //                              DIENTHOAI = u.DIENTHOAI,
        //    //                              DM_CUMTHI_ID = u.DM_CUMTHI_ID,
        //    //                              MADONVI = u.DM_CUMTHI.MA_CUMTHI,
        //    //                              TENDONVI = u.DM_CUMTHI.TEN_CUMTHI,
        //    //                              TYPE = 2,
        //    //                              TYPEID = (int)u.DM_CUMTHI_ID,
        //    //                              TYPENAME = "Hội đồng thi",
        //    //                              DM_VAITRO_ID = u.DM_VAITRO_ID,
        //    //                              TENVAITRO = u.DM_VAITRO.TEN_VAITRO
        //    //                          }).OrderBy(o => o.MADONVI);
        //    //        lstSource.AddRange(lstDataCum);
        //    //        //Lấy danh sách người dùng thuộc điểm tiếp nhận
        //    //        //var lstDataDiem = (from u in repository.All.Where(o => o.DM_DIEM_TIEPNHAN_ID != null)
        //    //        //                   select new NguoiDungBO
        //    //        //                   {
        //    //        //                       DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
        //    //        //                       TENDANGNHAP = u.TENDANGNHAP,
        //    //        //                       MATKHAU = u.MATKHAU,
        //    //        //                       TRANGTHAI = u.TRANGTHAI,
        //    //        //                       DIENTHOAI = u.DIENTHOAI,
        //    //        //                       DM_DIEM_TIEPNHAN_ID = u.DM_DIEM_TIEPNHAN_ID,
        //    //        //                       TENDONVI = u.DM_DIEM_TIEPNHAN.TEN_DIEM_TIEPNHAN,
        //    //        //                       TYPE = 4,
        //    //        //                       TYPENAME = "Điểm tiếp nhận",
        //    //        //                       DM_VAITRO_ID = u.DM_VAITRO_ID,
        //    //        //                       TENVAITRO = u.DM_VAITRO.TEN_VAITRO
        //    //        //                   });
        //    //        //lstSource.AddRange(lstDataDiem);
        //    //        //Lấy danh sách người dùng thuộc trường
        //    //        var lstDataTruong = (from u in this.All.Where(o => o.DM_TRUONGDH_ID != null)
        //    //                             select new NguoiDungBO
        //    //                             {
        //    //                                 DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
        //    //                                 TENDANGNHAP = u.TENDANGNHAP,
        //    //                                 MATKHAU = u.MATKHAU,
        //    //                                 MAHOA_MK = u.MAHOA_MK,
        //    //                                 TRANGTHAI = u.TRANGTHAI,
        //    //                                 DIENTHOAI = u.DIENTHOAI,
        //    //                                 DM_TRUONGDH_ID = u.DM_TRUONGDH_ID,
        //    //                                 MADONVI = u.DM_TRUONGDH.MA_TRUONGDH,
        //    //                                 TENDONVI = u.DM_TRUONGDH.TEN_TRUONGDH,
        //    //                                 TYPE = 3,
        //    //                                 TYPEID = (int)u.DM_TRUONGDH_ID,
        //    //                                 TYPENAME = "Trường ĐH, CĐ",
        //    //                                 DM_VAITRO_ID = u.DM_VAITRO_ID,
        //    //                                 TENVAITRO = u.DM_VAITRO.TEN_VAITRO
        //    //                             }).OrderBy(o => o.MADONVI);
        //    //        lstSource.AddRange(lstDataTruong);

        //    //    }
        //    //    #endregion
        //    //    #region Đăng nhập với người dùng là sở
        //    //    else if (loaitruong == 2)
        //    //    {

        //    //        int[] diemtiepnhanid = DmDiemTiepnhanBusiness.All.Where(o => o.DM_DONVI_QL_ID == userInfo.DmDVQLID).Select(o => o.DM_DIEM_TIEPNHAN_ID).ToArray();

        //    //        if (diemtiepnhanid.Length > 0)
        //    //        {
        //    //            var lstdiemtiepnhanso = (from u in this.All.Where(o => diemtiepnhanid.Contains((int)o.DM_DIEM_TIEPNHAN_ID))
        //    //                                     select new NguoiDungBO
        //    //                                     {
        //    //                                         DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
        //    //                                         TENDANGNHAP = u.TENDANGNHAP,
        //    //                                         MATKHAU = u.MATKHAU,
        //    //                                         MAHOA_MK = u.MAHOA_MK,
        //    //                                         TRANGTHAI = u.TRANGTHAI,
        //    //                                         DIENTHOAI = u.DIENTHOAI,
        //    //                                         DM_DIEM_TIEPNHAN_ID = u.DM_DIEM_TIEPNHAN_ID,
        //    //                                         MADONVI = u.DM_DIEM_TIEPNHAN.MA_DIEM_TIEPNHAN,
        //    //                                         TENDONVI = u.DM_DIEM_TIEPNHAN.TEN_DIEM_TIEPNHAN,
        //    //                                         TYPE = 4,
        //    //                                         TYPEID = (int)u.DM_DIEM_TIEPNHAN_ID,
        //    //                                         TYPENAME = "Điểm tiếp nhận",
        //    //                                         DM_VAITRO_ID = u.DM_VAITRO_ID,
        //    //                                         TENVAITRO = u.DM_VAITRO.TEN_VAITRO
        //    //                                     }).OrderBy(o => o.MADONVI);
        //    //            lstSource.AddRange(lstdiemtiepnhanso);
        //    //        }

        //    //    }
        //    //    #endregion
        //    //    else { throw new BusinessException("Bạn không có quyền truy cập chức năng này"); }
        //    //}
        //    //else
        //    //{ throw new BusinessException("Bạn không có quyền truy cập chức năng này"); }

        //    //return lstSource.Where(o => o.TRANGTHAI != 2).ToList();
        //    return lstSource.ToList();
        //}
        //lấy danh sách người dùng theo phòng ban
        public IPagedList<NguoiDungBO> GetDSByPhongBanID(int idPHongBan, List<CCTC_THANHPHAN> listChild, int maxpage = 20, int page = 1)
        {
            var listIDChild = listChild.Select(x => x.ID).ToList();
            listIDChild.Add(idPHongBan);
            //var cosoResult = this.context.COSO.Find(userInfo.CoSoID);
            List<NguoiDungBO> lstSource = new List<NguoiDungBO>();
            var lstDataTruong = (from u in context.DM_NGUOIDUNG
                                 join dv in context.CCTC_THANHPHAN on u.DM_PHONGBAN_ID equals dv.ID
                                 into group1
                                 from gDonVi in group1.DefaultIfEmpty()
                                 //join chucvu in this.context.DM_CHUCVU on u.CHUCVU_ID equals chucvu.ID
                                 //into g6
                                 //from gChucVu in g6.DefaultIfEmpty()
                                 orderby u.DM_NGUOIDUNG_ID descending
                                 where u.DM_PHONGBAN_ID != null && listIDChild.Contains((int)u.DM_PHONGBAN_ID)
                                 select new NguoiDungBO
                                 {
                                     DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
                                     TENDANGNHAP = u.TENDANGNHAP,
                                     MATKHAU = u.MATKHAU,
                                     MAHOA_MK = u.MAHOA_MK,
                                     TRANGTHAI = u.TRANGTHAI.Value,
                                     EMAIL = u.EMAIL,
                                     DIENTHOAI = u.DIENTHOAI,
                                     TENDONVI = gDonVi.NAME,
                                     DM_DONVI_ID = gDonVi.ID,
                                     TYPE = 3,

                                     HOTEN = u.HOTEN,

                                     OptionRole = u.OptionRole,
                                     FTS = u.FTS

                                 })
                                 .ToList();
            foreach (var item in lstDataTruong)
            {
                var listvaitro = this.context.NGUOIDUNG_VAITRO.Where(x => x.NGUOIDUNG_ID == item.DM_NGUOIDUNG_ID).Select(X => X.VAITRO_ID).ToList();
                if (listvaitro.Count > 0)
                {
                    var listTenVaitro = this.context.DM_VAITRO.Where(x => listvaitro.Contains(x.DM_VAITRO_ID)).Select(x => x.TEN_VAITRO).ToList();
                    for (int i = 0; i < listTenVaitro.Count; i++)
                    {
                        if (i == 0)
                        {
                            item.TENVAITRO += listTenVaitro[i];
                        }
                        else
                        {
                            item.TENVAITRO += ", " + listTenVaitro[i];
                        }

                    }


                }

            }


            lstSource.AddRange(lstDataTruong);

            return lstSource.ToPagedList(page, maxpage);
        }

        public NguoiDungBO GetByID(int id)
        {
            var lstDataTruong = (from u in context.DM_NGUOIDUNG
                                 where u.DM_NGUOIDUNG_ID == id
                                 join dv in context.CCTC_THANHPHAN on u.DM_PHONGBAN_ID equals dv.ID
                                 into group1
                                 from gDonVi in group1.DefaultIfEmpty()

                                 orderby u.DM_NGUOIDUNG_ID descending
                                 select new NguoiDungBO
                                 {
                                     DM_NGUOIDUNG_ID = u.DM_NGUOIDUNG_ID,
                                     TENDANGNHAP = u.TENDANGNHAP,
                                     MATKHAU = u.MATKHAU,
                                     MAHOA_MK = u.MAHOA_MK,
                                     TRANGTHAI = u.TRANGTHAI.Value,
                                     EMAIL = u.EMAIL,
                                     DIENTHOAI = u.DIENTHOAI,

                                     TENDONVI = gDonVi.NAME,
                                     DM_PHONGBAN_ID = gDonVi.ID,
                                     ANH_DAIDIEN = u.ANH_DAIDIEN,
                                     TYPE = 3,

                                     HOTEN = u.HOTEN,

                                     OptionRole = u.OptionRole,

                                     FTS = u.FTS

                                 }).FirstOrDefault();
            if (lstDataTruong != null)
            {
                var listvaitro = this.context.NGUOIDUNG_VAITRO.Where(x => x.NGUOIDUNG_ID == lstDataTruong.DM_NGUOIDUNG_ID).Select(X => X.VAITRO_ID).ToList();
                if (listvaitro.Count > 0)
                {
                    var listTenVaitro = this.context.DM_VAITRO.Where(x => listvaitro.Contains(x.DM_VAITRO_ID)).Select(x => x.TEN_VAITRO).ToList();
                    for (int i = 0; i < listTenVaitro.Count; i++)
                    {
                        if (i == 0)
                        {
                            lstDataTruong.TENVAITRO += listTenVaitro[i];
                        }
                        else
                        {
                            lstDataTruong.TENVAITRO += ", " + listTenVaitro[i];
                        }

                    }


                }
            }


            return lstDataTruong;
        }

        public void Save(NguoiDungBO NguoiDungBO)
        {
            ValidateForm(NguoiDungBO);
            //CheckRole(NguoiDungBO.DM_VAITRO_ID, NguoiDungBO.RoleUser);
            if (NguoiDungBO.RoleUser == 2)
            {
                //CheckDTN(NguoiDungBO.DM_DONVI_QL_ID, NguoiDungBO.TYPEID);
            }
            string mahoa = "";

            DM_NGUOIDUNG nguoidung = this.Find(NguoiDungBO.DM_NGUOIDUNG_ID);
            if (nguoidung == null)
            {
                nguoidung = new DM_NGUOIDUNG();
            }
            if (NguoiDungBO != null)
            {
                nguoidung.TENDANGNHAP = NguoiDungBO.TENDANGNHAP.Trim();
                nguoidung.CHUCVU_ID = NguoiDungBO.CHUCVU_ID;
                nguoidung.TRANGTHAI = NguoiDungBO.TRANGTHAI;
                nguoidung.OptionRole = NguoiDungBO.OptionRole;
                nguoidung.FTS = NguoiDungBO.FTS;
                nguoidung.ANH_DAIDIEN = NguoiDungBO.ANH_DAIDIEN;
                if (NguoiDungBO.DIENTHOAI != null)
                {
                    nguoidung.DIENTHOAI = NguoiDungBO.DIENTHOAI.Trim();
                }

                nguoidung.DM_VAITRO_ID = NguoiDungBO.DM_VAITRO_ID;

                //if (NguoiDungBO.TYPE != 0)
                //{
                //    if (NguoiDungBO.TYPE == 1)
                //    {
                //        nguoidung.DM_PHONGBAN_ID = NguoiDungBO.TYPEID;
                //    }
                //}
                if (NguoiDungBO.DM_NGUOIDUNG_ID == 0)
                {
                    mahoa = MaHoaMatKhau.GenerateRandomString(5);
                    nguoidung.MAHOA_MK = mahoa;
                    nguoidung.MATKHAU = MaHoaMatKhau.Encode_Data(NguoiDungBO.MATKHAU.Trim() + mahoa);
                    nguoidung.EMAIL = NguoiDungBO.EMAIL;
                    nguoidung.CAPCOSO_ID = NguoiDungBO.CAPCOSO_ID;
                    nguoidung.COSO_ID = NguoiDungBO.COSO_ID;
                    nguoidung.DM_DONVI_ID = NguoiDungBO.DM_DONVI_ID;
                    nguoidung.DIENTHOAI = NguoiDungBO.DIENTHOAI;
                    nguoidung.HOTEN = NguoiDungBO.HOTEN;
                    nguoidung.DM_PHONGBAN_ID = NguoiDungBO.DM_PHONGBAN_ID;
                    this.Insert(nguoidung, NguoiDungBO.MATKHAU.Trim());
                }
                if (NguoiDungBO.DM_NGUOIDUNG_ID != 0)
                {

                    if (NguoiDungBO != null)
                    {
                        nguoidung.TENDANGNHAP = NguoiDungBO.TENDANGNHAP.Trim();
                        //nguoidung.EMAIL = NguoiDungBO.EMAIL;
                        //nguoidung.COSO_ID = NguoiDungBO.COSO_ID;
                        nguoidung.TRANGTHAI = NguoiDungBO.TRANGTHAI;
                        if (NguoiDungBO.DIENTHOAI != null)
                        {
                            nguoidung.DIENTHOAI = NguoiDungBO.DIENTHOAI.Trim();
                        }

                        nguoidung.DM_VAITRO_ID = NguoiDungBO.DM_VAITRO_ID;

                        //if (NguoiDungBO.TYPE != 0)
                        //{
                        //    if (NguoiDungBO.TYPE == 1)
                        //    {
                        //        nguoidung.DM_PHONGBAN_ID = NguoiDungBO.TYPEID;
                        //    }
                        //}
                        if (NguoiDungBO.MATKHAU != NguoiDungBO.PASS_OLD)
                        {
                            mahoa = MaHoaMatKhau.GenerateRandomString(5);
                            nguoidung.MAHOA_MK = mahoa;
                            nguoidung.MATKHAU = MaHoaMatKhau.Encode_Data(NguoiDungBO.MATKHAU.Trim() + mahoa);
                        }
                        nguoidung.EMAIL = NguoiDungBO.EMAIL;
                        nguoidung.COSO_ID = NguoiDungBO.COSO_ID;
                        nguoidung.DM_DONVI_ID = NguoiDungBO.DM_DONVI_ID;
                        nguoidung.HOTEN = NguoiDungBO.HOTEN;
                        nguoidung.CAPCOSO_ID = NguoiDungBO.CAPCOSO_ID;
                        nguoidung.DM_PHONGBAN_ID = NguoiDungBO.DM_PHONGBAN_ID;

                        this.Save(nguoidung);
                    }
                }


                this.Save();
                #region Lưu vai trò người dùng
                if (NguoiDungBO.DM_NGUOIDUNG_ID > 0)
                {
                    var lstVaiTroNguoiDung = GetBusiness<NguoiDungVaiTroBusiness>().GetListByNguoiDung((int)NguoiDungBO.DM_NGUOIDUNG_ID);
                    GetBusiness<NguoiDungVaiTroBusiness>().DeleteAll(lstVaiTroNguoiDung);
                    GetBusiness<NguoiDungVaiTroBusiness>().Save();
                }
                if (NguoiDungBO.ListVaiTro != null && NguoiDungBO.ListVaiTro.Count > 0)
                {
                    foreach (var item in NguoiDungBO.ListVaiTro)
                    {
                        NGUOIDUNG_VAITRO nt = new NGUOIDUNG_VAITRO();
                        nt.NGUOIDUNG_ID = (int)nguoidung.DM_NGUOIDUNG_ID;
                        nt.VAITRO_ID = item;
                        nt.NGAYTAO = DateTime.Now;
                        nt.ROLE_DEFAULT = false;
                        GetBusiness<NguoiDungVaiTroBusiness>().Save(nt);
                    }
                }
                #endregion
            }

        }
        public void SaveResetMK(NguoiDungBO nguoidungBO)
        {
            if (nguoidungBO != null)
            {
                DM_NGUOIDUNG nguoidung = this.Find(nguoidungBO.DM_NGUOIDUNG_ID);
                if (nguoidung.DM_NGUOIDUNG_ID != 0)
                {
                    string mahoa = "";
                    if (!string.IsNullOrWhiteSpace(nguoidungBO.MATKHAUMOI))
                    {
                        mahoa = MaHoaMatKhau.GenerateRandomString(5);
                        nguoidung.MAHOA_MK = mahoa;
                        nguoidung.MATKHAU = MaHoaMatKhau.Encode_Data(nguoidungBO.MATKHAUMOI.Trim() + mahoa);
                        nguoidung.TRANGTHAI = nguoidungBO.TRANGTHAI;
                        nguoidung.DIENTHOAI = nguoidungBO.DIENTHOAI;
                    }
                    this.Update(nguoidung, nguoidungBO.MATKHAUMOI);
                    this.Save();
                }
            }
        }

        //public void CheckRole(int roleid, int roleUser)
        //{
        //    string CauHinhVaiTro = ""; string[] itemVaiTro;
        //    CauHinhVaiTro = getCauHinhVaiTro(roleUser);
        //    itemVaiTro = CauHinhVaiTro.Split(',');
        //    int[] roleids = (from u in GetBusiness<DmVaitroBusiness>().All.Where(o => o.TRANGTHAI == 1).AsEnumerable()
        //                       join i in itemVaiTro
        //                       on u.MA_VAITRO equals i.Trim()
        //                       select u.DM_VAITRO_ID).ToArray();
        //    if (!roleids.Contains(roleid))
        //    { throw new BusinessException("Bạn không có quyền thêm vai trò này"); }
        //}


        //public string getCauHinhVaiTro(int loaitruong)
        //{
        //    string CauHinhVaiTro = "";
        //    if (loaitruong == 1)
        //    {
        //        CauHinhVaiTro = GetBusiness<CauhinhHethongBusiness>().All.Where(o => o.LOAI_CAU_HINH == "TAO_TK_QLTHI" && o.MA_CAU_HINH == "01").Select(o => o.GIA_TRI).FirstOrDefault();
        //    }
        //    if (loaitruong == 2)
        //    {
        //        CauHinhVaiTro = GetBusiness<CauhinhHethongBusiness>().All.Where(o => o.LOAI_CAU_HINH == "TAO_TK_QLTHI" && o.MA_CAU_HINH == "02").Select(o => o.GIA_TRI).FirstOrDefault();
        //    }
        //    return CauHinhVaiTro;
        //}
        public bool isTenDangNhapExist(string TenDangNhap, decimal NguoiDungId = 0)
        {
            bool check = false;
            IEnumerable<DM_NGUOIDUNG> lstNguoiDung = null;
            if (NguoiDungId != 0)
            {
                lstNguoiDung = this.All.Where(o => o.TENDANGNHAP.ToLower().Equals(TenDangNhap.ToLower())
                    && o.DM_NGUOIDUNG_ID != NguoiDungId);
            }
            else
            {
                lstNguoiDung = this.All.Where(o => o.TENDANGNHAP.ToLower().Equals(TenDangNhap.ToLower()));
            }

            if (lstNguoiDung.Any())
            {
                check = true;
            }
            return check;
        }

        private bool TenDangNhapExist(string TenDangNhap, int? DonViID)
        {
            if (!string.IsNullOrEmpty(TenDangNhap) && DonViID.HasValue)
            {
                return this.All.Where(x => x.DM_DONVI_ID == DonViID && x.TENDANGNHAP.ToLower().Equals(TenDangNhap.ToLower())).Count() > 0;
            }
            return false;
        }

        public List<DMNguoiDungBO> GetByPhongBan(int id)
        {

            var result = (from nguoiDung in this.context.DM_NGUOIDUNG
                          //join NguoiDungCanBo in this.context.HSCB_NGUOIDUNG_CANBO
                          //on nguoiDung.DM_NGUOIDUNG_ID equals NguoiDungCanBo.NGUOIDUNG_ID
                          //into g1
                          //from gNguoiDung in g1.DefaultIfEmpty()
                          //join coso in this.context.COSO
                          //on nguoiDung.COSO_ID equals coso.COSO_ID
                          //join phongban in this.context.DM_PHONGBAN
                          //on nguoiDung.DM_PHONGBAN_ID equals phongban.DM_PHONGBAN_ID
                          //into g2
                          //from gPhongBan in g2.DefaultIfEmpty()
                          where nguoiDung.DM_PHONGBAN_ID == id
                          select new DMNguoiDungBO
                          {
                              DIENTHOAI = nguoiDung.DIENTHOAI,
                              DM_NGUOIDUNG_ID = nguoiDung.DM_NGUOIDUNG_ID,
                              EMAIL = nguoiDung.EMAIL,
                              //TEN_PHONGBAN = gPhongBan.TENPHONGBAN,
                              //TEN_COSO = coso.TENCOSO,
                              HOTEN = nguoiDung.HOTEN,
                              TENDANGNHAP = nguoiDung.TENDANGNHAP,
                          });
            return result.OrderBy(x => x.DM_NGUOIDUNG_ID).ToList();
        }

        public void ValidateForm(NguoiDungBO NguoiDungBO)
        {
            Regex r = new Regex("^[a-zA-Z0-9_]*$");
            Regex regexSDT = new Regex("^[0-9.]+$");
            if (string.IsNullOrEmpty(NguoiDungBO.TENDANGNHAP))
            {
                throw new BusinessException("Bạn chưa nhập Tên đăng nhập");
            }
            //#region kiểm tra tên đăng nhập có tồn tại trong đơn vị đã chọn

            //if (!string.IsNullOrEmpty(NguoiDungBO.TENDANGNHAP))
            //{
            //    if (this.isTenDangNhapExist(NguoiDungBO.TENDANGNHAP.Trim(), NguoiDungBO.DM_NGUOIDUNG_ID, NguoiDungBO.DM_DONVI_ID))
            //    {
            //        var donvi = GetBusiness<DmDonViBusiness>().All.Where(x => x.ID == NguoiDungBO.DM_DONVI_ID).FirstOrDefault();
            //        if (donvi != null)
            //        {
            //            throw new BusinessException("Đơn vị " + donvi.TEN_DONVI + " đã tồn tại tên đăng nhập '" + NguoiDungBO.TENDANGNHAP + "'");
            //        }
            //        else
            //        {
            //            throw new BusinessException("Đơn vị bạn chọn đã tồn tại tên đăng nhập '" + NguoiDungBO.TENDANGNHAP + "'");
            //        }
            //    }
            //}

            //#endregion

            if (!string.IsNullOrEmpty(NguoiDungBO.TENDANGNHAP))
            {
                if (!r.IsMatch(NguoiDungBO.TENDANGNHAP.Trim()))
                {
                    throw new BusinessException("Tên đăng nhập không được chứa kí tự đặc biệt");
                }
            }
            if (NguoiDungBO.TENDANGNHAP.Length > 50)
            {
                throw new BusinessException("Tên đăng nhập không được quá 50 kí tự");
            }
            if (!string.IsNullOrEmpty(NguoiDungBO.MATKHAU))
            {
                if (NguoiDungBO.MATKHAU.Length > 100 || NguoiDungBO.MATKHAU.Length < 8)
                {
                    throw new BusinessException("Mật khẩu phải lớn hơn 7 và nhỏ hơn 100 ký tự");
                }
            }
            if (string.IsNullOrEmpty(NguoiDungBO.MATKHAU))
            {
                throw new BusinessException("Bạn chưa nhập mật khẩu");
            }
            if (!string.IsNullOrEmpty(NguoiDungBO.DIENTHOAI))
            {
                if (!regexSDT.IsMatch(NguoiDungBO.DIENTHOAI.Trim()))
                {
                    throw new BusinessException("Số điện thoại nhập sai định dạng");
                }
                if (NguoiDungBO.DIENTHOAI.Trim().Length < 10)
                {
                    throw new BusinessException("Điện thoại phải nhập ít nhất 10 kí tự");
                }
                if (NguoiDungBO.DIENTHOAI.Length > 20)
                {
                    throw new BusinessException("Số điện thoại không được nhập quá 20 kí tự");
                }
            }
        }
        public DM_NGUOIDUNG GetDataByUserID(long? ID)
        {
            var NguoiDung = (from nguoidung in this.context.DM_NGUOIDUNG
                             where nguoidung.DM_NGUOIDUNG_ID == ID
                             select nguoidung).FirstOrDefault();
            return NguoiDung;

        }
        public List<DM_NGUOIDUNG> GetDataByDonVi(int DONVI_ID)
        {
            var result = from nguoidung in this.context.DM_NGUOIDUNG
                         where nguoidung.DM_DONVI_ID == DONVI_ID
                         select nguoidung;
            return result.ToList();
        }
        /// <summary>
        /// DANH SÁCH (ID | TÊN ĐĂNG NHẬP) NGƯỜI DÙNG
        /// </summary>
        /// <param name="lstNguoiDungID"></param>
        /// <returns></returns>
        public string[] GetListNguoiDungInfo(List<int> lstNguoiDungID, decimal userCurrent = 0)
        {
            string[] result = new string[2];
            if (lstNguoiDungID != null && lstNguoiDungID.Count > 0)
            {
                var lst = this.All.Where(x => lstNguoiDungID.Contains((int)x.DM_NGUOIDUNG_ID) && x.DM_NGUOIDUNG_ID != userCurrent).ToList();
                if (lst != null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        result[0] += item.DM_NGUOIDUNG_ID + ",";
                        result[1] += item.TENDANGNHAP + ",";
                    }
                }
            }
            return result;
        }


        public List<DM_NGUOIDUNG> GetListByListID(List<long> listID)
        {
            var result = this.All.Where(o => listID.Contains(o.DM_NGUOIDUNG_ID));
            return result.ToList();
        }
        public List<DM_NGUOIDUNG> GetDataByCoSo(int COSO_ID)
        {
            var result = from nguoidung in this.context.DM_NGUOIDUNG
                         where nguoidung.COSO_ID == COSO_ID
                         select nguoidung;
            return result.ToList();
        }
        //TODO:
        //public List<DM_NGUOIDUNG> GetListByListDonViID(int cosoID, List<int> listDonViID, string Role)
        //{
        //    var result = from user in this.context.DM_NGUOIDUNG
        //                 join donvi in this.context.DM_DONVI
        //                 on user.DM_DONVI_ID equals donvi.ID
        //                 join vaitro in this.context.DM_THAOTAC
        //                 on user.DM_VAITRO_ID equals vaitro.DM_THAOTAC_ID
        //                 join user_thaotac in this.context.VAITRO_THAOTAC;
        //    return result.ToList();
        //}

        public List<NguoiDungVaiTroBO> GetListNguoiDung(int COSO_ID, int DONVI_ID, List<string> ListRole, List<long> lstSelected = null, List<long> lstExisted = null)
        {
            var ListThaoTac = GetBusiness<VaitroThaotacBusiness>().All
                                 .Where(o =>
                                     o.VAITRO_CHUCNANG.DM_CHUCNANG.TRANGTHAI == 1 &&
                                     o.TRANGTHAI == 1 && o.VAITRO_CHUCNANG.TRANGTHAI == 1 &&
                                     o.DM_THAOTAC.TRANGTHAI == 1 && o.COSO_ID == COSO_ID && ListRole.Contains(o.DM_THAOTAC.THAOTAC))
                                 .Select(o => new ThaoTacBO
                                 {
                                     DM_CHUCNANG_ID = o.VAITRO_CHUCNANG.DM_VAITRO_ID.Value,
                                     DM_THAOTAC_ID = (int)o.DM_THAOTAC_ID.Value,
                                     TEN_THAOTAC = o.DM_THAOTAC.TEN_THAOTAC,
                                     THAOTAC = o.DM_THAOTAC.THAOTAC,
                                     MENU_LINK = o.DM_THAOTAC.MENU_LINK,
                                     TT_HIENTHI = o.DM_THAOTAC.TT_HIENTHI,
                                     IS_HIENTHI = o.DM_THAOTAC.IS_HIENTHI
                                 }).OrderBy(x => x.TT_HIENTHI).ToList();
            var lstRole = ListThaoTac.Select(x => x.DM_CHUCNANG_ID).GroupBy(a => a).Select(o => o.First()).ToList();
            if (lstSelected != null && lstSelected.Count > 0)
            {
                var result = (from vaitro_nguoidung in this.context.NGUOIDUNG_VAITRO
                              join nguoidung in this.context.DM_NGUOIDUNG
                               on vaitro_nguoidung.NGUOIDUNG_ID equals (int)nguoidung.DM_NGUOIDUNG_ID
                               into group2
                              from g2 in group2.DefaultIfEmpty()
                              join vaitro in this.context.DM_VAITRO
                              on vaitro_nguoidung.VAITRO_ID equals vaitro.DM_VAITRO_ID
                              into group1
                              from g1 in group1.DefaultIfEmpty()
                              where lstRole.Contains(vaitro_nguoidung.VAITRO_ID.Value)
                              select new NguoiDungVaiTroBO
                              {
                                  DM_NGUOIDUNG_ID = g2.DM_NGUOIDUNG_ID,
                                  HOTEN = g2.HOTEN,
                                  VAITRO = g1.TEN_VAITRO,
                                  MA_VAITRO = g1.MA_VAITRO,
                                  DM_VAITRO_ID = vaitro_nguoidung.VAITRO_ID,
                                  TENDANGNHAP = g2.TENDANGNHAP,
                                  DONVI_ID = g2.DM_DONVI_ID,
                                  COSO_ID = g2.COSO_ID,
                                  EMAIL = g2.EMAIL,
                                  DIENTHOAI = g2.DIENTHOAI,
                                  SELECTED = ((g2.DM_NGUOIDUNG_ID != null) ? lstSelected.Contains(g2.DM_NGUOIDUNG_ID) : false)
                              }).ToList();
                result = result.Where(x => x.DM_NGUOIDUNG_ID != null).ToList();
                if (lstExisted != null && lstExisted.Count > 0)
                {
                    result = result.Where(x => x.DM_NGUOIDUNG_ID.HasValue && !lstExisted.Contains(x.DM_NGUOIDUNG_ID.Value)).ToList();
                }
                if (DONVI_ID > 0)
                    result = result.Where(x => x.DONVI_ID.HasValue && x.DONVI_ID.Value == DONVI_ID).ToList();
                return result.ToList();
            }
            else
            {
                var result = (from vaitro_nguoidung in this.context.NGUOIDUNG_VAITRO
                              join nguoidung in this.context.DM_NGUOIDUNG
                               on vaitro_nguoidung.NGUOIDUNG_ID equals (int)nguoidung.DM_NGUOIDUNG_ID
                               into group2
                              from g2 in group2.DefaultIfEmpty()
                              join vaitro in this.context.DM_VAITRO
                              on vaitro_nguoidung.VAITRO_ID equals vaitro.DM_VAITRO_ID
                              into group1
                              from g1 in group1.DefaultIfEmpty()
                              where lstRole.Contains(vaitro_nguoidung.VAITRO_ID.Value)
                              select new NguoiDungVaiTroBO
                              {
                                  DM_NGUOIDUNG_ID = g2.DM_NGUOIDUNG_ID,
                                  HOTEN = g2.HOTEN,
                                  VAITRO = g1.TEN_VAITRO,
                                  MA_VAITRO = g1.MA_VAITRO,
                                  DM_VAITRO_ID = vaitro_nguoidung.VAITRO_ID,
                                  TENDANGNHAP = g2.TENDANGNHAP,
                                  EMAIL = g2.EMAIL,
                                  DIENTHOAI = g2.DIENTHOAI,
                                  DONVI_ID = g2.DM_DONVI_ID,
                                  COSO_ID = g2.COSO_ID,
                                  SELECTED = false
                              }).ToList();
                result = result.Where(x => x.DM_NGUOIDUNG_ID != null).ToList();
                if (lstExisted != null && lstExisted.Count > 0)
                {
                    result = result.Where(x => x.DM_NGUOIDUNG_ID.HasValue && !lstExisted.Contains(x.DM_NGUOIDUNG_ID.Value)).ToList();
                }
                if (DONVI_ID > 0)
                    result = result.Where(x => x.DONVI_ID.HasValue && x.DONVI_ID.Value == DONVI_ID).ToList();
                return result.ToList();
            }
        }

    }
}
