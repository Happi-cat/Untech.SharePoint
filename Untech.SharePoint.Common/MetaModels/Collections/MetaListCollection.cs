using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	/// <summary>
	/// Represents collection of <see cref="MetaList"/> with fast access by <see cref="MetaList.Title"/>.
	/// </summary>
	public sealed class MetaListCollection : ReadOnlyDictionary<string, MetaList>, IReadOnlyCollection<MetaList>
	{
		/// <summary>
		/// Initialize a new instance of <see cref="MetaListCollection"/> class around <paramref name="source"/>.
		/// </summary>
		/// <param name="source">Collection of <see cref="MetaList"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public MetaListCollection([NotNull]IEnumerable<MetaList> source)
			: base(CreateDictionary(source))
		{
		}

		IEnumerator<MetaList> IEnumerable<MetaList>.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		[NotNull]
		private static IDictionary<string, MetaList> CreateDictionary([NotNull]IEnumerable<MetaList> source)
		{
			Guard.CheckNotNull("source", source);

			return source.ToDictionary(n => n.Title);
		}
	}
}