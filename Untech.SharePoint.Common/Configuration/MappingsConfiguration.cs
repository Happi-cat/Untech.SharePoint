using System;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Services;

namespace Untech.SharePoint.Common.Configuration
{
	public class MappingsConfiguration
	{
		private readonly Container<Type, IMappingSource> _mappingSources = new Container<Type, IMappingSource>();

		public MappingsConfiguration Register<TContext>(Func<MappingSources, IMappingSource<TContext>> mappingBuilder)
			where TContext : ISpContext
		{
			Register(mappingBuilder(new MappingSources()));
			return this;
		}

		public MappingsConfiguration Register<TContext>(IMappingSource<TContext> contextProvider)
			where TContext : ISpContext
		{
			_mappingSources.Register(typeof(TContext), contextProvider);
			return this;
		}

		public IMappingSource Resolve<TContext>()
		{
			return _mappingSources.Resolve(typeof (TContext));
		}
	}
}