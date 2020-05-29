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
    /// Логика взаимодействия для Request.xaml
    /// </summary>
    public partial class Request : Window
    {
        public Request(EntityContext _db, Организация organization)
        {
            InitializeComponent();

            DataContext = new RequestViewModel(_db, organization);

            this.Title += (" " + organization.Наименование);
        }
    }
}
