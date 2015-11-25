using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Visitors;

namespace Untech.SharePoint.Common.Converters
{
	internal sealed class FieldConverterFinder : BaseMetaModelVisitor
	{
		private FieldConverterFinder()
		{
			Converters = new List<Type>();
		}

		private List<Type> Converters { get; set; }

		[NotNull]
		public static IEnumerable<Type> Find([CanBeNull]IMetaModel model)
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