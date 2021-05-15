using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTest.Models {

	public class Weather {
		public Records records { get; set; }
	}

	public class Records {
		public Location[ ] location { get; set; }
	}

	public class Location {
		public string locationName { get; set; }
		public Weatherelement[ ] weatherElement { get; set; }
	}

	public class Weatherelement {
		public string elementName { get; set; }
		public Time[ ] time { get; set; }
	}

	public class Time {
		public string startTime { get; set; }
		public string endTime { get; set; }
		public Parameter parameter { get; set; }
	}

	public class Parameter {
		public string parameterName { get; set; }
		public string parameterValue { get; set; }
		public string parameterUnit { get; set; }
	}
}