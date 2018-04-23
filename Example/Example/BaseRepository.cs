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
            T localObj = default(T);

            // Наследники типа T
            var subclassTypes = Assembly
           .GetAssembly(typeof(T))
           .GetTypes()
           .Where(t => t.IsSubclassOf(typeof(T)));

            // Количество полей из ридера не пустых (к примеру у Car нету заполненого поля Model)
            int countFieldReaderNotNull = 0;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetValue(i) != DBNull.Value)
                {
                    countFieldReaderNotNull++;
                }
            }
            // Если пришел тип Car, то возвращаем его проинициализированный
            Type t1 = typeof(T);
            var propsCount = t1.GetProperties().Count();
            var columsnName = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            // Проверяет совпадает ли количество полей в типе T с количеством не нулевых столбцов из ридера
            if (propsCount == countFieldReaderNotNull)
            {
                var props = t1.GetProperties();
                localObj = (T)Activator.CreateInstance(typeof(T));
                for (int i = 0; i < props.Length; i++)
                {
                    for (int j = 0; j < columsnName.Count; j++)
                    {
                        if (props[i].Name.Equals(columsnName[j]) && (props[i].GetSetMethod() != null))
                        {
                            var propertyInfo = localObj.GetType().GetProperty(props[i].Name);
                            propertyInfo.SetValue(localObj, Convert.ChangeType(reader[columsnName[j]], propertyInfo.PropertyType), null);
                        }
                    }
                }
            }
            // Если не верно, то в ридере унаследованный тип от T
            else
            {
                // Берет только те подтипы, у которых кол-во полей соответсвтуют не нулевым кол-во полям из ридера
                var typesOfSub = subclassTypes
                    .Where(element => element.GetProperties().Count() == countFieldReaderNotNull).Select(element => element);

                /* Берется список всех полей в readerNamse, в resultNames удаляются повторные имена. 
                 * Это для того что бы не учитывать поле CarType, т.к. оно инициализируется в конструкторе.
                 * Соответственно это поле не проверяется далее.
                */
                IList<string> readerNames = new List<string>();
                foreach (TypeInfo inf in typesOfSub)
                {
                    foreach (PropertyInfo prInfo in inf.GetProperties())
                    {
                        for (int j = 0; j < columsnName.Count; j++)
                        {
                            if (prInfo.Name.Equals(columsnName[j]) && reader[columsnName[j]] != DBNull.Value)
                            {
                                readerNames.Add(columsnName[j]);
                            }
                        }
                    }
                }
                var resultNames = readerNames.Select(el => el).Distinct().ToList();

                /*Поле isValide отвечает за то, что бы поле соотвствовало другому полю, к примеру id == id тогда true,
                 * елси false, то такого Property нету.
                 * Поле localCount подсчитывает кол-во соответсвией по полям. Это нужно для того, что-бы на выходе знать какой объект 
                 * инициализировать. К примеру есть тип SportCar и Tipper у них одинаковое кол-во полей, но в ридере поле Seat пустое
                 * значит поле localCount будет по кол-ву соответсвовать кол-ву полей у Tipper. 
                 * Его и создает.
                 * 
                 * 
                 * Весь код проверял, код работает.
                 */
                foreach (TypeInfo inf in typesOfSub)
                {
                    var isValide = false;
                    var localCount = 0;

                    foreach (PropertyInfo prInfo in inf.GetProperties())
                    {
                        for (int i = 0; i < resultNames.Count; i++)
                        {
                            if (resultNames[i].Equals(prInfo.Name) && reader[resultNames[i]] != DBNull.Value)
                            {
                                isValide = true;
                                localCount++;
                                break;
                            }
                        }
                        if (!isValide)
                        {
                            break;
                        }
                    }
                    if (isValide && (localCount == resultNames.Count))
                    {
                        localObj = (T)Activator.CreateInstance(inf);
                    }
                }
                var props = localObj.GetType().GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    for (int j = 0; j < resultNames.Count; j++)
                    {
                        if (props[i].Name.Equals(resultNames[j]) && (props[i].GetSetMethod() != null))
                        {
                            var propertyInfo = localObj.GetType().GetProperty(props[i].Name);
                            propertyInfo.SetValue(localObj, Convert.ChangeType(reader[resultNames[j]], propertyInfo.PropertyType), null);
                        }
                    }
                }

            }

            return localObj;
        }
    }
}
