using System;
using System.Linq;
using Microsoft.SharePoint;
using Untech.SharePoint.Common.MetaModels;

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
			var fields = ContentType.Fields.Where<MetaField>(n => !n.ReadOnly);
			foreach (var metaField in fields)
			{
				var getter = metaField.GetAdditionalProperty<Func<object, object>>("Getter");

				var value = getter(source);

				dest[metaField.InternalName] = metaField.Converter.ToSpValue(value);
			}
		}

		public void Map(SPListItem source, object dest)
		{
			var fields = ContentType.Fields.ToList<MetaField>();
			foreach (var metaField in fields)
			{
				var setter = metaField.GetAdditionalProperty<Action<object, object>>("Setter");

				var value = metaField.Converter.FromSpValue(source[metaField.InternalName]);

				setter(dest, value);
			}
		}
	}
}