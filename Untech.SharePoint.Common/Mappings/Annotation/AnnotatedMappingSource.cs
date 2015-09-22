using System;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	internal sealed class AnnotatedMappingSource<TContext> : IMappingSource<TContext>
		where TContext : ISpContext
	{
		private readonly Type _contextType;
		private readonly AnnotatedContextMapping<TContext> _contextMapping;

		public AnnotatedMappingSource()
		{
			_contextType = typeof(TContext);
			_contextMapping = new AnnotatedContextMapping<TContext>();
		}

		public IMetaContextProvider ContextProvider
		{
			get { return _contextMapping; }
		}

		public IListTitleResolver ListTitleResolver
		{
			get { return _contextMapping; }
		}

		public Type ContextType
		{
			get { return _contextType; }
		}
	}
}
