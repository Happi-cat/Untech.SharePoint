using System;
using Untech.SharePoint.Common.AnnotationMapping;

namespace Untech.SharePoint.Common.Test.AnnotationMapping.Models
{
	public class EntityWithIndexer : TestEntity
	{
		[SpField]
		public string this[string key]
		{
			get { throw new NotImplementedException(); } 
			set { throw new NotImplementedException(); }
		}
	}
}