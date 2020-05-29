using College_graduates.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_graduates
{
    class GraduateImport
    {
        EntityContext db;

        public GraduateImport(EntityContext _db)
        {
            db = _db;
        }

        // Метод на получение списка студентов последних курсов своих специальностей
        public ObservableCollection<Выпускник> ImportGraduates()
        {
            try
            {
                ObservableCollection<Выпускник> Graduates = new ObservableCollection<Выпускник>();

                var connection = System.Configuration.ConfigurationManager.ConnectionStrings["Мониторинг успеваемости"].ConnectionString;

                using (var con = new SqlConnection(connection))
                {
                    con.Open();

                    foreach (var item in db.Специальности.Local.ToList())
                    {
                        string sql = "SELECT Student.Fam, Student.Name, Student.Surname, LOWER(Specialnost.NazvanieB), Kyrs.NomerKyrsa " +
                            "FROM Gryppa INNER JOIN " +
                            "Kyrs ON Gryppa.IdKyrs = Kyrs.IdKyrs INNER JOIN " +
                            "Student ON Gryppa.IdGryppa = Student.IdGryppa INNER JOIN " +
                            "Specialnost ON Gryppa.IdSpecialnost = Specialnost.IdSpecialnost " +
                            "WHERE(Specialnost.NazvanieB = @Specialty) AND (Kyrs.NomerKyrsa = @Digit)";

                        SqlCommand cmd = new SqlCommand(sql, con);

                        SqlParameter specialty = new SqlParameter("@Specialty", item.Название_специальности.ToLower());
                        cmd.Parameters.Add(specialty);

                        SqlParameter digit = new SqlParameter("@Digit", System.Data.SqlDbType.Int);
                        digit.Value = 3;
                        if (item.Название_специальности.ToLower() == "ПОИТ".ToLower()) { digit.Value = 4; }
                        if (item.Название_специальности.ToLower() == "БУАиК".ToLower()) { digit.Value = 3; }
                        if (item.Название_специальности.ToLower() == "ОДвЛ".ToLower()) { digit.Value = 3; }
                        if (item.Название_специальности.ToLower() == "Правоведение".ToLower()) { digit.Value = 2; }
                        cmd.Parameters.Add(digit);

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var newGraduate = new Выпускник(db.Года_выпуска.Local.Where(q => q.Номер_года == DateTime.Now.Year).FirstOrDefault().ID_Года_выпуска)
                                    {
                                        Фамилия = reader.GetString(0),
                                        Имя = reader.GetString(1),
                                        Отчество = reader.GetString(2),
                                        ID_Специальности = db.Специальности.Local.Where(q => q.Название_специальности == item.Название_специальности).FirstOrDefault().ID_Специальности,
                                        Специальность = db.Специальности.Local.Where(q => q.ID_Специальности == item.ID_Специальности).FirstOrDefault(),
                                    };

                                    if (!db.Выпускники.Local
                                        .Where(q => q.Фамилия == newGraduate.Фамилия)
                                        .Where(q => q.Имя == newGraduate.Имя)
                                        .Where(q => q.Отчество == newGraduate.Отчество)
                                        .Where(q => q.ID_Года_выпуска == newGraduate.ID_Года_выпуска)
                                        .Any())
                                        Graduates.Add(newGraduate);
                                }
                            }
                        }
                    }
                }

                return Graduates;
            }
            catch (Exception ex)
            {
                var log = ex.Message;

                return null;
            }
        }

        // Добавление выбарнных стдуентов в основную базу приложения
        public void FinalImport(ObservableCollection<Выпускник> Graduates)
        {
            try
            {
                foreach (var item in Graduates)
                {
                    db.Выпускники.Local.Add(item);

                    item.Распределение = new ObservableCollection<Распределение>() { new Распределение(item.ID_Выпускника) };
                    item.Распределение.First().Информация = new ObservableCollection<Информация_о_распределении>();
                    for (int i = 0; i <= 6; i++)
                    {
                        item.Распределение.First().Информация.Add(new Информация_о_распределении() { ID_Распределения = item.Распределение.First().ID_Распределения, Номер_полугодия = i });
                    }
                }

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                var log = ex.Message;
            }
        }
    }
}
