using System;

namespace Untech.SharePoint.Common.Test.Tools.QueryTests
{
	[AttributeUsage(AttributeTargets.Method)]
	public class QueryCamlAttribute : Attribute
	{
		public QueryCamlAttribute(string caml)
		{
			Caml = caml;
		}

		public string Caml { get; private set; }
	}
}