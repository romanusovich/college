using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates.Class
{
    public class EntityContext : DbContext
    {
        public EntityContext() : base("Учет выпускников колледжа")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<Выпускник> Выпускники { get; set; }
        public DbSet<Специальность> Специальности { get; set; }
        public DbSet<Год_выпуска> Года_выпуска { get; set; }
        public DbSet<Средства_обучения> Средства_обучения { get; set; }
        public DbSet<Заявка> Заявки { get; set; }
        public DbSet<Организация> Организации { get; set; }
        public DbSet<Распределение> Распределение { get; set; }
        public DbSet<Информация_о_распределении> Информация { get; set; }
        public DbSet<Перераспределение> Перераспределение { get; set; }
        public DbSet<Район> Районы { get; set; }

    }
}
