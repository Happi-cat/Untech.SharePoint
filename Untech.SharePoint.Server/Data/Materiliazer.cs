using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Server.MetaModels;

namespace Untech.SharePoint.Server.Data
{
	internal class Materiliazer
	{
		public Materiliazer(MetaContentType contentType)
		{
			ContentType = contentType;
		}

		public MetaContentType ContentType { get; private set; }

		public void Map(object source, SPListItem dest)
		{
			IEnumerable<MetaField> fields = ContentType.Fields.Where<MetaField>(n => !n.ReadOnly);
			foreach (var metaField in fields)
			{
				var getter = metaField.GetMemberGetter();

				var value = getter(source);

				dest[metaField.InternalName] = metaField.Converter.ToSpValue(value);
			}
		}

		public void Map(SPListItem source, object dest)
		{
			IEnumerable<MetaField> fields = ContentType.Fields;
			foreach (var metaField in fields)
			{
				var setter = metaField.GetMemberSetter();

				var value = metaField.Converter.FromSpValue(source[metaField.InternalName]);

				setter(dest, value);
			}
		}
	}
}