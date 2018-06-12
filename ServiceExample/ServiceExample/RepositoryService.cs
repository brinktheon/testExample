using CommonLibrary.Model;
using CommonLibrary.NHibernate;
using System;
using System.Data.SqlClient;

namespace ServiceExample
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "RepositoryService" в коде и файле конфигурации.
    public class RepositoryService : IRepositoryService
    {
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
    }
}
