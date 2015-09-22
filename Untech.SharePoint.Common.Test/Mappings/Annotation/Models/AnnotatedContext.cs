using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class AnnotatedContext : ISpContext
	{
		[SpList(Title = "List1")]
		public ISpList<AnnotatedEntity> Entities { get; set; }

		[SpList(Title = "List1")]
		public ISpList<DerivedAnnotatedEntityWithIheritedAnnotation> DerivedEntities { get; set; }

		[SpList(Title = "List2")]
		public ISpList<DerivedAnnotatedEntityWithOverwrittenAnnotation> OtherEntities { get; set; }

		public ISpList<AnnotatedEntity> MissingAttribute { get; set; }

		public string NotAList { get; set; }
	}
}