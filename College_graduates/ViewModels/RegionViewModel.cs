using College_graduates.Class;
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

namespace College_graduates.ViewModels
{
    class RegionViewModel : INotifyPropertyChanged
    {
        EntityContext db;

        public ObservableCollection<Район> Regions { get; set; }

        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                Set(ref _pattern, value);
                SelectedRegion = Regions.FirstOrDefault(q =>
                    q.Название_района.ToLower().Contains(Pattern.ToLower())
                    );
            }
        }

        private Район _selectedRegion;
        public Район SelectedRegion
        {
            get => _selectedRegion;
            set => Set(ref _selectedRegion, value);
        }

        public RegionViewModel(EntityContext _db)
        {
            db = _db;

            Regions = db.Районы.Local;
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
                          var region = new Район();
                          Regions.Add(region);
                          SelectedRegion = region;
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
                      if (SelectedRegion.Организации?.Count <= 0)
                      {
                          try
                          {
                              Regions.Remove(SelectedRegion);
                              SelectedRegion = Regions.LastOrDefault();
                              db.SaveChanges();
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Выберите район" +
                              "\nНельзя удалить район, когда он используется в таблице с организациями" +
                              "\nЕсли район не упоминается в таблице с организациями, а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
