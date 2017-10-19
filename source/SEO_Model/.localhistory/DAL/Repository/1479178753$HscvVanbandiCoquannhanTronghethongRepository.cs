/**
 * The HiNet License
 *
 * Copyright 2015 HiNet JSC. All rights reserved.
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
    public class HscvVanbandiCoquannhanTronghethongRepository : GenericRepository<HSCV_VANBANDI_COQUANNHAN_TRONGHETHONG>
    {
        public HscvVanbandiCoquannhanTronghethongRepository() : base()
        {
        }
        public HscvVanbandiCoquannhanTronghethongRepository(Entities context)
            : base(context)
        {
        }
    }
}