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
    public class TDKTCaNhanDangKyRepository : GenericRepository<TDKT_CANHANDANGKY>
    {
        public TDKTCaNhanDangKyRepository() : base()
        {
        }
        public TDKTCaNhanDangKyRepository(Entities context)
            : base(context)
        {
        }
    }
}