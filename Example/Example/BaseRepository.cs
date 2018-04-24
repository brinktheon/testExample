using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Example
{
    abstract class BaseRepository<T>
    {
        protected SqlConnection connection = null;
        protected SqlCommand cmd;
        protected SqlDataReader reader;
        protected string stringConnection;

        public BaseRepository(string stringConnection)
        {
            this.stringConnection = stringConnection;
        }

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

        public T Serialize(SqlDataReader reader)
        {          
            Type t = Type.GetType($"{typeof(T).Namespace}.{reader["CarType"].ToString()}");
            T localObj = (T)Activator.CreateInstance(t);
          
            var columsnName = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();                
            var props = t.GetProperties();               
                for (int i = 0; i < props.Length; i++)
                {
                    for (int j = 0; j < columsnName.Count; j++)
                    {
                        if (props[i].Name.Equals(columsnName[j]))
                        {
                            var propertyInfo = localObj.GetType().GetProperty(props[i].Name);
                            propertyInfo.SetValue(localObj, Convert.ChangeType(reader[columsnName[j]], propertyInfo.PropertyType), null);
                        }
                    }
                }   
            return localObj;
        }
    }
}
