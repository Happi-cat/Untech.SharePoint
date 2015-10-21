using System;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Server.Data.Mapper
{
	internal sealed class FieldMapper
	{
		public FieldMapper(MetaField field)
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

		public void Map(object source, SPListItem dest)
		{
			try
			{
				if (MemberGetter == null || Field.ReadOnly || Field.IsCalculated)
				{
					return;
				}

				var clrValue = MemberGetter(source);
				var clientValue = Converter.ToSpValue(clrValue);
				dest[Field.InternalName] = clientValue;
			}
			catch (Exception e)
			{
				throw new DataMappingException(Field, e);
			}
		}

		public void Map(SPListItem source, object dest)
		{
			try
			{
				if (MemberSetter == null)
				{
					return;
				}

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