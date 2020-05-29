using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Перераспределение
    {
        [Key]
        public int ID_Перераспределения { get; set; }

        public int? ID_Выпускника { get; set; }
        public virtual Выпускник Выпускник { get; set; }

        public int? ID_Организации { get; set; }
        public virtual Организация Организация { get; set; }

        public DateTime Дата_перераспределения { get; set; }
        public string Должность { get; set; }
        public string Номер_подтверждения { get; set; }

        public Перераспределение()
        {
            
        }

        public Перераспределение(int? id_graduate, int? id_organization)
        {
            ID_Выпускника = id_graduate;
            ID_Организации = id_organization;
            Дата_перераспределения = DateTime.Now;
            Должность = "";
        }
    }
}
