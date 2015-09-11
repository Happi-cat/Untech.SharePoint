using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	public class TestContext
	{
		[SpList(Title = "List1")]
		public ISpList<TestEntity> Entities { get; set; }

		[SpList(Title = "List1")]
		public ISpList<DerivedTestEntity1> DerivedEntities { get; set; }

		[SpList(Title = "List2")]
		public ISpList<DerivedTestEntity2> OtherEntities { get; set; }

		public ISpList<TestEntity> MissingAttribute { get; set; }

		public string NotAList { get; set; }

		
	}
}