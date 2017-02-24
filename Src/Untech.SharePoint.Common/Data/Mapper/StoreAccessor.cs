using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Mapper
{
	/// <summary>
	/// Represents SP field accessor.
	/// </summary>
	/// <typeparam name="TSPItem">SP List Item type, i.e. SPListItem for SSOM, ListItem for CSOM.</typeparam>
	public abstract class StoreAccessor<TSPItem> : IFieldAccessor<TSPItem>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StoreAccessor{TSPItem}"/>
		/// </summary>
		/// <param name="field">Meta-data of the field that should be associated with that accessor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="field"/> is null.</exception>
		protected StoreAccessor([NotNull]MetaField field)
		{
			Guard.CheckNotNull(nameof(field), field);

			Field = field;
		}

		/// <summary>
		/// Gets <see cref="MetaField"/> associated with current accessor.
		/// </summary>
		[NotNull]
		protected MetaField Field { get; }

		/// <summary>
		/// Gets a value indicating whether the value can be read by <see cref="IFieldAccessor{T}.GetValue"/> method.
		/// </summary>
		public bool CanGetValue => true;

		/// <summary>
		/// Gets a value indicating whether the value can be set by <see cref="IFieldAccessor{T}.SetValue"/> method.
		/// </summary>
		public bool CanSetValue => !Field.ReadOnly && !Field.IsCalculated;

		/// <summary>
		/// Gets the value from the specified instance.
		/// </summary>
		/// <param name="instance">Value source instance.</param>
		/// <returns>Value of the field.</returns>
		public abstract object GetValue(TSPItem instance);

		/// <summary>
		/// Sets the value for the specified instance.
		/// </summary>
		/// <param name="instance">Value destination instance.</param>
		/// <param name="value">Value to set.</param>
		public abstract void SetValue(TSPItem instance, object value);
	}
}