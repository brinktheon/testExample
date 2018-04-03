using System;
using System.Data.SqlClient;

namespace Example
{
    class Car
    {
        public int Weight { get; set; } = 10;

        public CarType Type { get; set; }

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
