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
    public class Распределение
    {
        [Key]
        public int ID_Распределения { get; set; }

        public int? ID_Выпускника { get; set; }
        public virtual Выпускник Выпускник { get; set; }

        public int? ID_Заявки { get; set; }
        public virtual Заявка Заявка { get; set; }

        public DateTime Дата_получения_направления { get; set; }
        public string Куда_прибыл { get; set; }
        public DateTime Когда_прибыл { get; set; }
        public string Номер_подтверждения { get; set; }

        public virtual ObservableCollection<Информация_о_распределении> Информация { get; set; }

        public Распределение()
        {

        }

        public Распределение(int id_выпускника)
        {
            ID_Выпускника = id_выпускника;
            Дата_получения_направления = DateTime.Now;
            Когда_прибыл = DateTime.Now;
        }
    }
}
