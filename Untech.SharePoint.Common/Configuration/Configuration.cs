using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.Collections;
using System;
using Untech.SharePoint.Common.Services;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Configuration
{
	public class MappingSources
	{
		public IMappingSource Annotated<T>()
			where T: ISpContext
		{
			return new AnnotationMapping.AnnotatedMappingSource<T>();
		}
	}

	public class Configuration
	{
		private Container<Type, IMappingSource> _mappingSources = new Container<Type, IMappingSource>();

		public Configuration RegisterMapping<TContext>(Func<MappingSources, IMappingSource> mappingBuilder)
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