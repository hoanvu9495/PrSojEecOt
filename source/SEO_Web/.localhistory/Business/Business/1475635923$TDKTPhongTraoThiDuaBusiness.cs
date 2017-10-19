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

namespace Business.Business
{
    public class TDKTPhongTraoThiDuaBusiness : GenericBussiness<ACTION_AUDIT>
    {
        public ActionAuditBusiness(Entities context = null)
            : base()
        {
            repository = new ActionAuditRepository(context);
        }
    }
}
