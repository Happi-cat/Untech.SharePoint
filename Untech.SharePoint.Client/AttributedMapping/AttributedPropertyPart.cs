using System;
using System.Reflection;
using Untech.SharePoint.Client.Meta;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.AttributedMapping
{
	internal class AttributedPropertyPart : IMetaDataMemberProvider
	{
		public AttributedPropertyPart(MemberInfo member, Type parentType)
		{
			Member = member;
			ParentType = parentType;
		}

		public MemberInfo Member { get; private set; }

		public Type ParentType { get; private set; }

		public MetaDataMember GetMetaDataMember(MetaType metaType)
		{
			var attribute = Member.GetCustomAttribute<SpFieldAttribute>();
			if (attribute == null)
			{
				throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", Member.Name), "memberInfo");
			}

			var spFieldInternalName = attribute.InternalName ?? Member.Name;
			var customConverterType = attribute.CustomConverterType;

			return new MetaDataMember(metaType, Member, spFieldInternalName, null, customConverterType);
		}
	}
}