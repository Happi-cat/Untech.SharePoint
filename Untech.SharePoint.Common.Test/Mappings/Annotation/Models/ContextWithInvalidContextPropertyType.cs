﻿using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Mappings.Annotation.Models
{
	public class ContextWithInvalidContextPropertyType : AnnotatedContext
	{
		[SpList(Title = "WrongPropertyType")]
		public bool WrongPropertyType { get; set; }
	}
}