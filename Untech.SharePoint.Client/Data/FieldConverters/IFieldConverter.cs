using System;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data.FieldConverters
{
	public interface IFieldConverter
	{
		void Initialize(Field field, Type propertyType);

		object FromSpValue(object value);

		object ToSpValue(object value);
	}
}