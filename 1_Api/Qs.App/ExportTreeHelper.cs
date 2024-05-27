using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Qs.Comm;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Qs.Comm.Extensions;

namespace App
{
    class MIMEType
    {
        public const string xls = "application/ms-excel";
    }
    /// <summary>
    /// 描 述：NPOI Excel DataTable操作类
    /// </summary>
    public class ExportTreeHelper
    {
        #region ISheet单元格样式

        /// <summary>
        /// 返回单元格颜色样式
        /// </summary>
        /// <param name="color">传入的颜色HSSFColor.Blue.Index</param>
        /// <param name="workbook">ISheet</param>
        /// <returns></returns>
        public static ICellStyle cellColor(short color, IWorkbook workbook)
        {
            ICellStyle dtlStyle = workbook.CreateCellStyle();
            IFont dtlFont = workbook.CreateFont();
            dtlFont.Color = color;
            dtlStyle.SetFont(dtlFont);
            return dtlStyle;
        }

        /// <summary>
        /// 返回单元格数值型样式
        /// </summary>
        /// <param name="workbook">ISheet</param>
        /// <param name="format">数值样式0.0000</param>
        /// <returns></returns>
        public static ICellStyle cellNumericalFomat(IWorkbook workbook, string strFormat)
        {
            IDataFormat format = workbook.CreateDataFormat();
            ICellStyle cellPriceStyle = workbook.CreateCellStyle();
            cellPriceStyle.DataFormat = format.GetFormat(strFormat);
            return cellPriceStyle;
        }

        #endregion

        /// <summary>
        /// 如果文件名中没有后缀名，增加文件后缀名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string JointXls(string fileName)
        {
            if (!fileName.EndsWith(".xls"))
            {
                fileName += ".xls";
            }

            return fileName;
        }

        // 然后就是导出的方法，传入的参数为dtMain数据集，fileNameEXCEL名称，headerText表格标题

        public ActionResult ExportToExcel(DataTable dtMain, string fileName, string sheetName)
        {
            // using ()
            // {
            MemoryStream ms = GetDataToStr(dtMain, sheetName);
                return new FileStreamResult(ms, MIMEType.xls) {FileDownloadName = JointXls(fileName)};
            // }
        }

        //循环数据，得到数据流

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtMain"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public MemoryStream GetDataToStr(DataTable dtMain, string sheetName)
        {
            // MemoryStream ms = new MemoryStream();
            // IWorkbook workbook = NPOIHelper.CreateWorkbook(type)
            HSSFWorkbook workbook = new HSSFWorkbook();
            {
                ISheet sheet = workbook.CreateSheet(sheetName);
                {
                    int rowIndex = 0;
                    List<string> heads = GetExcelHead();

                    #region 表头及样式

                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        IRow headerRow = sheet.CreateRow(rowIndex++);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(sheetName);


                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;


                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, heads.Count - 1));
                    }

                    #endregion


                    #region 列头及样式

                    {
                        IRow headerRow = sheet.CreateRow(rowIndex++);
                        headerRow.HeightInPoints = 15;
                        // ICellStyle headStyle = Function.CellColor(HSSFColor.Black.Index, workbook);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        headStyle.FillForegroundColor = HSSFColor.BlueGrey.Index;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        for (int i = 0; i < heads.Count; i++)
                        {
                            ICell cell = headerRow.CreateCell(i);
                            cell.SetCellValue(heads[i]);
                            cell.CellStyle = headStyle;
                            sheet.SetColumnWidth(i, 50 * 90);
                        }
                    }

                    #endregion


