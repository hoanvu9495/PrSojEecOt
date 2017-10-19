/**
 * The HiNet License
 *
 * Copyright 2015 Viettel Telecom. All rights reserved.
 * HINET PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

/** 
* @author  NAMDV
*/

using DAL.Repository;
using Model.DBTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class EntitiesBusiness : GenericBussiness<Entities>
    {
        public EntitiesBusiness(Entities context = null)
            : base()
        {
            repository = new EntitiesRepository(context);
        }
    }
}
