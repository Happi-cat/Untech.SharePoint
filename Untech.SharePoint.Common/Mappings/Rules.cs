using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Mappings
{
	internal static class Rules
	{
		public static void CheckContentTypeField(PropertyInfo property)
		{
			if (!property.CanRead || !property.CanWrite)
			{
				throw new InvalidAnnotationException(string.Format("Property {1}.{0} should be readable and writable",
					property.DeclaringType, property.Name));
			}
			if (property.GetIndexParameters().Any())
			{
				throw new InvalidAnnotationException($"Indexer in {property.DeclaringType} cannot be annotated");
			}
		}

		public static void CheckContentTypeField(FieldInfo field)
		{
			if (field.IsInitOnly || field.IsLiteral)
			{
				throw new InvalidAnnotationException(string.Format("Field {1}.{0} cannot be readonly or const", field.Name,
					field.DeclaringType));
			}
		}

		public static void CheckContextList(PropertyInfo contextProperty)
		{
			if (!contextProperty.CanRead)
			{
				throw new InvalidAnnotationException(
					$"Property {contextProperty.Name} from {contextProperty.DeclaringType} should be readable");
			}

			if (!contextProperty.PropertyType.IsGenericType ||
				contextProperty.PropertyType.GetGenericTypeDefinition() != typeof(ISpList<>))
			{
				throw new InvalidAnnotationException(
					$"Property {contextProperty.Name} from {contextProperty.DeclaringType} should have 'ISpList<T>' type");
			}

			if (contextProperty.GetIndexParameters().Any())
			{
				throw new InvalidAnnotationException($"Indexer in {contextProperty.DeclaringType} cannot be annotated");
			}
		}
	}
}