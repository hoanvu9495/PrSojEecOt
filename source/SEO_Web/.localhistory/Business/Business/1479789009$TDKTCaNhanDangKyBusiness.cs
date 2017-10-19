/**
 * The HiNet License
 *
 * Copyright 2015 Hinet JSC. All rights reserved.
 * HINET PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

/** 
* @author  NAMDV
*/

using Business.CommonBusiness;
using DAL.Repository;
using Model.eAita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class TDKTCaNhanDangKyBusiness : GenericBussiness<TDKT_CANHANDANGKY>
    {
        public TDKTCaNhanDangKyBusiness(Entities context = null)
            : base()
        {
            repository = new TDKTCaNhanDangKyRepository(context);
        }
        public void Save(TDKT_CANHANDANGKY dondangky)
        {
            try
            {
                if (dondangky.ID == 0)
                {
                    this.repository.Insert(dondangky);
                }
                else
                    this.repository.Update(dondangky);
                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<int> getDanhHieuIdsDaDangKy(long user_id, int year)
        {

        }
    }
}
