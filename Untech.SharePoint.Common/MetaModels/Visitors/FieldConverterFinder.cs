using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Untech.SharePoint.Common.MetaModels.Visitors
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