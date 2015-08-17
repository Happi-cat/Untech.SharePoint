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
				throw new FieldConverterInitializationException(ConverterType, e);
			}
		}

		public object FromClientValue(object value)
		{
			try
			{
				return ConverterInstance.FromClientValue(value);
			}
			catch (Exception e)
			{
				throw new FieldConverterException(ConverterType, e);
			}
		}

		public object ToClientValue(object value)
		{
			try
			{
				return ConverterInstance.ToClientValue(value);
			}
			catch (Exception e)
			{
				throw new FieldConverterException(ConverterType, e);
			}
		}

		public string ToCamlValue(object value)
		{
			try
			{
				return ConverterInstance.ToCamlValue(value);
			}
			catch (Exception e)
			{
				throw new FieldConverterException(ConverterType, e);
			}
		}
	}
}