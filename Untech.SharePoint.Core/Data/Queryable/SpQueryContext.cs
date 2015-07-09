using System;
using System.Linq.Expressions;
using Microsoft.SharePoint;
using Untech.SharePoint.Core.Caml;

namespace Untech.SharePoint.Core.Data.Queryable
{
	internal class SpQueryContext
	{
		// Executes the expression tree that is passed to it. 
		internal static object Execute(SPList list, Expression expression, bool isEnumerable)
		{
			// The expression must represent a query over the data source. 
			if (!IsQueryOverDataSource(expression))
				throw new InvalidProgramException("No query over the data source was specified.");

			var caml = (new CamlTranslator()).Translate(null, expression);


			throw new NotImplementedException();
		}

		private static bool IsQueryOverDataSource(Expression expression)
		{
			// If expression represents an unqueried IQueryable data source instance, 
			// expression is of type ConstantExpression, not MethodCallExpression. 
			return (expression is MethodCallExpression);
		}

		private static SPListItemCollection Execute(SPList list, string caml)
		{
			throw new NotImplementedException();
		}
	}
}