using System;
using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class EntityWithWriteOnlyProperty : TestEntity
	{
		[SpField]
		public string WriteonlyProperty
		{
			get { throw new NotImplementedException(); }
		}
	}
}