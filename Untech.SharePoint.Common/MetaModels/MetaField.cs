using System;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaField : MetaModel
	{
		public MetaField(MetaContentType contentType, MemberInfo member, string internalName)
		{
			Guard.CheckNotNull("contentType", contentType);
			Guard.CheckNotNull("member", member);
			Guard.CheckNotNull("internalName", internalName);

			ContentType = contentType;
			Member = member;
			MemberName = member.Name;
			MemberType = TypeSystem.GetMemberType(member);
			DeclaringType = member.DeclaringType;

			FieldInternalName = internalName;
		}

		public MetaContentType ContentType { get; private set; }

		public string MemberName { get; private set; }

		public Type MemberType { get; private set; }

		public MemberInfo Member { get; private set; }

		public Type DeclaringType { get; private set; }

		public string FieldInternalName { get; private set; }

		public string FieldTypeAsString { get; set; }

		public Type CustomConverterType { get; set; }

		public override void Accept(IMetaModelVisitor visitor)
		{
			visitor.VisitField(this);
		}
	}
}