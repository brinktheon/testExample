using System;
using System.Data.SqlClient;

namespace CommonLibrary.Model
{
    public class Car : IIntegerKey
    {
        public virtual string Model { get; set; }

        public virtual CarType Type { get; set; }

        public virtual int Id { get; set; }

        public virtual int Seating { get; set; }

        public virtual int MaxSpeed { get; set; }

        public virtual int LiftingWeight { get; set; }

        public virtual int MaxWeight { get; set; }

        public override bool Equals(object obj)
        {
            var car = obj as Car;
            return car != null &&
                   Model == car.Model &&
                   Type == car.Type &&
                   Id == car.Id &&
                   Seating == car.Seating &&
                   MaxSpeed == car.MaxSpeed &&
                   LiftingWeight == car.LiftingWeight &&
                   MaxWeight == car.MaxWeight;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
