using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace MVC各種練習.Controllers {
	public class QRCodeController : Controller {

		public ActionResult Index( ) {
			ViewBag.img= CreateQRCode2( );
			return View( );
		}

		//創建QRCode
		public void CreateQRCode( ) {
			System.IO.MemoryStream ms = new System.IO.MemoryStream( );//記憶流

			//創建QRCode
			BarcodeWriter writer = new BarcodeWriter {
				Format = BarcodeFormat.QR_CODE,//條碼類型
				Options = new ZXing.QrCode.QrCodeEncodingOptions {
					Width = 500,//寬
					Height = 500,//高
					CharacterSet = "UTF-8"
				}
			};

			System.Drawing.Bitmap bitmap = writer.Write("123");//QRCode寫入字串

			bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);//點陣圖以jpeg格式寫入到記憶流裡

			Response.Clear( );//緩衝區內容清除
			Response.ContentType = "image/jpge";//緩衝區資料內容型態
			Response.BinaryWrite(ms.GetBuffer( ));//將二進位字元的字串寫入 HTTP 輸出資料流
			//Response.Flush( );//緩衝區所有資料發送到客戶端
			Response.End( );//緩衝區所有資料發送到客戶端並關閉
		}

		public static string CreateQRCode2( ) {
			System.IO.MemoryStream ms = new System.IO.MemoryStream( );//記憶流

			//創建QRCode
			BarcodeWriter writer = new BarcodeWriter {
				Format = BarcodeFormat.QR_CODE,//條碼類型
				Options = new ZXing.QrCode.QrCodeEncodingOptions {
					Width = 500,//寬
					Height = 500,//高
					CharacterSet = "UTF-8"
				}
			};

			System.Drawing.Bitmap bitmap = writer.Write("123");//QRCode寫入字串

			bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);//點陣圖以jpeg格式寫入到記憶流裡

			byte[ ] arr = ms.ToArray();

			return Convert.ToBase64String(arr);
		}

		public void aa( ) {
			Response.Write("這樣也可以輸出");
		}
	}
}