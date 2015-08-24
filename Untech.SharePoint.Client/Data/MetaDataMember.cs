using System;
using System.Reflection;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.FieldConverters;
using Untech.SharePoint.Client.Utility;

namespace Untech.SharePoint.Client.Data
{
	internal abstract class MetaDataMember
	{
		private readonly MetaType _declaringType;
		private readonly MemberInfo _member;
		private readonly string _name;
		private readonly Type _type;

		protected MetaDataMember(MetaType declaringType, MemberInfo memberInfo)
		{
			Guard.CheckNotNull("declaringType", declaringType);
			Guard.CheckNotNull("memberInfo", memberInfo);

			_declaringType = declaringType;
			_member = memberInfo;
			_name = memberInfo.Name;
			_type = TypeSystem.GetMemberType(memberInfo);
		}

		public MetaType DeclaringType { get { return _declaringType; } }

		public MemberInfo Member { get { return _member; } }

		public string Name { get { return _name; } }

		public Type Type { get { return _type; } }

		public abstract string SpFieldInternalName { get; }

		public abstract string SpFieldTypeAsString { get; }

		public abstract IFieldConverter Converter { get; }

		public abstract MetaAccessor<object> MemberAccessor { get; }

		public abstract MetaAccessor<ListItem> SpClientAccessor { get; }

		public override string ToString()
		{
			return string.Format("( Name={0}; SpFieldInternalName={1}; )", Name, SpFieldInternalName);
		}
	}
}