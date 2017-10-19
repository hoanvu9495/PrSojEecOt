/**
 * The HiNet License
 *
 * Copyright 2015 Hinet JSC. All rights reserved.
 * HINET PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

/** 
* @author  NAMDV
*/

using DAL.Repository;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.CommonBusiness;
using System.Web.Mvc;

namespace Business.Business
{
    public partial class DmChucnangBusiness : GenericBussiness<DM_CHUCNANG>
    {
        string show = "height:auto; width:600px; margin-left:20px;"; // thiết lập css cho danh sách chức năng cấp 2
        string hidden = "height:auto; width:600px; margin-left:20px; display:none;";
        public DmChucnangBusiness(Entities context = null)
            : base()
        {
            repository = new DmChucnangRepository(context);
        }
        public void Save(DM_CHUCNANG chucnang)
        {
            try
            {
                if (chucnang.DM_CHUCNANG_ID == 0)
                {
                    this.repository.Insert(chucnang);
                }
                else
                    this.repository.Update(chucnang);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public List<DMChucNangVM> GetAllVM()
        {
            var result = this.context.DM_CHUCNANG.Select(x => new DMChucNangVM()
            {
                CSSCLASS=x.CSSCLASS,
                DM_CHUCNANG_ID=x.DM_CHUCNANG_ID,
                ICONPATH=x.ICONPATH,
                IS_HIDDEN=x.IS_HIDDEN,
                MA_CHUCNANG=x.MA_CHUCNANG,
                TEN_CHUCNANG=x.TEN_CHUCNANG,
                TRANGTHAI=x.TRANGTHAI,
                TT_HIENTHI=x.TT_HIENTHI,
                URL=x.URL,
            }).OrderByDescending(x=>x.DM_CHUCNANG_ID).ToList();
            return result;
        }
        public DMChucNangVM GetById(int id)
        {
            var result = this.context.DM_CHUCNANG.Where(x => x.DM_CHUCNANG_ID == id).Select(x => new DMChucNangVM()
            {
                //CHUCNANG_CHA = x.CHUCNANG_CHA,
                CSSCLASS = x.CSSCLASS,
                DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
                ICONPATH = x.ICONPATH,
                IS_HIDDEN = x.IS_HIDDEN,
                MA_CHUCNANG = x.MA_CHUCNANG,
                TEN_CHUCNANG = x.TEN_CHUCNANG,
                TRANGTHAI = x.TRANGTHAI,
                TT_HIENTHI = x.TT_HIENTHI,
                URL = x.URL,
            }).FirstOrDefault();
            return result;
        }
        public List<dmChucNangBO> GetListChucNangFull(int DM_VAITRO_ID)
        {
            var thaotacResult = GetBusiness<VaitroChucnangBusiness>().All.Where(x => x.DM_VAITRO_ID == DM_VAITRO_ID && x.MAC_DINH == 1).FirstOrDefault();
            int SetChecked = 0;
            if (thaotacResult != null && thaotacResult.MAC_DINH.HasValue && thaotacResult.MAC_DINH > 0)
            {
                SetChecked = (int)thaotacResult.DM_CHUCNANG_ID;
            }
            //var listChucNangParent = this.All.Where(x => x.CHUCNANG_CHA == null).ToList().Select(x => new dmChucNangBO()
            //{
            //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //    TEN_CHUCNANG = x.TEN_CHUCNANG,
            //    MA_CHUCNANG = x.MA_CHUCNANG
            //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
            //var listChucNangChild = this.All.Where(x => x.CHUCNANG_CHA > 0 && x.TRANGTHAI == 1).ToList().Select(x => new dmChucNangBO()
            //{
            //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //    CHUCNANG_CHA = x.CHUCNANG_CHA,
            //    TEN_CHUCNANG = x.TEN_CHUCNANG,
            //    MA_CHUCNANG = x.MA_CHUCNANG,
            //    selected = (SetChecked == x.DM_CHUCNANG_ID)
            //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
            //List<dmChucNangBO> listChucNang = new List<dmChucNangBO>();
            //if (listChucNangParent != null)
            //{
            //    foreach (var cn in listChucNangParent)
            //    {
            //        dmChucNangBO chucNang = new dmChucNangBO();
            //        chucNang.DM_CHUCNANG_ID = cn.DM_CHUCNANG_ID;
            //        chucNang.TEN_CHUCNANG = cn.TEN_CHUCNANG;
            //        chucNang.MA_CHUCNANG = cn.MA_CHUCNANG;
            //        chucNang.selected = (SetChecked == cn.DM_CHUCNANG_ID);
            //        chucNang.ListChildren = listChucNangChild.Where(x => x.CHUCNANG_CHA == cn.DM_CHUCNANG_ID).ToList();
            //        listChucNang.Add(chucNang);
            //    }
            //}
            //return listChucNang;
            return null;

        }
        /// <summary>
        /// trả về số lượng danh sách các chức năng cấp 2( có CHUCNANG_CHA != null)
        /// </summary>
        /// <param name="DM_CHUCNANG_ID">mã tự tăng của DM_CHUCNANG</param>
        /// <returns></returns>
        //public int GetCountChucNang(int DM_CHUCNANG_ID)
        //{
        //    var chucnangResult = this.All.Where(x => x.CHUCNANG_CHA != null && x.CHUCNANG_CHA == DM_CHUCNANG_ID);
        //    return chucnangResult.Count();
        //}
        public List<dmChucNangBO> GetChildMenu(int VaiTroID, string CheckArea)
        {
            int ChucNangChaID = 0;
            var Result = (
                from cn in this.context.DM_CHUCNANG
                join vtcn in this.context.VAITRO_CHUCNANG
                on cn.DM_CHUCNANG_ID equals vtcn.DM_CHUCNANG_ID
                into group1
                from g1 in group1.DefaultIfEmpty()
                select new dmChucNangBO()
                {
                    DM_CHUCNANG_ID = cn.DM_CHUCNANG_ID,
                    URL = cn.URL,
                    TT_HIENTHI = cn.TT_HIENTHI,
                    IS_HIDDEN = cn.IS_HIDDEN,
                    TEN_CHUCNANG = cn.TEN_CHUCNANG,
                    VAITROID = g1.DM_VAITRO_ID,
                    TRANGTHAI = cn.TRANGTHAI,
                    //CHUCNANG_CHA = cn.CHUCNANG_CHA
                }
                );
            if (!string.IsNullOrEmpty(CheckArea))
            {
                //ChucNangChaID = (int)this.All.Where(o => o.URL.ToLower().Contains(CheckArea.ToLower())).Select(o => o.CHUCNANG_CHA).FirstOrDefault();
                //if (ChucNangChaID > 0)
                //    Result = Result.Where(o => o.CHUCNANG_CHA == ChucNangChaID);
            }
            if (VaiTroID > 0)
            {
                Result = Result.Where(x => x.VAITROID == VaiTroID && x.TRANGTHAI == 1);
            }
            return Result.ToList();
        }
        /// <summary>
        /// kiểm tra số lượng DM_CHUCNANG_ID trong bảng VAITRO_CHUCNANG
        /// </summary>
        /// <param name="DM_VAITRO_ID"> mã tự tăng của bảng VAITRO_CHUCNANG</param>
        /// <param name="DM_CHUCNANG_ID"></param>
        /// <returns></returns>
        public int CheckChucNang(int DM_VAITRO_ID, int DM_CHUCNANG_ID)
        {
            //var result = (from v in this.context.VAITRO_CHUCNANG join c in this.context.DM_CHUCNANG on v.DM_CHUCNANG_ID equals c.DM_CHUCNANG_ID where v.DM_VAITRO_ID == DM_VAITRO_ID && c.CHUCNANG_CHA == DM_CHUCNANG_ID select v).ToList();
            //if (result.Count() > 0)
            //{
            //    return result.Count();

            //}
            return -1;
        }
        /// <summary>
        /// trả về danh sách các chức năng cấp 1 có CHUCNANG_CHA = null
        /// </summary>
        /// <param name="DM_VAITRO_ID">mã tự tăng của bảng DM_VAITRO</param>
        ///// <param name="VaiTro_ChucNang">Số lượng danh sách vai trò chức năng của bảng VAITRO_CHUCNANG</param>
        /// <returns></returns>
        public List<dmChucNangBO> GetListChucNang(int DM_VAITRO_ID, int? coso_id = 0)
        {
            var ListThaoTac = GetBusiness<VaitroThaotacBusiness>().All
                .Where(o => o.VAITRO_CHUCNANG.DM_VAITRO_ID == DM_VAITRO_ID &&
                    o.VAITRO_CHUCNANG.DM_CHUCNANG.TRANGTHAI == 1 &&
                    o.TRANGTHAI == 1 && o.VAITRO_CHUCNANG.TRANGTHAI == 1 &&
                    o.DM_THAOTAC.TRANGTHAI == 1).Select(o => o.DM_THAOTAC_ID).ToList();
                              
            var ListChucnang = this.All.ToList().Select(x => new dmChucNangBO()
            {
                DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
                TEN_CHUCNANG = x.TEN_CHUCNANG,
                MA_CHUCNANG = x.MA_CHUCNANG
            }).OrderBy(x => x.TEN_CHUCNANG).ToList();

            List<dmChucNangBO> listChucNang = new List<dmChucNangBO>();

            //var listParent = this.All.Where(x => x.CHUCNANG_CHA == null).ToList().Select(x => new dmChucNangBO()
            //{
            //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //    TEN_CHUCNANG = x.TEN_CHUCNANG,
            //    MA_CHUCNANG = x.MA_CHUCNANG
            //    //selected = (this.GetCountChucNang(x.DM_CHUCNANG_ID) == this.CheckChucNang(DM_VAITRO_ID, x.DM_CHUCNANG_ID)),
            //    //display = this.CheckChucNang(DM_VAITRO_ID, x.DM_CHUCNANG_ID) > 0 ? show : hidden
            //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
            //List<dmChucNangBO> listChucNang = new List<dmChucNangBO>();
            //var listChildren = this.All.Where(c => c.CHUCNANG_CHA > 0).ToList().Select(x => new dmChucNangBO()
            //            {
            //                DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //                TEN_CHUCNANG = x.TEN_CHUCNANG,
            //                MA_CHUCNANG = x.MA_CHUCNANG,
            //                CHUCNANG_CHA = x.CHUCNANG_CHA
            //            }).OrderBy(x => x.TEN_CHUCNANG).ToList();
            var listThaoTac = (from t in context.DM_THAOTAC
                               select new DmThaoTacBO()
                               {
                                   DM_THAOTAC_ID = t.DM_THAOTAC_ID,
                                   TEN_THAOTAC = t.TEN_THAOTAC,
                                   DM_CHUCNANG_ID = t.DM_CHUCNANG_ID,
                                   Checked = ListThaoTac.Contains(t.DM_THAOTAC_ID)
                               }).OrderBy(t => t.TEN_THAOTAC).ToList();
            if (ListChucnang != null && ListChucnang.Count > 0)
            {
                foreach (var parent in ListChucnang)
                {
                    dmChucNangBO chucnang = new dmChucNangBO();
                    chucnang.DM_CHUCNANG_ID = parent.DM_CHUCNANG_ID;
                    chucnang.TEN_CHUCNANG = parent.TEN_CHUCNANG;
                    chucnang.MA_CHUCNANG = parent.MA_CHUCNANG;
                    //chucnang.ListChildren = listChildren.Where(x => x.CHUCNANG_CHA == parent.DM_CHUCNANG_ID).ToList();
                    if (chucnang.ListChildren != null)
                    {
                        foreach (var item in chucnang.ListChildren)
                        {
                            item.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == item.DM_CHUCNANG_ID).ToList();
                        }
                    }
                    chucnang.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == parent.DM_CHUCNANG_ID).ToList();
                    listChucNang.Add(chucnang);
                }
            }
            return listChucNang;
        }

