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
//using Oracle.DataAccess.Client;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
//using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Repository
{
    public class GenericRepository<T> where T : class
    {
        internal Entities Context;
        internal DbSet<T> DbSet;

        public GenericRepository()
        {
            Context = new Entities(GetProfiledConnection());
            DbSet = Context.Set<T>();
        }

        private static DbConnection GetProfiledConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["name"].ConnectionString;
            //var connection = new OracleConnection(connectionString);
            var connection = new SqlConnection(connectionString);
            return new EFProfiledDbConnection(connection, MiniProfiler.Current);
        }

        public GenericRepository(Entities context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public B GetSeqVal<B>(int NumOfVal = 1) {
            var type = typeof(T);
            string Name = type.Name;

            if (NumOfVal == 1)
            {
                return Context.Database.SqlQuery<B>("SELECT " + Name + "_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
            }

            Context.Database.ExecuteSqlCommand("alter sequence " + Name + " _SEQ increment by " + NumOfVal.ToString());
            B res = Context.Database.SqlQuery<B>("SELECT " + Name + "_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
            Context.Database.ExecuteSqlCommand("alter sequence " + Name + " _SEQ increment by 1");

            return res;
        }

        public IQueryable<T> All
        {
            get { return DbSet; }
        }

        public IQueryable<T> AllNoTracking
        {
            get { return DbSet.AsNoTracking(); }
        }

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Find(object id)
        {
            return DbSet.Find(id);
        }

        public void Insert(T entityToInsert)
        {
            DbSet.Add(entityToInsert);
        }

        public void Update(T entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
        }

        public void ExecuteSQL(string Sql)
        {
            ((IObjectContextAdapter)Context).ObjectContext.ExecuteStoreCommand(Sql);
        }

        public void Delete(T entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == System.Data.Entity.EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }


        public void Delete(object id)
        {
            T entityToDelete = DbSet.Find(id);
            if (entityToDelete != null)
            {
                this.Delete(entityToDelete);
            }
        }

        public void DeleteBy(Expression<Func<T, bool>> expression, params object[] parameters)
        {
            var query = this.All.Where(expression);
            string sqlSelect = query.ToString();
            int ind = sqlSelect.ToLower().IndexOf("from ");
            string sqlDelete = "delete " + sqlSelect.Substring(ind);
            ((IObjectContextAdapter)Context).ObjectContext.ExecuteStoreCommand(sqlDelete, parameters);
        }

        public void DeleteAll(List<T> entities)
        {
            foreach (T entity in entities)
            {
                this.Delete(entity);
            }
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                string error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        error += string.Format("{0}:{1}; ", ve.PropertyName, ve.ErrorMessage);
                    }

                }
                throw new Exception(error);
            }
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

        //private void DirectPushToDatabase(string Query, List<OracleDbType> ListType, List<List<object>> ListData, OracleConnection Conn)
        //{
        //    int Count = ListData[0].Count();
        //    if (Count > 0)
        //    {
        //        DateTime t = DateTime.Now;
        //        OracleCommand cmd = new OracleCommand(Query, Conn);
        //        cmd.CommandTimeout = 0;
        //        for (int i = 0; i < ListType.Count; i++)
        //        {
        //            OracleParameter param = new OracleParameter();
        //            param.OracleDbType = ListType[i];
        //            param.Value = ListData[i].ToArray();
        //            cmd.Parameters.Add(param);
        //        }
        //        cmd.ArrayBindCount = Count;
        //        cmd.ExecuteNonQuery();
        //        TimeSpan dt = DateTime.Now.Subtract(t);
        //    }
        //}
    }
}