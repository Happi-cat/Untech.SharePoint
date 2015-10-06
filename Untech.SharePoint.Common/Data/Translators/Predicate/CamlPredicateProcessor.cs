using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class CamlPredicateProcessor
	{
		private static readonly IReadOnlyDictionary<ExpressionType, ComparisonOperator> ComparisonMap = new Dictionary<ExpressionType, ComparisonOperator>
		{
			{ExpressionType.Equal, ComparisonOperator.Eq},
			{ExpressionType.NotEqual, ComparisonOperator.Neq},
			{ExpressionType.LessThanOrEqual, ComparisonOperator.Leq},
			{ExpressionType.LessThan, ComparisonOperator.Lt},
			{ExpressionType.GreaterThan, ComparisonOperator.Gt},
			{ExpressionType.GreaterThanOrEqual, ComparisonOperator.Geq}
		};

		private static readonly IReadOnlyDictionary<ExpressionType, ComparisonOperator> NullComparisonMap = new Dictionary<ExpressionType, ComparisonOperator>
		{
			{ExpressionType.Equal, ComparisonOperator.IsNull},
			{ExpressionType.NotEqual, ComparisonOperator.IsNotNull},
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
			throw CamlProcessorUtils.InvalidQuery(node);
		}

		#region [Private Methods]

		private WhereModel TranslateTrueProperty(MemberExpression memberNode)
		{
			var fieldRef = CamlProcessorUtils.GetFieldRef(memberNode);
			return new ComparisonModel(ComparisonOperator.Eq, fieldRef, true);
		}

		private WhereModel TranslateFalseProperty(UnaryExpression unaryNode)
		{
			var fieldRef = CamlProcessorUtils.GetFieldRef(unaryNode.Operand);
			return new ComparisonModel(ComparisonOperator.Eq, fieldRef, false);
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
			if (ComparisonMap.ContainsKey(binaryNode.NodeType))
			{
				if (binaryNode.Right.IsConstant(null))
				{
					return TranslateComparisonWithNull(binaryNode);
				}
				if (binaryNode.Left.NodeType == ExpressionType.MemberAccess)
				{
					var fieldRef = CamlProcessorUtils.GetFieldRef(binaryNode.Left);
					return new ComparisonModel(ComparisonMap[binaryNode.NodeType], fieldRef, GetValue(binaryNode.Right));
				}
			}
			throw CamlProcessorUtils.InvalidQuery(binaryNode);
		}

		private WhereModel TranslateComparisonWithNull(BinaryExpression binaryNode)
		{
			var fieldRef = CamlProcessorUtils.GetFieldRef(binaryNode.Left);
			if (NullComparisonMap.ContainsKey(binaryNode.NodeType))
			{
				return new ComparisonModel(NullComparisonMap[binaryNode.NodeType], fieldRef, null);
			}
			throw CamlProcessorUtils.InvalidQuery(binaryNode);
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
			throw CamlProcessorUtils.InvalidQuery(callNode);
		}

		private WhereModel TranslateContains(MethodCallExpression callNode)
		{
			return new ComparisonModel(ComparisonOperator.Contains, CamlProcessorUtils.GetFieldRef(callNode.Object), GetValue(callNode.Arguments[0]));
		}

		private WhereModel TranslateStartsWith(MethodCallExpression callNode)
		{
			return new ComparisonModel(ComparisonOperator.BeginsWith, CamlProcessorUtils.GetFieldRef(callNode.Object), GetValue(callNode.Arguments[0]));
		}

		private object GetValue(Expression node)
		{
			if (node.NodeType == ExpressionType.Constant)
			{
				return ((ConstantExpression) node).Value;
			}
			throw CamlProcessorUtils.InvalidQuery(node);
		}

		#endregion

	}
}