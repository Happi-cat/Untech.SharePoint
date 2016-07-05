using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels.Collections
{
	/// <summary>
	/// Represents collection of <see cref="MetaField"/> with fast access by <see cref="MetaField.MemberName"/>.
	/// </summary>
	public sealed class MetaFieldCollection : ReadOnlyDictionary<string, MetaField>, IReadOnlyCollection<MetaField>
	{
		/// <summary>
		/// Initialize a new instance of <see cref="MetaFieldCollection"/> class around <paramref name="source"/>.
		/// </summary>
		/// <param name="source">Collection of <see cref="MetaField"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public MetaFieldCollection([NotNull][ItemNotNull]IEnumerable<MetaField> source)
			: base(CreateDictionary(source))
		{
		}

		IEnumerator<MetaField> IEnumerable<MetaField>.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		[NotNull]
		private static IDictionary<string, MetaField> CreateDictionary([NotNull][ItemNotNull]IEnumerable<MetaField> source)
		{
			Guard.CheckNotNull(nameof(source), source);

			return source.ToDictionary(n => n.MemberName);
		}
	}
}