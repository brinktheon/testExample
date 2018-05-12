using AutoProjectWPF.ViewModel.Repositories;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AutoProjectWPF.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private string autoQuery = "select * from AutoConfig ac inner join AutoType at on ac.CarTypeId = at.id";
        private string autoQueryType = "select at.TypeName, at.Id from AutoType at";
        RealizeCacheRepository realize;
        BaseRepository<CarTypeViewModel> baserepo;


        public IEnumerable<string> ComboListColor { get { return typeof(Colors).GetProperties().Select(x => x.Name); } }

        //collection 2
        private ObservableCollection<Actions> listOfActions;
        public ObservableCollection<Actions> ListOfActions
        {
            get { return listOfActions; }
            set
            {
                listOfActions = value;
                OnPropertyChange();
            }
        }

        //collection 1
        private ObservableCollection<Action> lOfActions;
        public ObservableCollection<Action> LOfActions
        {
            get { return lOfActions; }
            set
            {
                lOfActions = value;
                OnPropertyChange();
            }
        }


        private ObservableCollection<CarTypeViewModel> typesOfCar;
        public ObservableCollection<CarTypeViewModel> TypesOfCar
        {
            get { return typesOfCar; }
            set
            {
                typesOfCar = value;
                OnPropertyChange();
            }
        }

        private ObservableCollection<Car> carCollection;
        public ObservableCollection<Car> CarCollection
        {
            get { return carCollection; }
            set
            {
                carCollection = value;
                OnPropertyChange();
            }
        }

        public MainViewModel()
        {
            realize = new RealizeCacheRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            baserepo = new BaseRepository<CarTypeViewModel>(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            CarCollection = new ObservableCollection<Car>(realize.Load(autoQuery));
            typesOfCar = new ObservableCollection<CarTypeViewModel>(baserepo.Load(autoQueryType));

            //Первый вариант
            LOfActions = new ObservableCollection<Action>() {
                () => { MessageBox.Show("Action 1"); },
                () => { MessageBox.Show("Action 2"); },
                () => { MessageBox.Show("Action 3"); }
            };

            //Второй вариант
            ListOfActions = new ObservableCollection<Actions>() {
                 new Actions()
                 {
                     Command = new ActionViewModel(obj => { MessageBox.Show("Action 1"); })
                 },
                 new Actions()
                 {
                     Command = new ActionViewModel(obj => { MessageBox.Show("Action 2"); })
                 },
                 new Actions()
                 {
                     Command = new ActionViewModel(obj => { MessageBox.Show("Action  3"); })
                 },
                 new Actions()
                 {
                     Command = new ActionViewModel(obj => { MessageBox.Show("Action  4"); })
                 }
            };
        }

        private Car selectedCar;
        public Car SelectedCar
        {
            get { return selectedCar; }
            set
            {
                selectedCar = value;
                OnPropertyChange();
            }
        }

        /*
         * Command to Create Item
         * Добавляет пустой элемент для редактирование
         * и дальнейшего сохранения (Command Save)
         */
        private ActionViewModel createItem;
        public ActionViewModel CreateItem
        {
            get
            {
                return createItem ??
                    (createItem = new ActionViewModel(obj =>
                    {
                        var car = new Car();
                        carCollection.Add(car);
                        SelectedCar = car;
                    }));
            }
        }

        /*
         * Command to Save Item 
         * Не реализовано по заданию
         */
        private ActionViewModel saveItem;
        public ActionViewModel SaveItem
        {
            get
            {
                return saveItem ??
                    (saveItem = new ActionViewModel(obj =>
                    {
                    }));
            }
        }

        /*
         * Command to Remove Item from Collection
         * Удаляет элемент из коллекции
         */
        private ActionViewModel removeItem;
        public ActionViewModel RemoveItem
        {
            get
            {
                return removeItem ??
                    (removeItem = new ActionViewModel(obj =>
                    {
                        if (obj is Car car)
                        {
                            CarCollection.Remove(car);
                        }
                    }));
            }
        }
    }
}
