using Model;

namespace AutoProjectWPF.ViewModel
{
    class CarTypeViewModel : BaseViewModel
    {
        private int id;
        private CarType type;

        public int Id
        {
            get { return Id; }
            set
            {
                id = value;
                OnPropertyChange();
            }
        }
        public CarType Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChange();
            }
        }
    }
}
