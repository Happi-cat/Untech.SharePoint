using System;
using System.Collections.Generic;
using System.Linq;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Extensions;

namespace Untech.SharePoint.Utils
{
	/// <summary>
	/// Provides a set of static methods for arguments validation.
	/// </summary>
	[PublicAPI]
	public static class Guard
	{
		/// <summary>
		/// Checks object equality to null.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="obj">Object to validate.</param>
		/// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
		public static void CheckNotNull([InvokerParameterName][CanBeNull]string paramName, [CanBeNull]object obj)
		{
			if (obj != null) return;

			throw new ArgumentNullException(paramName);
		}

		/// <summary>
		/// Checks string equality to null or empty string.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="value">Object to validate.</param>
		/// <exception cref="ArgumentException"><paramref name="value"/> is null.</exception>
		public static void CheckNotNullOrEmpty([InvokerParameterName][CanBeNull]string paramName, [CanBeNull]string value)
		{
			if (!string.IsNullOrEmpty(value)) return;

			throw new ArgumentException("String is null or empty", paramName);
		}

		/// <summary>
		/// Checks whether <see cref="IEnumerable{T}"/> is equal to null or empty.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="enumerable">Collection to validate.</param>
		/// <exception cref="ArgumentException"><paramref name="enumerable"/> is null or empty.</exception>
		public static void CheckNotNullOrEmpty<T>([InvokerParameterName][CanBeNull]string paramName, [CanBeNull]IEnumerable<T> enumerable)
		{
			if (enumerable != null && enumerable.Any()) return;

			throw new ArgumentException("Collection is null or empty", paramName);
		}

		/// <summary>
		/// Checks whether <paramref name="actualType"/> can be assigned to <paramref name="expectedType"/>.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="actualType">Actual <see cref="Type"/>.</param>
		/// <param name="expectedType">Expected type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="actualType"/> or <paramref name="expectedType"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="actualType"/> cannot be assigned to <paramref name="expectedType"/>.</exception>
		public static void CheckIsTypeAssignableTo([InvokerParameterName][CanBeNull]string paramName, [NotNull]Type actualType, [NotNull]Type expectedType)
		{
			CheckNotNull(nameof(actualType), actualType);
			CheckNotNull(nameof(expectedType), expectedType);

			if (expectedType.IsAssignableFrom(actualType))
			{
				return;
			}

			var message = string.Format("Parameter '{0}' is a '{2}', which cannot be assigned to type '{1}'.",
				paramName, expectedType, actualType);
			throw new ArgumentException(message, paramName);
		}

		/// <summary>
		/// Checks whether <paramref name="actualType"/> can be assigned to <typeparamref name="TExpected"/>.
		/// </summary>
		/// <typeparam name="TExpected">Expected type.</typeparam>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="actualType">Actual <see cref="Type"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="actualType"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="actualType"/> cannot be assigned to <typeparamref name="TExpected"/>.</exception>
		public static void CheckIsTypeAssignableTo<TExpected>([InvokerParameterName][CanBeNull]string paramName, [NotNull]Type actualType)
		{
			CheckIsTypeAssignableTo(paramName, actualType, typeof(TExpected));
		}

		/// <summary>
		/// Checks whether <paramref name="actualValue"/> can be assigned to <paramref name="expectedType"/>.
		/// </summary>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="actualValue">Object to check.</param>
		/// <param name="expectedType">Expected type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="expectedType"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="actualValue"/> cannot be assigned to <paramref name="expectedType"/>.</exception>
		public static void CheckIsObjectAssignableTo([InvokerParameterName][CanBeNull]string paramName, [CanBeNull]object actualValue, [NotNull]Type expectedType)
		{
			CheckNotNull(nameof(expectedType), expectedType);

			if (actualValue == null)
			{
				if (expectedType.IsNullAssignable())
				{
					return;
				}
				throw new ArgumentException(
					$"Parameter '{paramName}' is null, but expected type '{expectedType}' is not a System.Nullable`1 and is not a class type.", paramName);
			}

			if (expectedType.IsInstanceOfType(actualValue))
			{
				return;
			}

			throw new ArgumentException(string.Format("Parameter '{0}' is a '{2}', '{1}' is expected.",
				paramName, expectedType, actualValue), paramName);
		}

		/// <summary>
		/// Checks whether <paramref name="actualValue"/> can be assigned to <typeparamref name="TExpected"/>.
		/// </summary>
		/// <typeparam name="TExpected">Expected type.</typeparam>
		/// <param name="paramName">Parameter name.</param>
		/// <param name="actualValue">Object to check.</param>
		/// <exception cref="ArgumentException"><paramref name="actualValue"/> cannot be assigned to <typeparamref name="TExpected"/>.</exception>
		public static void CheckIsObjectAssignableTo<TExpected>([InvokerParameterName][CanBeNull]string paramName, [CanBeNull]object actualValue)
		{
			CheckIsObjectAssignableTo(paramName, actualValue, typeof(TExpected));
		}
	}
}