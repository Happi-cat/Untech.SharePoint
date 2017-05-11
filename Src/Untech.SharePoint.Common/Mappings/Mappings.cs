using Untech.SharePoint.Data;
using Untech.SharePoint.Mappings.Annotation;
using Untech.SharePoint.Mappings.ClassLike;

namespace Untech.SharePoint.Mappings
{
	/// <summary>
	/// Represents class that provides a set of methods that returns <see cref="MappingSource{TContext}"/>
	/// </summary>
	public sealed class Mappings
	{
		/// <summary>
		/// Creates <see cref="MappingSource{TContext}"/> based on annotation attributes.
		/// </summary>
		/// <typeparam name="TContext">Type of <see cref="ISpContext"/></typeparam>
		/// <returns>Mapping source for the specified <typeparamref name="TContext"/>.</returns>
		public MappingSource<TContext> Annotated<TContext>()
			where TContext : ISpContext
		{
			return new AnnotatedMappingSource<TContext>();
		}

		/// <summary>
		/// Creates <see cref="MappingSource{TContext}"/> based on <see cref="ContextMap{TContext}"/>.
		/// </summary>
		/// <param name="contextMap">Instance of context mapping</param>
		/// <typeparam name="TContext">Type of <see cref="ISpContext"/></typeparam>
		/// <returns>Mapping source for the specified <typeparamref name="TContext"/>.</returns>
		public MappingSource<TContext> ClassLike<TContext>(ContextMap<TContext> contextMap)
		   where TContext : ISpContext
		{
			return new ClassLikeMappingSource<TContext>(contextMap);
		}
	}
}