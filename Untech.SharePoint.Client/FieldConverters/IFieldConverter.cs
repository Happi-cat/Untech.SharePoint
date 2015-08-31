using System;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.FieldConverters
{
	public interface IFieldConverter
	{
		void Initialize(Field field, Type propertyType);

		object FromSpClientValue(object value);

		object ToSpClientValue(object value);

		string ToCamlValue(object value);
	}
}