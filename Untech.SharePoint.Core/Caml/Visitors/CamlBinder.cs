using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.SharePoint.Client.Search.Query;
using Untech.SharePoint.Core.Caml.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml.Visitors
{
	internal class CamlBinder : CamlExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType.In(new[] {typeof (Queryable), typeof (Enumerable)}))
			{
				switch (node.Method.Name)
				{
					case "Where":
						return VisitWhere(node.Type, node.Arguments[0], node.Arguments[1].GetLambda());
					case "OrderBy":
						return VisitOrderBy(node.Type, node.Arguments[0], node.Arguments[1].GetLambda(), true);
					case "OrderByDescending":
						return VisitOrderBy(node.Type, node.Arguments[0], node.Arguments[1].GetLambda(), false);
					case "ThenBy":
						return VisitOrderBy(node.Type, node.Arguments[0], node.Arguments[1].GetLambda(), true);
					case "ThenByDescending":
						return VisitOrderBy(node.Type, node.Arguments[0], node.Arguments[1].GetLambda(), false);
					case "GroupBy":
						if (node.Arguments.Count == 2)
						{
							return VisitGroupBy(node.Type, node.Arguments[0], node.Arguments[1].GetLambda());
						}
						break;
					case "First":
					case "FirstOrDefault":
					case "Single":
					case "SingleOrDefault":
						if (node.Arguments.Count == 1)
						{
							return VisitFirst(node.Arguments[0], null);
						}
						if (node.Arguments.Count == 2)
						{
							return VisitFirst(node.Arguments[0], node.Arguments[1].GetLambda());
						}
						break;
					case "Last":
					case "LastOrDefault":
						if (node.Arguments.Count == 1)
						{
							return VisitLast(node.Arguments[0], null);
						}
						if (node.Arguments.Count == 2)
						{
							return VisitLast(node.Arguments[0], node.Arguments[1].GetLambda());
						}
						break;
					case "Any":
						if (node.Arguments.Count == 1)
						{
							return VisitAnyAll(node.Arguments[0], node.Method, null);
						}
						if (node.Arguments.Count == 2)
						{
							return VisitAnyAll(node.Arguments[0], node.Method, node.Arguments[1].GetLambda());
						}
						break;
					case "All":
						if (node.Arguments.Count == 2)
						{
							return VisitAnyAll(node.Arguments[0], node.Method, node.Arguments[1].GetLambda());
						}
						break;
					case "Contains":
						return VisitContains(node.Arguments[0], node.Arguments[1]);
					case "Take":
						return VisitTake(node.Arguments[0], node.Arguments[1]);

				}
			}

			return base.VisitMethodCall(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			return base.VisitConstant(node);
		}

		private Expression VisitWhere(Type resultType, Expression source, LambdaExpression predicate)
		{
			var view = (ViewExpression) source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, predicate.Body, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);

		}

		private Expression VisitOrderBy(Type resultType, Expression source, LambdaExpression orderSelector, bool ascending)
		{
			var view = (ViewExpression)source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, null, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);
		}

		private Expression VisitGroupBy(Type resultType, Expression source, LambdaExpression groupSelector)
		{
			var view = (ViewExpression)source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, predicate.Body, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);
		
		}

		private Expression VisitFirst(Expression source, LambdaExpression predicate)
		{
			var view = (ViewExpression)source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, predicate.Body, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);
		}

		private Expression VisitLast(Expression source, LambdaExpression predicate)
		{
			var view = (ViewExpression)source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, predicate.Body, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);
		}

		private Expression VisitAnyAll(Expression source, MethodInfo method, LambdaExpression predicate)
		{
			var view = (ViewExpression)source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, predicate.Body, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);
		}

		private Expression VisitContains(Expression source, Expression match)
		{
			var view = (ViewExpression)source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, predicate.Body, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);
		}

		private Expression VisitTake(Expression source, Expression count)
		{
			var view = (ViewExpression)source;

			var query = (QueryExpression)view.Query;
			if (query == null)
			{
				query = new QueryExpression(resultType, predicate.Body, null, null);
			}
			else
			{
				var where = query.Where;
				@where = @where == null ? predicate.Body : Expression.And(@where, predicate.Body);

				query = query.Update(where, query.OrderBy, query.GroupBy);
			}

			return view.Update(view.ViewFields, query, view.RowLimit);
		}
	}
}