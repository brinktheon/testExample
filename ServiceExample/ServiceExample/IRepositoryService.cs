using CommonLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceExample
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IRepositoryService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IRepositoryService
    {
        [OperationContract]
        void Save(Car obj);

        [OperationContract]
        void Remove(Car obj);

        [OperationContract]
        void Update(Car obj);

        [OperationContract]
        Car GetByKey(int id);

        [OperationContract]
        Car StartCarEdit(int id);

        [OperationContract]
        Car SetCarValue(string name, object value);

        [OperationContract]
        Car CancelEdit(int id);
    }
}
