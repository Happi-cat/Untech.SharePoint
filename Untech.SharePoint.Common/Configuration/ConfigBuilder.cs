using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Configuration
{
	/// <summary>
	/// Represents class that can build <see cref="Config"/>.
	/// </summary>
	public sealed class ConfigBuilder
	{
		private readonly KeyedFactory<Type, Mappings.Mappings, IMappingSource> _mappingSourceBuilders;
		private readonly Queue<Action<FieldConverterContainer>> _converterRegistrators;

		/// <summary>
		/// Initializes new instance of <see cref="ConfigBuilder"/>.
		/// </summary>
		public ConfigBuilder()
		{
			_mappingSourceBuilders = new KeyedFactory<Type, Mappings.Mappings, IMappingSource>();
			_converterRegistrators = new Queue<Action<FieldConverterContainer>>();
		}

		/// <summary>
		/// Adds or updates <see cref="IMappingSource{TContext}"/> for <typeparamref name="TContext"/>
		/// </summary>
		/// <typeparam name="TContext">The type of the context to register.</typeparam>
		/// <param name="action">Action that will return new instance of <see cref="IMappingSource{TContext}"/>.</param>
		/// <returns>Current <see cref="ConfigBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
		[NotNull]
		public ConfigBuilder RegisterMappings<TContext>([NotNull]Func<Mappings.Mappings, IMappingSource<TContext>> action)
			where TContext: ISpContext
		{
			Guard.CheckNotNull("action", action);

			_mappingSourceBuilders.Register(typeof(TContext), action);
			return this;
		}

		/// <summary>
		/// Adds <paramref name="action"/> that will register all required instances <see cref="IFieldConverter"/> in <see cref="FieldConverterContainer"/>.
		/// </summary>
		/// <param name="action">Action to enqueue.</param>
		/// <returns>Current <see cref="ConfigBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
		[NotNull]
		public ConfigBuilder RegisterConverters([NotNull]Action<FieldConverterContainer> action)
		{
			Guard.CheckNotNull("action", action);

			_converterRegistrators.Enqueue(action);
			return this;
		}

		/// <summary>
		/// Returns new <see cref="Config"/> instance.
		/// </summary>
		/// <returns>New <see cref="Config"/> instance.</returns>
		[NotNull]
		public Config BuildConfig()
		{
			var mappings = new MappingSourceContainer();
			var converters = new FieldConverterContainer();

			foreach (var pair in _mappingSourceBuilders)
			{
				var mappingSource = pair.Value(new Mappings.Mappings());

				mappings.Register(pair.Key, mappingSource);

				FieldConverterFinder.Find(mappingSource.GetMetaContext())
					.Each(converters.Add);
			}

			foreach (var action in _converterRegistrators)
			{
				action(converters);
			}

			return new Config(converters, mappings);
		}
	}
}