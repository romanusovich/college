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

namespace College_graduates.Forms
{
    /// <summary>
    /// Логика взаимодействия для ImportForm.xaml
    /// </summary>
    public partial class ImportForm : Window
    {
        public ImportForm(EntityContext _db)
        {
            InitializeComponent();

            DataContext = new ImportViewModel(_db);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
