using System;
using System.Reflection;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.FieldConverters;

namespace Untech.SharePoint.Client.Data
{
	internal class AttributedMetaDataMember : MetaDataMember
	{
		private readonly MetaType _declaringType;
		private readonly MemberInfo _member;
		private readonly string _name;
		private readonly Type _type;
		private string _spFieldInternalName;
		private Type _customConverterType;
		private IFieldConverter _converter;
		private MetaAccessor<object> _memberAccessor;
		private MetaAccessor<ListItem> _spClientAccessor;

		public AttributedMetaDataMember(MetaType declarintType, PropertyInfo propertyInfo)
			: this(declarintType, (MemberInfo)propertyInfo)
		{
			_type = propertyInfo.PropertyType;
		}

		public AttributedMetaDataMember(MetaType declarintType, FieldInfo fieldInfo)
			: this(declarintType, (MemberInfo)fieldInfo)
		{
			_type = fieldInfo.FieldType;
		}

		private AttributedMetaDataMember(MetaType declaringType, MemberInfo memberInfo)
		{
			_declaringType = declaringType;
			_member = memberInfo;
			_name = memberInfo.Name;

			RetrieveSpFieldInfo(memberInfo);
			RegisterCustomConverter();
		}

		public SpFieldCollection Fields
		{
			get { return DeclaringType.List.Fields; }
		}

		public Field Field
		{
			get { return Fields.GetFieldByInternalName(SpFieldInternalName); }
		}

		public override MetaType DeclaringType
		{
			get { return _declaringType; }
		}

		public override MemberInfo Member
		{
			get { return _member; }
		}

		public override string Name
		{
			get { return _name; }
		}

		public override Type Type
		{
			get { return _type; }
		}

		public override string SpFieldInternalName
		{
			get { return _spFieldInternalName; }
		}

		public override string SpFieldTypeAsString
		{
			get { return Field.TypeAsString; }
		}

		public override IFieldConverter Converter
		{
			get { return _converter ?? (_converter = CreateConverter()); }
		}

		public override MetaAccessor<object> MemberAccessor
		{
			get { return _memberAccessor ?? (_memberAccessor = CreateMemberAccessor()); }
		}

		public override MetaAccessor<ListItem> SpClientAccessor
		{
			get { return _spClientAccessor ?? (_spClientAccessor = CreateSpClientAccessor()); }
		}

		public Type CustomConverterType
		{
			get { return _customConverterType; }
		}

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
			return new MemberMetaAccessor(this, null);
		}

		private MetaAccessor<ListItem> CreateSpClientAccessor()
		{
			return new SpClientMetaAccessor(this, Field);
		}

		private void RetrieveSpFieldInfo(MemberInfo memberInfo)
		{
			var attribute = memberInfo.GetCustomAttribute<SpFieldAttribute>();
			if (attribute == null)
			{
				throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", memberInfo.Name), "memberInfo");
			}

			_spFieldInternalName = attribute.InternalName ?? Name;
			_customConverterType = attribute.CustomConverterType;
		}

		private void RegisterCustomConverter()
		{
			if (CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(CustomConverterType);
			}
		}
	}
}