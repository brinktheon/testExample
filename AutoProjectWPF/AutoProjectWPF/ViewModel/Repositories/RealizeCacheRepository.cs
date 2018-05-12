using Model;
using System.Data.SqlClient;


namespace AutoProjectWPF.ViewModel.Repositories
{
    class RealizeCacheRepository : CachedRepositary<Car>
    {
        public RealizeCacheRepository(string stringConnection) : base(stringConnection)
        {

        }
   
        public override Car TypeOfEntity(SqlDataReader reader)
        {
            return new Car();
        }
    }
}