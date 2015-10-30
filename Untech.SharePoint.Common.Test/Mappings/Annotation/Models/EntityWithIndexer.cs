using System;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class EntityWithIndexer : Entity
	{
		[SpField]
		public string this[string key]
		{
			get { throw new NotImplementedException(); } 
			set { throw new NotImplementedException(); }
		}
	}
}