using System;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Data.Mapper
{
	public abstract class FieldMapper<TSPItem>
	{
		protected FieldMapper(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
			MemberGetter = MemberAccessUtility.CreateGetter(field.Member);
			MemberSetter = MemberAccessUtility.CreateSetter(field.Member);
		}

		public MetaField Field { get; private set; }

		public Func<object, object> MemberGetter { get; private set; }

		public Action<object, object> MemberSetter { get; private set; }

		public IFieldConverter Converter
		{
			get { return Field.Converter; }
		}

		public void Map(object source, TSPItem dest)
		{
			if (MemberGetter == null || Field.ReadOnly || Field.IsCalculated)
			{
				return;
			}

			try
			{
				var clrValue = MemberGetter(source);
				var clientValue = Converter.ToSpValue(clrValue);
				SetStoreValue(dest, clientValue);
			}
			catch (Exception e)
			{
				throw Error.CannotMapField(Field, e);
			}
		}

		public void Map(TSPItem source, object dest)
		{
			if (MemberSetter == null)
			{
				return;
			}

			try
			{
				var clientValue = GetStoreValue(source);
				var clrValue = Converter.FromSpValue(clientValue);
				MemberSetter(dest, clrValue);
			}
			catch (Exception e)
			{
				throw Error.CannotMapField(Field, e);
			}
		}

		protected abstract object GetStoreValue(TSPItem spItem);

		protected abstract void SetStoreValue(TSPItem spItem, object value);
	}
}