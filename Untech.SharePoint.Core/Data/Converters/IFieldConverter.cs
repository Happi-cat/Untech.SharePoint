using System;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Core.Data.Converters
{
	public interface IFieldConverter
	{
		void Initialize(Field field, Type propertyType);

		object FromSpValue(object value);

		object ToSpValue(object value);
	}
}