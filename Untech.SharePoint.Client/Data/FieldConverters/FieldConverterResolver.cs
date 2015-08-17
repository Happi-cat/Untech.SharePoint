using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Client.Reflection;
using Untech.SharePoint.Client.Utility;

namespace Untech.SharePoint.Client.Data.FieldConverters
{
	internal class FieldConverterResolver
	{
		private readonly Dictionary<string, Type> _builtInConverters = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

		public static FieldConverterResolver Instance
		{
			get { return Singleton<FieldConverterResolver>.GetInstance(RegisterBuiltInConverters); }
		}

		protected internal InstanceCreationFactory<IFieldConverter> ConverterFactory
		{
			get { return InstanceCreationFactory<IFieldConverter>.Instance; }
		}

		public void Register(Type type)
		{
			try
			{
				ConverterFactory.Register(type);
			}
			catch (Exception e)
			{
				throw new FieldConverterException(type, e);
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
			return Create(_builtInConverters[fieldType]);
		}

		protected internal static void RegisterBuiltInConverters(FieldConverterResolver instance)
		{
			Guard.CheckNotNull("instance", instance);

			var attributeType = typeof(SpFieldConverterAttribute);
			var interfaceType = typeof(IFieldConverter);

			var assembly = instance.GetType().Assembly;

			var types = assembly.GetTypes()
				.Where(type => type.IsDefined(attributeType))
				.Where(type => interfaceType.IsAssignableFrom(type))
				.ToList();

			foreach (var type in types)
			{
				var keys = type.GetCustomAttributes(attributeType)
					.Cast<SpFieldConverterAttribute>()
					.Select(attribute => attribute.FieldType)
					.ToList();

				foreach (var key in keys)
				{
					instance._builtInConverters.Add(key, type);
				}

				instance.Register(type);
			}
		}
	}
}
