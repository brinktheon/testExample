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
using System;
using System.Reflection;
using System.ComponentModel;

namespace AutoProjectWPF.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        RepositoryServiceClient client = new RepositoryServiceClient("NetTcpBinding_IRepositoryService");
        CachedRepositary<Car> CarRepo = new CachedRepositary<Car>();
        //Сделано для того, что бы записи напрямую считывались из БД, а не брались из кэша
        BaseRepository<Car> LoadingRecordsRepo = new BaseRepository<Car>();
        BaseRepository<CarTypeModel> CarTypeRepo = new BaseRepository<CarTypeModel>();

        // Предыдущий объект, для проверки для проверки на сохранение.
        private CarViewModel previewsCar;


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

            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

            ListOfActions = new ObservableCollection<ICommand>()
            {
                CreateEmptyItem,
                SaveItem,
                RemoveItem,
                UndoChangeItem
            };
        }

        /// <summary>
        /// Событие для закртия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            ChangeConfirmation();
        }

        /// <summary>
        /// Метод проверяет сохранены ли изменения в объекте
        /// Проверяет пустой ли предыдущий объект
        /// Загружает в сессию на изменения
        /// Проверяет поля двух объектов
        /// Если они не равны(объект изменен), то проходится по полям и устанавливает значения.
        /// После, если объект не сохранен, то запрашивет у пользователя Сохранить или Не сохранять объект.
        /// </summary>
        private void ChangeConfirmation()
        {
            if (previewsCar != null)
            {
                try
                {
                    var editorObject = new CarViewModel(client.StartCarEdit(previewsCar.Id));

                    if (!(editorObject.ReturnCar()).Equals(previewsCar.ReturnCar()))
                    {
                        int indexOfPreviewsCarInList = CarCollection.IndexOf(previewsCar);
                        int IdPreviewsCar = previewsCar.Id;
                        foreach (PropertyInfo info in editorObject.ReturnCar().GetType().GetProperties())
                        {
                            if (!info.GetValue(editorObject.ReturnCar()).Equals(info.GetValue(previewsCar.ReturnCar())))
                            {
                                if (info.GetValue(previewsCar.ReturnCar()).Equals(""))
                                {
                                    MessageBox.Show("Поле должно быть заполнено");
                                    previewsCar = null;
                                    carCollection[indexOfPreviewsCarInList] = new CarViewModel(client.CancelEdit(IdPreviewsCar));
                                    return;
                                }
                                else
                                {
                                    editorObject = new CarViewModel(client.SetCarValue(info.Name, info.GetValue(previewsCar.ReturnCar())));
                                }
                            }
                        }
                        previewsCar = null;

                        MessageBoxResult messageBox = MessageBox.Show("Сохранить внесенные имзенения ?", "Caption", MessageBoxButton.YesNo);
                        if (messageBox == MessageBoxResult.Yes)
                        {
                            client.Update(CarCollection[indexOfPreviewsCarInList].ReturnCar());
                            CarCollection[indexOfPreviewsCarInList] = editorObject;
                            MessageBox.Show("Изменения успешно сохранены");
                        }
                        if (messageBox == MessageBoxResult.No)
                        {
                            carCollection[indexOfPreviewsCarInList] = new CarViewModel(client.CancelEdit(IdPreviewsCar));
                            MessageBox.Show("Изменения отменены");
                        }
                    }
                } 
                catch (Exception e)
                {
                    MessageBox.Show($"В ходе работы возникла ошибка {e.Message}\nДанные восстановлены");
                    CarCollection[CarCollection.IndexOf(previewsCar)] = new CarViewModel(client.CancelEdit(previewsCar.Id));
                }
            }
        }


        private CarViewModel selectedCar;
        public CarViewModel SelectedCar
        {
            get { return selectedCar; }
            set
            {
                ChangeConfirmation();
                selectedCar = value;
                previewsCar = SelectedCar;
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
                            client.Update(SelectedCar.ReturnCar());
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
                        client.Remove(selectedCar.ReturnCar());
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
