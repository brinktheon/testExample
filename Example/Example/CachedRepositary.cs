using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    abstract class CachedRepositary<T> : BaseRepository<T> where T : IIntegerKey
    {
        protected static IDictionary<int, T> LocalCache = new Dictionary<int, T>();
        private string sql;

        public CachedRepositary(string stringConnection) : base(stringConnection) { }        

        public T LoadById(int id)
        {
            sql = "SELECT * FROM AutoConfig WHERE AutoConfig.id = " + id + ";";

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
