/**
 * The HiNet License
 *
 * Copyright HiNet. All rights reserved.
 * Use is subject to license terms.
 */

/** 
* @author  NAMDV
*/

using Model.eAita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ActionAuditRepository : GenericRepository<ACTION_AUDIT>
    {
        public ActionAuditRepository() : base()
        {
        }
        public ActionAuditRepository(Entities context)
            : base(context)
        {
        }
    }
}