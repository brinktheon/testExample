using System;
using System.Data.SqlClient;

namespace Model
{
    class TruckCar : Car
    {
        public int LiftingWeight { get; set; }

        public CarType Type { get; set; }

        public TruckCar()
        {
            this.Type = CarType.TruckCar;
        }

        public TruckCar(int Id, string Model, int LiftingWeight) : base(Id, Model)
        {
            this.LiftingWeight = LiftingWeight;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Подъемный вес авто: {1}\n", base.GetInfo(), this.LiftingWeight);
        }
    }
}
