using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoProjectWPF.ViewModel
{
    class CarViewModel : BaseViewModel
    {
        private Car car;

        public CarViewModel(Car car)
        {
            this.car = car;
        }

        public bool PassengerCarVisibility => Type == CarType.PassengerCar || Type == CarType.SportCar;

        public bool TruckCarCarVisibility => Type == CarType.TruckCar || Type == CarType.Tipper;

        public bool SportCarCarVisibility => Type == CarType.SportCar;

        public bool TipperrCarVisibility => Type == CarType.Tipper;

        public string Model
        {
            get { return car.Model; }
            set
            {
                car.Model = value;
                OnPropertyChange();
            }
        }

        public CarType Type
        {
            get { return car.Type; }
            set
            {
                car.Type = value;
                OnPropertyChange();
            }
        }

        public int Id
        {
            get { return car.Id; }
            set
            {
                car.Id = value;
                OnPropertyChange();
            }
        }

        public int Seating
        {
            get { return car.Seating; }
            set
            {
                car.Seating = value;
                OnPropertyChange();
            }
        }

        public int MaxSpeed
        {
            get { return car.MaxSpeed; }
            set
            {
                car.MaxSpeed = value;
                OnPropertyChange();
            }
        }

        public int LiftingWeight
        {
            get { return car.LiftingWeight; }
            set
            {
                car.LiftingWeight = value;
                OnPropertyChange();
            }
        }

        public int MaxWeight
        {
            get { return car.MaxWeight; }
            set
            {
                car.MaxWeight = value;
                OnPropertyChange();
            }
        }
    }
}
