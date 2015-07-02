using System;

namespace Untech.SharePoint.Core.Caml
{
	public interface ISpModelContext
	{
		string GetSpFieldInternalName(Type modelType, string propertyOrFieldName);

		string GetSpFieldTypeAsString(Type modelType, string propertyOrFieldName);

		object ConvertToSpValue(Type modelType, string propertyOrFieldName, object value);

	}
}