using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtuaBusiness.Arquivos
{
    public class ExcelBLL : IExcelBLL
    {
        private readonly ILogger<ExcelBLL> _logger;

        public ExcelBLL(ILogger<ExcelBLL> logger)
        {
            _logger = logger;
        }

        public List<String> LerPlanilhaFreteGMBXlsx(String Arquivo)
        {
            var lista = new List<String>();
            String item = "";
            try
            {
                XSSFWorkbook wb;
                using (FileStream file = new FileStream(Arquivo, FileMode.Open, FileAccess.Read))
                {
                    wb = new XSSFWorkbook(file);
                }
              
                ISheet sheet = wb.GetSheetAt(0);

                var rows = sheet.GetRowEnumerator();
                rows.MoveNext();

                IRow r = (XSSFRow)rows.Current;
                int z = r.LastCellNum ;
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    // 11 12
                    if (sheet.GetRow(row) != null) // quando a linha é nula pular
                    {

                        for (int i = 0; i < z; i++)
                        {
                            var cell = sheet.GetRow(row).GetCell(i);//valida o conteudo da celula

                            if (i==0)
                            {
                                item = getCellValue(cell).ToString() + ";";
                            }
                            else
                            {
                                item += sheet.GetRow(row).GetCell(i).ToString() + ";";
                            }
                        }
                        if(!String.IsNullOrWhiteSpace(item))
                        lista.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                var test = ex.Message + " " + item;
                _logger.LogError(test, "Erro em LerPlanilhaFreteGMBXlsx");
            }

            return lista;
        }

        public List<String> LerPlanilhaFreteGMBXls(String Arquivo)
        {
            var lista = new List<String>();
            String item = "";
            try
            {
                HSSFWorkbook wb;
                using (FileStream file = new FileStream(Arquivo, FileMode.Open, FileAccess.Read))
                {
                    wb = new HSSFWorkbook(file);
                }

                ISheet sheet = wb.GetSheetAt(0);

                for (int row = 2; row <= sheet.LastRowNum; row++)
                {

                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                    {
                        item = sheet.GetRow(row).GetCell(0).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(1).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(2).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(3).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(4).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(5).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(6).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(7).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(8).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(9).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(10).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(11).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(12).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(13).ToString() + ";";
                        item += sheet.GetRow(row).GetCell(14).ToString();



                        lista.Add(item);
                    }


                }
            }
            catch (Exception ex)
            {

                var test = ex.Message + " " + item;

                _logger.LogError(test, "Erro em LerPlanilhaFreteGMBXls");
            }

            return lista;
        }

        private object getCellValue(ICell cell)
        {
            object cValue = string.Empty;
            switch (cell.CellType)
            {
                case (CellType.Unknown | CellType.Formula | CellType.Blank):
                    cValue = cell.ToString();
                    break;
                case CellType.Numeric:
                    cValue = cell.NumericCellValue;
                    break;
                case CellType.String:
                    cValue = cell.StringCellValue;
                    break;
                case CellType.Boolean:
                    cValue = cell.BooleanCellValue;
                    break;
                case CellType.Error:
                    cValue = cell.ErrorCellValue;
                    break;
                default:
                    cValue = string.Empty;
                    break;
            }
            return cValue;
        }
    }
}
