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
		string dbString = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;//取得Web.config檔案裡的connectionStrings的值
		public ActionResult Index( ) {
			List<ADONet> data = new List<ADONet>( );

			using( SqlConnection conn = new SqlConnection(dbString) ) {//資料庫連線，使用using可以執行完時自動關閉連線
				string sql = @"select * from TodoList order by CreateTime desc;";

				conn.Open( );//開始連線，若無法連線修改Web.config檔案裡的connectionStrings的值

				SqlCommand cmd = new SqlCommand(sql, conn);//創建執行sql語句命令(寫法一)
				SqlDataReader dr = cmd.ExecuteReader( );//執行sql，返回結果的SqlDataReader物件

				while( dr.Read( ) ) {//一列一列資料讀取
					ADONet d = new ADONet( );
					d.ID = Guid.Parse(dr["ID"].ToString( ));
					d.Name = dr["Name"].ToString( );
					d.CreateTime = Convert.ToDateTime(dr["CreateTime"]);

					data.Add(d);
				}
			}

			return View(data);
		}

		//新增
		public ActionResult Create( ) {
			return PartialView( );//部分檢視不會套用模板
		}

		[HttpPost]
		public ActionResult Create( ADONet data ) {
			using( SqlConnection conn = new SqlConnection(dbString) ) {//資料庫連線，使用using可以執行完時自動關閉連線
				string sql = @"insert into TodoList (ID,Name,CreateTime) values (@ID,@Name,@CreateTime)";

				conn.Open( );//開始連線

				SqlCommand cmd = conn.CreateCommand( );//創建執行sql語句命令(寫法二)
				cmd.CommandText = sql;//設定命令要執行的sql語句
				cmd.Parameters.AddWithValue("ID", Guid.NewGuid( ));//@ID參數給值
				cmd.Parameters.AddWithValue("Name", data.Name);//@Name參數給值
				cmd.Parameters.AddWithValue("CreateTime", DateTime.Now);//@CreateTime參數給值

				cmd.ExecuteNonQuery( );//執行sql，返回整數

			}
			return PartialView( );
		}

		//編輯
		public ActionResult Edit( ) {
			ADONet data = new ADONet( );

			using( SqlConnection conn = new SqlConnection(dbString) ) {//資料庫連線，使用using可以執行完時自動關閉連線
				string sql = @"select * from TodoList where ID=@ID;";

				conn.Open( );//開始連線，若無法連線修改Web.config檔案裡的connectionStrings的值

				SqlCommand cmd = conn.CreateCommand( );//創建執行sql語句命令(寫法二)
				cmd.CommandText = sql;//設定命令要執行的sql語句
				cmd.Parameters.AddWithValue("ID", Request["ID"]);//@ID參數給值、Request["ID"]取得網址參數(寫法一)
				SqlDataReader dr = cmd.ExecuteReader( );//執行sql，返回結果的SqlDataReader物件

				if( dr.HasRows ) {//是否有值
					dr.Read( );//一列一列資料讀取
					data.ID = Guid.Parse(dr["ID"].ToString( ));
					data.Name = dr["Name"].ToString( );
				}
			}

			return PartialView(data);
		}

		[HttpPost]
		public ActionResult Edit( ADONet data ) {
			using( SqlConnection conn = new SqlConnection(dbString) ) {//資料庫連線，使用using可以執行完時自動關閉連線
				string sql = @"update TodoList set Name=@Name,CreateTime=@CreateTime where ID=@ID";

				conn.Open( );//開始連線

				SqlCommand cmd = conn.CreateCommand( );//創建執行sql語句命令(寫法二)
				cmd.CommandText = sql;//設定命令要執行的sql語句
				cmd.Parameters.AddWithValue("ID", Request["ID"]);//@ID參數給值、Request["ID"]取得網址參數(寫法一)
				cmd.Parameters.AddWithValue("Name", data.Name);
				cmd.Parameters.AddWithValue("CreateTime", DateTime.Now);

				cmd.ExecuteNonQuery( );//執行sql，返回整數

			}

			return PartialView(data);
		}

		//刪除
		public ActionResult Delete( string ID ) {
			using( SqlConnection conn = new SqlConnection(dbString) ) {//資料庫連線，使用using可以執行完時自動關閉連線
				string sql = @"delete from TodoList where ID=@ID";

				conn.Open( );//開始連線

				SqlCommand cmd = conn.CreateCommand( );//創建執行sql語句命令(寫法二)
				cmd.CommandText = sql;//設定命令要執行的sql語句
				cmd.Parameters.AddWithValue("ID", ID);//@ID參數給值、string ID取得網址參數(寫法二)

				cmd.ExecuteNonQuery( );//執行sql，返回整數

			}

			return RedirectToAction("Index");
		}
	}
}