using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Visitors
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

		public virtual void VisitUnkown(MetaModel model)
		{
			throw new NotSupportedException();
		}

		public virtual void Visit(MetaModel model)
		{
			if (model != null)
			{
				model.Accept(this);
			}
		}

		protected void VisitCollection(IEnumerable<MetaModel> models)
		{
			models.Each(Visit);
		}
	}
}