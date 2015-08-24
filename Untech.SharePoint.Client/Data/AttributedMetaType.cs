using System;
using System.Linq;
using System.Reflection;

namespace Untech.SharePoint.Client.Data
{
	internal sealed class AttributedMetaType : MetaType
	{
		private DataMemberCollection _dataMembers;

		public AttributedMetaType(MetaModel model, MetaList list, Type type)
			: base(model, list, type)
		{
			InitDataMembers();
		}


		public override DataMemberCollection DataMembers
		{
			get { return _dataMembers; }
		}

		private void InitDataMembers()
		{
			var attributeType = typeof(SpFieldAttribute);

			var properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Where(n => n.CanRead || n.CanWrite)
				.Select(CreateDataMember)
				.ToList();

			var fields = Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Select(CreateDataMember)
				.ToList();

			_dataMembers = new DataMemberCollection(properties.Concat(fields));
		}

		private MetaDataMember CreateDataMember(MemberInfo memberInfo)
		{
			return new AttributedMetaDataMember(this, memberInfo);
		}
	}
}