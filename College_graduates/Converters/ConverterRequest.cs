using College_graduates.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace College_graduates
{
    class ConverterRequest : IValueConverter
    {
        EntityContext db;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            db = new EntityContext();
            db.Выпускники.Load();

            if (value == null)
                return value;

            switch (parameter)
            {
                case "ToFIO":
                    return db.Выпускники.Where(q => q.ID_Выпускника == (int)value).Select(q => q.Фамилия + " " + q.Имя + " " + q.Отчество).FirstOrDefault();

                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
