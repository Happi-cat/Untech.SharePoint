using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Diagnostics;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class CamlSelectableFieldsProcessor : ExpressionVisitor, IProcessor<Expression, IEnumerable<MemberRefModel>>
	{

		public CamlSelectableFieldsProcessor()
		{
			SelectableFields = new List<MemberRefModel>();
		}

		[NotNull]
		protected List<MemberRefModel> SelectableFields { get; private set; }

		[NotNull]
		public IEnumerable<MemberRefModel> Process([CanBeNull]Expression node)
		{
			Logger.Log(LogLevel.Trace, LogCategories.SelectableFieldsProcessor, 
				"Original predicate:\n{0}", node);

			Visit(node);

			Logger.Log(LogLevel.Trace, LogCategories.SelectableFieldsProcessor, 
				"Selectable fields:\n{0}", SelectableFields.JoinToString(""));

			return SelectableFields;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression.NodeType == ExpressionType.Parameter)
			{
				SelectableFields.Add(new MemberRefModel(node.Member));
			}
			if (node.Expression.NodeType.In(new[] {ExpressionType.Convert, ExpressionType.ConvertChecked}))
			{
				var unaryNode = (UnaryExpression) node.Expression;

				if (unaryNode.Operand.NodeType == ExpressionType.Parameter)
				{
					SelectableFields.Add(new MemberRefModel(node.Member));
				}
			}
			return base.VisitMember(node);
		}
	}
}