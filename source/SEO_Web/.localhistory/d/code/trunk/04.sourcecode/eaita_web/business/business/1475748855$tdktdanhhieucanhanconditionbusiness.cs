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
using Model.eAita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.CommonBusiness;

namespace Business.Business
{
    public class TdktDanhHieuCaNhanConditionBusiness : GenericBussiness<TDKT_DANH_HIEU_CA_NHAN_CONDITION>
    {
        public TdktDanhHieuCaNhanConditionBusiness(Entities context = null)
            : base()
        {
            repository = new TdktDanhHieuCaNhanConditionRepository(context);
        }
        public void Save(TDKT_DANH_HIEU_CA_NHAN_CONDITION danhhieu)
        {
            try
            {
                if (danhhieu.ID == 0)
                {
                    this.repository.Insert(danhhieu);
                }
                else
                    this.repository.Update(danhhieu);
                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TDKTDanhHieuCaNhanBO> getConditionDanhHieuCaNhan(int DANH_HIEU_ID){
            var result = from dh_cond in this.context.TDKT_DANH_HIEU_CA_NHAN_CONDITION
                         join dhcn in this.context.TDKT_DANHHIEUCANHAN
                         on dh_cond.COND_DANH_HIEU_ID equals dhcn.ID
                         select new TDKTDanhHieuCaNhanBO{
                             ID = dh_cond.ID,
                             DANH_HIEU_ID = dh_cond.DANH_HIEU_ID,
                             TEN_DANH_HIEU_COND = dhcn.DANHHIEUTHIDUA,
                             COND_DANH_HIEU_ID = dh_cond.COND_DANH_HIEU_ID,
                             COND_SO_LUONG =dh_cond.COND_SO_LUONG,
                             DIEUKIEN_ID = dh_cond.DIEUKIEN_ID
                         };
                         return result.Where(x => x.DANH_HIEU_ID == DANH_HIEU_ID).ToList();
        }
        public List<TDKTDanhHieuCaNhanBO> getLstConditionDanhHieuCaNhan(int DANH_HIEU_ID)
        {
            var result = from dh_cond in this.context.TDKT_DANH_HIEU_CA_NHAN_CONDITION
                         join dhcn in this.context.TDKT_DANHHIEUCANHAN
                         on dh_cond.COND_DANH_HIEU_ID equals dhcn.ID
                         select new TDKTDanhHieuCaNhanBO
                         {
                             ID = dh_cond.ID,
                             DANH_HIEU_ID = dh_cond.DANH_HIEU_ID,
                             TEN_DANH_HIEU_COND = dhcn.DANHHIEUTHIDUA,
                             COND_DANH_HIEU_ID = dh_cond.COND_DANH_HIEU_ID,
                             COND_SO_LUONG = dh_cond.COND_SO_LUONG,
                             DIEUKIEN_ID = dh_cond.DIEUKIEN_ID
                         };
            return result.Where(x => x.DANH_HIEU_ID == DANH_HIEU_ID).ToList();
        }
    }
}
