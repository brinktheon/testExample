using AutoProjectWPF.Model;
using AutoProjectWPF.ViewModel.Repositories;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoProjectWPF.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        CachedRepositary<Car> CarRepo;
        //Сделано для того, что бы записи напрямую считывались из БД, а не брались из кэша
        BaseRepository<Car> LoadingRecordsRepo;
        BaseRepository<CarTypeModel> CarTypeRepo;

        private int counOfNoChangeItems;

        public IEnumerable<string> ComboListColor { get { return typeof(Colors).GetProperties().Select(x => x.Name); } }

        /*Коллекция для отката изменения, содержит в себе 
         * Id измененных элементов в начальном состоянии.
         */
        private IList<int> IdChangedItems;


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
            CarRepo = new CachedRepositary<Car>();
            CarTypeRepo = new BaseRepository<CarTypeModel>();
            LoadingRecordsRepo = new BaseRepository<Car>();
            CarCollection = new ObservableCollection<CarViewModel>(CarRepo.Load().Select(car => new CarViewModel(car)));
            typesOfCar = new ObservableCollection<CarTypeViewModel>(CarTypeRepo.Load().Select(carModel => new CarTypeViewModel(carModel)));
            counOfNoChangeItems = carCollection.Count;
            IdChangedItems = new List<int>();

            ListOfActions = new ObservableCollection<ICommand>()
            {
                CreateEmptyItem,
                AddItem,
                SaveItem,
                SaveAllItem,
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
        /// Доабвляет заполненный элемент в коллекцию
        /// </summary>
        private ActionViewModel addItem;
        public ActionViewModel AddItem
        {
            get
            {
                return addItem ??
                    (addItem = new ActionViewModel("Add\nToList",
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
        /// <summary>
        /// Регестрирует изменения в записи путем добавления
        /// Id изменяемого объектка в коллекцию.
        /// </summary>
        private ActionViewModel saveItem;
        public ActionViewModel SaveItem
        {
            get
            {
                return saveItem ??
                    (saveItem = new ActionViewModel("Apply\nChange",
                    obj =>
                    {
                        try
                        {
                            IdChangedItems.Add(selectedCar.Id);
                            MessageBox.Show("Изменения сохранены.");
                        }
                        catch (NullReferenceException e)
                        {
                            MessageBox.Show("Не выбран сохраняемый объект. \n" + e.Message, "Ошибка выбора данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }));
            }
        }
        /// <summary>
        /// Сохраняет все изменения в БД
        /// </summary>
        private ActionViewModel saveAllItem;
        public ActionViewModel SaveAllItem
        {
            get
            {
                return saveAllItem ??
                    (saveAllItem = new ActionViewModel("SaveAll\nRecords",
                    obj =>
                    {
                        try
                        {
                            //Сохраняет измененные запси
                            foreach (int id in IdChangedItems)
                            {
                                CarRepo.Update(CarCollection.First(x => x.Id == id).ReturnCar());
                            }
                            //Сохраняет новые добавленные записи
                            foreach (CarViewModel item in CarCollection.Skip(counOfNoChangeItems))
                            {
                                CarRepo.Save(item.ReturnCar());
                            }

                        }
                        catch (NullReferenceException e)
                        {
                            MessageBox.Show("Не выбран сохраняемый объект. \n" + e.Message, "Ошибка выбора данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        counOfNoChangeItems = carCollection.Count;
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
        /// Отмена изменений на форме
        /// </summary>
        private ActionViewModel undoChangeItem;
        public ActionViewModel UndoChangeItem
        {
            get
            {
                return undoChangeItem ??
                    (undoChangeItem = new ActionViewModel("Undo\nChanges",
                    obj =>
                    {   //Возвращает кол-во элементов коллекции к последнему сохраненному состоянию
                        CarCollection = new ObservableCollection<CarViewModel>(CarCollection.Take(counOfNoChangeItems));

                        //Берет все элементы по Id из коллекции измененных элементов и выгружает их последнее состояние из БД
                        /*
                         * Была идея обновить данные просто выгрузив из БД, но думаю что это слишком затратно по времени.
                         * Хотя по тому, что я сделал тоже есть сомнения, где то читал, что вложенные циклы долго работают.
                         */
                        for (int i = 0; i < CarCollection.Count; i++)
                        {
                            foreach (int id in IdChangedItems)
                            {
                                if (CarCollection[i].Id == id)
                                {
                                    CarCollection[i] = new CarViewModel(LoadingRecordsRepo.GetByKey(id));
                                }
                            }
                        }
                        IdChangedItems.Clear();
                        SelectedCar = CarCollection.FirstOrDefault();
                    }));
            }
        }
    }
}
