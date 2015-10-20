using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Data
{
	public class DataMappingException : Exception
	{
		public DataMappingException(MetaField field)
		{
			
		}

		public DataMappingException(MetaField field, Exception inner)
		{

		}
	}
}