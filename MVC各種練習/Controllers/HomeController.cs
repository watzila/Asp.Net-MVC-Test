using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace MVCTest.Controllers {
	public class HomeController : Controller {
		private static Models.Records r = new Models.Records( );
		public ActionResult Index( ) {
			//建立http連線
			using( HttpClient c = new HttpClient( ) ) {

				//基底網址uri
				c.BaseAddress = new Uri("https://opendata.cwb.gov.tw");

				//.NET 4.5需手動添加建立 SSL/TLS 的安全通道
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 

				//get，回傳HttpResponseMessage
				HttpResponseMessage response = c.GetAsync("/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-422B0FA3-E374-492D-B54A-4D8942BE2B7E&format=JSON").Result;

				//以非同步作業方式將HTTP內容序列化為字串，回傳字串
				string result = response.Content.ReadAsStringAsync( ).Result;

				//將JSON格式轉換成物件並放入Models.Weather
				Models.Weather w = JsonConvert.DeserializeObject<Models.Weather>(result);
				r = w.records;
			}
			return View(r);
		}

		[HttpPost]
		public ActionResult Index(string city ) {
			Models.Records rr = new Models.Records( );

			city = city.Trim( );//去除前後空格

			city = ( city[0] == '台' ) ? city.Replace("台", "臺") : city;//把台換成臺(單引號是char雙引號是string)

			rr.location = r.location.Where(item => item.locationName == city).ToArray( );//使用lamba來搜尋

			return View(rr);
		}
	}
}