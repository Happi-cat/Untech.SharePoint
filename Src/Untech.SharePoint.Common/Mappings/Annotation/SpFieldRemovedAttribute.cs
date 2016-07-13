using System;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	/// <summary>
	/// Property or field attribute that specifies field that was in SP ContentType but it is currently removed.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[PublicAPI]
	public sealed class SpFieldRemovedAttribute : Attribute 
	{

	}
}
