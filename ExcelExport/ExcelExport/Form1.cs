using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System;
using System.Data.Entity.Migrations.Model;
using System.Drawing;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        List<Flat> flats;
        RealEstateEntities context = new RealEstateEntities();
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet XlSheet;
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }
        private void LoadData()
        {
            context.Flats.Load();
            flats = context.Flats.ToList();
        }
        private void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value);
                XlSheet = xlWB.ActiveSheet;
                CreateTable();
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {

                string errMsg = string.Format($"Error: {ex.Message} \n" +
                                              $"Line: {ex.Source}");
                xlWB.Close();
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }

        }
        private void CreateTable()
        {
            string[] headers = new string[]
            {
                "Kód",
                "Eladó",
                "Oldal",
                "Kerület",
                "Lift",
                "Szobák száma",
                "Alapterület (m2)",
                "Ár (mFt)",
                "Négyzetméter ár (Ft/m2)"
            };
            for (int i = 0; i < headers.Length; i++)
            {
                XlSheet.Cells[1, 1 + i] = headers[i];
            }
            object[,] values = new object[flats.Count, headers.Length];
            for (int i = 0; i < flats.Count; i++)
            {
                values[i, 0] = flats[i].Code;
                values[i, 1] = flats[i].Vendor;
                values[i, 2] = flats[i].Side;
                values[i, 3] = flats[i].District;
                values[i, 4] = flats[i].Elevator;
                values[i, 5] = flats[i].NumberOfRooms;
                values[i, 6] = flats[i].FloorArea;
                values[i, 7] = flats[i].Price;
                values[i, 8] = $"={GetCell(2+i,8)}/{GetCell(2+i,7)}";
            }
            XlSheet.Range[GetCell(2, 1), GetCell(1 + values.GetLength(0), values.GetLength(1))].Value2 = values;
            formatHeader(headers);
            formatTable(headers);
            firstCol();
            lastCol(headers);

        }
        private void lastCol(string[] headers)
        {
            Excel.Range colRange = XlSheet.Range[GetCell(2, headers.Length), GetCell(XlSheet.UsedRange.Rows.Count-1, headers.Length)];
            colRange.Interior.Color = Color.LightGreen;
            colRange.NumberFormat = "#,##0.00";
        }
        private void firstCol()
        {
            Excel.Range colRange = XlSheet.Range[GetCell(2, 1), GetCell(XlSheet.UsedRange.Rows.Count-1, 1)];
            colRange.Interior.Color = Color.LightYellow;
            colRange.Font.Italic = true;
        }
        private void formatTable(string[] headers)
        {
            Excel.Range tableRange = XlSheet.Range[GetCell(2, 1), GetCell(XlSheet.UsedRange.Rows.Count, headers.Length)];
            tableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
        }
        private void formatHeader(string[] headers)
        {
            Excel.Range headerRange = XlSheet.Range[GetCell(1, 1), GetCell(1, headers.Length)];
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
        }
        private string GetCell(int x, int y)
        {
            string rtn = "";
            int dividend = y;
            int modulo;
            while (dividend >0)
            {
                modulo = (dividend - 1) % 26;
                rtn = $"{Convert.ToChar(65 + modulo)}{rtn}";
                dividend = (int)((dividend - modulo) / 26);
            }
            rtn = $"{rtn}{x}";
            return rtn;
        }
    }
}
