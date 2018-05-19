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
        private string sql;

        public CachedRepositary(string stringConnection) : base(stringConnection) { }

        public T LoadById(int id)
        {
            sql = "select * from AutoConfig ac inner join AutoType at on ac.CarTypeId = at.id and  ac.Id = " + id + "; ";

            if (!LocalCache.TryGetValue(id, out T loaclCar))
            {
                loaclCar = base.Load(sql)[0];
            }
            return loaclCar;
        }

        public IList<T> LoadFromCacheByLinq(Func<T, bool> predicate)
        {
            return LocalCache.Values.Where(predicate).ToList();
        }

        public override List<T> Load(string sql)
        {
            foreach (T value in base.Load(sql))
            {
                LocalCache[value.Id] = value;
            }
            return base.Load(sql);
        }
    }
}
