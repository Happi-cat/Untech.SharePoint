using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Untech.SharePoint.Core.Extensions;

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
		protected ISpModelContext ModelContext { get; private set; }

		protected IReadOnlyDictionary<ExpressionType, string> SupportedBinaryExpressions { get; private set; }

		public XElement Translate(ISpModelContext modelContext, Expression predicate)
		{
			ModelContext = modelContext;

			Visit(predicate);

			return Root;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			switch (node.Method.Name)
			{
				case "Contains":
					Current = VisitStringOrArrayContains(node);
					break;
				case "StartsWith":
					Current = VisitStringContainsOrStartsWith(node, Tags.BeginsWith);
					break;
				case "Where":
					Current = null;
					Visit(node.Arguments[1]);
					Root = new XElement(Tags.Where, Current);
					break;
				default:
					if (node.Method.DeclaringType != typeof(Queryable))
					{
						throw new NotSupportedException(string.Format("Method call {0} not supported", node));
					}
					Visit(node.Arguments[0]);
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

			if (left.NodeType == ExpressionType.Constant && right.NodeType == ExpressionType.MemberAccess)
			{
				return VisitComparison((MemberExpression)right, (ConstantExpression)left, node.NodeType);
			}
			if (left.NodeType == ExpressionType.MemberAccess && right.NodeType == ExpressionType.Constant)
			{
				return VisitComparison((MemberExpression)left, (ConstantExpression)right, node.NodeType);
			}

			throw new NotSupportedException(string.Format("Comparison {0} not supported", node));
		}

		private XElement VisitStringOrArrayContains(MethodCallExpression node)
		{
			if (node.Method.DeclaringType != null &&
				node.Method.DeclaringType == typeof(string))
			{
				return VisitStringContainsOrStartsWith(node, Tags.Contains);
			}
			if (node.Method.DeclaringType != null &&
				node.Method.DeclaringType == typeof(Enumerable))
			{
				return VisitArrayContains(node);
			}
			throw new NotSupportedException(string.Format("Method call {0} not supported", node));
		}

		private XElement VisitStringContainsOrStartsWith(MethodCallExpression node, string operatorTag)
		{
			if (node.Object == null || node.Object.NodeType != ExpressionType.MemberAccess)
			{
				throw new NotSupportedException(string.Format("Method call {0} not supported", node));
			}

			var memberExpression = (MemberExpression)node.Object;

			return new XElement(operatorTag,
				TranslatorHelpers.GetFieldRef(ModelContext, memberExpression),
				TranslatorHelpers.GetValue(ModelContext, memberExpression, node.Arguments[0]));
		}

		private XElement VisitArrayContains(MethodCallExpression node)
		{
			if (node.Arguments[0].NodeType != ExpressionType.Constant ||
				node.Arguments[1].NodeType != ExpressionType.MemberAccess)
			{
				throw new NotSupportedException(string.Format("Method call {0} not supported", node));
			}

			var listExpression = (ConstantExpression)node.Arguments[0];
			var memberExpression = (MemberExpression)node.Arguments[1];

			var values = (IEnumerable)listExpression.Value;

			var valuesElement = new XElement(Tags.Values);

			foreach (var value in values)
			{
				valuesElement.Add(new XElement(Tags.Value, value));
			}

			return new XElement(Tags.In,
				TranslatorHelpers.GetFieldRef(ModelContext, memberExpression),
				valuesElement);
		}

		private XElement VisitComparison(MemberExpression left, ConstantExpression right, ExpressionType type)
		{
			var tag = SupportedBinaryExpressions[type];

			if (right.Value == null)
			{
				return VisitComparisonWithNull(left, type);
			}

			return new XElement(tag,
				TranslatorHelpers.GetFieldRef(ModelContext, left),
				TranslatorHelpers.GetValue(ModelContext, left, right));
		}

		private XElement VisitComparisonWithNull(MemberExpression left, ExpressionType type)
		{
			switch (type)
			{
				case ExpressionType.Equal:
					return new XElement(Tags.IsNull, TranslatorHelpers.GetFieldRef(ModelContext, left));
				case ExpressionType.NotEqual:
					return new XElement(Tags.IsNotNull, TranslatorHelpers.GetFieldRef(ModelContext, left));
			}
			throw new NotSupportedException(string.Format("{0} operation can't be used in comparison with null", type));
		}
	}
}