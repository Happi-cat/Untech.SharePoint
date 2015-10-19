using System;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaField : IMetaModel
	{
		public MetaField(MetaContentType contentType, MemberInfo member, string internalName)
		{
			Guard.CheckNotNull("contentType", contentType);
			Guard.CheckNotNull("member", member);
			Guard.CheckNotNullOrEmpty("internalName", internalName);

			ContentType = contentType;

			Member = member;
			MemberName = member.Name;
			MemberType = TypeSystem.GetMemberType(member);
			MemberDeclaringType = member.DeclaringType;

			InternalName = internalName;
		}

		public Guid Id { get; set; }

		public string Title { get; set; }

		public string InternalName { get; private set; }

		public string TypeAsString { get; set; }

		public bool IsCalculated { get; set; }

		public bool ReadOnly { get; set; }

		public bool Required { get; set; }

		public bool AllowMultipleValues { get; set; }

		public string LookupList { get; set; }

		public string LookupDisplayColumn { get; set; }

		public Type CustomConverterType { get; set; }

		public MemberInfo Member { get; private set; }

		public string MemberName { get; private set; }

		public Type MemberType { get; private set; }

		public Type MemberDeclaringType { get; private set; }

		public MetaContentType ContentType { get; private set; }

		public void Accept(IMetaModelVisitor visitor)
		{
			visitor.VisitField(this);
		}
	}
}