using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Mappings;

namespace Untech.SharePoint.Common.Configuration
{
	public sealed class ConfigBuilder
	{
		private readonly Queue<Action<MappingsContainer>> _mappingsRegistrators;
		private readonly Queue<Action<FieldConvertersContainer>> _converterRegistrators;

		public ConfigBuilder()
		{
			_mappingsRegistrators = new Queue<Action<MappingsContainer>>();
			_converterRegistrators = new Queue<Action<FieldConvertersContainer>>();
		}

		public ConfigBuilder RegisterMappings(Action<MappingsContainer> action)
		{
			_mappingsRegistrators.Enqueue(action);
			return this;
		}

		public ConfigBuilder RegisterConverters(Action<FieldConvertersContainer> action)
		{
			_converterRegistrators.Enqueue(action);
			return this;
		}

		public Config BuildConfig()
		{
			var mappingsContainer = new MappingsContainer();
			var fieldConvertersContainer = new FieldConvertersContainer();

			foreach (var action in _mappingsRegistrators)
			{
				action(mappingsContainer);
			}

			foreach (var action in _converterRegistrators)
			{
				action(fieldConvertersContainer);
			}

			return new Config
			{
				FieldConverters = fieldConvertersContainer,
				Mappings = mappingsContainer
			};
		}
	}
}