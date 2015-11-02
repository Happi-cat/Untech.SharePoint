using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Untech.SharePoint.Common.Test
{
	public static class CustomAssert
	{
		public static void Throw<TException>(Action action)
			where TException: Exception
		{
			try
			{
				action();
			}
			catch (TException)
			{
				return;
			}
			Assert.Fail("Exception '{0}' wasn't thrown.", typeof(TException));
		}

		public static void AreEqualAfterVisit<T>(IEnumerable<ExpressionVisitor> visitors, Expression<Func<T, bool>> original, Expression<Func<T, bool>> expected)
		{
			var processed = visitors.Aggregate((Expression)original, (expr, visitor) => visitor.Visit(expr));
			
			Assert.AreEqual(expected.ToString(), processed.ToString());
		}
	}
}