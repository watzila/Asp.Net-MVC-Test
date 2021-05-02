using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Controllers {
	public class LinqController : Controller {
		public class Test {
			public int id { get; set; }
			public string str { get; set; }
		}

		// GET: Linq
		public ActionResult Index( ) {
			string[ ] str = { "a", "b", "c", "c" };
			int[ ] int1 = { 1, 2, 3, 4, 5 };
			int[ ] int2 = { 1, 0, 32, 14, 5 };
			List<Test> t = new List<Test> { new Test { id = 3, str = "aa" }, new Test { id = 2, str = "bb" }, new Test { id = 1, str = "cc" }, new Test { id = 4, str = "bb" } };

			var s1 = from i in str select i;
			var s2 = from i in str where i == "c" select i;
			var s3 = from i in t orderby i.id select i;
			var s4 = from i in t group i by  i.str into aa select new { key=aa.Key};

			var newInt1 = from a in int1 from b in int2 select a + b;
			var newInt2 = from a in int1 join b in int2 on a equals b select new {aa=a,bb=b };

			var res2 = str.Where(i => i == "c");
			var res3 = int1.SelectMany(i => int2, ( i, ii ) => new { aa = i, bb = ii });


			ViewBag.str1 = s1.ToArray( );
			ViewBag.str2 = s2.ToArray( );

			return View( );
		}
	}
}