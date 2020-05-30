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
    class RequestViewModel : INotifyPropertyChanged
    {
        EntityContext db;
        
        public ObservableCollection<Заявка> Requests { get; set; }
        public ObservableCollection<Специальность> Specialtys { get; set; }
        public ObservableCollection<Выпускник> Graduates { get; set; }

        string _pattern;
        public string Pattern
        {
            get => _pattern;
            set
            {
                Set(ref _pattern, value);
                SelectedRequest = Requests.FirstOrDefault(q => 
                q.Дата.ToString().ToLower().Contains(Pattern.ToLower()) ||
                q.Должность.ToLower().Contains(Pattern.ToLower()) ||
                q.Количество_мест.ToString().ToLower().Contains(Pattern.ToLower()) ||
                q.Специальность.Название_специальности.ToLower().Contains(Pattern.ToLower()) ||
                q.Номер.ToString().ToLower().Contains(Pattern.ToLower())
                );
            }
        }

        private Заявка _selectedRequest;
        public Заявка SelectedRequest
        {
            get => _selectedRequest;
            set => Set(ref _selectedRequest, value);
        }

        public Специальность SelectedSpecialty { get; set; }
        public Выпускник SelectedGraduate { get; set; }

        private Организация _selectedOrganization;
        public Организация SelectedOrganization
        {
            get { return _selectedOrganization; }
            set { _selectedOrganization = value; NotifyPropertyChanged(); }
        }

        public RequestViewModel(EntityContext _db, Организация organization)
        {
            db = _db;

            SelectedOrganization = organization;

            Requests = db.Заявки.Local;
            Specialtys = db.Специальности.Local;
            Graduates = db.Выпускники.Local;
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
                          var request = new Заявка(SelectedOrganization.ID_Организации);
                          SelectedOrganization.Заявки.Add(request);
                          SelectedRequest = request;
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
                      if (SelectedRequest?.Распределения?.Count <= 0)
                      {
                          try
                          {
                              var request = SelectedRequest;
                              SelectedOrganization.Заявки.Remove(request);
                              SelectedRequest = SelectedOrganization.Заявки.LastOrDefault();
                              db.SaveChanges();
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Выберите заявку" +
                              "\nДанная заявка используется в другой таблице, удалите распределения выпускников, по этой заявке. " +
                              "\nЕсли заявка нигде не упоминается, а ошибка осталась, то обратитесь к разработчику", "Не удалось удалить строку");
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
                          if (SelectedGraduate != null)
                              db.Выпускники.Local.Where(q => q.ID_Выпускника == SelectedGraduate.ID_Выпускника).FirstOrDefault().Распределение.FirstOrDefault().ID_Заявки = SelectedRequest.ID_Заявки;

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

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
