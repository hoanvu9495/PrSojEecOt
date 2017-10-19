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
    public class VaitroChucnangRepository : GenericRepository<VAITRO_CHUCNANG>
    {
        public VaitroChucnangRepository() : base()
        {
        }
        public VaitroChucnangRepository(Entities context)
            : base(context)
        {
        }
    }
}