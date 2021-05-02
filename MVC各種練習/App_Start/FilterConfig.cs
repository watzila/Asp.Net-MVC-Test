using System.Web;
using System.Web.Mvc;

namespace MVC各種練習 {
	public class FilterConfig {
		public static void RegisterGlobalFilters( GlobalFilterCollection filters ) {
			filters.Add(new HandleErrorAttribute( ));
		}
	}
}
