using System;
using System.Collections.Generic;
using Untech.SharePoint.Common.Converters;

namespace Untech.SharePoint.Common.Configuration
{
	public abstract class BaseConfiguration
	{
		private readonly Queue<Action<MappingsContainer>> _mappingsRegistrators;
		private readonly Queue<Action<FieldConvertersContainer>> _converterRegistrators;

		protected BaseConfiguration()
		{
			_mappingsRegistrators = new Queue<Action<MappingsContainer>>();
			_converterRegistrators = new Queue<Action<FieldConvertersContainer>>();
		}

		public BaseConfiguration RegisterMapping(Action<MappingsContainer> action)
		{
			_mappingsRegistrators.Enqueue(action);
			return this;
		}

		public BaseConfiguration RegisterConverters(Action<FieldConvertersContainer> action)
		{
			_converterRegistrators.Enqueue(action);
			return this;
		}

		public void BuildConfig()
		{
			var mappingsContainer = new MappingsContainer();
			var fieldContainer = new FieldConvertersContainer();

			foreach (var action in _mappingsRegistrators)
			{
				action(mappingsContainer);
			}

			foreach (var action in _converterRegistrators)
			{
				action(fieldContainer);
			}
		}
	}
}