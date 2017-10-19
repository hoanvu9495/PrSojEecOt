/**
 * The HiNet License
 *
 * Copyright 2015 Hinet JSC. All rights reserved.
 * Use is subject to license terms.
 */

/** 
* @author  
* @version $Revision: $
*/

using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class HuyenRepository : GenericRepository<HUYEN>
    {
        public HuyenRepository() : base()
        {
        }
        public HuyenRepository(Entities context)
            : base(context)
        {
        }
    }
}