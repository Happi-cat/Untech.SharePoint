using System;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Client.Data.Mapper
{
	internal sealed class FieldMapper
	{
		public FieldMapper(MetaField field)
		{
			Common.Utils.Guard.CheckNotNull("field", field);

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

		public void Map(object source, ListItem dest)
		{
			if (MemberGetter == null || Field.ReadOnly || Field.IsCalculated)
			{
				return;
			}

			try
			{
				var clrValue = MemberGetter(source);
				var clientValue = Converter.ToSpValue(clrValue);
				dest[Field.InternalName] = clientValue;
			}
			catch (Exception e)
			{
				throw new DataMappingException(Field, e);
			}
		}

		public void Map(ListItem source, object dest)
		{
			if (MemberSetter == null)
			{
				return;
			}

			try
			{
				var clientValue = source[Field.InternalName];
				var clrValue = Converter.FromSpValue(clientValue);
				MemberSetter(dest, clrValue);
			}
			catch (Exception e)
			{
				throw new DataMappingException(Field, e);
			}
		}
	}
}