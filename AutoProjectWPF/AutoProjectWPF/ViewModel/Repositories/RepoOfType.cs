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
            switch (reader["Type"])
            {
                case "Car":
                    local = CarType.Car;
                    break;
                case "PassengerCar":
                    local = CarType.PassengerCar;
                    break;
                case "TruckCar":
                    local = CarType.TruckCar;
                    break;
                case "SportCar":
                    local = CarType.SportCar;
                    break;
                case "Tipper":
                    local = CarType.Tipper;
                    break;
            }
            return local;
        }
    }
}
