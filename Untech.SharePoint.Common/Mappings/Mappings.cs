using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Mappings
{
	/// <summary>
	/// Represents class that provides a set of methods that returns <see cref="IMappingSource{TContext}"/>
	/// </summary>
	public sealed class Mappings
	{
		/// <summary>
		/// Creates <see cref="IMappingSource{TContext}"/> based on annotation attributes.
		/// </summary>
		/// <typeparam name="TContext">Type of <see cref="ISpContext"/></typeparam>
		/// <returns>Mapping source for the specified <typeparamref name="TContext"/>.</returns>
		public IMappingSource<TContext> Annotated<TContext>()
			where TContext : ISpContext
		{
			return new AnnotatedMappingSource<TContext>();
		}
	}
}