using System;
using System.Data.SqlClient;

namespace Model
{
    class Car : IIntegerKey
    {
        public string Model { get; set; }

        public string TypeName { get; set; }

        public int Id { get; set; }

        public int Seating { get; set; }

        public int MaxSpeed { get; set; }

        public int LiftingWeight { get; set; }

        public int MaxWeight { get; set; }

        public Car()
        {

        }
    }
}
