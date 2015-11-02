using System;
using System.Reflection;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.Utils.Reflection
{
	internal static class ReflectionError
	{
		internal static Exception CtorNotFound(Type type, Type[] argumentTypes)
		{
			return new ArgumentException(string.Format("Type '{0}' has no constructor that matches parameters list ({1})",
				type, argumentTypes.JoinToString()));
		}

		internal static Exception CannotCreateGetter(MemberInfo member)
		{
			return new ArgumentException(string.Format("Cannot create getter for member '{0}' in type '{1}'",
				member.Name, member.DeclaringType));
		}

		internal static Exception CannotCreateSetter(MemberInfo member)
		{
			return new ArgumentException(string.Format("Cannot create setter for member '{0}' in type '{1}'",
				member.Name, member.DeclaringType));
		}

		internal static Exception CannotCreateGetterForIndexer(MemberInfo member)
		{
			return new ArgumentException(string.Format("Cannot create getter for indexer '{0}' in type '{1}'",
				member.Name, member.DeclaringType));
		}

		internal static Exception CannotCreateSetterForIndexer(MemberInfo member)
		{
			return new ArgumentException(string.Format("Cannot create setter for indexer '{0}' in type '{1}'",
				member.Name, member.DeclaringType));
		}

		internal static Exception CannotCreateGetterForWriteOnly(MemberInfo member)
		{
			return new ArgumentException(string.Format("Cannot create getter for write-only member '{0}' in type '{1}'",
				member.Name, member.DeclaringType));
		}

		internal static Exception CannotCreateSetterForReadOnly(MemberInfo member)
		{
			return new ArgumentException(string.Format("Cannot create setter for read-only member '{0}' in type '{1}'",
				member.Name, member.DeclaringType));
		}
	}
}