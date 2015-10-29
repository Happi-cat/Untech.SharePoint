namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	/// <summary>
	/// Represents meta model visitor interface.
	/// </summary>
	public interface IMetaModelVisitor
	{
		/// <summary>
		/// Visit <see cref="IMetaModel"/>
		/// </summary>
		/// <param name="model">Model to visit.</param>
		void Visit(IMetaModel model);

		/// <summary>
		/// Visit <see cref="MetaContext"/>
		/// </summary>
		/// <param name="context">Context to visit.</param>
		void VisitContext(MetaContext context);

		/// <summary>
		/// Visit <see cref="MetaList"/>
		/// </summary>
		/// <param name="list">List to visit.</param>
		void VisitList(MetaList list);

		/// <summary>
		/// Visit <see cref="MetaContentType"/>
		/// </summary>
		/// <param name="contentType">ContentType to visit.</param>
		void VisitContentType(MetaContentType contentType);

		/// <summary>
		/// Visit <see cref="MetaField"/>
		/// </summary>
		/// <param name="field">Field to visit.</param>
		void VisitField(MetaField field);
	}
}