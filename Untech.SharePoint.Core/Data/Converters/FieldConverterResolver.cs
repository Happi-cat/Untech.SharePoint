using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Core.Reflection;
using Untech.SharePoint.Core.Utility;

namespace Untech.SharePoint.Core.Data.Converters
{
	internal class FieldConverterResolver
	{
		private readonly Dictionary<string, Type> _builtinConverters = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

		public static FieldConverterResolver Instance
		{
			get { return Singleton<FieldConverterResolver>.GetInstance(n => n.Initialize()); }
		}

		protected internal InstanceCreationFactory<IFieldConverter> FieldConverterFactory
		{
			get { return InstanceCreationFactory<IFieldConverter>.Instance; }
		}

		public void Initialize()
		{
			var attributeType = typeof(SpFieldConverterAttribute);
			var objectType = typeof(IFieldConverter);

			var assembly = GetType().Assembly;

			var types = assembly.GetTypes()
				.Where(type => type.IsDefined(attributeType))
				.Where(type => objectType.IsAssignableFrom(type))
				.ToList();

			foreach (var type in types)
			{
				var keys = type.GetCustomAttributes(attributeType)
					.Cast<SpFieldConverterAttribute>()
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
			try
			{
				FieldConverterFactory.Register(type);
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
				return new FieldConverterWrapper(type, FieldConverterFactory.Create(type));
			}
			catch (Exception e)
			{
				throw new FieldConverterException(type, e);
			}
		}

		public IFieldConverter Create(string fieldType)
		{
			return Create(_builtinConverters[fieldType]);
		}
	}
}
