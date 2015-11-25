﻿using System;
using System.Linq;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Collections;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.Utils;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Converters
{
	/// <summary>
	/// Represents container of <see cref="IFieldConverter"/> types.
	/// </summary>
	public class FieldConverterContainer : IFieldConverterResolver
	{
		private readonly Container<string, Type> _fieldTypesMap = new Container<string, Type>();
		private readonly KeyedFactory<Type, IFieldConverter> _fieldConvertersBuilders = new KeyedFactory<Type, IFieldConverter>();

		/// <summary>
		/// Adds built-in field converters from the specified <see cref="Assembly"/>.
		/// Built-in field converter should inherit <see cref="IFieldConverter"/> and should be marked with <see cref="SpFieldConverterAttribute"/>.
		/// </summary>
		/// <param name="assembly">Assembly with built-in converters.</param>
		/// <exception cref="ArgumentNullException"><paramref name="assembly"/> is null.</exception>
		public void AddFromAssembly([NotNull]Assembly assembly)
		{
			Guard.CheckNotNull("assembly", assembly);

			assembly.GetTypes()
				.Where(n => n.IsDefined(typeof(SpFieldConverterAttribute)))
				.Where(n => typeof(IFieldConverter).IsAssignableFrom(n) && !n.IsAbstract)
				.Each(RegisterBuiltInConverter);
		}

		/// <summary>
		/// Adds <typeparamref name="TConverter"/>.
		/// </summary>
		/// <typeparam name="TConverter">Type of field converter to add.</typeparam>
		public void Add<TConverter>()
			where TConverter : IFieldConverter
		{
			var converterType = typeof (TConverter);

			Register(converterType, InstanceCreationUtility.GetCreator<IFieldConverter>(converterType));
		}

		/// <summary>
		/// Adds the specified <paramref name="converterType"/>.
		/// </summary>
		/// <param name="converterType">Type of the field converter to add.</param>
		public void Add(Type converterType)
		{
			Guard.CheckNotNull("converterType", converterType);

			Register(converterType, InstanceCreationUtility.GetCreator<IFieldConverter>(converterType));
		}

		/// <summary>
		/// Determines whether <paramref name="typeAsString"/> can be resolved by current resolver.
		/// </summary>
		/// <param name="typeAsString">SP field type as string.</param>
		/// <returns>true if can resovle the specified <paramref name="typeAsString"/>.</returns>
		public bool CanResolve([NotNull] string typeAsString)
		{
			Guard.CheckNotNull("typeAsString", typeAsString);

			return _fieldTypesMap.IsRegistered(typeAsString);
		}

		/// <summary>
		/// Resolves <paramref name="typeAsString"/> and returns new instance of the associated <see cref="IFieldConverter"/>.
		/// </summary>
		/// <param name="typeAsString">SP field type as string.</param>
		/// <returns>New instance of the <see cref="IFieldConverter"/> that matchs to the specified SP field type.</returns>
		[NotNull]
		public IFieldConverter Resolve([NotNull] string typeAsString)
		{
			Guard.CheckNotNull("typeAsString", typeAsString);

			return Resolve(_fieldTypesMap.Resolve(typeAsString));
		}

		/// <summary>
		/// Determines whether <paramref name="converterType"/> can be resolved by current resolver.
		/// </summary>
		/// <param name="converterType">SP field converter type to check.</param>
		/// <returns>true if can resovle the specified <paramref name="converterType"/>.</returns>
		public bool CanResolve([NotNull] Type converterType)
		{
			Guard.CheckNotNull("converterType", converterType);

			return _fieldConvertersBuilders.IsRegistered(converterType);
		}

		/// <summary>
		/// Resolves <paramref name="converterType"/> and returns new instance of the associated <see cref="IFieldConverter"/>.
		/// </summary>
		/// <param name="converterType">type of the field converter to instantiate.</param>
		/// <returns>New instance of the <see cref="IFieldConverter"/>.</returns>
		[NotNull]
		public IFieldConverter Resolve([NotNull] Type converterType)
		{
			Guard.CheckNotNull("converterType", converterType);

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