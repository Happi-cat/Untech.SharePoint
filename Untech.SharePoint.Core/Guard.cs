using Microsoft.SharePoint;
using System;
using Untech.SharePoint.Core.Extensions;

namespace Untech.SharePoint.Core
{
	internal static class Guard
	{
		public static void NotNull(object obj, string paramName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}

		public static void ThrowIfNot<T>(object obj, string message)
		{
			if (!(obj is T))
			{
				throw new ArgumentException(message);
			}
		}

		public static void FieldValueType(SPField field, Type fieldValueType, string paramName)
		{
			if (field.FieldValueType != fieldValueType)
			{
				throw new ArgumentException(string.Format("SPField should have {0} FieldValueType", fieldValueType), paramName);
			}
		}

		public static void FieldValueType(SPField fieldToCheck, Type fieldValueType, SPFieldType calculatedFieldType, string paramName)
		{
			var calculatedField = fieldToCheck as SPFieldCalculated;
			if (calculatedField == null)
			{
				if (fieldToCheck.FieldValueType != fieldValueType)
				{
					throw new ArgumentException(string.Format("SPField should have {0} FieldValueType", fieldValueType), paramName);
				}
			}
			else
			{
				if (calculatedField.OutputType != calculatedFieldType)
				{
					throw new ArgumentException(string.Format("SPFieldCalculated should have {0} OutputType", calculatedFieldType), paramName);
				}
			}
		}

		public static void PropertyType(Type typeToCheck, Type[] allowedTypes, string paramName)
		{
			if (!typeToCheck.In(allowedTypes))
			{
				throw new ArgumentException();
			}
		}
	}
}