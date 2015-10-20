using System;
using System.Reflection;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	public sealed class MetaField : BaseMetaModel
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

		public string LookupField { get; set; }

		public Type CustomConverterType { get; set; }

		public MemberInfo Member { get; private set; }

		public string MemberName { get; private set; }

		public Type MemberType { get; private set; }

		public MetaContentType ContentType { get; private set; }

		public IFieldConverter Converter { get; set; }

		public override void Accept(IMetaModelVisitor visitor)
		{
			visitor.VisitField(this);
		}
	}
}