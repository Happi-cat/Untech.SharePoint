using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal static class AnnotationUtils
	{
		public static bool CanRegisterListProvider(PropertyInfo property)
		{
			return property.IsDefined<SpListAttribute>() &&
			       property.CanRead &&
			       property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof (ISpList<>);
		}

		public static bool CanRegisterFieldPart(PropertyInfo property)
		{
			return property.IsDefined<SpFieldAttribute>() && !property.IsDefined<SpFieldRemovedAttribute>() &&
				   (property.CanRead || property.CanWrite) &&
				   property.GetIndexParameters().IsNullOrEmpty();
		}
	}
}