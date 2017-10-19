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
    public class XaBusiness : GenericBussiness<XA>
    {
        public XaBusiness(Entities context = null)
            : base()
        {
            repository = new XaRepository(context);
        }

        public void Save(XA XA)
        {
            try
            {
                if (XA.XA_ID == 0)
                {
                    this.repository.Insert(XA);
                }
                else
                    this.repository.Update(XA);

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
