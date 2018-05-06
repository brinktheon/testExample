using System;
using System.Data.SqlClient;

namespace Model
{
    class Car : IIntegerKey
    {
        public string Model { get; set; } = "None";

        public CarType Type { get; set; }

        public int Id { get; set; }

        public Car()
        {
            this.Type = CarType.Car;
        }

        public Car(int Id, string Model)
        {
            this.Model = Model;
            this.Id = Id;
        }

        public virtual string GetInfo()
        {
            return String.Format("Модель авто: {0}\n", this.Model);
        }
    }
}
