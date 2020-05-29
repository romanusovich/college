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
    /// Логика взаимодействия для YearOfOutlet.xaml
    /// </summary>
    public partial class YearOfOutlet : Window
    {
        public YearOfOutlet(EntityContext _db)
        {
            InitializeComponent();

            DataContext = new YearOfOutletViewModel(_db);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
