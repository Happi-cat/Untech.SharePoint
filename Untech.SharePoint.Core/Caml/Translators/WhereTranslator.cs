using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class WhereTranslator : ExpressionVisitor, ICamlTranslator
	{
		private XElement _current;

		public XElement Translate(ISpModelContext modelContext, Expression predicate)
		{
			Visit(predicate);

			return _current != null
				? new XElement(Tags.Where, _current)
				: null;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			if (node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse)
			{
				_current = VisitAndOr(node);
			}
			else if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual)
			{
				_current = VisitEqual(node);
			}
			else
			{
				return base.VisitBinary(node);
			}
			return node;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			_current = VisitMemberMethodCall(node);

			return base.VisitMethodCall(node);
		}

		private XElement VisitAndOr(BinaryExpression node)
		{
			var tag = "";

			if (node.NodeType == ExpressionType.AndAlso) tag = Tags.And;
			if (node.NodeType == ExpressionType.OrElse) tag = Tags.Or;


			_current = null;

			Visit(node.Left);
			var leftElement = _current;

			Visit(node.Right);
			var rightElement = _current;

			return new XElement(tag, leftElement, rightElement);
		}

		private XElement VisitEqual(BinaryExpression node)
		{
			if (node.Left.NodeType == ExpressionType.Constant &&
				node.Right.NodeType == ExpressionType.MemberAccess)
			{
				return VisitEqual((MemberExpression)node.Right, (ConstantExpression)node.Left, node.NodeType);
			}
			if (node.Left.NodeType == ExpressionType.MemberAccess &&
				node.Right.NodeType == ExpressionType.Constant)
			{
				return VisitEqual((MemberExpression)node.Left, (ConstantExpression)node.Right, node.NodeType);
			}
			throw new ArgumentException("");
		}

		private XElement VisitMemberMethodCall(MethodCallExpression node)
		{
			switch (node.Method.Name)
			{
				case "Contains":

					if (node.Method.DeclaringType != null && node.Method.DeclaringType.FullName == typeof(string).FullName)
						return WriteMethodFieldValue(node, Tags.Contains);
					if (node.Method.DeclaringType != null && node.Method.DeclaringType.FullName == typeof(Enumerable).FullName)
						return WriteContainsList(node);
					throw new NotSupportedException();

				case "StartsWith":
					return WriteMethodFieldValue(node, Tags.BeginsWith);
			}
			//throw new ArgumentException();
			return null;
		}

		private XElement WriteMethodFieldValue(MethodCallExpression mce, string operatorTag)
		{
			var me = (MemberExpression)mce.Object;

			if (me != null)
			{
				var fieldRef = VisitField(me);
				var value = VisitValue(mce.Arguments[0]);

				return new XElement(operatorTag, fieldRef, value);
			}

			throw new NotSupportedException();
		}

		private XElement WriteContainsList(MethodCallExpression mce)
		{
			var me = (MemberExpression)mce.Arguments[1];

			if (me != null)
			{
				var listMe = (ConstantExpression)mce.Arguments[0];

				if (listMe == null)
					throw new NotSupportedException();

				//var fieldType = GetFieldType(me);
				//var type = list.Value.GetType();
				//var fieldInfo = type.GetField(listName);
				var values = (IEnumerable)listMe.Value;

				var fieldRef = VisitField(me);

				var valuesElement = new XElement(Tags.Values);

				foreach (var value in values)
				{
					valuesElement.Add(new XElement(Tags.Value, value));
				}

				return new XElement(Tags.In, fieldRef, valuesElement);
			}

			throw new NotSupportedException();
		}
		private XElement VisitEqual(MemberExpression left, ConstantExpression right, ExpressionType type)
		{
			var tag = "";
			if (type == ExpressionType.Equal) tag = Tags.Eq;
			if (type == ExpressionType.NotEqual) tag = Tags.Neq;
			if (type == ExpressionType.GreaterThan) tag = Tags.Gt;
			if (type == ExpressionType.GreaterThanOrEqual) tag = Tags.Geq;
			if (type == ExpressionType.LessThan) tag = Tags.Lt;
			if (type == ExpressionType.LessThanOrEqual) tag = Tags.Leq;

			if (right.Value == null)
			{
				return VisitEqualNull(left, type);
			}

			var leftElement = VisitField(left);
			var rightElement = VisitValue(right);

			return new XElement(tag, leftElement, rightElement);
		}

		private XElement VisitEqualNull(MemberExpression left, ExpressionType type)
		{
			var tag = "";
			if (type == ExpressionType.Equal) tag = Tags.IsNull;
			if (type == ExpressionType.NotEqual) tag = Tags.IsNotNull;

			return new XElement(tag, VisitField(left));
		}

		private XElement VisitField(Expression node)
		{
			var member = (MemberExpression)node;
			var name = member.Member.Name;

			return new XElement(Tags.FieldRef, new XAttribute(Tags.Name, name));
		}

		private XElement VisitValue(Expression node)
		{
			if (node.NodeType == ExpressionType.Constant)
			{
				var constExp = (ConstantExpression)node;
				var value = (constExp.Value ?? "").ToString();

				return new XElement(Tags.Value, value);
			}
			throw new NotSupportedException();
		}
	}
}