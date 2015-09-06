using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Visitors
{
	public class FieldConverterRegistrator : BaseMetaModelVisitor
	{
		public override void VisitField(MetaField field)
		{
			if (field.CustomConverterType != null)
			{
				RegisterConverter(field.CustomConverterType);
			}
		}

		protected virtual void RegisterConverter(Type converterType)
		{
			
		}
	}
}