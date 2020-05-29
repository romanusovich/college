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
    class MeanViewModel : INotifyPropertyChanged
    {
        EntityContext db;

        public ObservableCollection<Средства_обучения> Means { get; set; }

        private Средства_обучения _selectedMean;
        public Средства_обучения SelectedMean
        {
            get => _selectedMean;
            set => Set(ref _selectedMean, value);
        }

        public MeanViewModel(EntityContext _db)
        {
            db = _db;

            Means = db.Средства_обучения.Local;
        }

        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                Set(ref _pattern, value);
                SelectedMean = Means.FirstOrDefault(q => q.Вид_средств_обучения.ToString().ToLower().Contains(Pattern.ToLower()));
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
                          var mean = new Средства_обучения();
                          Means.Add(mean);
                          SelectedMean = mean;
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
                      if (SelectedMean.Выпускники?.Count <= 0)
                      {
                          try
                          {
                              Means.Remove(SelectedMean);
                              SelectedMean = Means.LastOrDefault();
                              db.SaveChanges();
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Выберите средства обучения" +
                              "\nНельзя удалить средства обучения, пока есть выпускники относящиеся к ним" +
                              "\nЕсли нет выпускников, относящихся к данным средствам обучения, а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
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
