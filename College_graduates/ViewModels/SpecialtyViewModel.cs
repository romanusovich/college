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
    class SpecialtyViewModel : INotifyPropertyChanged
    {
        EntityContext db;

        public ObservableCollection<Специальность> Specialtys { get; set; }

        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                Set(ref _pattern, value);
                SelectedSpecialty = Specialtys.FirstOrDefault(q =>
                    q.Название_специальности.ToLower().Contains(Pattern.ToLower())
                    );
            }
        }

        private Специальность _selectedSpecialty;
        public Специальность SelectedSpecialty
        {
            get => _selectedSpecialty;
            set => Set(ref _selectedSpecialty, value);
        }

        public SpecialtyViewModel(EntityContext _db)
        {
            db = _db;

            Specialtys = db.Специальности.Local;
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
                          var specialty = new Специальность();
                          Specialtys.Add(specialty);
                          SelectedSpecialty = specialty;
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
                      if (SelectedSpecialty.Выпускники.Count <= 0 &&
                          SelectedSpecialty.Заявки.Count <= 0)
                      {
                          try
                          {
                              var specialty = SelectedSpecialty;
                              Specialtys.Remove(specialty);
                              SelectedSpecialty = Specialtys.LastOrDefault();
                              db.SaveChanges();
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Выберите специальность" +
                              "\nНельзя удалить специальность, когда она используется в таблице с выпускниками или заявками" +
                              "\n Если специальность нигде не упоминается, а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
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
