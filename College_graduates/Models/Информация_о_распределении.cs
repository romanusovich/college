using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Информация_о_распределении
    {
        [Key]
        public int ID_Информации { get; set; }

        public int? ID_Распределения { get; set; }
        public virtual Распределение Распределение { get; set; }

        public int Номер_полугодия { get; set; }

        public int? ID_Организации { get; set; }
        public virtual Организация Организация { get; set; }

        public string Должность { get; set; }

        public Информация_о_распределении()
        {
            
        }
    }
}
