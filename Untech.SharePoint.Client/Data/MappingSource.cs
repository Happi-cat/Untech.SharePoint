using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Client.Data.FieldConverters;
using Untech.SharePoint.Client.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal abstract class MappingSource
	{


	}

	internal interface IMetaList
	{
		MetaModel Model { get; }

		string ListTitle { get; }

		IReadOnlyCollection<Field> ListFields { get; }

		IMetaType ItemType { get; }
	}

	internal interface IMetaType
	{
		MetaModel Model { get; }

		IMetaList List { get; }

		Type Type { get; }

		IReadOnlyCollection<IMetaDataMember> DataMembers { get; }
	}

	internal interface IMetaDataMember
	{
		IMetaType DeclaringType { get; }

		MemberInfo Member { get; }

		string Name { get; }

		Type Type { get; }

		string SpFieldInternalName { get; }

		string SpFieldTypeAsString { get; }

		IFieldConverter Converter { get; }

		MetaAccessor<object> MemberAccessor { get; }

		MetaAccessor<ListItem> SpClientAccessor { get; }
	}

	internal sealed class TypeMapper
	{
		public TypeMapper(IMetaType type)
		{
			Type = type;
		}

		public IMetaType Type { get; private set; }

		public void Map(object source, ListItem dest)
		{
			Type.DataMembers
				.Select(n => new DataMemberMapper(n))
				.ToList()
				.ForEach(n => n.Map(source, dest));
		}

		public void Map(ListItem source, object dest)
		{
			Type.DataMembers
				.Select(n => new DataMemberMapper(n))
				.ToList()
				.ForEach(n => n.Map(source, dest));
		}
	}

	internal sealed class DataMemberMapper
	{
		public DataMemberMapper(IMetaDataMember member)
		{
			DataMember = member;
		}

		public IMetaDataMember DataMember { get; private set; }

		public void Map(object source, ListItem dest)
		{
			try
			{
				if (DataMember.MemberAccessor.CanRead &&
					DataMember.SpClientAccessor.CanWrite)
				{
					var clrValue = DataMember.MemberAccessor.GetValue(source);
					var clientValue = DataMember.Converter.ToSpClientValue(clrValue);
					DataMember.SpClientAccessor.SetValue(dest, clientValue);
				}
			}
			catch (Exception e)
			{
				throw new MemberMappingException(DataMember, e);
			}
		}

		public void Map(ListItem source, object dest)
		{
			try
			{
				if (DataMember.SpClientAccessor.CanRead &&
					DataMember.MemberAccessor.CanWrite)
				{
					var clientValue = DataMember.SpClientAccessor.GetValue(source);
					var clrValue = DataMember.Converter.FromSpClientValue(clientValue);
					DataMember.MemberAccessor.SetValue(dest, clrValue);
				}
			}
			catch (Exception e)
			{
				throw new MemberMappingException(DataMember, e);
			}
		}
	}

	internal abstract class MetaAccessor<T>
	{
		public MetaAccessor(IMetaDataMember member) 
		{
			Guard.CheckNotNull("member", member);

			DataMember = member;
		}
		
		public IMetaDataMember DataMember { get; private set; }

		public virtual bool CanRead { get { return true; } }

		public virtual bool CanWrite { get { return true; } }
		
		public abstract object GetValue(T instance);

		public abstract void SetValue(T instance, object value);
	}

	internal class MemberMetaAccessor : MetaAccessor<object>
	{
		public MemberMetaAccessor(IMetaDataMember member, MemberAccessor accessor)
			:base(member)
		{
			Accessor = accessor;
		}

		public IMetaDataMember DataMember { get; private set; }

		public MemberAccessor Accessor { get; private set; }

		public override object GetValue(object instance)
		{
			if (!CanRead) throw new InvalidOperationException();

			return Accessor[instance, DataMember.Name];
		}

		public override void SetValue(object instance, object value)
		{
			if (!CanWrite) throw new InvalidOperationException();

			Accessor[instance, DataMember.Name] = value;
		}

		public override bool CanRead
		{
			get { return Accessor.CanRead(DataMember.Name); }
		}

		public override bool CanWrite
		{
			get { return Accessor.CanWrite(DataMember.Name); }
		}
	}

	internal class SpClientMetaAccessor : MetaAccessor<ListItem>
	{
		public SpClientMetaAccessor(IMetaDataMember member, Field field)
			: base(member)
		{
			SpField = field;
		}

		public Field SpField { get; private set; }

		public override object GetValue(ListItem instance)
		{
			if (!CanRead) throw new InvalidOperationException();

			return instance[DataMember.SpFieldInternalName];
		}

		public override void SetValue(ListItem instance, object value)
		{
			if (!CanWrite) throw new InvalidOperationException();

			instance[DataMember.SpFieldInternalName] = value;
		}

		public override bool CanWrite
		{
			get { return !SpField.ReadOnlyField; }
		}
	}

	internal class AttributedMetaDataMember : IMetaDataMember
	{
		public AttributedMetaDataMember(IMetaType declarintType, PropertyInfo propertyInfo)
			: this(declarintType, (MemberInfo)propertyInfo)
		{
			Type = propertyInfo.PropertyType;
		}

		public AttributedMetaDataMember(IMetaType declarintType, FieldInfo fieldInfo)
			: this(declarintType, (MemberInfo)fieldInfo)
		{
			Type = fieldInfo.FieldType;
		}

		private AttributedMetaDataMember(IMetaType declaringType, MemberInfo memberInfo)
		{
			DeclaringType = declaringType;
			Member = memberInfo;
			Name = memberInfo.Name;

			RetrieveSpFieldInfo(memberInfo);
			RegisterCustomConverter();
		}

		public IReadOnlyCollection<Field> Fields { get; private set; }

		public IMetaType DeclaringType { get; private set; }

		public MemberInfo Member { get; private set; }

		public string Name { get; private set; }

		public Type Type { get; private set; }

		public string SpFieldInternalName { get; private set; }

		public string SpFieldTypeAsString { get; private set; }

		public IFieldConverter Converter { get; private set; }

		public MetaAccessor<object> MemberAccessor { get; private set; }

		public MetaAccessor<ListItem> SpClientAccessor { get; private set; }

		public Type CustomConverterType { get; private set; }

		private void RetrieveSpFieldInfo(MemberInfo memberInfo)
		{
			var attribute = memberInfo.GetCustomAttribute<SpFieldAttribute>();
			if (attribute == null)
			{
				throw new ArgumentException(string.Format("Member {0} has no attribute SpFieldAttribute", memberInfo.Name), "memberInfo");
			}

			SpFieldInternalName = attribute.InternalName ?? Name;
			CustomConverterType = attribute.CustomConverterType;
		}

		private void RegisterCustomConverter()
		{
			if (CustomConverterType != null)
			{
				FieldConverterResolver.Instance.Register(CustomConverterType);
			}
		}

		private void InitConverter()
		{

		}

		private void InitAccessors()
		{

		}
	}

}
