//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;

//namespace Untech.SharePoint.Core.Data.Queryable
//{
//	internal class SPQueryable<T> : IOrderedQueryable<T>
//	{
//		public SPQueryable(IQueryContext queryContext)
//		{
//			Initialize(new SPQueryProvider(queryContext), null);
//		}

//		public SPQueryable(IQueryProvider provider)
//		{
//			Initialize(provider, null);
//		}

//		internal SPQueryable(IQueryProvider provider, Expression expression)
//		{
//			Initialize(provider, expression);
//		}

//		private void Initialize(IQueryProvider provider, Expression expression)
//		{
//			if (provider == null)
//				throw new ArgumentNullException("provider");
//			if (expression != null && !typeof(IQueryable<T>).
//				   IsAssignableFrom(expression.Type))
//				throw new ArgumentException(
//					 String.Format("Not assignable from {0}", expression.Type), "expression");

//			Provider = provider;
//			Expression = expression ?? Expression.Constant(this);
//		}

//		public IEnumerator<T> GetEnumerator()
//		{
//			return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
//		}

//		IEnumerator IEnumerable.GetEnumerator()
//		{
//			return (Provider.Execute<IEnumerable>(Expression)).GetEnumerator();
//		}

//		public Type ElementType
//		{
//			get { return typeof(T); }
//		}

//		public Expression Expression { get; private set; }
//		public IQueryProvider Provider { get; private set; }
//	}

//	public class SPQueryProvider : IQueryProvider
//	{
//		private readonly IQueryContext queryContext;

//		public SPQueryProvider(IQueryContext queryContext)
//		{
//			this.queryContext = queryContext;
//		}

//		public virtual IQueryable CreateQuery(Expression expression)
//		{
//			Type elementType = TypeSystem.GetElementType(expression.Type);
//			try
//			{
//				return
//				   (IQueryable)Activator.CreateInstance(typeof(SPQueryable<>).
//						  MakeGenericType(elementType), this, expression);
//			}
//			catch (TargetInvocationException e)
//			{
//				throw e.InnerException;
//			}
//		}

//		public virtual IQueryable<T> CreateQuery<T>(Expression expression)
//		{
//			return new SPQueryable<T>(this, expression);
//		}

//		object IQueryProvider.Execute(Expression expression)
//		{
//			return queryContext.Execute(expression, false);
//		}

//		T IQueryProvider.Execute<T>(Expression expression)
//		{
//			return (T)queryContext.Execute(expression,
//					   (typeof(T).Name == "IEnumerable`1"));
//		}
//	}

//	public interface IQueryContext
//	{
//		object Execute(Expression expression, bool isEnumerable);
//	}
//	internal static class TypeSystem
//	{
//		internal static Type GetElementType(Type seqType)
//		{
//			Type ienum = FindIEnumerable(seqType);
//			if (ienum == null) return seqType;
//			return ienum.GetGenericArguments()[0];
//		}

//		private static Type FindIEnumerable(Type seqType)
//		{
//			if (seqType == null || seqType == typeof(string))
//				return null;

//			if (seqType.IsArray)
//				return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());

//			if (seqType.IsGenericType)
//			{
//				foreach (Type arg in seqType.GetGenericArguments())
//				{
//					Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
//					if (ienum.IsAssignableFrom(seqType))
//					{
//						return ienum;
//					}
//				}
//			}

//			Type[] ifaces = seqType.GetInterfaces();
//			if (ifaces != null && ifaces.Length > 0)
//			{
//				foreach (Type iface in ifaces)
//				{
//					Type ienum = FindIEnumerable(iface);
//					if (ienum != null) return ienum;
//				}
//			}

//			if (seqType.BaseType != null && seqType.BaseType != typeof(object))
//			{
//				return FindIEnumerable(seqType.BaseType);
//			}

//			return null;
//		}
//	}

//	internal class ExpressionTreeHelpers
//	{
//		internal static bool IsMemberEqualsValueExpression(Expression exp, Type declaringType, string memberName)
//		{
//			if (exp.NodeType != ExpressionType.Equal)
//				return false;

//			BinaryExpression be = (BinaryExpression)exp;

//			// Assert. 
//			if (IsSpecificMemberExpression(be.Left, declaringType, memberName) &&
//				IsSpecificMemberExpression(be.Right, declaringType, memberName))
//				throw new Exception("Cannot have 'member' == 'member' in an expression!");

//			return (IsSpecificMemberExpression(be.Left, declaringType, memberName) ||
//				IsSpecificMemberExpression(be.Right, declaringType, memberName));
//		}

//		internal static bool IsSpecificMemberExpression(Expression exp, Type declaringType, string memberName)
//		{
//			return ((exp is MemberExpression) &&
//				(((MemberExpression)exp).Member.DeclaringType == declaringType) &&
//				(((MemberExpression)exp).Member.Name == memberName));
//		}

//		internal static string GetValueFromEqualsExpression(BinaryExpression be, Type memberDeclaringType, string memberName)
//		{
//			if (be.NodeType != ExpressionType.Equal)
//				throw new Exception("There is a bug in this program.");

//			if (be.Left.NodeType == ExpressionType.MemberAccess)
//			{
//				MemberExpression me = (MemberExpression)be.Left;

//				if (me.Member.DeclaringType == memberDeclaringType && me.Member.Name == memberName)
//				{
//					return GetValueFromExpression(be.Right);
//				}
//			}
//			else if (be.Right.NodeType == ExpressionType.MemberAccess)
//			{
//				MemberExpression me = (MemberExpression)be.Right;

//				if (me.Member.DeclaringType == memberDeclaringType && me.Member.Name == memberName)
//				{
//					return GetValueFromExpression(be.Left);
//				}
//			}

//			// We should have returned by now. 
//			throw new Exception("There is a bug in this program.");
//		}

//		internal static string GetValueFromExpression(Expression expression)
//		{
//			if (expression.NodeType == ExpressionType.Constant)
//				return (string)(((ConstantExpression)expression).Value);
//			throw new InvalidQueryException(
//				String.Format("The expression type {0} is not supported to obtain a value.", expression.NodeType));
//		}
//	}

//	class InvalidQueryException : Exception
//	{
//		private string message;

//		public InvalidQueryException(string message)
//		{
//			this.message = message + " ";
//		}

//		public override string Message
//		{
//			get
//			{
//				return "The client query is invalid: " + message;
//			}
//		}
//	}
//}
