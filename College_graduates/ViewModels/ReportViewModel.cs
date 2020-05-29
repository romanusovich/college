using College_graduates.Class;
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

namespace College_graduates.ViewModels
{
    class ReportViewModel : INotifyPropertyChanged
    {
        EntityContext db;

        public ObservableCollection<Организация> Organizations { get; set; }
        public ObservableCollection<Специальность> Specialtys { get; set; }
        public ObservableCollection<Год_выпуска> Years { get; set; }

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

        private Год_выпуска _selectedYear;
        public Год_выпуска SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; NotifyPropertyChanged(); }
        }

        private DateTime _selectedDateFrom;
        public DateTime SelectedDateFrom
        {
            get { return _selectedDateFrom; }
            set { _selectedDateFrom = value; NotifyPropertyChanged(); }
        }

        private DateTime _selectedDateTo;
        public DateTime SelectedDateTo
        {
            get { return _selectedDateTo; }
            set { _selectedDateTo = value; NotifyPropertyChanged(); }
        }

        public string Type;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReportViewModel(EntityContext _db, string _type)
        {
            db = _db;

            Organizations = db.Организации.Local;
            Specialtys = db.Специальности.Local;
            Years = db.Года_выпуска.Local;

            Type = _type;

            SelectedDateFrom = DateTime.Now;
            SelectedDateTo = DateTime.Now;
        }

        // Команда на открытие формы
        private RelayCommand formReport;
        public RelayCommand FormReport
        {
            get
            {
                return formReport ??
                  (formReport = new RelayCommand(obj =>
                  {
                      switch (Type)
                      {
                          case "Statement":
                              Statement statement = new Statement();
                              statement.Report(SelectedSpecialty, SelectedYear);
                              break;

                          case "RequestsList":
                              RequestsList requestsList = new RequestsList();
                              requestsList.Report(SelectedYear, SelectedSpecialty);
                              break;

                          case "WorkResults":
                              WorkResults workResults = new WorkResults();
                              workResults.Report(SelectedYear, SelectedSpecialty);
                              break;

                          case "OrganizationRequests":
                              OrganizationRequests organizationRequests = new OrganizationRequests();
                              organizationRequests.Report(SelectedYear, SelectedOrganization);
                              break;

                          case "OrganizationRequestsPeriod":
                              OrganizationRequestsPeriod organizationRequestsPeriod = new OrganizationRequestsPeriod();
                              organizationRequestsPeriod.Report(SelectedDateFrom, SelectedDateTo, SelectedOrganization);
                              break;

                          case "UnemployedGraduates":
                              UnemployedGraduates unemployedGraduates = new UnemployedGraduates();
                              unemployedGraduates.Report(SelectedSpecialty, SelectedYear);
                              break;

                          case "EnteredGraduates":
                              EnteredGraduates enteredGraduates = new EnteredGraduates();
                              enteredGraduates.Report(SelectedSpecialty, SelectedYear);
                              break;

                          case "OwnGraduates":
                              OwnGraduates ownGraduates = new OwnGraduates();
                              ownGraduates.Report(SelectedSpecialty, SelectedYear);
                              break;
                      }
                  }));
            }
        }
    }
}
