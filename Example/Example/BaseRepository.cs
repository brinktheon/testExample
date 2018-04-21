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
            T localCar = default(T);

            var subclassTypes = Assembly
           .GetAssembly(typeof(T))
           .GetTypes()
           .Where(t => t.IsSubclassOf(typeof(T)));

            switch ((int)reader["EntityID"])
            {
                case 1:
                    localCar = (T)typeof(T).GetConstructor(new[] { typeof(int), typeof(int) })
                                .Invoke(new object[] { (int)reader["id"], (int)reader["Weight"] });
                    break;
                case 2:
                    localCar = (T)subclassTypes.Where(q => q.Name.Equals("PassengerCar")).Single()
                                .GetConstructor(new[] { typeof(int), typeof(int), typeof(int) })
                                .Invoke(new object[] { (int)reader["id"], (int)reader["Weight"], (int)reader["Seating"] });
                    break;
                case 3:
                    localCar = (T)subclassTypes.Where(q => q.Name.Equals("TruckCar")).Single()
                                .GetConstructor(new[] { typeof(int), typeof(int), typeof(int) })
                                .Invoke(new object[] { (int)reader["id"], (int)reader["Weight"], (int)reader["LiftingWeight"] });
                    break;
                case 4:
                    localCar = (T)subclassTypes.Where(q => q.Name.Equals("SportCar")).Single()
                                .GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(string) })
                                .Invoke(new object[] { (int)reader["id"], (int)reader["Weight"], (int)reader["Seating"], (string)reader["Model"] });
                    break;
                case 5:
                    localCar = (T)subclassTypes.Where(q => q.Name.Equals("Tipper")).Single()
                                .GetConstructor(new[] { typeof(int), typeof(int), typeof(int), typeof(string) })
                                .Invoke(new object[] { (int)reader["id"], (int)reader["Weight"], (int)reader["Seating"], (string)reader["Model"] });
                    break;
            }

            return localCar;
        }
    }
}
