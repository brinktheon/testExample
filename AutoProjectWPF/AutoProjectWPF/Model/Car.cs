using System;
using System.Data.SqlClient;

namespace Model
{
    class Car : IIntegerKey
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
            return obj is Car car &&
                   Id == car.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