        /// <summary>
        /// Lấy danh sách chức năng đổ ra dropdownlist
        /// </summary>
        /// <param name="DM_VAITRO_ID"></param>
        /// <param name="CoSo_ID"></param>
        /// <returns></returns>
        public List<SelectListItem> GetListChucNang(int select=0)
        {
            var list = this.context.DM_CHUCNANG.Select(x => new SelectListItem()
            {
                Text = x.TEN_CHUCNANG,
                Value = x.DM_CHUCNANG_ID.ToString(),
                Selected = select > 0 && select == x.DM_CHUCNANG_ID
            }).ToList();
            return list;
        }
        /// <summary>
        /// trả về danh sách các chức năng cấp 1 có CHUCNANG_CHA = null
        /// </summary>
        /// <param name="DM_VAITRO_ID">mã tự tăng của bảng DM_VAITRO</param>
        ///// <param name="VaiTro_ChucNang">Số lượng danh sách vai trò chức năng của bảng VAITRO_CHUCNANG</param>
        /// <returns></returns>
        public List<dmChucNangBO> GetListChucNang(List<int> ListVaiTro, int? CoSo_ID = 0)
        {
            var ListThaoTac = GetBusiness<VaitroThaotacBusiness>().All
                .Where(o => ListVaiTro.Contains(o.VAITRO_CHUCNANG.DM_VAITRO_ID.Value) &&
                    o.VAITRO_CHUCNANG.DM_CHUCNANG.TRANGTHAI == 1 &&
                    o.TRANGTHAI == 1 && o.VAITRO_CHUCNANG.TRANGTHAI == 1 &&
                    o.DM_THAOTAC.TRANGTHAI == 1).Select(o => o.DM_THAOTAC_ID).ToList();

            //var listParent = this.All.Where(x => x.CHUCNANG_CHA == null).ToList().Select(x => new dmChucNangBO()
            //{
            //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //    TEN_CHUCNANG = x.TEN_CHUCNANG,
            //    MA_CHUCNANG = x.MA_CHUCNANG
            //    //selected = (this.GetCountChucNang(x.DM_CHUCNANG_ID) == this.CheckChucNang(DM_VAITRO_ID, x.DM_CHUCNANG_ID)),
            //    //display = this.CheckChucNang(DM_VAITRO_ID, x.DM_CHUCNANG_ID) > 0 ? show : hidden
            //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
            //List<dmChucNangBO> listChucNang = new List<dmChucNangBO>();
            //var listChildren = this.All.Where(c => c.CHUCNANG_CHA > 0).ToList().Select(x => new dmChucNangBO()
            //{
            //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //    TEN_CHUCNANG = x.TEN_CHUCNANG,
            //    MA_CHUCNANG = x.MA_CHUCNANG,
            //    CHUCNANG_CHA = x.CHUCNANG_CHA
            //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
            //var listThaoTac = (from t in context.DM_THAOTAC
            //                   select new DmThaoTacBO()
            //                   {
            //                       DM_THAOTAC_ID = t.DM_THAOTAC_ID,
            //                       TEN_THAOTAC = t.TEN_THAOTAC,
            //                       DM_CHUCNANG_ID = t.DM_CHUCNANG_ID,
            //                       Checked = ListThaoTac.Contains(t.DM_THAOTAC_ID)
            //                   }).OrderBy(t => t.TEN_THAOTAC).ToList();
            //if (listParent != null && listParent.Count > 0)
            //{
            //    foreach (var parent in listParent)
            //    {
            //        dmChucNangBO chucnang = new dmChucNangBO();
            //        chucnang.DM_CHUCNANG_ID = parent.DM_CHUCNANG_ID;
            //        chucnang.TEN_CHUCNANG = parent.TEN_CHUCNANG;
            //        chucnang.MA_CHUCNANG = parent.MA_CHUCNANG;
            //        chucnang.ListChildren = listChildren.Where(x => x.CHUCNANG_CHA == parent.DM_CHUCNANG_ID).ToList();
            //        if (chucnang.ListChildren != null)
            //        {
            //            foreach (var item in chucnang.ListChildren)
            //            {
            //                item.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == item.DM_CHUCNANG_ID).ToList();
            //            }
            //        }
            //        chucnang.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == parent.DM_CHUCNANG_ID).ToList();
            //        listChucNang.Add(chucnang);
            //    }
            //}
            //return listChucNang;
            return null;
        }
        /// <summary>
        /// trả về danh sách người dùng thao tác
        /// </summary>
        /// <param name="NGUOIDUNG_ID"></param>
        /// <param name="CoSo_ID"></param>
        /// <returns></returns>
        //public List<dmChucNangBO> GetListNguoiDungChucNang(decimal? NGUOIDUNG_ID = 0)
        //{
        //    var listNguoiDungChucNang = GetBusiness<NguoidungChucnangBusiness>().All.Where(x => x.NGUOIDUNG_ID == NGUOIDUNG_ID).Select(x => x.NGUOIDUNG_CHUCNANG_ID).ToList();
        //    var ListThaoTac = GetBusiness<NguoidungThaotacBusiness>().All
        //        .Where(o => listNguoiDungChucNang.Contains(o.NGUOIDUNG_CHUCNANG_ID.Value) && o.TRANGTHAI == 1).Select(o => o.DM_THAOTAC_ID).ToList();