                    //获取第一层级，并循环构建单元格。注：我这里的数据集第一层级是PID父级ID为空，并且Level层级为1的，这里你需要改一下符合你自己的数据集。
                    List<DataRow> listDr = dtMain.AsEnumerable().Where(dr =>
                        string.IsNullOrEmpty(dr["ParentId"].ToString()) && dr["Level"].ToString() == "1"
                    ).ToList();
                    foreach (DataRow dr in listDr)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);
                        Dictionary<int, string> colMap = GetDutyColMap();
                        //根据主表该行的列名，判断该列的样式，并赋值
                        getCellByCol(dr, dataRow, colMap, workbook);
                        rowIndex++;
                        //获取该级的下级,并循环构建单元格
                        getchildDrToCell(dr, dtMain, ref rowIndex, colMap, workbook, sheet);
                    }
                }
                sheet.CreateFreezePane(dtMain.Columns.Count, 2); //冻结窗格 ，把标题和列头冻结 
                sheet.SetColumnWidth(0, 50 * 100);
                sheet.SetColumnWidth(1, 70 * 160);
                // workbook.Write(ms);


                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
            // return ms;
        }



        /// <summary>
        /// 获取下级的数据，并构建行
        /// </summary>
        /// <param name="dr">当前行</param>
        /// <param name="dt">数据集</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colMap">数据列名</param>
        /// <param name="workbook">IWorkbook</param>
        /// <param name="sheet">ISheet</param>
        private void getchildDrToCell(DataRow dr, DataTable dt, ref int rowIndex, Dictionary<int, string> colMap,
            IWorkbook workbook
            , ISheet sheet)
        {
            //获取该级的下级,并循环构建单元格
            List<DataRow> dtlListDr = dt.AsEnumerable()
                .Where(dtl => dtl["ParentId"].ToString() == dr["UserId"].ToString()).ToList();
            if (dtlListDr.Count() > 0)
            {
                int startRow = rowIndex;
                foreach (DataRow drDtl in dtlListDr)
                {
                    IRow dataDtlRow = sheet.CreateRow(rowIndex);
                    //构建当前行
                    getCellByCol(drDtl, dataDtlRow, colMap, workbook);
                    rowIndex++;
                    //递归寻找下级
                    getchildDrToCell(drDtl, dt, ref rowIndex, colMap, workbook, sheet);
                }

                sheet.GroupRow(startRow, rowIndex);
                sheet.SetRowGroupCollapsed(startRow, false);
            }
        }



        /// <summary>
        /// 根据列，构建表格行 这个方法，实际是处理金额单价等小数位数。这里就用到了我们最开始新建的那个单元格样式几个方法。这里需要根据实际情况来写，比如单价金额需要保留几位小数四舍五入等需要特殊处理的。
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dataRow"></param>
        /// <param name="colMap"></param>
        /// <param name="workbook"></param>
        private void getCellByCol(DataRow dr, IRow dataRow, Dictionary<int, string> colMap, IWorkbook workbook)
        {
            //根据主表该行的列名，判断该列的样式，并赋值
            foreach (KeyValuePair<int, string> kv in colMap)
            {
                ICell cell = dataRow.CreateCell(kv.Key);
                // //单价是3位小数，金额0位，数量4位
                // if (kv.Value == "ListQty")
                // { cell.CellStyle = Function.cellNumericalFomat(workbook, "0.0000"); }
                // else if (kv.Value == "Price")
                // { cell.CellStyle = Function.cellNumericalFomat(workbook, "0.000"); }
                // else if (kv.Value == "Money")
                // { cell.CellStyle = Function.cellNumericalFomat(workbook, "0"); }
                // if (kv.Value == "Qty" || kv.Value == "Price" || kv.Value == "Money")
                // {
                //     cell.SetCellValue(Convert.ToDouble(dr[kv.Value].SyToDecimal()));
                // }
                // else cell.SetCellValue(dr[kv.Value].SyToString());
                cell.SetCellValue(dr[kv.Value].ToString());
            }
        }



        /// <summary>
        /// 这个方法是告诉程序我们需要导出的字段中文名字，
        /// </summary>
        /// <returns></returns>      
        private List<string> GetExcelHead()
        {
            List<string> heads = new List<string>();
            heads.AddRange(new string[] {"手机号", "用户", "下级人数", "团队人数"});
            return heads;
        }

        /// <summary>
        /// 这个是告诉程序我们对应的数据集里面的列名。注：这个方法必须和上面GetExcelHead列名字段严格一一对应，不允许多列少列错位。
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetDutyColMap()
        {
            Dictionary<int, string> colMap = new Dictionary<int, string>();
            colMap.Add(0, "Phone");
            colMap.Add(1, "Name");
            colMap.Add(2, "SonCount");
            colMap.Add(3, "TeamCount");
            return colMap;
        }
    }
}
