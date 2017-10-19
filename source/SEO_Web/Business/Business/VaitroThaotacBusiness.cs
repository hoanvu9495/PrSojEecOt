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

namespace Business.Business
{
    public class VaitroThaotacBusiness : GenericBussiness<VAITRO_THAOTAC>
    {
        public VaitroThaotacBusiness(Entities context = null)
            : base()
        {
            repository = new VaitroThaotacRepository(context);
        }

        public void Save(VAITRO_THAOTAC vaitro_thaotac)
        {
            try
            {
                if (vaitro_thaotac.VAITRO_THAOTAC_ID == 0)
                {
                    this.repository.Insert(vaitro_thaotac);
                }
                else
                    this.repository.Update(vaitro_thaotac);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public bool IsExist(int VAITRO_CHUCNANG_ID, long thaotacID,int coso)
        {
            var obj = this.context.VAITRO_THAOTAC.Where(x => x.VAITRO_CHUCNANG_ID == VAITRO_CHUCNANG_ID && x.DM_THAOTAC_ID == thaotacID && x.COSO_ID == coso).FirstOrDefault();
            if (obj!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
