using System;
using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class EntityWithReadOnlyProperty : TestEntity
	{
		[SpField]
		public string ReadonlyProperty
		{
			get { throw new NotImplementedException(); }
		}
	}
}