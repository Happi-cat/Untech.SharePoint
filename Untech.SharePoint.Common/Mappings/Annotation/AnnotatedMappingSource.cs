using System.Reflection;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Mappings.Annotation
{
	internal sealed class AnnotatedMappingSource<TContext> : MappingSource<TContext>
		where TContext : ISpContext
	{
		private readonly AnnotatedContextMapping<TContext> _contextMapping;

		public AnnotatedMappingSource()
		{
			_contextMapping = new AnnotatedContextMapping<TContext>();
		}

		public override MetaContext GetMetaContext()
		{
			return _contextMapping.GetMetaContext();
		}

		public override string GetListUrlFromContextMember(MemberInfo member)
		{
			return _contextMapping.GetListUrlFromContextMember(member);
		}
	}
}
