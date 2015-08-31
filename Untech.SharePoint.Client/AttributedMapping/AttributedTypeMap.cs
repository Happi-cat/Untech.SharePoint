using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Client.Meta;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.AttributedMapping
{
	public class AttributedTypeMap<T> : IMetaTypeProvider
	{
		public AttributedTypeMap()
		{
			Type = typeof (T);
			Initialize();
		}

		public Type Type { get; private set; }

		public IList<IMetaDataMemberProvider> Properties { get; private set; }

		public MetaType GetMetaType(MetaList metaList)
		{
			return new MetaType(metaList, Type, Properties);
		}

		private void Initialize()
		{
			var attributeType = typeof(SpFieldAttribute);

			var properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Where(n => n.CanRead || n.CanWrite)
				.Select(CreateDataMember)
				.ToList();

			var fields = Type.GetFields(BindingFlags.Instance | BindingFlags.Public)
				.Where(n => n.IsDefined(attributeType))
				.Select(CreateDataMember)
				.ToList();

			Properties = new List<IMetaDataMemberProvider>(properties.Concat(fields));
		}

		private AttributedPropertyPart CreateDataMember(MemberInfo memberInfo)
		{
			return new AttributedPropertyPart(memberInfo, Type);
		}
	}
}