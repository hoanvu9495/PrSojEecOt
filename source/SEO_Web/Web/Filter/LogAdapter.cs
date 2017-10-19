using Business.Business;
using Model.eAita;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Web.Filter
{
    public static class LogAdapter
    {
        public static List<ACTION_AUDIT> ListToInsert = new List<ACTION_AUDIT>();

        private static int GetMaxBulk()
        {
            return Int32.Parse(ConfigurationManager.AppSettings["MaxBulkLog"]);
        }

        public static void Insert(ACTION_AUDIT ActionAudit)
        {
            Entities context = new Entities();
            ActionAuditBusiness aab = new ActionAuditBusiness(context);
            aab.Insert(ActionAudit);
            aab.Save();
            /*
            ListToInsert.Add(ActionAudit);
            if (ListToInsert.Count >= GetMaxBulk())
            {
                Entities context = new Entities();
                ActionAuditBusiness aab = new ActionAuditBusiness(context);
                aab.BulkInsert(ListToInsert);
                ListToInsert.Clear();
            }
            */
        }
    }
}