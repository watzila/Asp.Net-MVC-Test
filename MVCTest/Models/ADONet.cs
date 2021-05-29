using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTest.Models {
	public class ADONet {
		//以下的名稱和類型必須和資料庫一樣
		public Guid ID { get; set; }
		public string Name { get; set; }
	}
}