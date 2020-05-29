using College_graduates.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace College_graduates.ViewModels
{
    class ImportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Выпускник> AddGraduates { get; set; }
        public ObservableCollection<Выпускник> RemoveGraduates { get; set; }

        private Выпускник _addedGraduate;
        public Выпускник AddedGraduate
        {
            get { return _addedGraduate; }
            set { _addedGraduate = value; NotifyPropertyChanged(); }
        }

        private Выпускник _removedGraduate;
        public Выпускник RemovedGraduate
        {
            get { return _removedGraduate; }
            set { _removedGraduate = value; NotifyPropertyChanged(); }
        }

        private GraduateImport _importer;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ImportViewModel(EntityContext _db)
        {
            _importer = new GraduateImport(_db);
            AddGraduates = _importer.ImportGraduates();

            RemoveGraduates = new ObservableCollection<Выпускник>();
        }

        private RelayCommand addGraduate;
        public RelayCommand AddGraduate
        {
            get
            {
                return addGraduate ??
                  (addGraduate = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (AddedGraduate != null)
                          {
                              var selectedGraduate = AddedGraduate;
                              AddGraduates.Add(selectedGraduate);
                              RemoveGraduates.Remove(selectedGraduate);
                              AddedGraduate = RemoveGraduates.LastOrDefault();
                          }
                      }
                      catch
                      {
                          MessageBox.Show("В случае этой ошибки стоит сразу обращаться к разработчику", "Не удалось переместить выпускника");
                      }
                  }));
            }
        }

        private RelayCommand removeGraduate;
        public RelayCommand RemoveGraduate
        {
            get
            {
                return removeGraduate ??
                  (removeGraduate = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (RemovedGraduate != null)
                          {
                              var selectedGraduate = RemovedGraduate;
                              AddGraduates.Remove(selectedGraduate);
                              RemoveGraduates.Add(selectedGraduate);
                              RemovedGraduate = AddGraduates.LastOrDefault();
                          }
                      }
                      catch
                      {
                          MessageBox.Show("В случае этой ошибки стоит сразу обращаться к разработчику", "Не удалось переместить выпускника");
                      }
                  }));
            }
        }

        private RelayCommand import;
        public RelayCommand Import
        {
            get
            {
                return import ??
                  (import = new RelayCommand(obj =>
                  {
                      try
                      {
                          _importer.FinalImport(AddGraduates);
                      }
                      catch
                      {
                          MessageBox.Show("В случае этой ошибки стоит сразу обращаться к разработчику", "Не удалось импортировать выпускников");
                      }
                  }));
            }
        }
    }
}
