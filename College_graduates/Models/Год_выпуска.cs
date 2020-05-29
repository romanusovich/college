using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Год_выпуска
    {
        [Key]
        public int ID_Года_выпуска { get; set; }
        public int Номер_года { get; set; }

        public virtual ObservableCollection<Выпускник> Выпускники { get; set; }

        public Год_выпуска()
        {
            Номер_года = 0;
        }
    }
}
