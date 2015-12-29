using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Test.Tools.Generators
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
			return _values[Rand.Next(_values.Count)];
		}
	}
}