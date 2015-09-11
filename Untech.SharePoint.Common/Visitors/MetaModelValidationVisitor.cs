using System;
using System.Reflection;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Visitors
{
	public class MetaModelValidationVisitor : BaseMetaModelVisitor
	{
		public override void VisitContentType(MetaContentType contentType)
		{
			if (string.IsNullOrEmpty(contentType.Id))
			{
				throw new Exception();
			}

			base.VisitContentType(contentType);
		}

		public override void VisitField(MetaField field)
		{
			if (string.IsNullOrEmpty(field.TypeAsString))
			{
				throw new Exception();
			}

			base.VisitField(field);
		}

		protected virtual void VisitFieldMember(MemberInfo member)
		{
			
		}
	}

}