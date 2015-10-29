using System;

namespace Untech.SharePoint.Common.Converters
{
	public interface IFieldConverterResolver
	{
		bool CanResolve(string typeAsString);

		IFieldConverter Resolve(string typeAsString);

		bool CanResolve(Type converType);

		IFieldConverter Resolve(Type converterType);
	}
}