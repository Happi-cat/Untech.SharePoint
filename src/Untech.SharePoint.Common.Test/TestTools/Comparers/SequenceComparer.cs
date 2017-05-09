using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.TestTools.Comparers
{
	public class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>>
	{
		public static readonly IEqualityComparer<IEnumerable<T>> Default = new SequenceComparer<T>();

		public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
		{
			if (object.Equals(x, y)) return true;
			if (x == null || y == null) return false;

			return x.SequenceEqual(y);
		}

		public int GetHashCode(IEnumerable<T> obj)
		{
			throw new NotImplementedException();
		}
	}
}