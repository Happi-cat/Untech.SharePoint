using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Core.Caml
{
	public interface ISpModelContext
	{
		string GetSpFieldInternalName(Type modelType, string propertyOrFieldName);

		IEnumerable<string> GetSpFieldsInternalNames(Type modelType);

		string GetSpFieldTypeAsString(Type modelType, string propertyOrFieldName);

		object ConvertToSpValue(Type modelType, string propertyOrFieldName, object value);

		Type ElementType { get; }
	}
}