using Untech.SharePoint.Common.AnnotationMapping;
using Untech.SharePoint.Common.Data;

namespace Untech.SharePoint.Common.Test.AnnotationMapping
{
	public class TestContext
	{
		[SpList(ListTitle = "List1")]
		public ISpList<TestEntity> Entities { get; set; }

		[SpList(ListTitle = "List1")]
		public ISpList<DerivedTestEntity1> DerivedEntities { get; set; }

		[SpList(ListTitle = "List2")]
		public ISpList<DerivedTestEntity2> OtherEntities { get; set; }

		public ISpList<TestEntity> MissingAttribute { get; set; }

		public string NotAList { get; set; }

		
	}
}