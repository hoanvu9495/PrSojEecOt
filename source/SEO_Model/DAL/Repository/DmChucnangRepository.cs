/**
 * The HiNet License
 *
 * Copyright HiNet. All rights reserved.
 * Use is subject to license terms.
 */

/** 
* @author  NAMDV
*/

using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class DmChucnangRepository : GenericRepository<DM_CHUCNANG>
    {
        public DmChucnangRepository() : base()
        {
        }
        public DmChucnangRepository(Entities context)
            : base(context)
        {
        }
    }
}