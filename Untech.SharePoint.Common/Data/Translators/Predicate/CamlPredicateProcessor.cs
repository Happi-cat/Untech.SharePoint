using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class CamlPredicateProcessor : IProcessor<Expression, WhereModel>
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
				new XorRewriter(),
				new PushNotDownVisitor(),
				new InRewriter(),
				new RedundantConditionRemover(),
				new SwapComparisonVisitor()
			};
		}

		[NotNull]
		private IEnumerable<ExpressionVisitor> PreProcessors { get; set; }

		public WhereModel Process([NotNull]Expression predicate)
		{
			Guard.CheckNotNull("predicate", predicate);

			Logger.Trace(LogCategories.PredicateProcessor, "Orignal predicate:\n{0}", predicate);

			predicate = predicate.StripQuotes();
			if (predicate.NodeType == ExpressionType.Lambda)
			{
				predicate = ((LambdaExpression) predicate).Body;
			}
			
			predicate = PreProcessors.Aggregate(predicate, (n, v) => v.Visit(n));

			Logger.Trace(LogCategories.PredicateProcessor, "Pre-processed predicate:\n{0}", predicate);

			var result = Translate(predicate);

			Logger.Trace(LogCategories.PredicateProcessor, "WhereModel that was generated from predicate:\n{0}", result);

			return result;
		}

		#region [Private Methods]

		[NotNull]
		private WhereModel Translate([NotNull]Expression node)
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
					return Translate(((UnaryExpression) node).Operand).Negate();
				case ExpressionType.Constant:
					return TranslateConstBoolean((ConstantExpression) node);
			}
			throw Error.SubqueryNotSupported(node);
		}

		[NotNull]
		private WhereModel TranslateConstBoolean([NotNull]ConstantExpression constNode)
		{
			if (constNode.Type != typeof (bool))
			{
				throw Error.SubqueryNotSupported(constNode);
			}

			var value = (bool) constNode.Value;
			var tag = value ? ComparisonOperator.IsNotNull : ComparisonOperator.IsNull;

			return new ComparisonModel(tag, new KeyRefModel(), null);
		}

		[NotNull]
		private WhereModel TranslateTrueProperty([NotNull]MemberExpression memberNode)
		{
			var fieldRef = CamlProcessorUtils.GetFieldRef(memberNode);
			return new ComparisonModel(ComparisonOperator.Eq, fieldRef, true);
		}

		[NotNull]
		private WhereModel TranslateFalseProperty([NotNull]UnaryExpression unaryNode)
		{
			var fieldRef = CamlProcessorUtils.GetFieldRef(unaryNode.Operand);
			return new ComparisonModel(ComparisonOperator.Eq, fieldRef, false);
		}

		[NotNull]
		private WhereModel TranslateAnd([NotNull]BinaryExpression binaryNode)
		{
			return WhereModel.And(Translate(binaryNode.Left), Translate(binaryNode.Right));
		}

		[NotNull]
		private WhereModel TranslateOr([NotNull]BinaryExpression binaryNode)
		{
			return WhereModel.Or(Translate(binaryNode.Left), Translate(binaryNode.Right));
		}

		[NotNull]
		private WhereModel TranslateComparison([NotNull]BinaryExpression binaryNode)
		{
			if (ComparisonMap.ContainsKey(binaryNode.NodeType))
			{
				if (binaryNode.Right.IsConstant(null))
				{
					return TranslateComparisonWithNull(binaryNode);
				}
				var fieldRef = CamlProcessorUtils.GetFieldRef(binaryNode.Left);
				return new ComparisonModel(ComparisonMap[binaryNode.NodeType], fieldRef, GetValue(binaryNode.Right));
			}
			throw Error.SubqueryNotSupported(binaryNode);
		}

		[NotNull]
		private WhereModel TranslateComparisonWithNull([NotNull]BinaryExpression binaryNode)
		{
			var fieldRef = CamlProcessorUtils.GetFieldRef(binaryNode.Left);
			if (NullComparisonMap.ContainsKey(binaryNode.NodeType))
			{
				return new ComparisonModel(NullComparisonMap[binaryNode.NodeType], fieldRef, null);
			}
			throw Error.SubqueryNotSupported(binaryNode);
		}

		[NotNull]
		private WhereModel TranslateCall([NotNull]MethodCallExpression callNode)
		{
			if (MethodUtils.IsOperator(callNode.Method, MethodUtils.EContains))
			{
				return new ComparisonModel(ComparisonOperator.Includes,
					CamlProcessorUtils.GetFieldRef(callNode.Arguments[0]),
					GetValue(callNode.Arguments[1]));
			}
			if (MethodUtils.IsOperator(callNode.Method, MethodUtils.ListContains))
			{
				return new ComparisonModel(ComparisonOperator.Includes,
					CamlProcessorUtils.GetFieldRef(callNode.Object),
					GetValue(callNode.Arguments[0]));
			}
			if (callNode.Method == MethodUtils.StrContains)
			{
				return TranslateStrContains(callNode);
			}
			if (callNode.Method == MethodUtils.StrStartsWith)
			{
				return TranslateStrStartsWith(callNode);
			}
			if (callNode.Method.Name == "Contains" && 
				callNode.Object != null && 
				(callNode.Object.Type.IsArray || callNode.Object.Type.IsIEnumerable()) && 
				callNode.Arguments.Count == 1)
			{
				return new ComparisonModel(ComparisonOperator.Includes,
					CamlProcessorUtils.GetFieldRef(callNode.Object),
					GetValue(callNode.Arguments[0]));
			}
			throw Error.SubqueryNotSupported(callNode);
		}

		[NotNull]
		private WhereModel TranslateStrContains([NotNull]MethodCallExpression callNode)
		{
			return new ComparisonModel(ComparisonOperator.Contains, 
				CamlProcessorUtils.GetFieldRef(callNode.Object), 
				GetValue(callNode.Arguments[0]));
		}

		[NotNull]
		private WhereModel TranslateStrStartsWith([NotNull]MethodCallExpression callNode)
		{
			return new ComparisonModel(ComparisonOperator.BeginsWith, 
				CamlProcessorUtils.GetFieldRef(callNode.Object), 
				GetValue(callNode.Arguments[0]));
		}

		[CanBeNull]
		private object GetValue([NotNull]Expression node)
		{
			if (node.NodeType == ExpressionType.Constant)
			{
				return ((ConstantExpression) node).Value;
			}
			throw Error.SubqueryNotSupported(node);
		}

		#endregion

	}
}