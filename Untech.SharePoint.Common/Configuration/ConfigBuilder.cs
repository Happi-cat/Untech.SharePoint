using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.Visitors;

namespace Untech.SharePoint.Common.Configuration
{
	public sealed class ConfigBuilder
	{
		private readonly Container<Type, Func<Mappings.Mappings, IMappingSource>> _mappingSourceBuilders;
		private readonly Queue<Action<FieldConverterContainer>> _converterRegistrators;

		public ConfigBuilder()
		{
			_mappingSourceBuilders = new Container<Type, Func<Mappings.Mappings, IMappingSource>>();
			_converterRegistrators = new Queue<Action<FieldConverterContainer>>();
		}

		public ConfigBuilder RegisterMappings<TContext>(Func<Mappings.Mappings, IMappingSource<TContext>> action)
			where TContext: ISpContext
		{
			_mappingSourceBuilders.Register(typeof(TContext), action);
			return this;
		}

		public ConfigBuilder RegisterConverters(Action<FieldConverterContainer> action)
		{
			_converterRegistrators.Enqueue(action);
			return this;
		}

		public Config BuildConfig()
		{
			var mappings = new MappingSourceContainer();
			var converters = new FieldConverterContainer();

			foreach (var pair in _mappingSourceBuilders)
			{
				var mappingSource = pair.Value(new Mappings.Mappings());

				mappings.Register(pair.Key, mappingSource);

				var customConverters = FieldConverterFinder.Find(mappingSource.GetMetaContext());

				customConverters.Each(n => converters.Add(n));
			}

			foreach (var action in _converterRegistrators)
			{
				action(converters);
			}

			return new Config
			{
				FieldConverters = converters,
				Mappings = mappings
			};
		}
	}
}