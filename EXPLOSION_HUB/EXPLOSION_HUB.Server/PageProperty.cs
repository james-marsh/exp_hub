using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPLOSION_HUB.Server
{
	public class PageProperty
	{
		public int Page { get; set; }

		public int Rows { get; set; }

		public int TotalPages { get; set; }

		public int TotalRecords { get; set; }

		public string OrderByColumn { get; set; }

		public string SortDirection { get; set; }
	}
}
