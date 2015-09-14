using System;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedFieldPart : IMetaFieldProvider
	{
		private readonly MemberInfo _member;
		private readonly SpFieldAttribute _fieldAttribute;

		public AnnotatedFieldPart(MemberInfo member)
		{
			Guard.CheckNotNull("member", member);

			_member = member;

			_fieldAttribute = member.GetCustomAttribute<SpFieldAttribute>();
			if (_fieldAttribute == null)
			{
				throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", _member.Name), "member");
			}
		}

		public string InternalName
		{
			get
			{
				return string.IsNullOrEmpty(_fieldAttribute.Name) ? _member.Name : _fieldAttribute.Name;
			}
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