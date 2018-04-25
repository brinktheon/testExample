using System;
using System.Data.SqlClient;

namespace Example
{
    class Car : IIntegerKey
    {
        public int Weight { get; set; } = 10;

        public CarType Type { get; set; }

        //Добавил set; для считывания из бд Id
        public int Id { get; set; }

        public Car()
        {
            this.Type = CarType.Car;
        }

        public Car(int Id, int Weight)
        {
            this.Weight = Weight;
            this.Id = Id;
        }

        public virtual string GetInfo()
        {
            return String.Format("Вес авто: {0}\n", this.Weight);
        }
    }
}
