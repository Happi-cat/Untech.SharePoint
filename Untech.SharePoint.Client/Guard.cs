using System;

namespace Untech.SharePoint.Client
{
	internal static class Guard
	{
		public static void CheckNotNull(string paramName, object obj)
		{
			if (obj != null) return;

			throw new ArgumentNullException(paramName);
		}

		public static void CheckTypeIsAssignableFrom(string paramName, Type actualType, Type expectedType)
		{
			CheckNotNull("actualType", actualType);
			CheckNotNull("expectedType", expectedType);

			if (expectedType.IsAssignableFrom(actualType))
			{
				return;
			}

			var message = string.Format("Parameter '{0}' is a '{2}', which cannot be assigned to type '{1}'.",
				paramName,
				expectedType,
				actualType);
			throw new ArgumentException(message, paramName);
		}

		public static void CheckTypeIsAssignableFrom<TExpected>(string paramName, Type actualType)
		{
			CheckTypeIsAssignableFrom(paramName, actualType, typeof(TExpected));
		}

	}
}