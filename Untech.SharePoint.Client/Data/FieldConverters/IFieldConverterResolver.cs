using System;
namespace Untech.SharePoint.Client.Data.FieldConverters
{
	interface IFieldConverterResolver
	{
		IFieldConverter Create(string fieldType);

		IFieldConverter Create(Type type);

		void Register(Type type);
	}
}
