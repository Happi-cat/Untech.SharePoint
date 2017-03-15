using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.TestTools.Generators.Basic
{
	public class RangeGenerator<T> : BaseRandomGenerator, IValueGenerator<T>
	{
		[NotNull]
		private readonly IReadOnlyList<T> _values;

		public RangeGenerator([CanBeNull]IEnumerable<T> values)
		{
			_values = values.EmptyIfNull().ToList();
		}

		public T Generate()
		{
			if (_values.Count == 0) return default(T);
			return _values[Rand.Next(_values.Count)];
		}
	}
}