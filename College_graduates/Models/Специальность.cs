using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Специальность
    {
        [Key]
        public int ID_Специальности { get; set; }
        public string Название_специальности { get; set; }
        public virtual ObservableCollection<Выпускник> Выпускники { get; set; }
        public virtual ObservableCollection<Заявка> Заявки { get; set; }

        public Специальность()
        {
            Название_специальности = "";
        }
    }
}
