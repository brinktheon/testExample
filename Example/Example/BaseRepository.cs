using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public abstract T Serialize(SqlDataReader reader);
    }
}
