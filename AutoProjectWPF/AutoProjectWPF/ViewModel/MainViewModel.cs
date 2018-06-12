using CommonLibrary.Repositories;
using CommonLibrary.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AutoProjectWPF.RepService;
using System.ServiceModel;
using ServiceExample;
using System;

namespace AutoProjectWPF.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        RepositoryServiceClient client = new RepositoryServiceClient("NetTcpBinding_IRepositoryService");
        CachedRepositary<Car> CarRepo = new CachedRepositary<Car>();
        //Сделано для того, что бы записи напрямую считывались из БД, а не брались из кэша
        BaseRepository<Car> LoadingRecordsRepo = new BaseRepository<Car>();
        BaseRepository<CarTypeModel> CarTypeRepo = new BaseRepository<CarTypeModel>();

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
            CarCollection = new ObservableCollection<CarViewModel>(CarRepo.Load().Select(car => new CarViewModel(car)));
            typesOfCar = new ObservableCollection<CarTypeViewModel>(CarTypeRepo.Load().Select(carModel => new CarTypeViewModel(carModel)));

            ListOfActions = new ObservableCollection<ICommand>()
            {
                CreateEmptyItem,
                SaveItem,
                RemoveItem,
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

        /// <summary>
        /// Добавляет пустой элемент для заполнения(не добавляя в коллекцию)
        /// </summary>
        private ActionViewModel createEmptyItem;
        public ActionViewModel CreateEmptyItem
        {
            get
            {
                return createEmptyItem ??
                    (createEmptyItem = new ActionViewModel("Create\nRecord",
                    obj =>
                    {
                        SelectedCar = new CarViewModel(new Car()
                        {
                            Id = CarCollection.Count + 1
                        });
                    }));
            }
        }
        /// <summary>
        /// Записывает в БД обновленный или новый элемент,
        /// при записи нового, добавляет его сначала в коллекцию,
        /// а после в БД.
        /// </summary>
        private ActionViewModel saveItem;
        public ActionViewModel SaveItem
        {
            get
            {
                return saveItem ??
                    (saveItem = new ActionViewModel("Save",
                    obj =>
                    {
                        if (CarCollection.Contains(SelectedCar))
                        {
                            CarRepo.Update(SelectedCar.ReturnCar());
                        }
                        else
                        {
                            SelectedCar.Type = SelectedType.Type;
                            CarCollection.Add(SelectedCar);
                            try
                            {
                                client.Save(SelectedCar.ReturnCar());
                            }
                            catch (FaultException error)
                            {
                                MessageBox.Show(error.Message);
                            }
                        }
                    }));
            }
        }
        /// <summary>
        /// Удаляет элемент из БД
        /// </summary>
        private ActionViewModel removeItem;
        public ActionViewModel RemoveItem
        {
            get
            {
                return removeItem ??
                    (removeItem = new ActionViewModel("Remove\nRecord",
                    obj =>
                    {
                        CarRepo.Remove(selectedCar.ReturnCar());
                        CarCollection.Remove(selectedCar);
                        SelectedCar = CarCollection.FirstOrDefault();
                    }));
            }
        }
        /// <summary>
        /// Отмена изменений на форме у выбранного объекта
        /// </summary>
        private ActionViewModel undoChangeItem;
        public ActionViewModel UndoChangeItem
        {
            get
            {
                return undoChangeItem ??
                    (undoChangeItem = new ActionViewModel("Undo\nChange",
                    obj =>
                    {
                        // Если вызвать у не сохраненного обхекта в БД, а только что созданного, то отката не будет.
                        if (CarCollection.Contains(SelectedCar))
                        {
                            CarCollection[CarCollection.IndexOf(SelectedCar)] = new CarViewModel(LoadingRecordsRepo.GetByKey(SelectedCar.Id));
                            SelectedCar = CarCollection.FirstOrDefault();
                        }
                    }));
            }
        }
    }
}
