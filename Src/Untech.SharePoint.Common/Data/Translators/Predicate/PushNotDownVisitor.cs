using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class PushNotDownVisitor : ExpressionVisitor
	{
		public PushNotDownVisitor()
		{
			NegateRules = new List<INegateRule>
			{
				new ComparisonNegateRule(),
				new LogicalJoinNegateRule(),
				new NotNegateRule(),
				new BoolConstNegateRule()
			};
		}

		private IReadOnlyCollection<INegateRule> NegateRules { get; }

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.Method != null || node.NodeType != ExpressionType.Not)
			{
				return base.VisitUnary(node);
			}

			var operand = node.Operand.StripQuotes();

			var rule = NegateRules.FirstOrDefault(n => n.CanNegate(operand));
			return rule != null ? Visit(rule.Negate(operand)) : base.VisitUnary(node);
		}

		#region [Nested Classes]

		private interface INegateRule
		{
			bool CanNegate(Expression node);

			Expression Negate(Expression node);
		}

		private class BoolConstNegateRule : INegateRule
		{
			public bool CanNegate(Expression node)
			{
				return node.NodeType == ExpressionType.Constant;
			}

			public Expression Negate(Expression node)
			{
				var constNode = (ConstantExpression)node;

				return constNode.Type == typeof(bool) ? Expression.Constant(!(bool)constNode.Value) : constNode;
			}
		}

		private class ComparisonNegateRule : INegateRule
		{

			private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> NegateMap = new Dictionary
				<ExpressionType, ExpressionType>
			{
				{ExpressionType.Equal, ExpressionType.NotEqual},
				{ExpressionType.NotEqual, ExpressionType.Equal},
				{ExpressionType.LessThan, ExpressionType.GreaterThanOrEqual},
				{ExpressionType.LessThanOrEqual, ExpressionType.GreaterThan},
				{ExpressionType.GreaterThan, ExpressionType.LessThanOrEqual},
				{ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan}
			};

			public bool CanNegate(Expression node)
			{
				return NegateMap.ContainsKey(node.NodeType);
			}

			public Expression Negate(Expression node)
			{
				var binaryNode = (BinaryExpression)node;

				return Expression.MakeBinary(NegateMap[node.NodeType], binaryNode.Left, binaryNode.Right,
					binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);
			}
		}

		private class LogicalJoinNegateRule : INegateRule
		{
			private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> NegateMap = new Dictionary
				<ExpressionType, ExpressionType>
			{
				{ExpressionType.And, ExpressionType.Or},
				{ExpressionType.AndAlso, ExpressionType.OrElse},
				{ExpressionType.Or, ExpressionType.And},
				{ExpressionType.OrElse, ExpressionType.AndAlso}
			};

			public bool CanNegate(Expression node)
			{
				return NegateMap.ContainsKey(node.NodeType);
			}

			public Expression Negate(Expression node)
			{
				var binaryNode = (BinaryExpression)node;

				return Expression.MakeBinary(NegateMap[node.NodeType],
					Expression.Not(binaryNode.Left), Expression.Not(binaryNode.Right),
					binaryNode.IsLiftedToNull, binaryNode.Method, binaryNode.Conversion);
			}
		}

		private class NotNegateRule : INegateRule
		{
			public bool CanNegate(Expression node)
			{
				return node.NodeType == ExpressionType.Not;
			}

			public Expression Negate(Expression node)
			{
				var notNode = (UnaryExpression)node;

				return notNode.Operand;
			}
		}

		#endregion

	}
}