using System;
using System.Collections.Generic;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Collections;
using Untech.SharePoint.Converters;
using Untech.SharePoint.Data;
using Untech.SharePoint.Extensions;
using Untech.SharePoint.Mappings;
using Untech.SharePoint.Utils;

namespace Untech.SharePoint.Configuration
{
	/// <summary>
	/// Represents class that can build <see cref="Config"/>.
	/// </summary>
	[PublicAPI]
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
		/// Adds or updates <see cref="MappingSource{TContext}"/> for <typeparamref name="TContext"/>
		/// </summary>
		/// <typeparam name="TContext">The type of the context to register.</typeparam>
		/// <param name="action">Action that will return new instance of <see cref="MappingSource{TContext}"/>.</param>
		/// <returns>Current <see cref="ConfigBuilder"/> instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
		[NotNull]
		public ConfigBuilder RegisterMappings<TContext>([NotNull]Func<Mappings.Mappings, MappingSource<TContext>> action)
			where TContext : ISpContext
		{
			Guard.CheckNotNull(nameof(action), action);

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
			Guard.CheckNotNull(nameof(action), action);

			_converterRegistrators.Enqueue(action);
			return this;
		}

		/// <summary>
		/// Returns new <see cref="Config"/> instance.
		/// </summary>
		/// <returns>New <see cref="Config"/> instance.</returns>
		[Pure]
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