using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels.Providers;
using Untech.SharePoint.Common.Services;

namespace Untech.SharePoint.Common.AnnotationMapping
{
	internal sealed class AnnotatedMappingSource<T> : IMappingSource
		where T : ISpContext
	{
		private readonly AnnotatedContextMapping<T> _contextMapping;

		public AnnotatedMappingSource()
		{
			_contextMapping = new AnnotatedContextMapping<T>();
		}

		public IMetaContextProvider ContextProvider
		{
			get { return _contextMapping; }
		}

		public IListTitleResolver ListTitleResolver
		{
			get { return _contextMapping; }
		}
	}
}
