using System;
using System.Collections.Generic;

namespace Untech.SharePoint.Common.MetaModels.Visitors
{
	public class FieldConverterFinder : BaseMetaModelVisitor
	{
		public FieldConverterFinder()
		{
			Converters = new List<Type>();
		}

		protected List<Type> Converters { get; private set; }

		public static IEnumerable<Type> Find(IMetaModel model)
		{
			var finder = new FieldConverterFinder();
			
			finder.Visit(model);

			return finder.Converters;
		}

		public override void VisitField(MetaField field)
		{
			if (field.CustomConverterType != null)
			{
				Converters.Add(field.CustomConverterType);
			}
		}
	}
}