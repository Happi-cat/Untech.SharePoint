using System;
using System.Reflection;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Client.FieldConverters;
using Untech.SharePoint.Client.Meta.Accessors;
using Untech.SharePoint.Client.Meta.Collections;
using Untech.SharePoint.Client.Utils;
using Untech.SharePoint.Client.Utils.Reflection;

namespace Untech.SharePoint.Client.Meta
{
	public class MetaDataMember
	{
		private readonly MetaType _declaringType;
		private readonly MemberInfo _member;
		private readonly string _name;
		private readonly Type _type;
		
		private readonly string _spFieldInternalName;
		private readonly string _spFieldTypeAsString;
		private readonly Type _customConverterType;
		private IFieldConverter _converter;
		private MetaAccessor<object> _memberAccessor;
		private MetaAccessor<ListItem> _spClientAccessor;

		public MetaDataMember(MetaType declaringType, MemberInfo memberInfo, string internalName, string fieldTypeAsString, Type converter)
		{
			Guard.CheckNotNull("declaringType", declaringType);
			Guard.CheckNotNull("memberInfo", memberInfo);

			_declaringType = declaringType;
			_member = memberInfo;
			_name = memberInfo.Name;
			_type = TypeSystem.GetMemberType(memberInfo);

			_spFieldInternalName = internalName;
			_spFieldTypeAsString = fieldTypeAsString;
			_customConverterType = converter;
		}

		public MetaType DeclaringType { get { return _declaringType; } }

		public MemberInfo Member { get { return _member; } }

		public string Name { get { return _name; } }

		public Type Type { get { return _type; } }

		public SpFieldCollection Fields
		{
			get { return DeclaringType.List.Fields; }
		}

		public Field Field
		{
			get { return Fields.GetFieldByInternalName(SpFieldInternalName); }
		}

		public string SpFieldInternalName
		{
			get { return _spFieldInternalName; }
		}

		public string SpFieldTypeAsString
		{
			get { return _spFieldTypeAsString ?? Field.TypeAsString; }
		}

		public IFieldConverter Converter
		{
			get { return _converter ?? (_converter = CreateConverter()); }
		}

		public MetaAccessor<object> MemberAccessor
		{
			get { return _memberAccessor ?? (_memberAccessor = CreateMemberAccessor()); }
		}

		public MetaAccessor<ListItem> SpClientAccessor
		{
			get { return _spClientAccessor ?? (_spClientAccessor = CreateSpClientAccessor()); }
		}

		public Type CustomConverterType
		{
			get { return _customConverterType; }
		}

		#region [Private Methods]

		private IFieldConverter CreateConverter()
		{
			var converter = CustomConverterType == null
				? FieldConverterResolver.Instance.Create(SpFieldTypeAsString)
				: FieldConverterResolver.Instance.Create(CustomConverterType);

			converter.Initialize(Field, Type);

			return converter;
		}

		private MetaAccessor<object> CreateMemberAccessor()
		{
			return new MemberMetaAccessor(this, MemberAccessorPool.Instance.Get(DeclaringType.Type));
		}

		private MetaAccessor<ListItem> CreateSpClientAccessor()
		{
			return new SpClientMetaAccessor(this, Field);
		}

		#endregion
	}
}