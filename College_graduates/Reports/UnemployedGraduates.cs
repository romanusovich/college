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
    class UnemployedGraduates
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

                    sheet.Name = "Список выпускников " + year.Номер_года;

                    sheet.Cells[1, 2] = "Список не доехавших выпускников \nза " + year.Номер_года + "; Специальность - " + specialty.Название_специальности;
                    Excel.Range range = sheet.Range[sheet.Cells[1, 2], sheet.Cells[1, 5]];
                    range.Font.Size = 16;
                    range.Font.Bold = true;
                    range.Merge(Type.Missing);
                    range.RowHeight = 45;

                    sheet.Cells[2, 2] = "№ п/п";
                    sheet.Cells[2, 3] = "Фамилия, имя, отчество";

                    var graduates = from graduate in db.Выпускники.Local
                                    where graduate.ID_Года_выпуска == year.ID_Года_выпуска &&
                                          graduate.ID_Специальности == specialty.ID_Специальности &&
                                          graduate.Распределение.First().Куда_прибыл == null
                                    orderby graduate.Фамилия
                                    select graduate;
                    int i = 3;
                    foreach (var item in graduates)
                    {
                        sheet.Cells[i, 2] = i - 2;
                        sheet.Cells[i, 3] = item.Фамилия + " " + item.Имя + " " + item.Отчество;
                        i++;
                    }

                    range = sheet.Range[sheet.Cells[2, 2], sheet.Cells[i - 1, 3]];

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
