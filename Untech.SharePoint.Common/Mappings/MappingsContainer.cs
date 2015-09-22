using System;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Mappings
{
	public class MappingsContainer : IMappingSourceResolver
	{
		private readonly Container<Type, IMappingSource> _mappingSources = new Container<Type, IMappingSource>();

		public void Annotated<TContext>()
			where TContext : ISpContext
		{
			Register(new AnnotatedMappingSource<TContext>());
		}

		public IMappingSource Resolve<TContext>()
			where TContext : ISpContext
		{
			return _mappingSources.Resolve(typeof (TContext));
		}

		public IMappingSource Resolve(Type contextType)
		{
			return _mappingSources.Resolve(contextType);
		}

		#region [Private Methods]

		private void Register<TContext>(IMappingSource<TContext> contextProvider)
			where TContext : ISpContext
		{
			_mappingSources.Register(typeof (TContext), contextProvider);

		}

		#endregion

	}
}