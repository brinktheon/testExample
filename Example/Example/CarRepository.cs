using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class CarRepository : BaseRepository<Car>
    {
        public CarRepository(string stringConnection) : base(stringConnection)
        {
        }

        public override Car Serialize(SqlDataReader reader)
        {
            Car localCar = null;

            switch ((int)reader["EntityID"])
            {
                case 1:
                    localCar = new Car((int)reader["Weight"]);
                    break;
                case 2:
                    localCar = new PassengerCar((int)reader["Weight"], (int)reader["Seating"]);
                    break;
                case 3:
                    localCar = new TruckCar((int)reader["Weight"], (int)reader["LiftingWeight"]);
                    break;
                case 4:
                    localCar = new SportCar((int)reader["Weight"], (int)reader["Seating"], (string)reader["Model"]);
                    break;
                case 5:
                    localCar = new Tipper((int)reader["Weight"], (int)reader["LiftingWeight"], (string)reader["Model"]);
                    break;
            }

            return localCar;
        }
    }
}
