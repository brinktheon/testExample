using System;
using System.Data.SqlClient;

namespace Example
{
    class PassengerCar : Car
    {

        public int Seating { get; set; } = 2;

        public CarType Type { get; set; }

        public PassengerCar()
        {
            this.Type = CarType.PassengerTransport;
        }

        public PassengerCar(int Id, int Weight, int Seating) : base(Id, Weight)
        {
            this.Seating = Seating;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Места для сидения авто: {1}\n", base.GetInfo(), this.Seating);
        }
    }
}