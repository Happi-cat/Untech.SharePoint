using System;
using System.Reflection;
using Microsoft.SharePoint.Client;
using Untech.SharePoint.Client.Data.FieldConverters;
using Untech.SharePoint.Client.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class AttributedMetaDataMember : MetaDataMember
	{
		private string _spFieldInternalName;
		private Type _customConverterType;
		private IFieldConverter _converter;
		private MetaAccessor<object> _memberAccessor;
		private MetaAccessor<ListItem> _spClientAccessor;

		public AttributedMetaDataMember(MetaType declarintType, MemberInfo memberInfo)
			: base(declarintType, memberInfo)
		{
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

		#endregion

	}
}