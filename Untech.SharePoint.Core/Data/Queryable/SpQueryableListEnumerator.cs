using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Data.Converters;
using Untech.SharePoint.Core.Reflection;

namespace Untech.SharePoint.Core.Data.Queryable
{
	internal class SpQueryableListEnumerator<TElement> : IEnumerator<TElement>
	{
		internal SpQueryableListEnumerator(SPFieldCollection fields, IEnumerator<SPListItem> spListItemIterator)
		{
			Guard.ThrowIfArgumentNull(spListItemIterator, "spListItemIterator");

			SPListItemIterator = spListItemIterator;

			MetaModel = MetaModelPool.Instance.Get<TElement>();
			ModelConverters = new ModelConverters(MetaModel, fields);
			Mapper = new DataMapper(MetaModel, ModelConverters);

			Creator = InstanceCreationUtility.GetCreator<TElement>(typeof(TElement));
		}

		protected IEnumerator<SPListItem> SPListItemIterator { get; set; }

		protected MetaModel MetaModel { get; private set; }

		protected ModelConverters ModelConverters { get; private set; }

		protected DataMapper Mapper { get; private set; }

		protected Func<TElement> Creator {get; private set; }

		public TElement Current { get; private set; }

		public void Dispose()
		{
			SPListItemIterator.Dispose();
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			if (!SPListItemIterator.MoveNext()) return false;

			Current = Creator();

			Mapper.Map(SPListItemIterator.Current, Current);

			return true;
		}

		public void Reset()
		{
			Current = default(TElement);

			SPListItemIterator.Reset();
		}
	}
}
