using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AutoProjectWPF.ViewModel.Repositories
{
    class CachedRepositary<T> : BaseRepository<T> where T : IIntegerKey, new()
    {
        protected static IDictionary<int, T> LocalCache = new Dictionary<int, T>();

        public CachedRepositary() { }

        public T LoadById(int id)
        {
            if (!LocalCache.TryGetValue(id, out T loaclObject))
            {
                loaclObject = session.Get<T>(id);
            }
            return loaclObject;
        }

        public T GetByLinq(Func<T, bool> predicate)
        {
            T localObject = session.Query<T>().FirstOrDefault(predicate);
            // Если объекта нет в кэше добавляю его
            if (!LocalCache.ContainsKey(localObject.Id))
            {
                LocalCache[localObject.Id] = localObject;
            }

            return localObject;
        }

        public IList<T> GetByQuery(string query)
        {
            IList<T> localList = session.CreateSQLQuery(query).AddEntity(typeof(T)).List<T>();
            foreach (T value in localList)
            {
                // Если объекта нет в кэше добавляю его
                if (!LocalCache.ContainsKey(value.Id))
                {
                    LocalCache[value.Id] = value;
                }
            }
            return localList;
        }

        public IList<T> LoadFromCacheByLinq(Func<T, bool> predicate)
        {
            return LocalCache.Values.Where(predicate).ToList();
        }

        // Немного изменил, а то два раза выгрузка данных получается.
        public override List<T> Load()
        {
            List<T> localList = base.Load();
            foreach (T value in localList)
            {
                LocalCache[value.Id] = value;
            }
            return localList;
        }
    }
}
