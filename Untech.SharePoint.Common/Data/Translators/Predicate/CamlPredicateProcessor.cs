using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
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

		public CamlPredicateProcessor()
		{
			PreProcessors = new List<ExpressionVisitor>
			{
				new Evaluator(),
				new StringIsNullOrEmptyRewriter(),
				new PushNotDownVisitor(),
				new InRewriter(),
				new RedundantConditionRemover(),
				new SwapComparisonVisitor()
			};
		}

		protected IEnumerable<ExpressionVisitor> PreProcessors { get; set; }

		public WhereModel Process(Expression predicate)
		{
			predicate = predicate.StripQuotes();
			if (predicate.NodeType == ExpressionType.Lambda)
			{
				predicate = ((LambdaExpression) predicate).Body;
			}
			
			predicate = PreProcessors.Aggregate(predicate, (n, v) => v.Visit(n));

			return Translate(predicate);
		}

		protected WhereModel Translate(Expression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					return TranslateAnd((BinaryExpression) node);
				case ExpressionType.Equal:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.NotEqual:
					return TranslateComparison((BinaryExpression) node);
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					return TranslateOr((BinaryExpression) node);
				case ExpressionType.Call:
					return TranslateCall((MethodCallExpression) node);
				case ExpressionType.Quote:
					return Translate(((UnaryExpression) node).Operand);
				case ExpressionType.MemberAccess:
					return TranslateTrueProperty((MemberExpression) node);
				case ExpressionType.Not:
					return TranslateFalseProperty((UnaryExpression) node);
			}
			throw InvalidQuery(node);
		}

		#region [Private Methods]

		private WhereModel TranslateTrueProperty(MemberExpression memberNode)
		{
			return new ComparisonModel(ComparisonOperator.Eq, GetFieldRef(memberNode), true);
		}

		private WhereModel TranslateFalseProperty(UnaryExpression unaryNode)
		{
			return new ComparisonModel(ComparisonOperator.Eq, GetFieldRef(unaryNode.Operand), false);
		}

		private WhereModel TranslateAnd(BinaryExpression binaryNode)
		{
			return WhereModel.And(Translate(binaryNode.Left), Translate(binaryNode.Right));
		}

		private WhereModel TranslateOr(BinaryExpression binaryNode)
		{
			return WhereModel.Or(Translate(binaryNode.Left), Translate(binaryNode.Right));
		}

		private WhereModel TranslateComparison(BinaryExpression binaryNode)
		{
			if (ExpressionTypeMap.ContainsKey(binaryNode.NodeType))
			{
				if (binaryNode.Right.IsConstant(null))
				{
					return TranslateComparisonWithNull(binaryNode);
				}
				if (binaryNode.Left.NodeType == ExpressionType.MemberAccess)
				{
					return new ComparisonModel(ExpressionTypeMap[binaryNode.NodeType], GetFieldRef(binaryNode.Left),
						GetValue(binaryNode.Right));
				}
			}
			throw InvalidQuery(binaryNode);
		}

		private WhereModel TranslateComparisonWithNull(BinaryExpression binaryNode)
		{
			if (binaryNode.NodeType == ExpressionType.Equal)
			{
				return new ComparisonModel(ComparisonOperator.IsNull, GetFieldRef(binaryNode.Left), null);
			}
			if (binaryNode.NodeType == ExpressionType.NotEqual)
			{
				return new ComparisonModel(ComparisonOperator.IsNotNull, GetFieldRef(binaryNode.Left), null);
			}
			throw InvalidQuery(binaryNode);
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
			throw InvalidQuery(callNode);
		}

		private WhereModel TranslateContains(MethodCallExpression callNode)
		{
			return new ComparisonModel(ComparisonOperator.Contains, GetFieldRef(callNode.Object), GetValue(callNode.Arguments[0]));
		}

		private WhereModel TranslateStartsWith(MethodCallExpression callNode)
		{
			return new ComparisonModel(ComparisonOperator.BeginsWith, GetFieldRef(callNode.Object),
				GetValue(callNode.Arguments[0]));
		}

		private object GetValue(Expression node)
		{
			if (node.NodeType == ExpressionType.Constant)
			{
				return ((ConstantExpression) node).Value;
			}
			throw InvalidQuery(node);
		}

		private FieldRefModel GetFieldRef(Expression node)
		{
			if (node.NodeType == ExpressionType.MemberAccess)
			{
				return new FieldRefModel(((MemberExpression) node).Member);
			}
			throw InvalidQuery(node);
		}

		private static Exception InvalidQuery(Expression node)
		{
			return new NotSupportedException(node.ToString());
		}

		#endregion

	}
}