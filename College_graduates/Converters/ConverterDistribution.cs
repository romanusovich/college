using College_graduates.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace College_graduates
{
    public class ConverterDistribution : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var distribution = (ObservableCollection<Распределение>)value;

            if (distribution == null || distribution.Count == 0)
                return null;

            switch (parameter)
            {
                case "Куда_получено":
                    return distribution.First().Заявка?.Организация?.Наименование;

                case "Дата_получения":
                    return distribution.First().Дата_получения_направления.ToShortDateString();

                case "Куда_прибыл":
                    return distribution.First().Куда_прибыл;

                case "Когда_прибыл":
                    return distribution.First().Когда_прибыл.ToShortDateString();

                default:
                    return distribution.First();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
