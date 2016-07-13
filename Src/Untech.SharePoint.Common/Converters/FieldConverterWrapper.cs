using System;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters
{
	internal class FieldConverterWrapper : IFieldConverter
	{
		private Type ConverterType { get; }
		private IFieldConverter ConverterInstance { get; }

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
				throw new FieldConverterInitializationException(ConverterType, field, e);
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
				throw Error.CannotConvertFromSpValue(ConverterType, value, e);
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
				throw Error.CannotConvertToSpValue(ConverterType, value, e);
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
				throw Error.CannotConvertToCamlValue(ConverterType, value, e);
			}
		}

		
	}
}