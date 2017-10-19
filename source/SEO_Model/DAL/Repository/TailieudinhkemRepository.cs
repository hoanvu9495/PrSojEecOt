/**
 * The HiNet License
 *
 * Copyright 2015 HiNet JSC. All rights reserved.
 * Use is subject to license terms.
 */

/** 
* @author  LUANLT
*/

using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class TailieudinhkemRepository : GenericRepository<TAILIEUDINHKEM>
    {
        public TailieudinhkemRepository() : base()
        {
        }
        public TailieudinhkemRepository(Entities context)
            : base(context)
        {
        }
    }
}