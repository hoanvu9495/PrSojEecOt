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
    public class TinhBusiness : GenericBussiness<TINH>
    {
        public TinhBusiness(Entities context = null)
            : base()
        {
            repository = new TinhRepository(context);
        }

        public void Save(TINH tinh)
        {
            try
            {
                if (tinh.TINH_ID == 0)
                {
                    this.repository.Insert(tinh);
                }
                else
                    this.repository.Update(tinh);

                this.repository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<TINH> GetListTinhThanh()
        {
            return this.All.ToList();
        }
    }
}
