using System;
using System.Data.SqlClient;

namespace Model
{
    class SportCar : PassengerCar
    {

        public int MaxSpeed { get; set; }

        public CarType Type { get; set; }

        public SportCar()
        {
            this.Type = CarType.SportCar;
        }

        public SportCar(int Id, string Model, int Seating, int MaxSpeed) : base(Id, Model, Seating)
        {
            this.MaxSpeed = MaxSpeed;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Максимальная скорость авто: {1}\n", base.GetInfo(), this.Model);
        }
    }
}