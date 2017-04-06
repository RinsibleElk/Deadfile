using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Deadfile.Infrastructure.Interfaces;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace Deadfile.Infrastructure.Services
{
    public sealed class ExcelService : IExcelService
    {
        public void Export(IDataGridPresenter dataGrid)
        {
            var dgrid = dataGrid.DataGrid;
            var excel = new Application {Visible = true};
            

            var workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            var sheet1 = (Worksheet)workbook.Sheets[1];

            for (var j = 0; j < dgrid.Columns.Count; j++)
            {
                var myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dgrid.Columns[j].Header;
            }
            for (var i = 0; i < dgrid.Columns.Count; i++)
            {
                var isDate = (dgrid.Columns[i].Header as string)?.Contains("Date") ?? false;
                for (var j = 0; j < dgrid.Items.Count; j++)
                {
                    var b = dgrid.Columns[i].GetCellContent(dgrid.Items[j]) as TextBlock;
                    var myRange = (Range)sheet1.Cells[j + 2, i + 1];
                    var text = b?.Text ?? "";
                    if (isDate)
                    {
                        var dt = DateTime.Parse(text).ToOADate();
                        myRange.NumberFormat = "DD/MM/YYYY";
                        myRange.Value2 = dt;
                    }
                    else
                        myRange.Value2 = text;
                }
            }
        }
    }
}
