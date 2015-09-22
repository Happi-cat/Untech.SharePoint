using System;
using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Services;

namespace Untech.SharePoint.Common.Configuration
{
	public class MappingsContainer
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

		private void Register<TContext>(IMappingSource<TContext> contextProvider)
			where TContext : ISpContext
		{
			_mappingSources.Register(typeof(TContext), contextProvider);

		}
	}
}