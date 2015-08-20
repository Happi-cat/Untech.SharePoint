using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Core.Caml.Expressions
{
	internal enum CamlExpressionType
	{
		View = 1000,
		Query,
		FieldRef,
		IsToday,
		Grouping,
	}

	internal static class CamlExpressionExtensions
	{
		internal static bool IsCamlExpression(this ExpressionType et)
		{
			return ((int)et) >= 1000;
		}
	}
}
