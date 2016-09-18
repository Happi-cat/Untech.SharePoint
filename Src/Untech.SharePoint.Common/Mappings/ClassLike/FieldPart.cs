using System;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.MetaModels.Providers;

namespace Untech.SharePoint.Common.Mappings.ClassLike
{
	/// <summary>
	/// Represents provider of <see cref="MetaField"/> that allows to configure field mapping in fluent way.
	/// </summary>
	[PublicAPI]
	public sealed class FieldPart : IMetaFieldProvider
	{
		private readonly MemberInfo _member;
		private string _internalName;
		private string _typeAsString;
		private Type _customConverterType;

		internal FieldPart(MemberInfo member)
		{
			_member = member;
		}

		/// <summary>
		/// Sets internal name of the current SP field. Default internal name is member name.
		/// </summary>
		/// <param name="internalName">The internal name of the current SP field.</param>
		/// <returns>Current instance.</returns>
		[NotNull]
		public FieldPart InternalName([CanBeNull]string internalName)
		{
			_internalName = internalName;
			return this;
		}

		/// <summary>
		/// Sets type of the current SP field.
		/// </summary>
		/// <param name="typeAsString">The type of the current SP field.</param>
		/// <returns>Current instance.</returns>
		[NotNull]
		public FieldPart TypeAsString([CanBeNull]string typeAsString)
		{
			_typeAsString = typeAsString;
			return this;
		}

		/// <summary>
		/// Sets custome converter <see cref="IFieldConverter"/> for the specified SP field.
		/// </summary>
		/// <typeparam name="TConverter">Type of SP field converter.</typeparam>
		/// <returns>Current instance.</returns>
		[NotNull]
		public FieldPart CustomConverter<TConverter>()
			where TConverter : IFieldConverter, new()
		{
			_customConverterType = typeof (TConverter);
			return this;
		}

		MetaField IMetaFieldProvider.GetMetaField(MetaContentType parent)
		{
			var internalName = string.IsNullOrEmpty(_internalName) ? _member.Name : _internalName;
			return new MetaField(parent, _member, internalName)
			{
				TypeAsString = _typeAsString,
				CustomConverterType = _customConverterType
			};
		}
	}
}