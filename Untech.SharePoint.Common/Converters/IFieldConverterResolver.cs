using System;

namespace Untech.SharePoint.Common.Converters
{
	public interface IFieldConverterResolver
	{
		IFieldConverter Resolve(string typeAsString);

		IFieldConverter Resolve(Type converterType);
	}
}