        //    //var listParent = this.All.Where(x => x.CHUCNANG_CHA == null).ToList().Select(x => new dmChucNangBO()
        //    //{
        //    //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
        //    //    TEN_CHUCNANG = x.TEN_CHUCNANG,
        //    //    MA_CHUCNANG = x.MA_CHUCNANG
        //    //    //selected = (this.GetCountChucNang(x.DM_CHUCNANG_ID) == this.CheckChucNang(DM_VAITRO_ID, x.DM_CHUCNANG_ID)),
        //    //    //display = this.CheckChucNang(DM_VAITRO_ID, x.DM_CHUCNANG_ID) > 0 ? show : hidden
        //    //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
        //    //List<dmChucNangBO> listChucNang = new List<dmChucNangBO>();
        //    //var listChildren = this.All.Where(c => c.CHUCNANG_CHA > 0).ToList().Select(x => new dmChucNangBO()
        //    //{
        //    //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
        //    //    TEN_CHUCNANG = x.TEN_CHUCNANG,
        //    //    MA_CHUCNANG = x.MA_CHUCNANG,
        //    //    CHUCNANG_CHA = x.CHUCNANG_CHA
        //    //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
        //    //var listThaoTac = (from t in context.DM_THAOTAC
        //    //                   select new DmThaoTacBO()
        //    //                   {
        //    //                       DM_THAOTAC_ID = t.DM_THAOTAC_ID,
        //    //                       TEN_THAOTAC = t.TEN_THAOTAC,
        //    //                       DM_CHUCNANG_ID = t.DM_CHUCNANG_ID,
        //    //                       Checked = ListThaoTac.Contains(t.DM_THAOTAC_ID)
        //    //                   }).OrderBy(t => t.TEN_THAOTAC).ToList();
        //    //if (listParent != null && listParent.Count > 0)
        //    //{
        //    //    foreach (var parent in listParent)
        //    //    {
        //    //        dmChucNangBO chucnang = new dmChucNangBO();
        //    //        chucnang.DM_CHUCNANG_ID = parent.DM_CHUCNANG_ID;
        //    //        chucnang.TEN_CHUCNANG = parent.TEN_CHUCNANG;
        //    //        chucnang.MA_CHUCNANG = parent.MA_CHUCNANG;
        //    //        chucnang.ListChildren = listChildren.Where(x => x.CHUCNANG_CHA == parent.DM_CHUCNANG_ID).ToList();
        //    //        if (chucnang.ListChildren != null)
        //    //        {
        //    //            foreach (var item in chucnang.ListChildren)
        //    //            {
        //    //                item.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == item.DM_CHUCNANG_ID).ToList();
        //    //            }
        //    //        }
        //    //        chucnang.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == parent.DM_CHUCNANG_ID).ToList();
        //    //        listChucNang.Add(chucnang);
        //    //    }
        //    //}
        //    //return listChucNang;
        //    return null;
        //}
        /// <summary>
        /// trả về danh sách các chức năng cấp 2 có CHUCNANG_CHA != null
        /// </summary>
        /// <param name="VaiTroChucNang">Danh sách các vai trò chức năng của bảng VAITRO_CHUCNANG</param>
        /// <param name="VaiTro_ChucNang">Số lượng danh sách vai trò chức năng của bảng VAITRO_CHUCNANG </param>
        /// <returns></returns>
        public List<dmChucNangBO> GetListChucNangCap2(List<VAITRO_CHUCNANG> VaiTroChucNang, int VaiTro_ChucNang)
        {
            //var chucNangCap2 = this.All.Where(x => x.CHUCNANG_CHA != null);
            //List<dmChucNangBO> Result = new List<dmChucNangBO>();
            //if (VaiTro_ChucNang > 0)
            //{
            //    foreach (var chucnangCon in chucNangCap2)
            //    {
            //        dmChucNangBO chucnang = new dmChucNangBO();
            //        chucnang.DM_CHUCNANG_ID = chucnangCon.DM_CHUCNANG_ID;
            //        chucnang.MA_CHUCNANG = chucnangCon.MA_CHUCNANG;
            //        chucnang.TEN_CHUCNANG = chucnangCon.TEN_CHUCNANG;
            //        chucnang.CHUCNANG_CHA = chucnangCon.CHUCNANG_CHA;
            //        bool check = false;
            //        foreach (var item in VaiTroChucNang)
            //        {
            //            if (item.DM_CHUCNANG_ID == chucnangCon.DM_CHUCNANG_ID)
            //            {
            //                check = true;
            //            }
            //        }
            //        chucnang.selected = check;
            //        Result.Add(chucnang);
            //    }
            //}
            //else
            //{
            //    Result = chucNangCap2.ToList().Select(x => new dmChucNangBO()
            //    {
            //        DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //        MA_CHUCNANG = x.MA_CHUCNANG,
            //        TEN_CHUCNANG = x.TEN_CHUCNANG,
            //        CHUCNANG_CHA = x.CHUCNANG_CHA,
            //        selected = false
            //    }).ToList();
            //}
            //return Result;
            return null;
        }
        public int FindMa(int? CHUCNANG_ID, string MACHUCNANG)
        {
            if (CHUCNANG_ID != 0)
            {
                var result = this.All.Where(x => x.DM_CHUCNANG_ID == CHUCNANG_ID).FirstOrDefault();
                if (result.MA_CHUCNANG.ToLower().Trim() == MACHUCNANG.ToLower().Trim())
                {
                    return 0;
                }
                else
                {
                    return this.All.Where(x => x.MA_CHUCNANG.ToLower().Trim().Equals(MACHUCNANG.ToLower().Trim())).Count();

                }
            }
            return this.All.Where(x => x.MA_CHUCNANG.ToLower().Trim().Equals(MACHUCNANG.ToLower().Trim())).Count();
        }


