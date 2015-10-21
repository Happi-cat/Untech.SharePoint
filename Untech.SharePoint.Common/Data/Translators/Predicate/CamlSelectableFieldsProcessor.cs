using System.Collections.Generic;
using System.Linq.Expressions;
using Untech.SharePoint.Common.Data.QueryModels;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Data.Translators.Predicate
{
	internal class CamlSelectableFieldsProcessor : ExpressionVisitor, IExpressionProcessor<IEnumerable<FieldRefModel>>
	{

		public CamlSelectableFieldsProcessor()
		{
			SelectableFields = new List<FieldRefModel>();
		}

		public List<FieldRefModel> SelectableFields { get; private set; }

		public IEnumerable<FieldRefModel> Process(Expression node)
		{
			Visit(node);

			return SelectableFields;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression.NodeType == ExpressionType.Parameter)
			{
				SelectableFields.Add(new FieldRefModel(node.Member));
			}
			if (node.Expression.NodeType.In(new[] {ExpressionType.Convert, ExpressionType.ConvertChecked}))
			{
				var unaryNode = (UnaryExpression) node.Expression;

				if (unaryNode.Operand.NodeType == ExpressionType.Parameter)
				{
					SelectableFields.Add(new FieldRefModel(node.Member));
				}
			}
			return base.VisitMember(node);
		}
	}
}