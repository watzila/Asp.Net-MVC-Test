using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using ZXing;

namespace MVC各種練習.Controllers {
	public class EmailController : Controller {

		public ActionResult Index( ) {
			return View( );
		}

		[HttpPost]
		public ActionResult Index( string email) {
			string smtpAddress = "smtp.gmail.com";//gmail服務器地址
			int port = 587;//gmail服務器阜號
			string myEmail = "jiansong@chis.com.tw";
			string myPwd = "jian4542song";
			string emailTo = email; 

			using(MailMessage mail = new MailMessage ()) {
				mail.From = new MailAddress(myEmail);//自己的信箱
				mail.To.Add(emailTo);//收信方email 可以用逗號區分多個收件人
				mail.Subject = "測試";//主旨
				//mail.Body = "";//內容
				mail.IsBodyHtml = true;//若你的內容是HTML格式，則為True

				//mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));//附件
				mail.AlternateViews.Add(EmailBody( ));

				using( SmtpClient smtp = new SmtpClient(smtpAddress, port) ) {
					smtp.Credentials = new NetworkCredential(myEmail, myPwd);
					smtp.EnableSsl = true;
					smtp.Send(mail);
				}
			}

				return View( );
		}

		public AlternateView EmailBody( ) {
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
			ms.Position = 0;

			//string path = Server.MapPath(@"Images/QRCode.jpg");
			//LinkedResource img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);

			LinkedResource img = new LinkedResource(ms, MediaTypeNames.Image.Jpeg);
			img.ContentId = "qr";
			string body = "<h1>Hello~~</h1><img src=cid:qr></img>";
			AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
			av.LinkedResources.Add(img);
			return av;
		}
	}
}