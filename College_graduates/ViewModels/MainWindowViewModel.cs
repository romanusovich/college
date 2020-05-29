using College_graduates.Class;
using College_graduates.Forms;
using College_graduates.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace College_graduates.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        EntityContext db;

        public ObservableCollection<Выпускник> Graduates { get; set; }
        public ObservableCollection<Организация> Organizations { get; set; }
        public ObservableCollection<Специальность> Specialtys { get; set; }
        public ObservableCollection<Средства_обучения> Means { get; set; }
        public ObservableCollection<Заявка> Requests { get; set; }
        public ObservableCollection<Перераспределение> Redistributions { get; set; }
        public ObservableCollection<Распределение> Distributions { get; set; }
        public ObservableCollection<Год_выпуска> Years { get; set; }

        private Выпускник _selectedGraduate;
        public Выпускник SelectedGraduate 
        { 
            get { return _selectedGraduate; }
            set 
            {
                SelectedDistribution = value?.Распределение.First();
                Set(ref _selectedGraduate, value); 
                NotifyPropertyChanged(); 
            }
        }

        private Организация _selectedOrganization;
        public Организация SelectedOrganization 
        { 
            get { return _selectedOrganization; } 
            set { _selectedOrganization = value; NotifyPropertyChanged(); }
        }

        private Специальность _selectedSpecialty;
        public Специальность SelectedSpecialty 
        {
            get { return _selectedSpecialty; } 
            set { _selectedSpecialty = value; NotifyPropertyChanged(); } 
        }

        private Средства_обучения _selectedMean;
        public Средства_обучения SelectedMean 
        { 
            get { return _selectedMean; }
            set { _selectedMean = value; NotifyPropertyChanged(); }
        }

        private Заявка _selectedRequest;
        public Заявка SelectedRequest 
        {
            get { return _selectedRequest; } 
            set { _selectedRequest = value; NotifyPropertyChanged(); }
        }

        private Перераспределение _selectedRedistribution;
        public Перераспределение SelectedRedistribution 
        { 
            get { return _selectedRedistribution; }
            set { _selectedRedistribution = value; NotifyPropertyChanged(); }
        }

        private Распределение _selectedDistribution;
        public Распределение SelectedDistribution 
        {
            get { return _selectedDistribution; }
            set 
            {
                SelectedInfo_Zero = value?.Информация.Where(q => q.Номер_полугодия == 0).FirstOrDefault();
                SelectedInfo_First = value?.Информация.Where(q => q.Номер_полугодия == 1).FirstOrDefault();
                SelectedInfo_Second = value?.Информация.Where(q => q.Номер_полугодия == 2).FirstOrDefault();
                SelectedInfo_Third = value?.Информация.Where(q => q.Номер_полугодия == 3).FirstOrDefault();
                SelectedInfo_Fourth = value?.Информация.Where(q => q.Номер_полугодия == 4).FirstOrDefault();
                SelectedInfo_Fifth = value?.Информация.Where(q => q.Номер_полугодия == 5).FirstOrDefault();
                SelectedInfo_Sixth = value?.Информация.Where(q => q.Номер_полугодия == 6).FirstOrDefault();
                _selectedDistribution = value; 
                NotifyPropertyChanged(); 
            } 
        }

        private Год_выпуска _selectedYear;
        public Год_выпуска SelectedYear 
        {
            get { return _selectedYear; }
            set 
            {
                _selectedYear = value;
                Requests = new ObservableCollection<Заявка>(db.Заявки.Local.Where(q => q.Дата.Year == _selectedYear?.Номер_года));
                NotifyPropertyChanged(); 
            }
        }

        private Информация_о_распределении _selectedInfo_Zero;
        public Информация_о_распределении SelectedInfo_Zero
        {
            get { return _selectedInfo_Zero; }
            set { _selectedInfo_Zero = value; NotifyPropertyChanged(); }
        }

        private Информация_о_распределении _selectedInfo_First;
        public Информация_о_распределении SelectedInfo_First
        {
            get { return _selectedInfo_First; }
            set { _selectedInfo_First = value; NotifyPropertyChanged(); }
        }

        private Информация_о_распределении _selectedInfo_Second;
        public Информация_о_распределении SelectedInfo_Second
        {
            get { return _selectedInfo_Second; }
            set { _selectedInfo_Second = value; NotifyPropertyChanged(); }
        }

        private Информация_о_распределении _selectedInfo_Third;
        public Информация_о_распределении SelectedInfo_Third
        {
            get { return _selectedInfo_Third; }
            set { _selectedInfo_Third = value; NotifyPropertyChanged(); }
        }

        private Информация_о_распределении _selectedInfo_Fourth;
        public Информация_о_распределении SelectedInfo_Fourth
        {
            get { return _selectedInfo_Fourth; }
            set { _selectedInfo_Fourth = value; NotifyPropertyChanged(); }
        }

        private Информация_о_распределении _selectedInfo_Fifth;
        public Информация_о_распределении SelectedInfo_Fifth
        {
            get { return _selectedInfo_Fifth; }
            set { _selectedInfo_Fifth = value; NotifyPropertyChanged(); }
        }

        private Информация_о_распределении _selectedInfo_Sixth;
        public Информация_о_распределении SelectedInfo_Sixth
        {
            get { return _selectedInfo_Sixth; }
            set { _selectedInfo_Sixth = value; NotifyPropertyChanged(); }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                Set(ref _pattern, value);
                SelectedGraduate = Graduates.FirstOrDefault(q =>
                    q.Фамилия.ToLower().Contains(Pattern.ToLower()) ||
                    q.Имя.ToLower().Contains(Pattern.ToLower()) ||
                    q.Отчество.ToLower().Contains(Pattern.ToLower())
                    );
            }
        }

        public MainWindowViewModel()
        {
            db = new EntityContext();

            db.Специальности.Load();
            db.Средства_обучения.Load();
            db.Года_выпуска.Load();
            db.Выпускники.Load();
            db.Районы.Load();
            db.Организации.Load();
            db.Заявки.Load();
            db.Распределение.Load();
            db.Информация.Load();
            db.Перераспределение.Load();

            Years = db.Года_выпуска.Local;
            Distributions = db.Распределение.Local;
            Redistributions = db.Перераспределение.Local;
            Requests = db.Заявки.Local;
            Graduates = db.Выпускники.Local;
            Organizations = db.Организации.Local;
            Specialtys = db.Специальности.Local;
            Means = db.Средства_обучения.Local;
        }

        // Команда на добавление выпускника
        private RelayCommand addCommandGraduate;
        public RelayCommand AddCommandGraduate
        {
            get
            {
                return addCommandGraduate ??
                  (addCommandGraduate = new RelayCommand(obj =>
                  {
                      try
                      {
                          var graduate = new Выпускник(SelectedYear.ID_Года_выпуска);
                          Graduates.Add(graduate);

                          graduate.Распределение = new ObservableCollection<Распределение>() { new Распределение(graduate.ID_Выпускника) };
                          graduate.Распределение.First().Информация = new ObservableCollection<Информация_о_распределении>();
                          for (int i = 0; i <= 6; i++)
                          {
                              graduate.Распределение.First().Информация.Add(new Информация_о_распределении() { ID_Распределения = graduate.Распределение.First().ID_Распределения, Номер_полугодия = i });
                          }

                          SelectedGraduate = graduate;
                      }
                      catch
                      {
                          Graduates.Remove(SelectedGraduate);
                          MessageBox.Show("Выберите год выпуска. " +
                              "\nЕсли год выпуска выбран, а ошибка осталась, то обратитесь к разработчику", "Не удалось вставить строку");
                      }
                  }));
            }
        }

        // Команда на удаление выпускника
        private RelayCommand removeCommandGraduate;
        public RelayCommand RemoveCommandGraduate
        {
            get
            {
                return removeCommandGraduate ??
                  (removeCommandGraduate = new RelayCommand(obj =>
                  {
                      var answer = MessageBox.Show("Вы действительно хотите удалить выпускника? Это также удалит всю связанную с ним информацию", "Подтверждение", MessageBoxButton.YesNo);
                      if (answer == MessageBoxResult.Yes)
                      {
                          try
                          {
                              var selectedGraduate = SelectedGraduate;

                              for (int i = 0; i <= 6; i++) // 6 полугодий
                              {
                                  db.Информация.Local.Remove(selectedGraduate.Распределение.First().Информация.FirstOrDefault());
                              }
                              var count = selectedGraduate?.Перераспределение?.Count;
                              for (int i = 0; i < count; i++)
                              {
                                  Redistributions.Remove(selectedGraduate.Перераспределение.FirstOrDefault());
                              }
                              Distributions.Remove(selectedGraduate.Распределение.First());
                              Graduates.Remove(selectedGraduate);

                              SelectedGraduate = Graduates.LastOrDefault();

                              db.SaveChanges();
                          }
                          catch
                          {
                              MessageBox.Show("Выберите выпускника" +
                                  "\nДанный студент используется в другой таблице. Удалите все упоминания о нем" +
                                  "\nЕсли студент нигде не упоминается, а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
                          }
                      }
                  }));
            }
        }

        // Команада на сохранение выпускника
        private RelayCommand saveCommandGraduate;
        public RelayCommand SaveCommandGraduate
        {
            get
            {
                return saveCommandGraduate ??
                  (saveCommandGraduate = new RelayCommand(obj =>
                  {
                      try
                      {
                          db.SaveChanges();
                      }
                      catch
                      {
                          MessageBox.Show("Убедитесь в заполнености всех полей и правильности их ввода. " +
                              "\nЕсли все поля правильно заполнены, а ошибка осталась, то обратитесь к разработчику", "Не удалось сохранить строку");
                      }
                  }));
            }
        }

        // Команда на добавление перераспределения
        private RelayCommand addCommandRedistribution;
        public RelayCommand AddCommandRedistribution
        {
            get
            {
                return addCommandRedistribution ??
                  (addCommandRedistribution = new RelayCommand(obj =>
                  {
                      try
                      {
                          var redistribution = new Перераспределение(SelectedGraduate.ID_Выпускника, Organizations?.FirstOrDefault()?.ID_Организации);
                          Redistributions.Add(redistribution);
                          SelectedRedistribution = redistribution;
                      }
                      catch
                      {
                          Redistributions.Remove(SelectedRedistribution);
                          MessageBox.Show("Выберите выпускника" +
                              "\nДобавьте организации" +
                              "\nЕсли выполнены советы выше, а ошибка осталась, то обратитесь к разработчику", "Не удалось вставить строку");
                      }
                  }));
            }
        }

        // Команда на удаление перераспределения
        private RelayCommand removeCommandRedistribution;
        public RelayCommand RemoveCommandRedistribution
        {
            get
            {
                return removeCommandRedistribution ??
                  (removeCommandRedistribution = new RelayCommand(obj =>
                  {
                      try
                      {
                          Redistributions.Remove(SelectedRedistribution);
                          SelectedRedistribution = Redistributions.LastOrDefault();
                          db.SaveChanges();
                      }
                      catch
                      {
                          MessageBox.Show("Выберите перераспределение" +
                              "\nЕсли перераспределение выбрано, а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
                      }
                  }));
            }
        }

        // Команда на сохранение перераспределения
        private RelayCommand saveCommandRedistribution;
        public RelayCommand SaveCommandRedistribution
        {
            get
            {
                return saveCommandRedistribution ??
                  (saveCommandRedistribution = new RelayCommand(obj =>
                  {
                      try
                      {
                          db.SaveChanges();
                      }
                      catch
                      {
                          MessageBox.Show("Убедитесь в заполнености всех полей и правильности их ввода. " +
                              "\nЕсли все поля правильно заполнены, а ошибка осталась, то обратитесь к разработчику", "Не удалось сохранить строку");
                      }
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showYears;
        public RelayCommand ShowYears
        {
            get
            {
                return showYears ??
                  (showYears = new RelayCommand(obj =>
                  {
                      YearOfOutlet wYearOfOutlet = new YearOfOutlet(db);
                      wYearOfOutlet.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showOrganization;
        public RelayCommand ShowOrganization
        {
            get
            {
                return showOrganization ??
                  (showOrganization = new RelayCommand(obj =>
                  {
                      Organization wOrganization = new Organization(db);
                      wOrganization.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showImport;
        public RelayCommand ShowImport
        {
            get
            {
                return showImport ??
                  (showImport = new RelayCommand(obj =>
                  {
                      ImportForm wImport = new ImportForm(db);
                      wImport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showSpecialty;
        public RelayCommand ShowSpecialty
        {
            get
            {
                return showSpecialty ??
                  (showSpecialty = new RelayCommand(obj =>
                  {
                      Specialty wSpecialty = new Specialty(db);
                      wSpecialty.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showMean;
        public RelayCommand ShowMean
        {
            get
            {
                return showMean ??
                  (showMean = new RelayCommand(obj =>
                  {
                      MeansOfEducation wMeans = new MeansOfEducation(db);
                      wMeans.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showStatement;
        public RelayCommand ShowStatement
        {
            get
            {
                return showStatement ??
                  (showStatement = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(true, false, true, false, "Statement", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showEntered;
        public RelayCommand ShowEntered
        {
            get
            {
                return showEntered ??
                  (showEntered = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(true, false, true, false, "EnteredGraduates", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showOrganizationRequests;
        public RelayCommand ShowOrganizationRequests
        {
            get
            {
                return showOrganizationRequests ??
                  (showOrganizationRequests = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(true, false, false, true, "OrganizationRequests", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showOrganizationRequestsPeriod;
        public RelayCommand ShowOrganizationRequestsPeriod
        {
            get
            {
                return showOrganizationRequestsPeriod ??
                  (showOrganizationRequestsPeriod = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(false, true, false, true, "OrganizationRequestsPeriod", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showOwnGraduates;
        public RelayCommand ShowOwnGraduates
        {
            get
            {
                return showOwnGraduates ??
                  (showOwnGraduates = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(true, false, true, false, "OwnGraduates", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showRequestsList;
        public RelayCommand ShowRequestsList
        {
            get
            {
                return showRequestsList ??
                  (showRequestsList = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(true, false, true, false, "RequestsList", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showUnemployedGraduates;
        public RelayCommand ShowUnemployedGraduates
        {
            get
            {
                return showUnemployedGraduates ??
                  (showUnemployedGraduates = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(true, false, true, false, "UnemployedGraduates", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showWorkResults;
        public RelayCommand ShowWorkResults
        {
            get
            {
                return showWorkResults ??
                  (showWorkResults = new RelayCommand(obj =>
                  {
                      ReportSettings wReport = new ReportSettings(true, false, true, false, "WorkResults", db);
                      wReport.ShowDialog();
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
