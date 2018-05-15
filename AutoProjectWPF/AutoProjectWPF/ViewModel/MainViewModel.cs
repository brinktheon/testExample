using AutoProjectWPF.ViewModel.Repositories;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoProjectWPF.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private string autoQuery = "select * from AutoConfig ac inner join AutoType at on ac.CarTypeId = at.id";
        private string autoQueryType = "select * from AutoType at";
        RealizeCacheRepository realize;
        BaseRepository<CarTypeViewModel> baserepo;

        //Pessanger
        private bool passengerCarVisibility;
        public bool PassengerCarVisibility
        {
            get
            {
                return passengerCarVisibility;
            }
            set
            {
                passengerCarVisibility = value;
                OnPropertyChange();
            }
        }
        //Sport
        private bool sportCarCarVisibility;
        public bool SportCarCarVisibility
        {
            get
            {
                return sportCarCarVisibility;
            }
            set
            {
                sportCarCarVisibility = value;
                OnPropertyChange();
            }
        }
        //Truck
        private bool truckCarCarVisibility;
        public bool TruckCarCarVisibility
        {
            get
            {
                return truckCarCarVisibility;
            }
            set
            {
                truckCarCarVisibility = value;
                OnPropertyChange();
            }
        }
        //Tipper
        private bool tipperCarVisibility;
        public bool TipperrCarVisibility
        {
            get
            {
                return tipperCarVisibility;
            }
            set
            {
                tipperCarVisibility = value;
                OnPropertyChange();
            }
        }


        public IEnumerable<string> ComboListColor { get { return typeof(Colors).GetProperties().Select(x => x.Name); } }

        private string comboListSelectedColor;
        public string ComboListSelectedColor
        {
            get { return comboListSelectedColor; }
            set
            {
                comboListSelectedColor = value;
                Application.Current.Resources["panel"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboListSelectedColor));
                OnPropertyChange();
            }
        }

        private ObservableCollection<ICommand> listOfActions;
        public ObservableCollection<ICommand> ListOfActions
        {
            get { return listOfActions; }
            set
            {
                listOfActions = value;
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

            ListOfActions = new ObservableCollection<ICommand>()
            {
                CreateItem,
                SaveItem,
                RemoveItem
            };
        }

        private Car selectedCar;
        public Car SelectedCar
        {
            get { return selectedCar; }
            set
            {
                selectedCar = value;
                PassengerCarVisibility = selectedCar.Type == CarType.PassengerCar || selectedCar.Type == CarType.SportCar;
                TruckCarCarVisibility = selectedCar.Type == CarType.TruckCar || selectedCar.Type == CarType.Tipper;
                SportCarCarVisibility = selectedCar.Type == CarType.SportCar;
                TipperrCarVisibility = selectedCar.Type == CarType.Tipper;
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
                    })
                    {
                        DisplayName = "Create"
                    });
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
                    })
                    {
                        DisplayName = "Save"
                    });
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
                    })
                    {
                        DisplayName = "Remove"
                    });
            }
        }
    }
}
