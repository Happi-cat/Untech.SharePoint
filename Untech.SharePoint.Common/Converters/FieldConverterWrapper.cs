using System;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Converters
{
	internal class FieldConverterWrapper : IFieldConverter
	{
		private Type ConverterType { get; set; }
		private IFieldConverter ConverterInstance { get; set; }

		public FieldConverterWrapper(Type converterType, IFieldConverter converterInstance)
		{
			ConverterType = converterType;
			ConverterInstance = converterInstance;
		}

		public void Initialize(MetaField field)
		{
			try
			{
				ConverterInstance.Initialize(field);
			}
			catch (Exception e)
			{
				throw new FieldConverterInitializationException(ConverterType, e);
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