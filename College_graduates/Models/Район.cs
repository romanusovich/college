using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class Район
    {
        [Key]
        public int ID_Района { get; set; }
        public string Название_района { get; set; }
        public virtual ObservableCollection<Организация> Организации { get; set; }

        public Район()
        {
            Название_района = "";
        }
    }
}
