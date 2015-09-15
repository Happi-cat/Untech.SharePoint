using System;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Services;

namespace Untech.SharePoint.Common.Configuration
{
	public class Configuration
	{
		private readonly Container<Type, IMappingSource> _mappingSources = new Container<Type, IMappingSource>();

		public Configuration RegisterMapping<TContext>(Func<MappingSources, IMappingSource<TContext>> mappingBuilder)
			where TContext : ISpContext
		{
			RegisterMapping<TContext>(mappingBuilder(new MappingSources()));
			return this;
		}

		private Configuration RegisterMapping<TContext>(IMappingSource contextProvider)
		{
			_mappingSources.Register(typeof(TContext), contextProvider);
			return this;
		}
	}
}