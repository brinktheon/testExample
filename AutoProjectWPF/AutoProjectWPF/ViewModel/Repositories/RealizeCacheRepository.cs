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
            Car local = null;
            switch (reader["CarTypeId"])
            {
                case 1:
                    local = new Car();
                    break;
                case 2:
                    local = new PassengerCar();
                    break;
                case 3:
                    local = new TruckCar();
                    break;
                case 4:
                    local = new SportCar();
                    break;
                case 5:
                    local = new Tipper();
                    break;
            }
            return local;
        }
    }
}