        public List<dmChucNangBO> GetListChucNang2NguoiDung(int NGUOIDUNG_ID)
        {
            //var ListThaoTac = GetBusiness<NguoidungThaotacBusiness>().All
            //    .Where(o => o.NGUOIDUNG_CHUCNANG.NGUOIDUNG_ID == NGUOIDUNG_ID &&
            //        o.NGUOIDUNG_CHUCNANG.DM_CHUCNANG.TRANGTHAI == 1 &&
            //        o.TRANGTHAI == 1 && o.NGUOIDUNG_CHUCNANG.TRANGTHAI == 1 &&
            //        o.DM_THAOTAC.TRANGTHAI == 1).Select(o => o.DM_THAOTAC_ID).ToList();

            //var listParent = this.All.Where(x => x.CHUCNANG_CHA == null).ToList().Select(x => new dmChucNangBO()
            //{
            //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //    TEN_CHUCNANG = x.TEN_CHUCNANG,
            //    MA_CHUCNANG = x.MA_CHUCNANG
            //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
            //List<dmChucNangBO> listChucNang = new List<dmChucNangBO>();
            //var listChildren = this.All.Where(c => c.CHUCNANG_CHA > 0).ToList().Select(x => new dmChucNangBO()
            //{
            //    DM_CHUCNANG_ID = x.DM_CHUCNANG_ID,
            //    TEN_CHUCNANG = x.TEN_CHUCNANG,
            //    MA_CHUCNANG = x.MA_CHUCNANG,
            //    CHUCNANG_CHA = x.CHUCNANG_CHA
            //}).OrderBy(x => x.TEN_CHUCNANG).ToList();
            //var listThaoTac = (from t in context.DM_THAOTAC
            //                   select new DmThaoTacBO()
            //                   {
            //                       DM_THAOTAC_ID = t.DM_THAOTAC_ID,
            //                       TEN_THAOTAC = t.TEN_THAOTAC,
            //                       DM_CHUCNANG_ID = t.DM_CHUCNANG_ID,
            //                       Checked = ListThaoTac.Contains(t.DM_THAOTAC_ID)
            //                   }).OrderBy(t => t.TEN_THAOTAC).ToList();
            //if (listParent != null && listParent.Count > 0)
            //{
            //    foreach (var parent in listParent)
            //    {
            //        dmChucNangBO chucnang = new dmChucNangBO();
            //        chucnang.DM_CHUCNANG_ID = parent.DM_CHUCNANG_ID;
            //        chucnang.TEN_CHUCNANG = parent.TEN_CHUCNANG;
            //        chucnang.MA_CHUCNANG = parent.MA_CHUCNANG;
            //        chucnang.ListChildren = listChildren.Where(x => x.CHUCNANG_CHA == parent.DM_CHUCNANG_ID).ToList();
            //        if (chucnang.ListChildren != null)
            //        {
            //            foreach (var item in chucnang.ListChildren)
            //            {
            //                item.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == item.DM_CHUCNANG_ID).ToList();
            //            }
            //        }
            //        chucnang.ListThaoTac = listThaoTac.Where(t => t.DM_CHUCNANG_ID == parent.DM_CHUCNANG_ID).ToList();
            //        listChucNang.Add(chucnang);
            //    }
            //}
            //return listChucNang;
            return null;
        }
    }
}
