using System.Collections.Generic;
using Untech.SharePoint.Common.Extensions;

namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	public abstract class BaseMetaModelVisitor : IMetaModelVisitor
	{
		public virtual void VisitContext(MetaContext context)
		{
			VisitCollection(context.Lists);
		}

		public virtual void VisitList(MetaList list)
		{
			VisitCollection(list.ContentTypes);
		}

		public virtual void VisitContentType(MetaContentType contentType)
		{
			VisitCollection(contentType.Fields);
		}

		public virtual void VisitField(MetaField field)
		{
			
		}

		public virtual void Visit(IMetaModel model)
		{
			if (model != null)
			{
				model.Accept(this);
			}
		}

		protected void VisitCollection(IEnumerable<IMetaModel> models)
		{
			models.Each(Visit);
		}
	}
}