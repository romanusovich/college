using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Заявка
    {
        [Key]
        public int ID_Заявки { get; set; }

        public int? ID_Организации { get; set; }
        public virtual Организация Организация { get; set; }

        public int? ID_Специальности { get; set; }
        public virtual Специальность Специальность { get; set; }

        public int Количество_мест { get; set; }
        public string Должность { get; set; }
        public DateTime Дата { get; set; }
        public int Номер { get; set; }

        public int? ID_Выпускника { get; set; } // Не обязательное свойство

        public string ConcatProperty { get { return Специальность?.Название_специальности + " " + Номер + " " + Организация?.Наименование + " " + Должность; } }

        public virtual ObservableCollection<Распределение> Распределения { get; set; }

        public Заявка()
        {

        }

        public Заявка(int id_организации)
        {
            ID_Организации = id_организации;
            Дата = DateTime.Now;
        }
    }
}
