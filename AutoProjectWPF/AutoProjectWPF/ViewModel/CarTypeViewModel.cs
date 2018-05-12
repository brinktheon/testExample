using Model;

namespace AutoProjectWPF.ViewModel
{
    class CarTypeViewModel : BaseViewModel
    {
        private int id;
        private string typeName;

        public int Id
        {
            get { return Id; }
            set
            {
                id = value;
                OnPropertyChange();
            }
        }

        public string TypeName
        {
            get { return typeName; }
            set
            {
                typeName = value;
                OnPropertyChange();
            }
        }
    }
}
