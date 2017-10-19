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
    public class TDKTPhongTraoThiDuaRepository : GenericRepository<TDKT_PHONGTRAO_THIDUA>
    {
        public TDKTPhongTraoThiDuaRepository() : base()
        {
        }
        public TDKTPhongTraoThiDuaRepository(Entities context)
            : base(context)
        {
        }
    }
}