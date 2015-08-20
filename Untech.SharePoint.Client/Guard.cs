using System;
using Untech.SharePoint.Client.Extensions;

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
				paramName, expectedType, actualType);
			throw new ArgumentException(message, paramName);
		}

		public static void CheckTypeIsAssignableFrom<TExpected>(string paramName, Type actualType)
		{
			CheckTypeIsAssignableFrom(paramName, actualType, typeof(TExpected));
		}


		public static void CheckType(string paramName, object actualValue, Type expectedType)
		{
			CheckNotNull("actualType", actualValue);
			CheckNotNull("expectedType", expectedType);

			if (actualValue == null)
			{
				if (expectedType.IsNullableType()) 
				{
					return;
				}
				throw new ArgumentException(string.Format("Parameter '{0}' is null, but '{2}' is not a nullable type.",
					paramName, expectedType), paramName);
			}

			if (expectedType.IsInstanceOfType(actualValue))
			{
				return;
			}

			throw new ArgumentException(string.Format("Parameter '{0}' is a '{2}', '{1}' is expected.",
				paramName, expectedType, actualValue), paramName);
		}

		public static void CheckType<TExpected>(string paramName, object actualValue)
		{
			CheckType(paramName, actualValue, typeof(TExpected));
		}

	}
}