using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class ExcelLoader
{
    public static ExcelData Load(string filepath)
    {
        ExcelData ret = null;

        if (File.Exists(filepath))
        {
            using (var stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
            {
                List<ExcelSheet> sheets = new List<ExcelSheet>();

                IWorkbook book = WorkbookFactory.Create(stream);

                int sheets_count = book.NumberOfSheets;

                for (int i = 0; i < sheets_count; ++i)
                {
                    ISheet sheet = book.GetSheetAt(0);

                    string name = sheet.SheetName;

                    int size_x = sheet.GetRow(0).LastCellNum; // columns
                    int size_y = sheet.LastRowNum + 1; // rows

                    string[,] excel_data = new string[size_y, size_x];

                    for (int y = 0; y < size_y; ++y)
                    {
                        IRow row = sheet.GetRow(y);

                        if (row != null && row.PhysicalNumberOfCells > 0)
                        {
                            for (int x = 0; x < size_x; ++x)
                            {
                                ICell cell = row.GetCell(x);

                                if (cell != null)
                                {
                                    cell.SetCellType(CellType.String);

                                    string cell_value = cell.StringCellValue;

                                    excel_data[y, x] = cell_value;
                                }
                            }
                        }
                    }

                    ExcelSheet excel = new ExcelSheet(name, size_y, size_x, excel_data);
                    sheets.Add(excel);
                }

                ret = new ExcelData(sheets);
            }
        }

        return ret;
    }
}

public class ExcelSheet








{
    public ExcelSheet(string name, int rows, int columns, string[,] excel_data)
    {
        this.name = name;
        this.rows = rows;
        this.columns = columns;

        this.excel_data = excel_data;
    }

    public string GetName()
    {
        return name;
    }

    public int GetRows()
    {
        return rows;
    }

    public int GetColumns()
    {
        return columns;
    }

    public string GetDataAt(int row, int column)
    {
        string ret = "";

        if (excel_data != null)
        {
            if (rows > row && columns > column)
            {
                ret = excel_data[row, column];
            }
        }

        return ret;
    }

    private string name = "";

    private string[,] excel_data = null;

    private int rows = 0;
    private int columns = 0;
}

public class ExcelData
{
    public ExcelData(List<ExcelSheet> sheets)
    {
        this.sheets = sheets;
    }

    public int GetSheetsCount()
    {
        return sheets.Count;
    }

    public ExcelSheet GetSheetAt(int index)
    {
        ExcelSheet ret = null;

        if (index < sheets.Count)
        {
            ret = sheets[index];
        }

        return ret;
    }

    private List<ExcelSheet> sheets = new List<ExcelSheet>();
}
