using System;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Configuration
{
	public class FieldConvertersConfiguration : IFieldConverterResolver
	{
		private readonly Container<string, Type> _fieldConvertersResolver = new Container<string, Type>();
		private readonly Container<Type, Func<IFieldConverter>> _fieldConvertersBuilders = new Container<Type, Func<IFieldConverter>>();

		public FieldConvertersConfiguration RegisterFromAssembly(Assembly assembly)
		{
			Guard.CheckNotNull("assembly", assembly);

			assembly.GetTypes()
				.Where(n => n.IsDefined(typeof(SpFieldConverterAttribute)))
				.Where(n => typeof(IFieldConverter).IsAssignableFrom(n) && !n.IsAbstract)
				.Each(RegisterBuiltInConverter);

			return this;
		}

		public FieldConvertersConfiguration Register<TConverter>()
			where TConverter : IFieldConverter
		{
			Register(typeof (TConverter));
			return this;
		}

		public FieldConvertersConfiguration Register(Type converterType)
		{
			Guard.CheckNotNull("converterType", converterType);

			Register(converterType, InstanceCreationUtility.GetCreator<IFieldConverter>(converterType));
			return this;
		}

		public FieldConvertersConfiguration Register<TConverter>(Func<IFieldConverter> converterBuilder)
			where TConverter: IFieldConverter
		{
			Guard.CheckNotNull("converterBuilder", converterBuilder);

			Register(typeof(TConverter), converterBuilder);
			return this;
		}

		public IFieldConverter Resolve(string typeAsString)
		{
			return Resolve(_fieldConvertersResolver.Resolve(typeAsString));
		}

		public IFieldConverter Resolve<TConverter>()
			where TConverter : IFieldConverter
		{
			return Resolve(typeof (TConverter));
		}

		public IFieldConverter Resolve(Type converterType)
		{
			var creator = _fieldConvertersBuilders.Resolve(converterType);
			return new FieldConverterWrapper(converterType, creator());
		}

		#region [Private Methods]

		private void RegisterBuiltInConverter(Type converterType)
		{
			var converterAttributes = converterType.GetCustomAttributes<SpFieldConverterAttribute>();

			var creator = InstanceCreationUtility.GetCreator<IFieldConverter>(converterType);

			converterAttributes
				.Where(n => !string.IsNullOrEmpty(n.FieldTypeAsString))
				.Each(n => _fieldConvertersResolver.Register(n.FieldTypeAsString, converterType));

			Register(converterType, creator);
		}

		private void Register(Type converterType, Func<IFieldConverter> converterBuilder)
		{
			_fieldConvertersBuilders.Register(converterType, converterBuilder);
		}

		#endregion

	}
}