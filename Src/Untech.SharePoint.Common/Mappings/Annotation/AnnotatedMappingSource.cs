using System.Reflection;
using Untech.SharePoint.Data;
using Untech.SharePoint.MetaModels;

namespace Untech.SharePoint.Mappings.Annotation
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
