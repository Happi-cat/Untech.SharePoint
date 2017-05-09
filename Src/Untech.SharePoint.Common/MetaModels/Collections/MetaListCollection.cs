using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.MetaModels.Collections
{
	/// <summary>
	/// Represents collection of <see cref="MetaList"/> with fast access by <see cref="MetaList.Url"/>.
	/// </summary>
	public sealed class MetaListCollection : ReadOnlyDictionary<string, MetaList>, IReadOnlyCollection<MetaList>
	{
		/// <summary>
		/// Initialize a new instance of <see cref="MetaListCollection"/> class around <paramref name="source"/>.
		/// </summary>
		/// <param name="source">Collection of <see cref="MetaList"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public MetaListCollection([NotNull][ItemNotNull]IEnumerable<MetaList> source)
			: base(CreateDictionary(source))
		{
		}

		IEnumerator<MetaList> IEnumerable<MetaList>.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		[NotNull]
		private static IDictionary<string, MetaList> CreateDictionary([NotNull][ItemNotNull]IEnumerable<MetaList> source)
		{
			Guard.CheckNotNull(nameof(source), source);

			return source.ToDictionary(list => list.Url, SiteRelativeUrlComparer.Default);
		}
	}
}