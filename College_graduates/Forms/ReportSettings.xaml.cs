using College_graduates.Class;
using College_graduates.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace College_graduates.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportSettings.xaml
    /// </summary>
    public partial class ReportSettings : Window
    {
        public ReportSettings(bool year, bool period, bool specialty, bool organization, string type, EntityContext _db)
        {
            InitializeComponent();

            DataContext = new ReportViewModel(_db, type);

            cbYears.IsEnabled = year;
            dpFrom.IsEnabled = period;
            dpTo.IsEnabled = period;
            cbSpecialtys.IsEnabled = specialty;
            cbOrganizations.IsEnabled = organization;
        }

        private void btnForm_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
