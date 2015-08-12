using System;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Core.Data;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal class FieldRefExpression : CamlExpression
	{
		public FieldRefExpression(MemberInfo member)
		{
			MetaProperty = ResolveMetaProperty(member);
		}

		public MetaProperty MetaProperty { get; private set; }

		public override ExpressionType NodeType { get { return (ExpressionType) CamlExpressionType.FieldRef; } }

		public override Type Type { get { return MetaProperty.MemberType; } }

		private MetaProperty ResolveMetaProperty(MemberInfo member)
		{
			var model = MetaModelPool.Instance.Get(member.DeclaringType);

			return model.MetaProperties[member.Name];
		}

		protected internal override Expression Accept(CamlExpressionVisitor visitor)
		{
			return visitor.VisitFieldRefExpression(this);
		}

		public override string ToString()
		{
			return string.Format(".FieldRef( Name: '{0}' )", MetaProperty.MemberName);
		}
	}
}