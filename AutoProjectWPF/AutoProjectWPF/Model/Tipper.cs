using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Tipper : TruckCar
    {
        public int MaxWeight { get; set; }

        public CarType Type { get; set; }

        public Tipper()
        {
            this.Type = CarType.Tipper;
        }

        public Tipper(int Id, string Model, int LiftingWeight, int MaxWeight) : base(Id, Model, LiftingWeight)
        {
            this.MaxWeight = MaxWeight;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Максимальный вес авто: {1}\n", base.GetInfo(), this.Model);
        }
    }
}
