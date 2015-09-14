using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal static class AnnotationConventions
	{
		public static bool HasListAnnotatinon(MemberInfo member)
		{
			return member.IsDefined(typeof(SpListAttribute));
		}

		public static bool HasFieldAnnotation(MemberInfo member)
		{
			return member.IsDefined(typeof(SpFieldAttribute)) && !member.IsDefined(typeof(SpFieldRemovedAttribute));
		}

		public static void ValidateList(PropertyInfo property)
		{
			if (!property.CanRead) 
			{
				throw new Exception();
			}

			if (!property.PropertyType.IsGenericType || property.PropertyType.GetGenericTypeDefinition() != typeof(ISpList<>))
			{
				throw new Exception();
			}
		}

		public static void ValidateField(PropertyInfo property)
		{
			if (!property.CanRead || !property.CanWrite) 
			{
				throw new Exception();
			}
			if (property.GetIndexParameters().Any())
			{
				throw new Exception();
			}
		}

		public static void ValidateField(FieldInfo field)
		{
			if (field.IsInitOnly || field.IsLiteral)
			{
				throw new Exception();
			}
		}
	}
}
