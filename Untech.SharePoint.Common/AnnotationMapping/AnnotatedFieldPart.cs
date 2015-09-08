using System;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedFieldPart : IMetaFieldProvider
	{
		public AnnotatedFieldPart(MemberInfo member)
		{
			Member = member;

			FieldAttribute = member.GetCustomAttribute<SpFieldAttribute>();
			if (FieldAttribute == null)
			{
				throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", Member.Name), "member");
			}
		}

		public MemberInfo Member { get; private set; }

		public SpFieldAttribute FieldAttribute { get; private set; }

		public string FieldInternalName
		{
			get
			{
				return string.IsNullOrEmpty(FieldAttribute.InternalName)
					? Member.Name
					: FieldAttribute.InternalName;
			}
		}

		public string FieldTypeAsString
		{
			get { return FieldAttribute.TypeAsString; }
		}

		public Type CustomConverterType
		{
			get { return FieldAttribute.CustomConverterType; }
		}

		public MetaField GetMetaField(MetaContentType parent)
		{
			var metaField = new MetaField(parent, Member, FieldInternalName)
			{
				CustomConverterType = CustomConverterType,
				FieldTypeAsString = FieldTypeAsString
			};

			return metaField;
		}
	}
}