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
			model?.Accept(this);
		}

		/// <summary>
		/// Visit <see cref="MetaContext"/>
		/// </summary>
		/// <param name="context">Context to visit.</param>
		public virtual void VisitContext(MetaContext context)
		{
			if (context == null) return;

			foreach (var model in (IEnumerable<MetaList>) context.Lists)
			{
				VisitList(model);
			}
		}

		/// <summary>
		/// Visit <see cref="MetaList"/>
		/// </summary>
		/// <param name="list">List to visit.</param>
		public virtual void VisitList(MetaList list)
		{
			if (list == null) return;

			foreach (var model in (IEnumerable<MetaContentType>)list.ContentTypes)
			{
				VisitContentType(model);
			}
		}

		/// <summary>
		/// Visit <see cref="MetaContentType"/>
		/// </summary>
		/// <param name="contentType">ContentType to visit.</param>
		public virtual void VisitContentType(MetaContentType contentType)
		{
			if (contentType == null) return;

			foreach (var model in (IEnumerable<MetaField>)contentType.Fields)
			{
				VisitField(model);
			}
		}

		/// <summary>
		/// Visit <see cref="MetaField"/>
		/// </summary>
		/// <param name="field">Field to visit.</param>
		public virtual void VisitField(MetaField field)
		{
			
		}
	}
}