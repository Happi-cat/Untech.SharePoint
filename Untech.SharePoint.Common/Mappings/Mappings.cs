﻿using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.Mappings.ClassLike;

namespace Untech.SharePoint.Common.Mappings
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

		public MappingSource<TContext> ClassLike<TContext>(ContextMap<TContext> contextMap)
		   where TContext : ISpContext
		{
			return new ClassLikeMappingSource<TContext>(contextMap);
		}
	}
}