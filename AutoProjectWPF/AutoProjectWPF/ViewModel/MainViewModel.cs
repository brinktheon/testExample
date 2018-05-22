using AutoProjectWPF.Model;
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
        CachedRepositary<Car> realize;
        BaseRepository<CarTypeModel> baserepo;
        private int counOfNoChangeItems;

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

        private CarTypeViewModel selectedType;
        public CarTypeViewModel SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                OnPropertyChange();
            }
        }

        private ObservableCollection<CarViewModel> carCollection;
        public ObservableCollection<CarViewModel> CarCollection
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
            realize = new CachedRepositary<Car>();
            baserepo = new BaseRepository<CarTypeModel>();
            CarCollection = new ObservableCollection<CarViewModel>(realize.Load().Select(car => new CarViewModel(car)));
            typesOfCar = new ObservableCollection<CarTypeViewModel>(baserepo.Load().Select(carModel => new CarTypeViewModel(carModel)));
            counOfNoChangeItems = carCollection.Count;

            ListOfActions = new ObservableCollection<ICommand>()
            {
                CreateEmptyItem,
                AddItem,
                RemoveItem,
                SaveItem,
                UndoChangeItem
            };
        }

        private CarViewModel selectedCar;
        public CarViewModel SelectedCar
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
         * и дальнейшего добавления в коллекцию (AddItem)
         */
        private ActionViewModel createEmptyItem;
        public ActionViewModel CreateEmptyItem
        {
            get
            {
                return createEmptyItem ??
                    (createEmptyItem = new ActionViewModel("Create",
                    obj =>
                    {
                        SelectedCar = new CarViewModel(new Car()
                        {
                            Id = CarCollection.Count + 1
                        });
                    }));
            }
        }
        /*
        * Command to Add Item
        * Добавляет созданный элемент в коллекцию
        */
        private ActionViewModel addItem;
        public ActionViewModel AddItem
        {
            get
            {
                return addItem ??
                    (addItem = new ActionViewModel("Add Item",
                    obj =>
                    {
                        if (!CarCollection.Contains(SelectedCar))
                        {
                            SelectedCar.Type = selectedType.Type;
                            carCollection.Add(SelectedCar);
                        }
                    }));
            }
        }
        /*
         * Command to Save Item 
         */
        private ActionViewModel saveItem;
        public ActionViewModel SaveItem
        {
            get
            {
                return saveItem ??
                    (saveItem = new ActionViewModel("Save",
                    obj =>
                    {
                        counOfNoChangeItems = carCollection.Count;
                        realize.Save(SelectedCar.ReturnCar());
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
                    (removeItem = new ActionViewModel("Remove",
                    obj =>
                    {
                        CarCollection.Remove(selectedCar);
                    }));
            }
        }
        /*
         * Отмена изменений на форме
         */
        private ActionViewModel undoChangeItem;
        public ActionViewModel UndoChangeItem
        {
            get
            {
                return undoChangeItem ??
                    (undoChangeItem = new ActionViewModel("UndoChange",
                    obj =>
                    {
                        CarCollection = new ObservableCollection<CarViewModel>(CarCollection.Take(counOfNoChangeItems));
                    }));
            }
        }
    }
}
