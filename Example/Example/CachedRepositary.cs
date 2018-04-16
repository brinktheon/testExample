using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    abstract class  CachedRepositary<T> : BaseRepository<T> where T : IIntegerKey
    {
        protected static IDictionary<int, T> LocalCache = new Dictionary<int, T>();
        private string sql;

        public CachedRepositary(string stringConnection) : base(stringConnection) { }

        public abstract override T Serialize(SqlDataReader reader);

        public T LoadById(int id)
        {
            T loaclCar = default(T);
            sql = "SELECT * FROM AutoConfig WHERE AutoConfig.id = " + id + ";";

            if (!Find(id))
            {
                OpenConnection(stringConnection);
                try
                {
                    cmd = new SqlCommand(sql, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        loaclCar = Serialize(reader);
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    reader.Close();
                    CloseConnection();
                }
            }
            loaclCar = LocalCache[id];

            return loaclCar;
        }

        public List<T> LoadFromCacheByLinq()
        {
            var val = LocalCache.Select(value => value.Value);


            return val.ToList<T>();
        }

        public override List<T> Load(string sql)
        {
            return base.Load(sql);
        }

        private bool Find(int id)
        {
            IEnumerable<int> keys = LocalCache.Keys;

            foreach (int value in keys)
            {
                if (value == id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
