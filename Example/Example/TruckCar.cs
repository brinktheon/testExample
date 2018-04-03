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
            this.Type =  CarType.FreightTransport;
        }

        public TruckCar(int Weight, int LiftingWeight) : base(Weight)
        {
            this.LiftingWeight = LiftingWeight;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Подъемный вес авто: {1}\n", base.GetInfo(), this.LiftingWeight);
        }
    }
}
