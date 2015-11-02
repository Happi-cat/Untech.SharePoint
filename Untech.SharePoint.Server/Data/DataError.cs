using System;

namespace Untech.SharePoint.Server.Data
{
	internal static class DataError
	{
		internal static Exception OperationNotAllowedForExternalList()
		{
			return new InvalidOperationException("This operation cannot be used with external list");
		}

		internal static Exception OperationRequireIdField()
		{
			return new InvalidOperationException("This operation require ID field");
		}
	}
}