using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Entity;
using System.Threading.Tasks;
using College_graduates.Class;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Collections.ObjectModel;

namespace College_graduates.ViewModels
{
    class OrganizationViewModel : INotifyPropertyChanged
    {
        EntityContext db;
        public ObservableCollection<Организация> Organizations { get; set; }
        public ObservableCollection<Район> Regions { get; set; }

        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                Set(ref _pattern, value);
                SelectedOrganization = Organizations.FirstOrDefault(q =>
                    q.Наименование.ToLower().Contains(Pattern.ToLower()) ||
                    q.Район.Название_района.ToLower().Contains(Pattern.ToLower()) ||
                    q.Населенный_пункт.ToLower().Contains(Pattern.ToLower()) ||
                    q.Адрес.ToLower().Contains(Pattern.ToLower())
                    );
            }
        }

        private Организация _selectedOrganization;
        public Организация SelectedOrganization 
        { 
            get => _selectedOrganization; 
            set => Set(ref _selectedOrganization, value); 
        }

        private Район _selectedRegion;
        public Район SelectedRegion
        { 
            get { return _selectedRegion; }
            set { _selectedRegion = value; NotifyPropertyChanged(); } 
        }

        public OrganizationViewModel(EntityContext _db)
        {
            db = _db;

            Regions = db.Районы.Local;
            Organizations = db.Организации.Local;
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          var organization = new Организация();
                          Organizations.Add(organization);
                          SelectedOrganization = organization;
                      }
                      catch
                      {
                          MessageBox.Show("В случае этой ошибки стоит сразу обращаться к разработчику", "Не удалось вставить строку");
                      }
                  }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      if (SelectedOrganization?.Заявки?.Count <= 0 || SelectedOrganization?.Заявки == null)
                      {
                          try
                          {
                              Organizations.Remove(SelectedOrganization);
                              SelectedOrganization = Organizations.LastOrDefault();
                              db.SaveChanges();
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Выберите организацию" +
                              "\nУдалите все заявки организации" +
                              "\nЕсли выполнены советы выше, а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
                      }
                  }));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          db.SaveChanges();
                      }
                      catch
                      {
                          MessageBox.Show("Убедитесь в заполненности всех полей и правильности их ввода. " +
                              "\nЕсли все поля правильно заполнены, а ошибка осталась, то обратитесь к разработчику", "Не удалось сохранить строку");
                      }
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showRequest;
        public RelayCommand ShowRequest
        {
            get
            {
                return showRequest ??
                  (showRequest = new RelayCommand(obj =>
                  {
                      Request wRequest = new Request(db, SelectedOrganization);
                      wRequest.ShowDialog();
                  }));
            }
        }

        // Команда на открытие формы
        private RelayCommand showRegion;
        public RelayCommand ShowRegion
        {
            get
            {
                return showRegion ??
                  (showRegion = new RelayCommand(obj =>
                  {
                      Region wRegion = new Region(db);
                      wRegion.ShowDialog();

                      db.Районы.Load();
                      Regions = db.Районы.Local;
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
