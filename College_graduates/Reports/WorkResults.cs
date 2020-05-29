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
    class WorkResults
    {
        EntityContext db;
        public void Report(Год_выпуска year, Специальность specialty)
        {
            using (db = new EntityContext())
            {
                Excel.Application ex = new Excel.Application();
                Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
                Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);

                try
                {
                    db.Выпускники.Load();

                    sheet.Name = "Информация о работе за " + year.Номер_года;

                    sheet.Cells[2, 2] = "№ п/п";
                    sheet.Cells[2, 3] = "Фамилия, имя, отчество";
                    sheet.Cells[2, 4] = "1 полугодие";
                    sheet.Cells[2, 5] = "2 полугодие";
                    sheet.Cells[2, 6] = "3 полугодие";
                    sheet.Cells[2, 7] = "4 полугодие";
                    sheet.Cells[2, 8] = "5 полугодие";
                    sheet.Cells[2, 9] = "6 полугодие";

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
                        sheet.Cells[i, 4] = item.Распределение?.First().Информация?.Where(q => q?.Номер_полугодия == 1).Select(q => q?.Организация?.Наименование).First();
                        sheet.Cells[i, 5] = item.Распределение?.First().Информация?.Where(q => q?.Номер_полугодия == 2).Select(q => q?.Организация?.Наименование).First();
                        sheet.Cells[i, 6] = item.Распределение?.First().Информация?.Where(q => q?.Номер_полугодия == 3).Select(q => q?.Организация?.Наименование).First();
                        sheet.Cells[i, 7] = item.Распределение?.First().Информация?.Where(q => q?.Номер_полугодия == 4).Select(q => q?.Организация?.Наименование).First();
                        sheet.Cells[i, 8] = item.Распределение?.First().Информация?.Where(q => q?.Номер_полугодия == 5).Select(q => q?.Организация?.Наименование).First();
                        sheet.Cells[i, 9] = item.Распределение?.First().Информация?.Where(q => q?.Номер_полугодия == 6).Select(q => q?.Организация?.Наименование).First();

                        i++;
                    }

                    Excel.Range range = sheet.Range[sheet.Cells[2, 2], sheet.Cells[i - 1, 9]];

                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                    range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;

                    range.EntireRow.AutoFit();
                    range.EntireColumn.AutoFit();

                    ex.Visible = true;
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
