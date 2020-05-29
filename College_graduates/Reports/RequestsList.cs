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
    class RequestsList
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
                    db.Заявки.Load();
                    db.Выпускники.Load();

                    sheet.Name = "Заявки по специальности";

                    sheet.Cells[1, 2] = "Заявки на специальность " + specialty.Название_специальности;
                    Excel.Range range = sheet.Range[sheet.Cells[1, 2], sheet.Cells[1, 6]];
                    range.Font.Size = 16;
                    range.Font.Bold = true;
                    range.Merge(Type.Missing);

                    sheet.Cells[2, 2] = "№ п/п";
                    sheet.Cells[2, 3] = "Наименование предприятия (организации)";
                    sheet.Cells[2, 4] = "Должность, профессия";
                    sheet.Cells[2, 5] = "Дата";
                    sheet.Cells[2, 6] = "Фамилия выпускника";

                    var requests = from q in db.Заявки.Local
                                   where q.ID_Специальности == specialty.ID_Специальности &&
                                         q.Дата.Year == year.Номер_года
                                   orderby q.Дата
                                   select q;

                    int i = 3;
                    foreach (var item in requests)
                    {
                        sheet.Cells[i, 2] = i - 2;
                        sheet.Cells[i, 3] = item.Организация.Наименование;
                        sheet.Cells[i, 4] = item.Должность;
                        sheet.Cells[i, 5] = item.Дата;
                        sheet.Cells[i, 6] = db.Выпускники.Local.Where(q => q.ID_Выпускника == item?.ID_Выпускника).FirstOrDefault()?.Фамилия;
                        i++;
                    }

                    range = sheet.Range[sheet.Cells[2, 2], sheet.Cells[i - 1, 6]];

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
