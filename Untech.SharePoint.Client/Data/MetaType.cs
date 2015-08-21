using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Client.Reflection;

namespace Untech.SharePoint.Client.Data
{
	//internal class MetaType
	//{
	//	public MetaType(Type type)
	//	{
	//		Type = type;

	//		MemberAccessor = new MemberAccessor();
	//		MemberAccessor.Initialize(type);

	//		InitDataMembers();
	//		InitDataMembersMap();
	//	}

	//	public Type Type { get; private set; }

	//	public MemberAccessor MemberAccessor { get; static private set; }

	//	public IReadOnlyCollection<MetaDataMember> DataMembers { get; private set; }

	//	protected IReadOnlyDictionary<string, MetaDataMember> DataMembersMap { get; private set; }

	//	public MetaDataMember GetDataMember(string memberName)
	//	{
	//		return DataMembersMap[memberName];
	//	}

	//	private void InitDataMembers()
	//	{
	//		var attributeType = typeof(SpFieldAttribute);

	//		var properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
	//			.Where(n => n.IsDefined(attributeType))
	//			.Where(n => n.CanRead && n.CanWrite)
	//			.ToList();

	//		var fields = Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
	//			.Where(n => n.IsDefined(attributeType))
	//			.ToList();

	//		var dataMembers = new List<MetaDataMember>();

	//		dataMembers.AddRange(properties.Select(CreateDataMember));
	//		dataMembers.AddRange(fields.Select(CreateDataMember));

	//		DataMembers = dataMembers;
	//	}

	//	private void InitDataMembersMap()
	//	{
	//		DataMembersMap = DataMembers.ToDictionary(n => n.Name);
	//	}

	//	private MetaDataMember CreateDataMember(PropertyInfo propertyInfo)
	//	{
	//		return new MetaDataMember(this, propertyInfo);	
	//	}

	//	private MetaDataMember CreateDataMember(FieldInfo fieldInfo)
	//	{
	//		return new MetaDataMember(this, fieldInfo);	
	//	}
	//}
}
