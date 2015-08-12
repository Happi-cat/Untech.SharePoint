using System;
using System.Linq.Expressions;
using Untech.SharePoint.Core.Caml.Expressions;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core.Caml
{
	public class SpecialExpressionBinder : ExpressionVisitor
	{
		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Member.DeclaringType == typeof (DateTime))
			{
				switch (node.Member.Name)
				{
					case "Today":
						return new TodayExpression();
					case "Now":
						return new NowExpression();
				}
			}

			return base.VisitMember(node);
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof (DateTime) && node.Method.Name == "AddDays")
			{
				var objectNode = node.Object.StripQuotes();
				if (objectNode.NodeType == ExpressionType.MemberAccess)
				{
					var memberNode = (MemberExpression) objectNode;
					if (memberNode.Member.DeclaringType == typeof(DateTime) && memberNode.Member.Name == "Today")
					{
						return new TodayExpression();
					}
				}

			}
			return base.VisitMethodCall(node);
		}
	}

}