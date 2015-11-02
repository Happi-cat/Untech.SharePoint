using System.Collections.Generic;

namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	/// <summary>
	/// Represents base meta models visitor.
	/// </summary>
	public abstract class BaseMetaModelVisitor : IMetaModelVisitor
	{
		/// <summary>
		/// Visit <see cref="IMetaModel"/>
		/// </summary>
		/// <param name="model">Model to visit.</param>
		public virtual void Visit(IMetaModel model)
		{
			if (model != null)
			{
				model.Accept(this);
			}
		}

		/// <summary>
		/// Visit <see cref="MetaContext"/>
		/// </summary>
		/// <param name="context">Context to visit.</param>
		public virtual void VisitContext(MetaContext context)
		{
			if (context == null) return;
			VisitCollection(context.Lists);
		}

		/// <summary>
		/// Visit <see cref="MetaList"/>
		/// </summary>
		/// <param name="list">List to visit.</param>
		public virtual void VisitList(MetaList list)
		{
			if (list == null) return;
			VisitCollection(list.ContentTypes);
		}

		/// <summary>
		/// Visit <see cref="MetaContentType"/>
		/// </summary>
		/// <param name="contentType">ContentType to visit.</param>
		public virtual void VisitContentType(MetaContentType contentType)
		{
			if (contentType == null) return;
			VisitCollection(contentType.Fields);
		}

		/// <summary>
		/// Visit <see cref="MetaField"/>
		/// </summary>
		/// <param name="field">Field to visit.</param>
		public virtual void VisitField(MetaField field)
		{
			
		}

		/// <summary>
		/// Visit each <see cref="IMetaModel"/> in the specified collection.
		/// </summary>
		/// <param name="models">Collection of <see cref="IMetaModel"/></param>
		protected void VisitCollection(IEnumerable<IMetaModel> models)
		{
			foreach (var model in models)
			{
				Visit(model);
			}
		}
	}
}