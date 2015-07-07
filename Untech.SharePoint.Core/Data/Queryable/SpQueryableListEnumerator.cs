using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Core.Reflection;

namespace Untech.SharePoint.Core.Data.Queryable
{
	internal class SpQueryableListEnumerator<TElement> : IEnumerator<TElement> where TElement : new()
	{
		internal SpQueryableListEnumerator(IEnumerator<SPListItem> spListItemIterator)
		{
			Guard.ThrowIfArgumentNull(spListItemIterator, "spListItemIterator");

			SPListItemIterator = spListItemIterator;
		}

		public IEnumerator<SPListItem> SPListItemIterator { get; set; }

		public TElement Current { get; private set; }

		public void Dispose()
		{
			SPListItemIterator.Dispose();
		}

		object System.Collections.IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			if (SPListItemIterator.MoveNext())
			{
				Current = new TElement();

				SpModelMapper.Map<TElement>(SPListItemIterator.Current, Current);

				return true;
			};

			return false;
		}

		public void Reset()
		{
			Current = null;

			SPListItemIterator.Reset();
		}
	}
}
