using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Data.Translators.ExpressionVisitors;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal class CamlPredicateProcessor
	{
		private static readonly IReadOnlyDictionary<ExpressionType, ComparisonOperator> ExpressionTypeMap = new Dictionary<ExpressionType, ComparisonOperator>
		{
			{ExpressionType.Equal, ComparisonOperator.Eq},
			{ExpressionType.NotEqual, ComparisonOperator.Neq},
			{ExpressionType.LessThanOrEqual, ComparisonOperator.Leq},
			{ExpressionType.LessThan, ComparisonOperator.Lt},
			{ExpressionType.GreaterThan, ComparisonOperator.Gt},
			{ExpressionType.GreaterThanOrEqual, ComparisonOperator.Geq}
		};

		private readonly IEnumerable<ExpressionVisitor> _preProcessors;

		public CamlPredicateProcessor()
		{
			_preProcessors = new List<ExpressionVisitor>
			{
				new Evaluator(),
				new StringIsNullOrEmptyRewriter(),
				new PushNotDownVisitor(),
				new RedundantConditionRemover(),
			};
		}

		public WhereModel Process(Expression predicate)
		{
			predicate = _preProcessors.Aggregate(predicate, (n, v) => v.Visit(n));

			throw new NotImplementedException();
		}

		protected WhereModel Translate(Expression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					return TranslateAnd((BinaryExpression)node);
				case ExpressionType.Equal:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.NotEqual:
					return TranslateComparison((BinaryExpression)node);
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					return TranslateOr((BinaryExpression)node);
				case ExpressionType.Not:
					return TranslateNot((UnaryExpression)node);
				case ExpressionType.Call:
					return TranslateCall((MethodCallExpression) node);

			}
			throw new NotSupportedException();
		}

		private WhereModel TranslateNot(UnaryExpression unaryNode)
		{
			throw new NotImplementedException();
		}

		private WhereModel TranslateAnd(BinaryExpression binaryNode)
		{
			if (binaryNode.Left.IsConstant(true))
			{
				return Translate(binaryNode.Right);
			}
			if (binaryNode.Right.IsConstant(true))
			{
				return Translate(binaryNode.Left);
			}
			return WhereModel.And(Translate(binaryNode.Left), Translate(binaryNode.Right));
		}

		private WhereModel TranslateOr(BinaryExpression binaryNode)
		{
			if (binaryNode.Left.IsConstant(false))
			{
				return Translate(binaryNode.Right);
			}
			if (binaryNode.Right.IsConstant(false))
			{
				return Translate(binaryNode.Left);
			}
			return WhereModel.Or(Translate(binaryNode.Left), Translate(binaryNode.Right));
		}

		private WhereModel TranslateComparison(BinaryExpression binaryNode)
		{
			if (ExpressionTypeMap.ContainsKey(binaryNode.NodeType))
			{
				return new ComparisonModel(ExpressionTypeMap[binaryNode.NodeType], null, null);
			}
			throw new NotImplementedException();
		}

		private WhereModel TranslateCall(MethodCallExpression callNode)
		{
			if (callNode.Method == OpUtils.StrContains)
			{
				return TranslateContains(callNode);
			}
			if (callNode.Method == OpUtils.StrStartsWith)
			{
				return TranslateStartsWith(callNode);
			}
			if (callNode.Method == OpUtils.StrIsNullOrEmpty)
			{
				return TranslateStartsWith(callNode);
			}
			throw new NotImplementedException();
		}

		private WhereModel TranslateContains(MethodCallExpression callNode)
		{
			throw new NotImplementedException();
		}

		private WhereModel TranslateStartsWith(MethodCallExpression callNode)
		{
			throw new NotImplementedException();
		}
	}
}