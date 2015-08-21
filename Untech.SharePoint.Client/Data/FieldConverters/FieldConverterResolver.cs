using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Client.Reflection;
using Untech.SharePoint.Client.Utility;

namespace Untech.SharePoint.Client.Data.FieldConverters
{
	internal class FieldConverterResolver : IFieldConverterResolver
	{
		public FieldConverterResolver()
		{
			BuiltInConverters = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
		}

		public static IFieldConverterResolver Instance
		{
			get { return Singleton<FieldConverterResolver>.GetInstance(RegisterBuiltInConverters); }
		}

		protected internal InstanceCreationFactory<IFieldConverter> ConverterFactory
		{
			get { return InstanceCreationFactory<IFieldConverter>.Instance; }
		}

		protected Dictionary<string, Type> BuiltInConverters { get; private set; }

		public void Register(Type type)
		{
			try
			{
				ConverterFactory.Register(type);
			}
			catch (Exception e)
			{
				throw new InvalidFieldConverterException(type, e);
			}
		}

		public IFieldConverter Create(Type type)
		{
			try
			{
				return new FieldConverterWrapper(type, ConverterFactory.Create(type));
			}
			catch (Exception e)
			{
				throw new FieldConverterException(type, e);
			}
		}

		public IFieldConverter Create(string fieldType)
		{
			return Create(BuiltInConverters[fieldType]);
		}

		protected internal static void RegisterBuiltInConverters(FieldConverterResolver instance)
		{
			Guard.CheckNotNull("instance", instance);

			var attributeType = typeof(SpFieldConverterAttribute);
			var interfaceType = typeof(IFieldConverter);

			var assembly = interfaceType.Assembly;

			var types = assembly.GetTypes()
				.Where(type => type.IsDefined(attributeType))
				.Where(type => interfaceType.IsAssignableFrom(type))
				.ToList();

			foreach (var type in types)
			{
				type.GetCustomAttributes(attributeType)
					.Cast<SpFieldConverterAttribute>()
					.Select(attribute => attribute.FieldTypeAsString)
					.ToList()
					.ForEach(n => instance.BuiltInConverters.Add(n, type));

				instance.Register(type);
			}
		}
	}
}
