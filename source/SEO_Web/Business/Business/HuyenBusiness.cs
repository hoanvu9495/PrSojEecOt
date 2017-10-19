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
    public class HuyenBusiness : GenericBussiness<HUYEN>
    {
        public HuyenBusiness(Entities context = null)
            : base()
        {
            repository = new HuyenRepository(context);
        }

        public void Save(HUYEN HUYEN)
        {
            try
            {
                if (HUYEN.HUYEN_ID == 0)
                {
                    this.repository.Insert(HUYEN);
                }
                else
                    this.repository.Update(HUYEN);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(string.Format("UserService.Save: {0}", ex.Message));
                throw new Exception(ex.Message);
            }
        }
    }
}
