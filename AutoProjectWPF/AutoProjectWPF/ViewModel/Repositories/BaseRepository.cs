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
        public BaseRepository() { }

        /// <summary>
        /// Возвращает список объектов по из БД 
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> Load()
        {
            var localListObjects = new List<T>();
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    localListObjects = session.Query<T>().ToList();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return localListObjects;
        }

        /// <summary>
        /// Возращает объект по ключу(Id)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetByKey(int id)
        {
            T localObject;
            using (var session = NHibernateHelper.OpenSession())
            {
                localObject = session.Get<T>(id);
            }
            return localObject;
        }

        /// <summary>
        /// Возвращает списко объектов по Linq запросу
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IList<T> GetByLinq(Func<T, bool> predicate)
        {
            var localListObjects = new List<T>();
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    localListObjects = session.Query<T>().Where(predicate).ToList();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return localListObjects;
        }

        /// <summary>
        /// Возвращает сиско объектов по sql запрсоу
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<T> GetByQuery(string query)
        {
            IList<T> localListObjects = new List<T>();
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    localListObjects = session.CreateSQLQuery(query).AddEntity(typeof(T)).List<T>();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return localListObjects;
        }

        /// <summary>
        /// Сохраняет объект в БД
        /// </summary>
        /// <param name="obj"></param>
        public void Save(T obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    session.Save(obj);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Обновляет объект в БД
        /// </summary>
        /// <param name="obj"></param>
        public void Update(T obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(obj);
                        transaction.Commit();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Удаляет объект из БД
        /// </summary>
        /// <param name="obj"></param>
        public void Remove(T obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        if (!session.Contains(obj))
                        {
                            session.Delete(obj);
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
