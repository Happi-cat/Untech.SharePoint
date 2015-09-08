using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.SharePoint.Common
{
	internal static class Guard
	{
		public static void CheckNotNull(string paramName, object obj)
		{
			if (obj != null) return;

			throw new ArgumentNullException(paramName);
		}

		public static void CheckNotNullOrEmpty<T>(string paramName, string value)
		{
			if (!string.IsNullOrEmpty(value)) return;

			throw new ArgumentException("String is null or empty", paramName);
		}

		public static void CheckNotNullOrEmpty<T>(string paramName, IEnumerable<T> enumerable)
		{
			if (enumerable != null && enumerable.Any()) return;

			throw new ArgumentException("Collection is null or empty", paramName);
		}

		public static void CheckTypeIsAssignableTo(string paramName, Type actualType, Type expectedType)
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

		public static void CheckTypeIsAssignableTo<TExpected>(string paramName, Type actualType)
		{
			CheckTypeIsAssignableTo(paramName, actualType, typeof(TExpected));
		}


		public static void CheckType(string paramName, object actualValue, Type expectedType)
		{
			CheckNotNull("actualType", actualValue);
			CheckNotNull("expectedType", expectedType);

			if (actualValue == null)
			{
				//if (expectedType.IsNullableType()) 
				//{
				//	return;
				//}
				//throw new ArgumentException(string.Format("Parameter '{0}' is null, but '{2}' is not a nullable type.",
				//	paramName, expectedType), paramName);
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