using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Configuration
{
	/// <summary>
	/// Represents configuration that is required by <see cref="SpContext{TContext,TCommonService}"/>.
	/// </summary>
	[PublicAPI]
	public sealed class Config
	{
		/// <summary>
		/// Intializes a new instance of the <see cref="Config"/> with the specified instances of <see cref="IFieldConverterResolver"/> and <see cref="IMappingSourceResolver"/>.
		/// </summary>
		/// <param name="fieldConverters">Field converters resolver.</param>
		/// <param name="mappings">Mappings source resolvers.</param>
		/// <exception cref="ArgumentNullException"><paramref name="fieldConverters"/> or <paramref name="mappings"/> is null.</exception>
		public Config([NotNull] IFieldConverterResolver fieldConverters, [NotNull] IMappingSourceResolver mappings)
		{
			Guard.CheckNotNull("fieldConverters", fieldConverters);
			Guard.CheckNotNull("mappings", mappings);

			FieldConverters = fieldConverters;
			Mappings = mappings;
		}


		/// <summary>
		/// Gets the <see cref="IFieldConverterResolver"/> resolver.
		/// </summary>
		[NotNull]
		public IFieldConverterResolver FieldConverters { get; private set; }

		/// <summary>
		/// Gets the <see cref="IMappingSourceResolver"/> resolver.
		/// </summary>
		[NotNull]
		public IMappingSourceResolver Mappings { get; private set; }
	}
}