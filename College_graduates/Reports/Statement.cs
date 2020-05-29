using College_graduates.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace College_graduates.Reports
{
    class Statement
    {
        EntityContext db;
        public void Report(Специальность specialty, Год_выпуска year)
        {
            using (db = new EntityContext())
            {
                Excel.Application ex = new Excel.Application();
                Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
                Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);

                try
                {
                    db.Выпускники.Load();

                    sheet.Name = "Ведомость от " + year.Номер_года;

                    sheet.Cells[1, 2] = "Ведомость от " + year.Номер_года + "; Специальность - " + specialty.Название_специальности;
                    Excel.Range range = sheet.Range[sheet.Cells[1, 2], sheet.Cells[1, 5]];
                    range.Font.Size = 16;
                    range.Font.Bold = true;
                    range.Merge(Type.Missing);

                    sheet.Cells[2, 2] = "№ п/п";
                    sheet.Cells[2, 3] = "Фамилия, имя, отчество";
                    sheet.Cells[2, 4] = "Наименование предприятия (организации)";
                    sheet.Cells[2, 5] = "Должность, профессия";


                    var graduates = from q in db.Выпускники.Local
                                where q.ID_Специальности == specialty.ID_Специальности &&
                                      q.ID_Года_выпуска == year.ID_Года_выпуска
                                orderby q.Фамилия
                                select q;
                    int i = 3;
                    foreach (var item in graduates)
                    {
                        sheet.Cells[i, 2] = i - 2;
                        sheet.Cells[i, 3] = item.Фамилия + " " + item.Имя + " " + item.Отчество;
                        sheet.Cells[i, 4] = item.Распределение.First()?.Заявка?.Организация?.Наименование;
                        sheet.Cells[i, 5] = item.Распределение.First()?.Информация.FirstOrDefault(q => q.Номер_полугодия == 0).Должность;
                        i++;
                    }

                    range = sheet.Range[sheet.Cells[2, 2], sheet.Cells[i - 1, 5]];

                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;

                    range.EntireRow.AutoFit();
                    range.EntireColumn.AutoFit();

                    ex.Visible = true;
                    range = null;
                }
                catch
                {
                    MessageBox.Show("Выберите все необходимые параметры" +
                        "\nЕсли параметры выбраны, а ошибка осталась, то обратитесь к разработчику", "Не удалось сформировать отчет");

                    workBook.Close(false);
                    ex.Quit();
                    ex = null;
                    workBook = null;
                    sheet = null;
                }
            }
        }
    }
}
