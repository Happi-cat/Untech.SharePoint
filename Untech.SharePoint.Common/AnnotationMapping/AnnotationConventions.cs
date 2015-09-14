using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal static class AnnotationUtils
	{
		public static bool HasListAnnotatinon(MemberInfo member)
		{
			return member.IsDefined(typeof(SpListAttribute));
		}

		public static bool HasFieldAnnotation(MemberInfo member)
		{
			return member.IsDefined(typeof(SpFieldAttribute)) && !member.IsDefined(typeof(SpFieldRemovedAttribute));
		}
	}
}
