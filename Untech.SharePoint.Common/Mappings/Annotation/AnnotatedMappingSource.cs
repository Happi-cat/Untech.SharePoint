using System;
using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

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

		public Type ContextType
		{
			get { return _contextType; }
		}

		public MetaContext GetMetaContext()
		{
			return _contextMapping.GetMetaContext();
		}

		public string GetListTitleFromContextMember(MemberInfo member)
		{
			return _contextMapping.GetListTitleFromContextMember(member);
		}
	}
}
