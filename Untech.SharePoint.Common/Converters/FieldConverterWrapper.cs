using System;

namespace Untech.SharePoint.Common.Converters
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

		public void Initialize(MetaModels.MetaField field)
		{
			throw new NotImplementedException();
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