using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Средства_обучения
    {
        [Key]
        public int ID_Средств_обучения { get; set; }
        public string Вид_средств_обучения { get; set; }
        public virtual ObservableCollection<Выпускник> Выпускники { get; set; }

        public Средства_обучения()
        {
            Вид_средств_обучения = "";
        }
    }
}
