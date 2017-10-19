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
    public class EntitiesRepository : GenericRepository<Entities>
    {
        public EntitiesRepository() : base()
        {
        }
        public EntitiesRepository(Entities context)
            : base(context)
        {
        }
    }
}