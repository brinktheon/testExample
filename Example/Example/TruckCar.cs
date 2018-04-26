using System;
using System.Data.SqlClient;

namespace Example
{
    class TruckCar : Car
    {
        public int LiftingWeight { get; set; }

        public CarType Type { get; set; }

        public TruckCar()
        {
            this.Type = CarType.TruckCar;
        }

        public TruckCar(int Id, int Weight, int LiftingWeight) : base(Id, Weight)
        {
            this.LiftingWeight = LiftingWeight;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Подъемный вес авто: {1}\n", base.GetInfo(), this.LiftingWeight);
        }
    }
}
