using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Data.Converters
{
	public interface IFieldConverter
	{
		void Initialize(SPField field, Type propertyType);

		object FromSpValue(object value);

		object ToSpValue(object value);
	}
}