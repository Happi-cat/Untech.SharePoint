using System;
using System.Linq.Expressions;
using System.Text;
using Untech.SharePoint.Core.Data.Queryable;

namespace Untech.SharePoint.Core.Caml
{
	public class CamlTranslator : ExpressionVisitor
	{
		private WhereTranslator _whereTranslator;

		protected new virtual Expression Visit(Expression exp)
		{
			if (exp == null)
			{
				return exp;
			}
			switch (exp.NodeType)
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.ArrayIndex:
				case ExpressionType.Coalesce:
				case ExpressionType.Divide:
				case ExpressionType.Equal:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LeftShift:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.NotEqual:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.Power:
				case ExpressionType.RightShift:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					return this.VisitBinary((BinaryExpression)exp);
				case ExpressionType.ArrayLength:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
					return this.VisitUnary((UnaryExpression)exp);
				case ExpressionType.Call:
					return this.VisitMethodCall((MethodCallExpression)exp);
				case ExpressionType.Conditional:
					return this.VisitConditional((ConditionalExpression)exp);
				case ExpressionType.Constant:
					return this.VisitConstant((ConstantExpression)exp);
				case ExpressionType.Invoke:
					return this.VisitInvocation((InvocationExpression)exp);
				case ExpressionType.Lambda:
					return this.VisitLambda((LambdaExpression)exp);
				case ExpressionType.ListInit:
					return this.VisitListInit((ListInitExpression)exp);
				case ExpressionType.MemberAccess:
					return this.VisitMemberAccess((MemberExpression)exp);
				case ExpressionType.MemberInit:
					return this.VisitMemberInit((MemberInitExpression)exp);
				case ExpressionType.New:
					return this.VisitNew((NewExpression)exp);
				case ExpressionType.NewArrayInit:
				case ExpressionType.NewArrayBounds:
					return this.VisitNewArray((NewArrayExpression)exp);
				case ExpressionType.Parameter:
					return this.VisitParameter((ParameterExpression)exp);
				case ExpressionType.TypeIs:
					return this.VisitTypeIs((TypeBinaryExpression)exp);
				default:
					throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
			}
		}

		protected override Expression VisitMethodCall(MethodCallExpression mc)
		{
			Type declaringType = mc.Method.DeclaringType;
			if (declaringType != typeof(SPQueryable<>))
				throw new NotSupportedException(
				  "The type for the query operator is not Queryable!");
			switch (mc.Method.Name)
			{
				case "Where":
					// is this really a proper Where? 
					var whereLambda = GetLambdaWithParamCheck(mc);
					if (whereLambda == null)
						break;
					VisitWhere(mc.Arguments[0], whereLambda);
					break;
				case "OrderBy":
				case "ThenBy":
					// is this really a proper Order By? 
					var orderLambda = GetLambdaWithParamCheck(mc);
					if (orderLambda == null)
						break;
					VisitOrderBy(mc.Arguments[0], orderLambda, OrderDirection.Ascending);
					break;
				case "OrderByDescending":
				case "ThenByDescending":
					// is this really a proper Order By Descending? 
					var orderDescLambda = GetLambdaWithParamCheck(mc);
					if (orderDescLambda == null)
						break;
					VisitOrderBy(mc.Arguments[0], orderDescLambda, OrderDirection.Descending);
					break;
				case "Select":
					// is this really a proper Select? 
					var selectLambda = GetLambdaWithParamCheck(mc);
					if (selectLambda == null)
						break;
					VisitSelect(mc.Arguments[0], selectLambda);
					break;
				case "Take":
					if (mc.Arguments.Count != 2)
						break;
					VisitTake(mc.Arguments[0], mc.Arguments[1]);
					break;
				case "First":
					// This custom provider does not support the use of a First operator 
					// that takes a predicate. Therefore we check to ensure that no more 
					// than one argument is provided. 
					if (mc.Arguments.Count != 1)
						break;
					VisitFirst(mc.Arguments[0], false);
					break;
				case "FirstOrDefault":
					// This custom provider does not support the use of a FirstOrDefault 
					// operator that takes a predicate. Therefore we check to ensure that 
					// no more than one argument is provided. 
					if (mc.Arguments.Count != 1)
						break;
					VisitFirst(mc.Arguments[0], true);
					break;
				default:
					return base.VisitMethodCall(mc);
			}
			Visit(mc.Arguments[0]);
			return mc;
		}

		private void VisitWhere(Expression queryable, LambdaExpression predicate)
		{
			// this custom provider cannot support more 
			// than one Where query operator in a LINQ query 
			if (_whereTranslator != null)
				throw new NotSupportedException(
				   "You cannot have more than one Where operator in this expression");
			_whereTranslator = new WhereTranslator(_model);
			_whereTranslator.Translate(predicate);
		}
	}

	internal class WhereTranslator : ExpressionVisitor
	{
		StringBuilder _sb;
		protected override Expression VisitBinary(BinaryExpression b)
		{
			_sb.Append("(");
			Visit(b.Left);
			switch (b.NodeType)
			{
				case ExpressionType.And:
				case ExpressionType.AndAlso:
					_sb.Append(" AND ");
					break;
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					_sb.Append(" OR ");
					break;
				case ExpressionType.Equal:
					if (IsComparingWithNull(b))
						_sb.Append(" IS ");
					else
						_sb.Append(" = ");
					break;
				case ExpressionType.GreaterThan:
					_sb.Append(" > ");
					break;

			}
			Visit(b.Right);
			_sb.Append(")");
			return b;
		}
	}
}