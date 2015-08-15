using System;
using Microsoft.SharePoint.Client;

namespace Untech.SharePoint.Client.Data.FieldConverters
{
	internal class FieldConverterWrapper : IFieldConverter
	{
		public Type ConverterType { get; set; }
		public IFieldConverter ConverterInstance { get; set; }

		public FieldConverterWrapper(Type converterType, IFieldConverter converterInstance)
		{
			ConverterType = converterType;
			ConverterInstance = converterInstance;
		}

		public void Initialize(Field field, Type propertyType)
		{
			try
			{
				ConverterInstance.Initialize(field, propertyType);
			}
			catch (Exception e)
			{
				throw new InvalidFieldConverterException(ConverterType, e);
			}
		}

		public object FromSpValue(object value)
		{
			try
			{
				return ConverterInstance.FromSpValue(value);
			}
			catch (Exception e)
			{
				throw new FieldConverterException(ConverterType, e);
			}
		}

		public object ToSpValue(object value)
		{
			try
			{
				return ConverterInstance.ToSpValue(value);
			}
			catch (Exception e)
			{
				throw new FieldConverterException(ConverterType, e);
			}
		}
	}
}