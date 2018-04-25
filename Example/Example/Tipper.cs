using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Tipper : TruckCar
    {
        public string Model { get; set; } = "None";

        public CarType Type { get; set; }

        public Tipper()
        {
            this.Type = CarType.Tipper;
        }

        public Tipper(int Id, int Weight, int LiftingWeight, string Model) : base(Id, Weight, LiftingWeight)
        {
            this.Model = Model;
        }

        public override string GetInfo()
        {
            return String.Format("{0}Модель авто: {1}\n", base.GetInfo(), this.Model);
        }
    }
}
