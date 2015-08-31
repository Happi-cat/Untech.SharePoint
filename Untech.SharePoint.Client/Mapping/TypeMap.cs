using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Client.Data;
using Untech.SharePoint.Client.Extensions;
using Untech.SharePoint.Client.Meta;
using Untech.SharePoint.Client.Meta.Providers;

namespace Untech.SharePoint.Client.Mapping
{
	public class TypeMap<T> : IMetaTypeProvider
	{
		public TypeMap()
		{
			Type = typeof (T);
			Properties = new List<IMetaDataMemberProvider>();
		}

		public Type Type { get; private set; }

		public IList<IMetaDataMemberProvider> Properties { get; private set; }

		public PropertyPart Map(Expression<Func<T, object>> memberExpression)
		{
			var expression = memberExpression.GetLambda().Body as MemberExpression;
			if (expression != null)
			return Map(expression.Member);

			throw new ArgumentException();
		}

		private PropertyPart Map(MemberInfo member)
		{
			var propertyMap = new PropertyPart(member, Type);

			Properties.Add(propertyMap);

			return propertyMap;
		}

		public MetaType GetMetaType(MetaList metaList)
		{
			return new MetaType(metaList, Type, Properties);
		}
	}
}