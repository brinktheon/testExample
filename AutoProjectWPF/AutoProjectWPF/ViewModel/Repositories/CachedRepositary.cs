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
                loaclObject = base.Load().Where(item => item.Id == id).FirstOrDefault();
            }
            return loaclObject;
        }

        public IList<T> LoadFromCacheByLinq(Func<T, bool> predicate)
        {
            return LocalCache.Values.Where(predicate).ToList();
        }

        public override List<T> Load()
        {
            foreach (T value in base.Load())
            {
                LocalCache[value.Id] = value;
            }
            return base.Load();
        }
    }
}
