using System;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	[AttributeUsage(AttributeTargets.Method)]
	public class QueryCamlAttribute : Attribute
	{
		public QueryCamlAttribute(string caml, string viewFields)
		{
			Caml = caml;
			ViewFields = viewFields.Split(',');
		}

		public string Caml { get; private set; }

		public string[] ViewFields { get; private set; }
	}
}