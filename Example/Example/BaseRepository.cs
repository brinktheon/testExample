using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Example
{
    abstract class BaseRepository<T> where T : new()
    {
        protected SqlConnection connection = null;
        protected SqlCommand cmd;
        protected SqlDataReader reader;
        protected string stringConnection;

        public BaseRepository(string stringConnection)
        {
            this.stringConnection = stringConnection;
        }

        public BaseRepository() { }

        public void OpenConnection(string stringConnection)
        {
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = stringConnection;
                connection.Open();

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public virtual List<T> Load(string sql)
        {
            OpenConnection(stringConnection);

            var localListObjects = new List<T>();
            try
            {
                cmd = new SqlCommand(sql, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    localListObjects.Add(Serialize(reader));
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
            return localListObjects;
        }

        public virtual T Serialize(SqlDataReader reader)
        {
            var results = default(T);

            var item = TypeOfEntity(reader);
            foreach (var property in item.GetType().GetProperties())
            {
                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                {
                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    if (convertTo.IsEnum)
                    {
                        property.SetValue(item, (Enum.Parse(convertTo, reader[property.Name].ToString())), null);
                    }
                    else
                        property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                }
            }
            results = item;

            return results;
        }

        public virtual T TypeOfEntity(SqlDataReader reader)
        {
            return new T();
        }
    }
}
