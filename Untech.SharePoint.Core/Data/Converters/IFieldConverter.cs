using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	public interface IFieldConverter
	{
		object FromSpValue(object value, SPField field, Type propertyType);

		object ToSpValue(object value, SPField field, Type propertyType);
	}
}