using JetBrains.Annotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings;

namespace Untech.SharePoint.Common.Configuration
{
	/// <summary>
	/// Represents configuration that is required by <see cref="SpContext{TContext,TCommonService}"/>.
	/// </summary>
	public class Config
	{
		/// <summary>
		/// Gets the <see cref="IFieldConverterResolver"/> resolver.
		/// </summary>
		[CanBeNull]
		public virtual IFieldConverterResolver FieldConverters { get; set; }

		/// <summary>
		/// Gets the <see cref="IMappingSourceResolver"/> resolver.
		/// </summary>
		[CanBeNull]
		public virtual IMappingSourceResolver Mappings { get; set; }
	}
}