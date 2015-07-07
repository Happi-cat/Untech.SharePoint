using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Core.Reflection;

namespace Untech.SharePoint.Core.Data.Queryable
{
	internal class SpQueryableListEnumerator<TElement> : IEnumerator<TElement> where TElement: new()
	{
		internal SpQueryableListEnumerator(IEnumerator<SPListItem> spListItemIterator)
		{
			Guard.ThrowIfArgumentNull(spListItemIterator, "spListItemIterator");

			SPListItemIterator = spListItemIterator;
			CurrentReady = false;
		}

		public IEnumerator<SPListItem> SPListItemIterator { get; set; }
		private bool CurrentReady { get; set; }
		private TElement CurrenElement { get; set; }

		public TElement Current
		{
			get
			{
				if (!CurrentReady)
				{
					CurrenElement = new TElement();

					SpModelMapper.Map<TElement>(SPListItemIterator.Current, CurrenElement);

					CurrentReady = true;
				}
				return CurrenElement;
			}
		}

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
			CurrentReady = false;

			return SPListItemIterator.MoveNext();
		}

		public void Reset()
		{
			CurrentReady = false;

			SPListItemIterator.Reset();
		}
	}
}
