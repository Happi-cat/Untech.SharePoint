using System;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Untech.SharePoint.Common.Converters
{
	/// <summary>
	/// Represents interface of <see cref="IFieldConverter"/> resolver.
	/// </summary>
	public interface IFieldConverterResolver
	{
		/// <summary>
		/// Determines whether <paramref name="typeAsString"/> can be resolved by current resolver.
		/// </summary>
		/// <param name="typeAsString">SP field type as string.</param>
		/// <returns>true if can resovle the specified <paramref name="typeAsString"/>.</returns>
		bool CanResolve([NotNull]string typeAsString);

		/// <summary>
		/// Resolves <paramref name="typeAsString"/> and returns new instance of the associated <see cref="IFieldConverter"/>.
		/// </summary>
		/// <param name="typeAsString">SP field type as string.</param>
		/// <returns>New instance of the <see cref="IFieldConverter"/> that matchs to the specified SP field type.</returns>
		[NotNull]
		IFieldConverter Resolve([NotNull]string typeAsString);

		/// <summary>
		/// Determines whether <paramref name="converterType"/> can be resolved by current resolver.
		/// </summary>
		/// <param name="converterType">SP field converter type to check.</param>
		/// <returns>true if can resovle the specified <paramref name="converterType"/>.</returns>
		bool CanResolve([NotNull]Type converterType);

		/// <summary>
		/// Resolves <paramref name="converterType"/> and returns new instance of the associated <see cref="IFieldConverter"/>.
		/// </summary>
		/// <param name="converterType">type of the field converter to instantiate.</param>
		/// <returns>New instance of the <see cref="IFieldConverter"/>.</returns>
		[NotNull]
		IFieldConverter Resolve([NotNull]Type converterType);
	}
}