namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	/// <summary>
	/// Represents meta model visitor interface.
	/// </summary>
	public interface IMetaModelVisitor
	{
		void Visit(IMetaModel model);

		void VisitContext(MetaContext context);

		void VisitList(MetaList list);

		void VisitContentType(MetaContentType contentType);

		void VisitField(MetaField field);
	}
}