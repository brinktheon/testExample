using System;
using System.Data.SqlClient;

namespace Model
{
    class PassengerCar : Car
    {

        public int Seating { get; set; } = 2;

        public CarType Type { get; set; }

        public PassengerCar()
        {
            this.Type = CarType.PassengerCar;
        }

        public PassengerCar(int Id, string Model, int Seating) : base(Id, Model)
        {
            this.Seating = Seating;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Места для сидения авто: {1}\n", base.GetInfo(), this.Seating);
        }
    }
}