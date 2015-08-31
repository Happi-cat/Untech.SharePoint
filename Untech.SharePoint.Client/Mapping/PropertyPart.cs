using System;
using System.Reflection;
using Untech.SharePoint.Client.FieldConverters;
using Untech.SharePoint.Client.Meta;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.Mapping
{
	public class PropertyPart : IMetaDataMemberProvider
	{
		private string _internalName;
		private string _typeAsString;
		private Type _converter;

		public PropertyPart(MemberInfo member, Type parentType)
		{
			Member = member;
			ParentType = parentType;
		}

		public MemberInfo Member { get; private set; }

		public Type ParentType { get; private set; }

		public PropertyPart InternalName(string internalName)
		{
			_internalName = internalName;
			return this;
		}

		public PropertyPart TypeAsString(string typeAsString)
		{
			_typeAsString = typeAsString;
			return this;
		}

		public PropertyPart Converter<TConverter>()
			where TConverter: IFieldConverter
		{
			_converter = typeof (TConverter);
			return this;
		}

		public MetaDataMember GetMetaDataMember(MetaType metaType)
		{
			return new MetaDataMember(metaType, Member, _internalName, _typeAsString, _converter);
		}
	}
}