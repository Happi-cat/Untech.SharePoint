using System;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Converters
{
	public class FieldConverterContainer : IFieldConverterResolver
	{
		private readonly Container<string, Type> _fieldTypesMap = new Container<string, Type>();
		private readonly KeyedFactory<Type, IFieldConverter> _fieldConvertersBuilders = new KeyedFactory<Type, IFieldConverter>();

		public void AddFromAssembly(Assembly assembly)
		{
			Guard.CheckNotNull("assembly", assembly);

			assembly.GetTypes()
				.Where(n => n.IsDefined(typeof(SpFieldConverterAttribute)))
				.Where(n => typeof(IFieldConverter).IsAssignableFrom(n) && !n.IsAbstract)
				.Each(RegisterBuiltInConverter);
		}

		public void Add<TConverter>()
			where TConverter : IFieldConverter
		{
			var converterType = typeof (TConverter);

			Register(converterType, InstanceCreationUtility.GetCreator<IFieldConverter>(converterType));
		}

		public void Add(Type converterType)
		{
			Guard.CheckNotNull("converterType", converterType);

			Register(converterType, InstanceCreationUtility.GetCreator<IFieldConverter>(converterType));
		}

		public void Add<TConverter>(Func<IFieldConverter> converterBuilder)
			where TConverter: IFieldConverter
		{
			Guard.CheckNotNull("converterBuilder", converterBuilder);

			Register(typeof(TConverter), converterBuilder);
		}

		public IFieldConverter Resolve(string typeAsString)
		{
			return Resolve(_fieldTypesMap.Resolve(typeAsString));
		}

		public IFieldConverter Resolve(Type converterType)
		{
			return new FieldConverterWrapper(converterType, _fieldConvertersBuilders.Create(converterType));
		}

		#region [Private Methods]

		private void RegisterBuiltInConverter(Type converterType)
		{
			var converterAttributes = converterType.GetCustomAttributes<SpFieldConverterAttribute>();

			var creator = InstanceCreationUtility.GetCreator<IFieldConverter>(converterType);

			converterAttributes
				.Where(n => !string.IsNullOrEmpty(n.FieldTypeAsString))
				.Each(n => _fieldTypesMap.Register(n.FieldTypeAsString, converterType));

			Register(converterType, creator);
		}

		private void Register(Type converterType, Func<IFieldConverter> converterBuilder)
		{
			_fieldConvertersBuilders.Register(converterType, converterBuilder);
		}

		#endregion

	}
}