using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Example
{
    class RealizeCacheRepository : CachedRepositary<Car>
    {
        public RealizeCacheRepository(string stringConnection) : base(stringConnection)
        {

        }

        public override Car Serialize(SqlDataReader reader, Type type)
        {
            Car localCar = base.Serialize(reader, type);

            switch ((int)localCar.Type)
            {
                case 1:
                    localCar = base.Serialize(reader, typeof(Car));
                    break;
                case 2:
                    localCar = base.Serialize(reader, typeof(PassengerCar));
                    break;
                case 3:
                    localCar = base.Serialize(reader, typeof(TruckCar));
                    break;
                case 4:
                    localCar = base.Serialize(reader, typeof(SportCar));
                    break;
                case 5:
                    localCar = base.Serialize(reader, typeof(Tipper));
                    break;
            }

            return localCar;
        }
    }
}