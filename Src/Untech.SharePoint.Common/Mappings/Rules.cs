using System.Reflection;
using Untech.SharePoint.Data;
using Untech.SharePoint.Mappings.Annotation;

namespace Untech.SharePoint.Mappings
{
	internal static class Rules
	{
		public static void CheckContentTypeField(PropertyInfo property)
		{
			if (!property.CanRead || !property.CanWrite)
			{
				throw new InvalidAnnotationException($"Property {property.Name} from {property.DeclaringType} should be readable and writable");
			}
			if (property.GetIndexParameters().Length > 0)
			{
				throw new InvalidAnnotationException($"Indexer in {property.DeclaringType} cannot be annotated");
			}
		}

		public static void CheckContentTypeField(FieldInfo field)
		{
			if (field.IsInitOnly || field.IsLiteral)
			{
				throw new InvalidAnnotationException($"Field {field.Name} from {field.DeclaringType} cannot be read only or const");
			}
		}

		public static void CheckContextList(PropertyInfo contextProperty)
		{
			if (!contextProperty.CanRead)
			{
				throw new InvalidAnnotationException($"Property {contextProperty.Name} from {contextProperty.DeclaringType} should be readable");
			}

			if (!contextProperty.PropertyType.IsGenericType
				|| contextProperty.PropertyType.GetGenericTypeDefinition() != typeof(ISpList<>))
			{
				throw new InvalidAnnotationException($"Property {contextProperty.Name} from {contextProperty.DeclaringType} should have 'ISpList<T>' type");
			}

			if (contextProperty.GetIndexParameters().Length > 0)
			{
				throw new InvalidAnnotationException($"Indexer in {contextProperty.DeclaringType} cannot be annotated");
			}
		}
	}
}