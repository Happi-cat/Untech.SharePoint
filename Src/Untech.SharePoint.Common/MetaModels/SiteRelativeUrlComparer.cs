using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	/// <summary>
	/// Represents a string comparison operations that allows to compare site-relative Url ignoring case.
	/// E.g. '/Lists/SomeList' will be identical with 'Lists/SomeList'.
	/// </summary>
	public class SiteRelativeUrlComparer : IEqualityComparer<string>
	{
		public static readonly IEqualityComparer<string> Default = new SiteRelativeUrlComparer();

		private readonly IEqualityComparer<string> _innerComparer = StringComparer.InvariantCultureIgnoreCase;

		public bool Equals(string x, string y)
		{
			if (x == y) return true;
			if (x == null || y == null) return false;

			x = x.TrimStart('/');
			y = y.TrimStart('/');

			return _innerComparer.Equals(x, y);
		}

		public int GetHashCode(string obj)
		{
			Guard.CheckNotNull(nameof(obj), obj);

			obj = obj.TrimStart('/');
			return _innerComparer.GetHashCode(obj);
		}
	}
}