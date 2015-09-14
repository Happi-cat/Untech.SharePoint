using System;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedFieldPart : IMetaFieldProvider
	{
		private readonly MemberInfo _member;
		private readonly SpFieldAttribute _fieldAttribute;

		protected AnnotatedFieldPart(MemberInfo member)
		{
			Guard.CheckNotNull("member", member);

			_member = member;
			_fieldAttribute = member.GetCustomAttribute<SpFieldAttribute>();
		}

		public static AnnotatedFieldPart Create(PropertyInfo property)
		{
			if (!property.CanRead || !property.CanWrite)
			{
				throw new AnnotationException(string.Format("Property {0} from {1} should be readable and writable", property.Name, property.DeclaringType));
			}
			if (property.GetIndexParameters().Any())
			{
				throw new AnnotationException(string.Format("Indexable property from {0} cannot be annotated", property.DeclaringType));
			}
			if (!property.IsDefined(typeof(SpFieldAttribute)) )
			{
				throw new AnnotationException(string.Format("Member {0} has no attribute SpFieldAttribute", property.Name));
			}

			return new AnnotatedFieldPart(property);
		}

		public static AnnotatedFieldPart Create(FieldInfo field)
		{
			if (field.IsInitOnly || field.IsLiteral)
			{
				throw new AnnotationException(string.Format("Field {0} from {1} cannot be readonly or const", field.Name, field.DeclaringType));
			}
			if (!field.IsDefined(typeof(SpFieldAttribute)))  
			{
				throw new AnnotationException(string.Format("Member {0} has no attribute SpFieldAttribute", field.Name));
			}

			return new AnnotatedFieldPart(field);
		}

		public string InternalName
		{
			get { return string.IsNullOrEmpty(_fieldAttribute.Name) ? _member.Name : _fieldAttribute.Name; }
		}

		public string TypeAsString
		{
			get { return _fieldAttribute.FieldType; }
		}

		public Type CustomConverterType
		{
			get { return _fieldAttribute.CustomConverterType; }
		}

		public MetaField GetMetaField(MetaContentType parent)
		{
			return new MetaField(parent, _member, InternalName)
			{
				CustomConverterType = CustomConverterType,
				TypeAsString = TypeAsString
			};
		}
	}
}