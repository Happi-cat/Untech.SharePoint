using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Core.Utility;

namespace Untech.SharePoint.Core.Data.Fields.Converters
{
	public class FieldConverterResolver
	{
		private readonly Dictionary<string, Type> _builtinConverters = new Dictionary<string, Type>();

		public static FieldConverterResolver Instance
		{
			get
			{
				return Singleton<FieldConverterResolver>.GetInstance(n => n.Initialize());
			}
		}

		public void Initialize()
		{
			var attributeType = typeof(SPFieldConverterAttribute);
			var objectType = typeof(IFieldConverter);

			var assembly = GetType().Assembly;

			var types = assembly.GetTypes()
				.Where(type => type.IsDefined(attributeType))
				.Where(type => objectType.IsAssignableFrom(type))
				.ToList();

			foreach (var type in types)
			{
				var keys = type.GetCustomAttributes(attributeType)
					.Cast<SPFieldConverterAttribute>()
					.Select(attribute => attribute.FieldType)
					.ToList();

				foreach (var key in keys)
				{
					_builtinConverters.Add(key, type);
				}

				Register(type);
			}
		}

		public void Register(Type type)
		{
			FieldConverterFactory.Instance.Register(type);
		}

		public IFieldConverter Get(Type type)
		{
			return FieldConverterFactory.Instance.Create(type);
		}

		public IFieldConverter Get(string fieldType)
		{
			return Get(_builtinConverters[fieldType]);
		}
	}
}
