using System.Collections.Generic;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Test.Data.Translators
{
	public class BaseExpressionTest
	{
		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public class Entity
		{
			public bool Bool1 { get; set; }

			public bool Bool2 { get; set; }

			public int Int1 { get; set; }

			public int Int2 { get; set; }

			public string String1 { get; set; }

			public string String2 { get; set; }

			public float Float1 { get; set; }

			public float Float2 { get; set; }

			public string[] StringCollection1 { get; set; }

			public IEnumerable<string> StringCollection2 { get; set; }

			public ICollection<string> StringCollection3 { get; set; }

			public List<string> StringCollection4 { get; set; }
		} 
	}
}