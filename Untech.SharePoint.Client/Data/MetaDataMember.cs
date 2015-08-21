using System;
using System.ComponentModel;
using System.Reflection;
using Untech.SharePoint.Client.Data.FieldConverters;
using Untech.SharePoint.Client.Reflection;

namespace Untech.SharePoint.Client.Data
{
	//internal class MetaDataMember
	//{
	//	public MetaDataMember(MetaType metaType, PropertyInfo propertyInfo)
	//	{
	//		Name = propertyInfo.Name;
	//		Type = propertyInfo.PropertyType;

	//		DeclaringType = metaType;

	//		RetrieveSpFieldInfo(propertyInfo);
	//		RetrieveDefaultValue(propertyInfo);
	//		RegisterCustomConverter();
	//	}

	//	public MetaDataMember(MetaType metaType, FieldInfo fieldInfo)
	//	{
	//		Name = fieldInfo.Name;
	//		Type = fieldInfo.FieldType;

	//		DeclaringType = metaType;

	//		RetrieveSpFieldInfo(fieldInfo);
	//		RetrieveDefaultValue(fieldInfo);
	//		RegisterCustomConverter();
	//	}

	//	public MetaType DeclaringType { get; private set; }

	//	public string Name { get; private set; }

	//	public Type Type { get; private set; }

	//	public string SpFieldInternalName { get; private set; }

	//	public Type CustomConverterType { get; private set; }

	//	public object DefaultValue { get; private set; }

	//	protected MemberAccessor MemberAccessor { get { return DeclaringType.MemberAccessor; } }


	//	public object GetValue(object instance)
	//	{
	//		return MemberAccessor[instance, Name];
	//	}

	//	public void SetValue(object instance, object value)
	//	{
	//		MemberAccessor[instance, Name] = value;
	//	}

	//	private void RetrieveSpFieldInfo(MemberInfo memberInfo)
	//	{
	//		var attribute = memberInfo.GetCustomAttribute<SpFieldAttribute>();
	//		if (attribute == null)
	//		{
	//			throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", memberInfo.Name), "memberInfo");
	//		}

	//		SpFieldInternalName = attribute.InternalName ?? Name;
	//		CustomConverterType = attribute.CustomConverterType;
	//	}

	//	private void RetrieveDefaultValue(MemberInfo memberInfo)
	//	{
	//		var attribute = memberInfo.GetCustomAttribute<DefaultValueAttribute>();
	//		if (attribute == null)
	//		{
	//			return;
	//		}

	//		DefaultValue = attribute.Value;
	//	}

	//	private void RegisterCustomConverter()
	//	{
	//		if (CustomConverterType != null)
	//		{
	//			FieldConverterResolver.Instance.Register(CustomConverterType);
	//		}
	//	}
	//}
}