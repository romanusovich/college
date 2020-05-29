using College_graduates.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace College_graduates.ViewModels
{
    class YearOfOutletViewModel : INotifyPropertyChanged
    {
        EntityContext db;

        public ObservableCollection<Год_выпуска> Years { get; set; }

        Год_выпуска _selectedYear;
        public Год_выпуска SelectedYear
        {
            get => _selectedYear;
            set => Set(ref _selectedYear, value);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public YearOfOutletViewModel(EntityContext _db)
        {
            db = _db;

            Years = db.Года_выпуска.Local;
        }

        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                Set(ref _pattern, value);
                SelectedYear = Years.FirstOrDefault(q => q.Номер_года.ToString().ToLower().Contains(Pattern.ToLower()));
            }
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
                          var year = new Год_выпуска();
                          Years.Add(year);
                          SelectedYear = year;
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
                      if (_selectedYear.Выпускники?.Count <= 0 || _selectedYear.Выпускники == null)
                      {
                          try
                          {
                              Years.Remove(SelectedYear);
                              SelectedYear = Years.LastOrDefault();
                              db.SaveChanges();
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Выберите год выпуска" +
                              "\nНельзя удалить год, пока есть выпускники относящиеся к нему" +
                              "\nЕсли нет выпускников, относящихся к выбранному году, , а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
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
                          MessageBox.Show("Убедитесь в заполнености всех полей и правильности их ввода. " +
                              "\nЕсли все поля правильно заполнены, а ошибка осталась, то обратитесь к разработчику", "Не удалось сохранить строку");
                      }
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
