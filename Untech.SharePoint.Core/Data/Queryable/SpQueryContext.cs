using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
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

			var query = new SPQuery();

			var viewFieldsXml = caml.Element(Tags.ViewFields);
			var rowLimitXml = caml.Element(Tags.RowLimit);
			var queryXml = caml.Elements()
				.Where(n => n.Name != Tags.ViewFields && n.Name != Tags.RowLimit)
				.ToList();

			query.Query = (new XElement(Tags.Query, queryXml)).Value;
			if (viewFieldsXml != null)
			{
				query.ViewFields = viewFieldsXml.Value;
				query.ViewFieldsOnly = true;
			}
			if (rowLimitXml != null)
			{
				query.RowLimit = (int)rowLimitXml.Value;
			}

			return list.GetItems(query);
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