using System.Collections;
using System.Collections.Generic;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaFieldCollection : IReadOnlyCollection<MetaField>
	{
		public IEnumerator<MetaField> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count { get; private set; }
	}
}