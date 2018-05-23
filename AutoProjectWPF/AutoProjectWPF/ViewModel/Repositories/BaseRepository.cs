using AutoProjectWPF.NHibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AutoProjectWPF.ViewModel.Repositories
{
    class BaseRepository<T> where T : new()
    {
        protected ISession session;
        protected ITransaction transaction;

        public BaseRepository()
        {
            session = NHibernateHelper.OpenSession();
        }

        public virtual List<T> Load()
        {
            var localListObjects = new List<T>();
            try
            {
                localListObjects = session.Query<T>().ToList();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return localListObjects;
        }

        public void Save(T obj)
        {
            transaction = session.BeginTransaction();
            session.Save(obj);
            transaction.Commit();
        }
    }
}
