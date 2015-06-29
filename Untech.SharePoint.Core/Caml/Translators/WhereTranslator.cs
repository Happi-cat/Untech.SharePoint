using System;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Untech.SharePoint.Core.Caml.Translators
{
	public class WhereTranslator : ExpressionVisitor
	{
		private XElement _root;
		private XElement _current;

		public XElement Translate(Expression predicate)
		{
			_root = new XElement(Tags.Where);

			Visit(predicate);

			_root.Add(_current);

			return _root;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse)
			{
				_current = VisitAndOr(node);
			}
			else if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual)
			{
				_current = VisitEqual(node);
			}
			else
			{
				return base.VisitBinary(node);
			}
			return node;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			return base.VisitMember(node);
		}

		private XElement VisitAndOr(BinaryExpression node)
		{
			var tag = "";

			if (node.NodeType == ExpressionType.AndAlso) tag = Tags.And;
			if (node.NodeType == ExpressionType.OrElse) tag = Tags.Or;


			_current = null;

			Visit(node.Left);
			var leftElement = _current;

			Visit(node.Right);
			var rightElement = _current;

			return new XElement(tag, leftElement, rightElement);
		}

		private XElement VisitEqual(BinaryExpression node)
		{
			var tag = "";

			if (node.NodeType == ExpressionType.Equal) tag = Tags.Eq;
			if (node.NodeType == ExpressionType.NotEqual) tag = Tags.Neq;
			if (node.NodeType == ExpressionType.GreaterThan) tag = Tags.Gt;
			if (node.NodeType == ExpressionType.GreaterThanOrEqual) tag = Tags.Geq;
			if (node.NodeType == ExpressionType.LessThan) tag = Tags.Lt;
			if (node.NodeType == ExpressionType.LessThanOrEqual) tag = Tags.Leq;

			_current = null;
			var leftElement = VisitField(node.Left);
			var rightElement = VisitValue(node.Right);

			return new XElement(tag, leftElement, rightElement);
		}

		private XElement VisitField(Expression node)
		{
			var member = (MemberExpression)node;
			var name = member.Member.Name;

			return new XElement(Tags.FieldRef, new XAttribute(Tags.Name, name));
		}

		private XElement VisitValue(Expression node)
		{
			if (node.NodeType == ExpressionType.Constant)
			{
				var constExp = (ConstantExpression)node;
				var value = (constExp.Value ?? "").ToString();

				return new XElement(Tags.Value, value);
			}
			if (node.NodeType == ExpressionType.MemberAccess)
			{
				LambdaExpression lambda = Expression.Lambda(node);
				Delegate fn = lambda.Compile();
				var constExp = Expression.Constant(fn.DynamicInvoke(null), node.Type);
				var value = (constExp.Value ?? "").ToString();

				return new XElement(Tags.Value, value);
			}
			throw new NotSupportedException();
		}
	}
}