using System;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Mapper
{
	public class FieldMapper<TSPItem>
	{
		public FieldMapper(MetaField field, IFieldAccessor<TSPItem> storeAccessor)
		{
			Guard.CheckNotNull("field", field);
			Guard.CheckNotNull("storeAccessor", storeAccessor);

			Field = field;
			MemberAccessor = new MemberAccessor(field.Member);
			StoreAccessor = storeAccessor;
		}

		public MetaField Field { get; private set; }

		public IFieldAccessor<object>  MemberAccessor { get; private set; }

		public IFieldAccessor<TSPItem> StoreAccessor { get; private set; }

		public IFieldConverter Converter
		{
			get { return Field.Converter; }
		}

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