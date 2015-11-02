using System;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Mapper
{
	/// <summary>
	/// Represents class that can map value from SP list field to the specified entity member.
	/// </summary>
	/// <typeparam name="TSPItem">Exact type of SP list item, i.e. SPListItem for SSOM, ListItem for CSOM.</typeparam>
	public sealed class FieldMapper<TSPItem>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldMapper{TSPItem}"/>.
		/// </summary>
		/// <param name="field">Field metadata.</param>
		/// <param name="storeAccessor">SP list field accessor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="field"/> or <paramref name="storeAccessor"/> is null.</exception>
		public FieldMapper([NotNull]MetaField field, [NotNull]IFieldAccessor<TSPItem> storeAccessor)
		{
			Guard.CheckNotNull("field", field);
			Guard.CheckNotNull("storeAccessor", storeAccessor);

			Field = field;
			MemberAccessor = new MemberAccessor(field.Member);
			StoreAccessor = storeAccessor;
		}

		/// <summary>
		/// Gets <see cref="MetaField"/> that is associated with current mapper.
		/// </summary>
		[NotNull]
		public MetaField Field { get; private set; }

		/// <summary>
		/// Gets field or property accessor.
		/// </summary>
		[NotNull]
		public IFieldAccessor<object>  MemberAccessor { get; private set; }

		/// <summary>
		/// Gets SP list field accessor.
		/// </summary>
		[NotNull]
		public IFieldAccessor<TSPItem> StoreAccessor { get; private set; }

		/// <summary>
		/// Gets <see cref="IFieldConverter"/> associated with current <see cref="Field"/>.
		/// </summary>
		public IFieldConverter Converter
		{
			get { return Field.Converter; }
		}

		/// <summary>
		/// Maps entity member to SP list field.
		/// </summary>
		/// <param name="source">Source entity to map.</param>
		/// <param name="dest">Destination SP list item.</param>
		/// <exception cref="DataMappingException">Cannot map or convert value.</exception>
		public void Map(object source, TSPItem dest)
		{
			if (!MemberAccessor.CanGetValue || !StoreAccessor.CanSetValue)
			{
				return;
			}

			try
			{
				var clrValue = MemberAccessor.GetValue(source);
				var clientValue = Converter.ToSpValue(clrValue);
				StoreAccessor.SetValue(dest, clientValue);
			}
			catch (Exception e)
			{
				throw Error.CannotMapField(Field, e);
			}
		}

		/// <summary>
		/// Maps SP list field to entity member.
		/// </summary>
		/// <param name="source">Source SP list item to map.</param>
		/// <param name="dest">Destination entity.</param>
		/// <exception cref="DataMappingException">Cannot map or convert value.</exception>
		public void Map(TSPItem source, object dest)
		{
			if (!StoreAccessor.CanGetValue || !MemberAccessor.CanSetValue)
			{
				return;
			}

			try
			{
				var clientValue = StoreAccessor.GetValue(source);
				var clrValue = Converter.FromSpValue(clientValue);
				MemberAccessor.SetValue(dest, clrValue);
			}
			catch (Exception e)
			{
				throw Error.CannotMapField(Field, e);
			}
		}
	}
}