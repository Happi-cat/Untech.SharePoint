using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.Collections;
using System;

namespace Untech.SharePoint.Common.Configuration
{
	public class MappingConfiguration
	{
		public IMetaContextProvider AnnotatedMapping<T>()
		{
			return new AnnotationMapping.AnnotatedContextMapping<T>();
		}
	}

	public class Configuration
	{
		private Container<Type, IMetaContextProvider> _metaContextProviders = new Container<Type,IMetaContextProvider>();

		public Configuration RegisterMapping<TContext>(Func<MappingConfiguration, IMetaContextProvider> contextProviderBuidler)
		{
			RegisterMapping<TContext>(contextProviderBuidler(null));
			return this;
		}

		public Configuration RegisterMapping<TContext>(IMetaContextProvider contextProvider)
		{
			_metaContextProviders.Register(typeof(TContext), contextProvider);
			return this;
		}
	}
}