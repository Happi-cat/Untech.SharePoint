using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Data.QueryModels;
using Untech.SharePoint.Diagnostics;
using Untech.SharePoint.Extensions;

namespace Untech.SharePoint.Data.Translators.Predicate
{
	internal class CamlSelectableFieldsProcessor : ExpressionVisitor, IProcessor<Expression, IEnumerable<MemberRefModel>>
	{
		public CamlSelectableFieldsProcessor()
		{
			SelectableFields = new List<MemberRefModel>();
		}

		[NotNull]
		private List<MemberRefModel> SelectableFields { get; }

		[NotNull]
		public IEnumerable<MemberRefModel> Process([CanBeNull]Expression node)
		{
			Logger.Trace(LogCategories.SelectableFieldsProcessor, "Original predicate:\n{0}", node);

			Visit(node);

			Logger.Trace(LogCategories.SelectableFieldsProcessor, "Selectable fields in predicate:\n{0}",
				SelectableFields.JoinToString("\n"));

			return SelectableFields;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			var objectNode = node.Expression;
			if (objectNode != null && objectNode.NodeType == ExpressionType.Parameter)
			{
				SelectableFields.Add(new MemberRefModel(node.Member));
			}
			if (objectNode != null && objectNode.NodeType.In(new[] { ExpressionType.Convert, ExpressionType.ConvertChecked }))
			{
				var unaryNode = (UnaryExpression)objectNode;

				if (unaryNode.Operand.NodeType == ExpressionType.Parameter)
				{
					SelectableFields.Add(new MemberRefModel(node.Member));
				}
			}
			return base.VisitMember(node);
		}
	}
}