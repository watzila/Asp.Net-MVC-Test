using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Controllers {
	public class ExcelController : Controller {
		// GET: Execl
		public ActionResult Index( ) {
			return View( );
		}

		[HttpPost]
		public ActionResult Update( IEnumerable<HttpPostedFileBase> excel ) {
			IWorkbook wb = null;
			var file = excel.First( );

			if( file.FileName.IndexOf(".xls") > 0 ) {
				wb = new HSSFWorkbook(file.InputStream);
			} else if( file.FileName.IndexOf(".xlsx") > 0 ) {
				wb = new XSSFWorkbook(file.InputStream);
			}

			if( wb != null ) {
				for( int i = 0; i < wb.NumberOfSheets; i++ ) {
					ISheet sheet = wb.GetSheetAt(i);
					DataTable dataTable = new DataTable( );
					dataTable.TableName = sheet.SheetName;

					if( sheet != null ) {
						int rowCount = sheet.LastRowNum;

						if( rowCount > 0 ) {
							IRow firstRow = sheet.GetRow(0);
							int cellCount = firstRow.LastCellNum;

							for( int j = firstRow.FirstCellNum; j < cellCount; j++) {
								ICell cell = firstRow.GetCell(i);

								if( cell != null ) {
									if( cell.StringCellValue != null ) {
										DataColumn column = new DataColumn(cell.StringCellValue);
										dataTable.Columns.Add(column);
									}
								}
							}
						}
					}
				}
			}

			return View( );
		}
	}
}