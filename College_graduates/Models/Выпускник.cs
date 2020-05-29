using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace College_graduates.Class
{
    public class Выпускник
    {
        [Key]
        public int ID_Выпускника { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }

        public int? ID_Специальности { get; set; }
        public virtual Специальность Специальность { get; set; }

        public int? ID_Средств_обучения { get; set; }
        public virtual Средства_обучения Средства_обучения { get; set; }

        public int? ID_Года_выпуска { get; set; }
        public virtual Год_выпуска Год_Выпуска { get; set; }

        public virtual ObservableCollection<Распределение> Распределение { get; set; }
        public virtual ObservableCollection<Перераспределение> Перераспределение { get; set; }

        public Выпускник()
        {
            Фамилия = "";
            Имя = "";
            Отчество = "";
        }

        public Выпускник(int id_year)
        {
            ID_Года_выпуска = id_year;
            Фамилия = "";
            Имя = "";
            Отчество = "";
        }
    }
}
