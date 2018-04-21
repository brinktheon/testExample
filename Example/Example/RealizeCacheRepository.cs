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
    }
}