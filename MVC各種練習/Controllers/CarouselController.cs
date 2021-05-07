using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Controllers {
	public class CarouselController : Controller {
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
		public ActionResult P( int num ) {
			if( num < 0 ) {
				num = r.location.Length + num;
			}
			var a = r.location[num];
			return PartialView(a);
		}
	}
}