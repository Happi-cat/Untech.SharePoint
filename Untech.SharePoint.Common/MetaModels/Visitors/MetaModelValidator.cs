using System;

namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	public class MetaModelValidator : BaseMetaModelVisitor
	{
		public override void VisitContentType(MetaContentType contentType)
		{
			if (string.IsNullOrEmpty(contentType.ContentTypeId))
			{
				throw new Exception();
			}

			base.VisitContentType(contentType);
		}

		public override void VisitField(MetaField field)
		{
			if (string.IsNullOrEmpty(field.FieldTypeAsString))
			{
				throw new Exception();
			}

			base.VisitField(field);
		}
	}

}