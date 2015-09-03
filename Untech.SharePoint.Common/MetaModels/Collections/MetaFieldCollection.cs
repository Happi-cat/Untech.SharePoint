using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	public sealed class MetaFieldCollection : ReadOnlyDictionary<string, MetaField>, IEnumerable<MetaField>
	{
		public MetaFieldCollection(IEnumerable<MetaField> enumerable)
			: base(CreateDictionary(enumerable))
		{
		}

		IEnumerator<MetaField> IEnumerable<MetaField>.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		private static IDictionary<string, MetaField> CreateDictionary(IEnumerable<MetaField> enumerable)
		{
			return enumerable.ToDictionary(n => n.MemberName);
		}
	}
}