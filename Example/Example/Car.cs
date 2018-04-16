using System;
using System.Data.SqlClient;

namespace Example
{
    class Car : IIntegerKey
    {
        public int Weight { get; set; } = 10;

        public CarType Type { get; set; }

        public int Id { get; }

        public Car()
        {
            this.Type = CarType.Auto;
        }

        public Car(int Weight)
        {
            this.Weight = Weight;
        }

        public virtual string GetInfo()
        {
            return String.Format("Вес авто: {0}\n", this.Weight);
        }
    }
}
