using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class WhereTranslator : ExpressionVisitor, ICamlTranslator
	{
		public WhereTranslator()
		{
			SupportedBinaryExpressions = new Dictionary<ExpressionType, string>
			{
				{ExpressionType.AndAlso, Tags.And},
				{ExpressionType.OrElse, Tags.Or},
				{ExpressionType.Equal, Tags.Eq},
				{ExpressionType.NotEqual, Tags.Neq},
				{ExpressionType.GreaterThan, Tags.Gt},
				{ExpressionType.GreaterThanOrEqual, Tags.Geq},
				{ExpressionType.LessThan, Tags.Lt},
				{ExpressionType.LessThanOrEqual, Tags.Leq},
			};
		}

		protected XElement Root { get; set; }
		protected XElement Current { get; set; }

		protected IReadOnlyDictionary<ExpressionType, string> SupportedBinaryExpressions { get; private set; }


		public XElement Translate(ISpModelContext modelContext, Expression predicate)
		{
			Visit(predicate);

			return Root;
		}


		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			switch (node.Method.Name)
			{
				case "Contains":
					Current = VisitStringContains(node);
					break;
				case "StartsWith":
					Current = VisitStrintStartsWith(node, Tags.BeginsWith);
					break;
				case "Where":
					Current = null;
					base.VisitMethodCall(node);
					Root = new XElement(Tags.Where, Current);
					break;
				default:
					if (node.Method.DeclaringType != typeof(System.Linq.Queryable))
					{
						throw new NotSupportedException();
					}
					base.VisitMethodCall(node);
					break;
			}
			return node;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (!SupportedBinaryExpressions.ContainsKey(node.NodeType))
			{
				throw new NotSupportedException(string.Format("{0} operation not supported", node.NodeType));
			}

			if (node.NodeType == ExpressionType.AndAlso ||
				node.NodeType == ExpressionType.OrElse)
			{
				Current = VisitAndAlsoOrElse(node);
			}
			else
			{
				Current = VisitComparison(node);
			}
			return node;
		}

		private XElement VisitAndAlsoOrElse(BinaryExpression node)
		{
			var tag = SupportedBinaryExpressions[node.NodeType];

			Current = null;
			Visit(node.Left);
			var left = Current;

			Current = null;
			Visit(node.Right);
			var right = Current;

			return new XElement(tag, left, right);
		}

		private XElement VisitComparison(BinaryExpression node)
		{
			var left = node.Left.StripQuotes();
			var right = node.Right.StripQuotes();

			if (left.NodeType == ExpressionType.Constant &&
				right.NodeType == ExpressionType.MemberAccess)
			{
				return VisitComparison((MemberExpression)right, (ConstantExpression)left, node.NodeType);
			}
			if (left.NodeType == ExpressionType.MemberAccess &&
				right.NodeType == ExpressionType.Constant)
			{
				return VisitComparison((MemberExpression)left, (ConstantExpression)right, node.NodeType);
			}

			throw new NotSupportedException(string.Format("{0} not supported", node));
		}

		private XElement VisitStringContains(MethodCallExpression node)
		{
			if (node.Method.DeclaringType != null && node.Method.DeclaringType.FullName == typeof(string).FullName)
				return VisitStrintStartsWith(node, Tags.Contains);
			if (node.Method.DeclaringType != null && node.Method.DeclaringType.FullName == typeof(Enumerable).FullName)
				return VisitArrayContains(node);
			throw new NotSupportedException();
		}

		private XElement VisitStrintStartsWith(MethodCallExpression mce, string operatorTag)
		{
			var me = (MemberExpression)mce.Object;

			if (me != null)
			{
				var fieldRef = VisitField(me);
				var value = VisitValue(mce.Arguments[0]);

				return new XElement(operatorTag, fieldRef, value);
			}

			throw new NotSupportedException();
		}

		private XElement VisitArrayContains(MethodCallExpression mce)
		{
			var me = (MemberExpression)mce.Arguments[1];

			if (me != null)
			{
				var listMe = (ConstantExpression)mce.Arguments[0];

				if (listMe == null)
					throw new NotSupportedException();

				//var fieldType = GetFieldType(me);
				//var type = list.Value.GetType();
				//var fieldInfo = type.GetField(listName);
				var values = (IEnumerable)listMe.Value;

				var fieldRef = VisitField(me);

				var valuesElement = new XElement(Tags.Values);

				foreach (var value in values)
				{
					valuesElement.Add(new XElement(Tags.Value, value));
				}

				return new XElement(Tags.In, fieldRef, valuesElement);
			}

			throw new NotSupportedException();
		}
		private XElement VisitComparison(MemberExpression left, ConstantExpression right, ExpressionType type)
		{
			var tag = SupportedBinaryExpressions[type];

			if (right.Value == null)
			{
				return VisitComparisonWithNull(left, type);
			}

			var leftElement = VisitField(left);
			var rightElement = VisitValue(right);

			return new XElement(tag, leftElement, rightElement);
		}

		private XElement VisitComparisonWithNull(MemberExpression left, ExpressionType type)
		{
			switch (type)
			{
				case ExpressionType.Equal:
					return new XElement(Tags.IsNull, VisitField(left));
				case ExpressionType.NotEqual:
					return new XElement(Tags.IsNotNull, VisitField(left));
			}
			throw new InvalidOperationException();
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
			throw new NotSupportedException();
		}
	}
}