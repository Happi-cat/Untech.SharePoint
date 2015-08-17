using System;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data.FieldConverters
{
	public interface IFieldConverter
	{
		void Initialize(Field field, Type propertyType);

		object FromClientValue(object value);

		object ToClientValue(object value);

		string ToCamlValue(object value);
	}
}