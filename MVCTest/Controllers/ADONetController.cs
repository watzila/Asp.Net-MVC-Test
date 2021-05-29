using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;//取得Web.config檔案裡的connectionStrings的值用
using System.Data.SqlClient;//和資料庫連線用
using MVCTest.Models;//引入Models資料夾的檔案

namespace MVCTest.Controllers {
	public class ADONetController : Controller {
		string dbString = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;
		public ActionResult Index( ) {
			using( SqlConnection conn = new SqlConnection(dbString) ) {//資料庫連線，使用using可以執行完時自動關閉連線

			}

				return View( );
		}

		public ActionResult Create( ) {
			return View( );
		}

		[HttpPost]
		public ActionResult Create(ADONet data) {
			return RedirectToAction( "Index");
		}
	}
}