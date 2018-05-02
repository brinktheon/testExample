using Model;
using System.Data.SqlClient;


namespace AutoProjectWPF.ViewModel.Repositories
{
    class RepoOfType : BaseRepository<CarType>
    {
        private CarType local;

        public RepoOfType(string stringConnection) : base(stringConnection)
        {

        }

        public override CarType TypeOfEntity(SqlDataReader reader)
        {
            switch (reader["CarTypeId"])
            {
                case 1:
                    local = CarType.Car;
                    break;
                case 2:
                    local = CarType.PassengerCar;
                    break;
                case 3:
                    local = CarType.TruckCar;
                    break;
                case 4:
                    local = CarType.SportCar;
                    break;
                case 5:
                    local = CarType.Tipper;
                    break;
            }
            return local;
        }
    }
}
