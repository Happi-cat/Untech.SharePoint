using Microsoft.SharePoint;
using System;
namespace Untech.SharePoint.Core.Data.Fields.Converters
{
	public interface IFieldConverter
	{
		object FromSpValue(object value, SPField field, Type propertyType);

		object ToSpValue(object value, SPField field, Type propertyType);
	}
}