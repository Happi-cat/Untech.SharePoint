using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class TestContext
	{
		[SpList(Title = "List1")]
		public ISpList<TestEntity> Entities { get; set; }

		[SpList(Title = "List1")]
		public ISpList<DerivedTestEntityWithIheritedAnnotation> DerivedEntities { get; set; }

		[SpList(Title = "List2")]
		public ISpList<DerivedTestEntityWithOverwrittenAnnotation> OtherEntities { get; set; }

		public ISpList<TestEntity> MissingAttribute { get; set; }

		public string NotAList { get; set; }
	}
}