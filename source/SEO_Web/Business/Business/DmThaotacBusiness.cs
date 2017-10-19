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
    public partial class DmThaotacBusiness  : GenericBussiness<DM_THAOTAC>
    {
        public DmThaotacBusiness(Entities context = null)
            : base()
        {
            repository = new DmThaotacRepository(context);
        }
        public void Save(DM_THAOTAC thaotac)
        {
            try
            {
                if (thaotac.DM_THAOTAC_ID == 0)
                {
                    this.repository.Insert(thaotac);
                }
                else
                {
                    this.repository.Update(thaotac);
                }

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public DmThaoTacBO GetByID(int id)
        {
            DmThaoTacBO ThaoTacResult = (from t in this.context.DM_THAOTAC
                                         where t.DM_THAOTAC_ID == id
                                         join c in this.context.DM_CHUCNANG on t.DM_CHUCNANG_ID equals
                                             c.DM_CHUCNANG_ID
                                         select new DmThaoTacBO()
                                         {
                                             DM_CHUCNANG_ID = t.DM_CHUCNANG_ID,
                                             DM_THAOTAC_ID = t.DM_THAOTAC_ID,
                                             THAOTAC = t.THAOTAC,
                                             MENU_LINK = t.MENU_LINK,
                                             TenChucNang = c.TEN_CHUCNANG,
                                             TEN_THAOTAC = t.TEN_THAOTAC,
                                             TRANGTHAI = t.TRANGTHAI,
                                             TT_HIENTHI = t.TT_HIENTHI,
                                             IS_HIENTHI = t.IS_HIENTHI
                                         }).FirstOrDefault();
            return ThaoTacResult;
        }

        public List<DmThaoTacBO> GetListBySearchParam(string tenchucnang = "", int? CHUCNANGCAP1 = 0, int? CHUCNANGCAP2 = 0)
        {

            List<DmThaoTacBO> ThaoTacResult = (from t in this.context.DM_THAOTAC
                                               join c in this.context.DM_CHUCNANG on t.DM_CHUCNANG_ID equals
                                                   c.DM_CHUCNANG_ID
                                               select new DmThaoTacBO()
                                               {
                                                   DM_CHUCNANG_ID = t.DM_CHUCNANG_ID,
                                                   DM_THAOTAC_ID = t.DM_THAOTAC_ID,
                                                   THAOTAC=t.THAOTAC,
                                                   MENU_LINK = t.MENU_LINK,
                                                   TenChucNang = c.TEN_CHUCNANG,
                                                   TEN_THAOTAC = t.TEN_THAOTAC,
                                                   TRANGTHAI = t.TRANGTHAI,
                                                   TT_HIENTHI=t.TT_HIENTHI,
                                                   IS_HIENTHI = t.IS_HIENTHI
                                               }).ToList();
            if (ThaoTacResult != null)
            {
                tenchucnang = tenchucnang.Trim().ToLower();
                if (!string.IsNullOrEmpty(tenchucnang))
                {
                    ThaoTacResult = ThaoTacResult.Where(o => o.TEN_THAOTAC.ToLower().Contains(tenchucnang)).ToList();
                }
                //nếu có chức năng cấp 2
                if (CHUCNANGCAP2 > 0)
                {
                    //thì lấy danh sách thao tác thuộc chức năng cấp 2 đó
                    ThaoTacResult = ThaoTacResult.Where(o => o.DM_CHUCNANG_ID == CHUCNANGCAP2).ToList();
                }
                //nếu không có chức năng cấp 2
                else
                {
                    //thì xét, nếu có chức năng cấp 1
                    //if (CHUCNANGCAP1 > 0)
                    //{
                    //    // lấy danh sách chức năng cấp 2 thuộc chức năng cấp 1 đó: DANHSACH_CHUCNANG
                    //    var DANHSACH_CHUCNANG = this.context.DM_CHUCNANG.Where(o => o.CHUCNANG_CHA == CHUCNANGCAP1)
                    //      .Select(o => o.DM_CHUCNANG_ID).ToList();
                    //    // nếu danh sách (DANHSACH_CHUCNANG) có chức năng
                    //    if (DANHSACH_CHUCNANG != null && DANHSACH_CHUCNANG.Count > 0)
                    //    {
                    //        //thì lấy những thao tác thuộc 1 trong các chức năng trong danh sách (DANHSACH_CHUCNANG) ở trên
                    //        ThaoTacResult = ThaoTacResult.Where(o => o.DM_CHUCNANG_ID.HasValue ? DANHSACH_CHUCNANG.Contains(o.DM_CHUCNANG_ID.Value) : true).ToList();
                    //    }
                    //    // nếu danh sách (DANHSACH_CHUCNANG) không có phần tử
                    //    else
                    //    {
                    //        //thì lấy danh sách thao tác thuộc chức năng cấp 1
                    //        ThaoTacResult = ThaoTacResult.Where(o => o.DM_CHUCNANG_ID == CHUCNANGCAP1).ToList();
                    //    }
                    //}
                }
            }
            return ThaoTacResult;
        }

        public List<SelectListItem> DSThaoTacByChucNang(int id)
        {

            var ThaoTacResult = (from t in this.context.DM_THAOTAC
                                            where t.DM_CHUCNANG_ID==id
                                               select new SelectListItem()
                                               {
                                                  Text=t.TEN_THAOTAC,
                                                  Value=t.DM_THAOTAC_ID.ToString()
                                               }).ToList();
            
            return ThaoTacResult;
        }
    }
}
