using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Организация
    {
        [Key]
        public int ID_Организации { get; set; }
        public string Наименование { get; set; }

        public int? ID_Района { get; set; }
        public Район Район { get; set; }

        public string Населенный_пункт { get; set; }
        public string Адрес { get; set; }

        public virtual ObservableCollection<Заявка> Заявки { get; set; }
        public virtual ObservableCollection<Перераспределение> Перераспределения { get; set; }
        public virtual ObservableCollection<Информация_о_распределении> Информация { get; set; }

        public Организация()   
        {
            Наименование = "";
            Заявки = new ObservableCollection<Заявка>();
        }
    }
}
