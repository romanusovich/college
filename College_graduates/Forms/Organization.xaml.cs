using College_graduates.Class;
using College_graduates.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace College_graduates
{
    /// <summary>
    /// Логика взаимодействия для Organization.xaml
    /// </summary>
    public partial class Organization : Window
    {
        public Organization(EntityContext _db)
        {
            InitializeComponent();

            DataContext = new OrganizationViewModel(_db);
        }
    }
}
