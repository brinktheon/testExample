using System;
using System.Data.SqlClient;

namespace Example
{
    class SportCar : PassengerCar
    {

        public string Model { get; set; } = "None";

        public CarType Type { get; set; }

        public SportCar()
        {
            this.Type = CarType.SportCar;
        }

        public SportCar(int Id, int Weight, int Seating, string Model) : base(Id, Weight, Seating)
        {
            this.Model = Model;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Модель авто: {1}\n", base.GetInfo(), this.Model);
        }
    }
}