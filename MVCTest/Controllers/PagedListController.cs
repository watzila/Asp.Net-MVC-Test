using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using MVCTest.Models;
using Newtonsoft.Json;
using PagedList;//到NuGet安裝PagedList.Mvc(舊版)

namespace MVCTest.Controllers {
	public class PagedListController : Controller {
		// GET: PagedList
		public ActionResult Index( int Page =1) {
			Records r = new Records( );

			//建立http連線
			using( HttpClient c = new HttpClient( ) ) {

				//基底網址uri
				c.BaseAddress = new Uri("https://opendata.cwb.gov.tw");

				//get，回傳HttpResponseMessage
				HttpResponseMessage response = c.GetAsync("/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-422B0FA3-E374-492D-B54A-4D8942BE2B7E&format=JSON").Result;

				//以非同步作業方式將HTTP內容序列化為字串，回傳字串
				string result = response.Content.ReadAsStringAsync( ).Result;

				//將JSON格式轉換成物件並放入Models.Weather
				Weather w = JsonConvert.DeserializeObject<Weather>(result);
				r = w.records;
			}

			IPagedList p = r.location.ToPagedList(Page, 5);//換頁

			return View(p);
		}
	}
}