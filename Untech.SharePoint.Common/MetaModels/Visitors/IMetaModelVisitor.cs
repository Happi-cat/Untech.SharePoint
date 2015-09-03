namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	public interface IMetaModelVisitor
	{
		void Visit(MetaModel model);

		void VisitUnkown(MetaModel model);

		void VisitContext(MetaContext context);

		void VisitList(MetaList list);

		void VisitContentType(MetaContentType contentType);

		void VisitField(MetaField field);

	}
}