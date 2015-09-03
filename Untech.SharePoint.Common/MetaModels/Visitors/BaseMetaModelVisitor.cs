using System;
using System.Linq;

namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	public abstract class BaseMetaModelVisitor : IMetaModelVisitor
	{
		public virtual void VisitContext(MetaContext context)
		{
			context.Lists
				.ToList()
				.ForEach(Visit);
		}

		public virtual void VisitList(MetaList list)
		{
			list.ContentTypes
				.ToList()
				.ForEach(Visit);
		}

		public virtual void VisitContentType(MetaContentType contentType)
		{
			contentType.Fields
				.ToList()
				.ForEach(Visit);
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
	}
}