/**
 * The HiNet License
 *
 * Copyright 2012 Viettel Telecom. All rights reserved.
 * HINET PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

/** 
* @author  
* @version $Revision: $
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Reflection;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.ComponentModel;
using DAL.Repository;
using Model.DBTool;
using log4net;


namespace Business.Business
{
    public partial class GenericBussiness<T> where T : class
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(T));
        protected GenericRepository<T> repository;

        public Entities context;
        public string CurrentUsername { get; set; }

        public GenericBussiness(Entities context = null)
        {
            this.context = context == null ? new Entities() : context;
            this.context.Configuration.AutoDetectChangesEnabled = false;
            this.context.Configuration.ValidateOnSaveEnabled = false;
        }


        public virtual IQueryable<T> All
        {
            get { return repository.All; }
        }

        public virtual IQueryable<T> AllNoTracking
        {
            get { return repository.AllNoTracking; }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return repository.AllIncluding(includeProperties);
        }

        public virtual T Find(object id)
        {
            return repository.Find(id);
        }

        public virtual T Insert(T entityToInsert)
        {
            Type t = entityToInsert.GetType();
            PropertyInfo md = t.GetProperty("NGAYTAO");

            if (md != null)
            {
                DateTime? val = (DateTime?)md.GetValue(entityToInsert, null);
                if (val == null || val.Value.Year < 2000)
                {
                    md.SetValue(entityToInsert, DateTime.Now, null);
                }
            }

            PropertyInfo createdBy = t.GetProperty("NGUOITAO");
            if (createdBy != null && createdBy.GetValue(entityToInsert, null) == null)
            {
                createdBy.SetValue(entityToInsert, CurrentUsername, null);
            }

            repository.Insert(entityToInsert);
            return entityToInsert;
        }

        public virtual T Update(T entityToUpdate)
        {
            Type t = entityToUpdate.GetType();
            PropertyInfo md = t.GetProperty("NGAYSUA");
            if (md != null)
            {
                md.SetValue(entityToUpdate, DateTime.Now, null);
            }

            PropertyInfo editedBy = t.GetProperty("NGUOISUA");
            if (editedBy != null && editedBy.GetValue(entityToUpdate, null) == null)
            {
                editedBy.SetValue(entityToUpdate, CurrentUsername, null);
            }

            repository.Update(entityToUpdate);
            return entityToUpdate;
        }

        public virtual T BaseUpdate(T entityToUpdate)
        {
            Type t = entityToUpdate.GetType();
            PropertyInfo md = t.GetProperty("NGAYSUA");
            if (md != null)
            {
                md.SetValue(entityToUpdate, DateTime.Now, null);
            }

            PropertyInfo editedBy = t.GetProperty("NGUOISUA");
            if (editedBy != null && editedBy.GetValue(entityToUpdate, null) == null)
            {
                editedBy.SetValue(entityToUpdate, CurrentUsername, null);
            }

            repository.Update(entityToUpdate);
            return entityToUpdate;
        }

        public virtual T NativeUpdate(T entityToUpdate)
        {
            repository.Update(entityToUpdate);
            return entityToUpdate;
        }

        public virtual void ExecuteSQL(string Sql)
        {
            repository.ExecuteSQL(Sql);
        }

        public virtual void Delete(object id)
        {
            repository.Delete(id);

        }

        public virtual void DeleteBy(Expression<Func<T, bool>> expression, params object[] parameters)
        {
            repository.DeleteBy(expression, parameters);
        }

        public virtual void DeleteAll(List<T> entities)
        {
            repository.DeleteAll(entities);
        }

        public virtual void Save()
        {
            repository.Save();
        }

        public virtual void Dispose()
        {
            repository.Dispose();
        }

        public virtual void Delete(object id, bool HasIsActive)
        {
            if (HasIsActive)
            {
                T o = repository.Find(id);
                Type t = o.GetType();
                PropertyInfo ia = t.GetProperty("IS_ACTIVE");
                ia.SetValue(o, Convert.ToInt16(1), null);

                PropertyInfo md = t.GetProperty("LAST_MODIFIED_DATE");
                if (md != null)
                {
                    md.SetValue(o, DateTime.Now, null);
                }
                repository.Update(o);
            }
            else
            {
                repository.Delete(id);
            }
        }

        public virtual B GetBusiness<B>()
        {
            try
            {
                B res = (B)typeof(B).GetConstructor(new Type[] { typeof(Entities) }).Invoke(new object[] { this.context });
                PropertyInfo currentUser = res.GetType().GetProperty("CurrentUsername");
                if (currentUser != null && currentUser.GetValue(res, null) == null)
                {
                    currentUser.SetValue(res, CurrentUsername, null);
                }
                return res;
            }
            catch
            {
                return default(B);
            }
        }
        //public virtual void BulkInsert(List<T> ListData)
        //{
        //    repository.BulkInsert(ListData);
        //}

        public virtual B GetSeqVal<B>(int NumOfVal = 1)
        {
            return repository.GetSeqVal<B>(NumOfVal);
        }
    }
}