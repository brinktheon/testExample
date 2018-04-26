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
            throw new NotImplementedException();
        }
    }
}
