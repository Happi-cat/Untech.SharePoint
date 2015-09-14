using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class SpFieldRemovedAttribute : Attribute
	{

	}
}
