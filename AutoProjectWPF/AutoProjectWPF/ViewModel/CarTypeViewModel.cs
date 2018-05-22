using AutoProjectWPF.Model;
using Model;

namespace AutoProjectWPF.ViewModel
{
    class CarTypeViewModel : BaseViewModel
    {
        private CarTypeModel carTypeModel;

        public CarTypeViewModel(CarTypeModel carTypeModel)
        {
            this.carTypeModel = carTypeModel;
        }

        public int Id
        {
            get { return carTypeModel.Id; }
            set
            {
                carTypeModel.Id = value;
                OnPropertyChange();
            }
        }

        public string TypeName
        {
            get { return carTypeModel.TypeName; }
            set
            {
                carTypeModel.TypeName = value;
                OnPropertyChange();
            }
        }

        public CarType Type
        {
            get { return carTypeModel.Type; }
            set
            {
                carTypeModel.Type = value;
                OnPropertyChange();
            }
        }
    }
}
