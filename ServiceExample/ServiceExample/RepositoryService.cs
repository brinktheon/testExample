using CommonLibrary.Model;
using CommonLibrary.NHibernate;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.ServiceModel;

namespace ServiceExample
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "RepositoryService" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class RepositoryService : IRepositoryService
    {
        // Объект сессии
        private Car localSessionCar;

        public void Save(Car obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    session.Save(obj);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Update(Car obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(obj);
                        transaction.Commit();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Remove(Car obj)
        {
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        if (!session.Contains(obj))
                        {
                            session.Delete(obj);
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Car GetByKey(int id)
        {
            Car localObject;
            using (var session = NHibernateHelper.OpenSession())
            {
                localObject = session.Get<Car>(id);
            }
            return localObject;
        }

        public Car StartCarEdit(int id)
        {
            localSessionCar = GetByKey(id);
            Console.WriteLine("StartCarEdit: "  + localSessionCar.Id);
            return localSessionCar;
        }

        public Car SetCarValue(string name, object value)
        {
            PropertyInfo info = localSessionCar.GetType().GetProperty(name);
            info.SetValue(localSessionCar, Convert.ChangeType(value, info.PropertyType), null);

            return localSessionCar;
        }

        public Car CancelEdit(int id)
        {
            Console.WriteLine("CancelEdit: " + id);
            return GetByKey(id);
        }
    }
}
