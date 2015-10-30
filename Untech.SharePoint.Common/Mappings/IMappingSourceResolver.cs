using System;

namespace Untech.SharePoint.Common.Mappings
{
	/// <summary>
	/// Exposes Resolve method that will return <see cref="IMappingSource"/> for the specified context type.
	/// </summary>
	public interface IMappingSourceResolver
	{
		/// <summary>
		/// Determines whether <paramref name="contextType"/> can be resolved by current resolver.
		/// </summary>
		/// <param name="contextType">Context type to check.</param>
		/// <returns>true if can resovle the specified <paramref name="contextType"/>.</returns>
		bool CanResolve(Type contextType);

		/// <summary>
		/// Resolves <paramref name="contextType"/>.
		/// </summary>
		/// <param name="contextType">Context type to resovle.</param>
		/// <returns>Instance of <see cref="IMappingSource"/> that is associated with current <paramref name="contextType"/>.</returns>
		IMappingSource Resolve(Type contextType);
	}
}