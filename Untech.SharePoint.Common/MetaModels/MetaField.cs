using System;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Extensions;
using Untech.SharePoint.Common.MetaModels.Visitors;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.MetaModels
{
	/// <summary>
	/// Represents MetaData for SP Field.
	/// </summary>
	[PublicAPI]
	public sealed class MetaField : BaseMetaModel
	{
		/// <summary>
		/// Initializes new instance of <see cref="MetaField"/>.
		/// </summary>
		/// <param name="contentType">Parent <see cref="MetaContentType"/>.</param>
		/// <param name="member">Field or property <see cref="MemberInfo"/>.</param>
		/// <param name="internalName">InternalName of the associated SP Field.</param>
		/// <exception cref="ArgumentNullException">if any argument is null.</exception>
		public MetaField([NotNull]MetaContentType contentType, [NotNull]MemberInfo member, [NotNull]string internalName)
		{
			Guard.CheckNotNull("contentType", contentType);
			Guard.CheckNotNull("member", member);
			Guard.CheckNotNullOrEmpty("internalName", internalName);

			ContentType = contentType;

			Member = member;
			MemberName = member.Name;
			MemberType = member.GetMemberType();

			InternalName = internalName;
		}

		/// <summary>
		/// Gets or sets SP Field Id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets SP Field DisplayName.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets SP Field internal name.
		/// </summary>
		[NotNull]
		public string InternalName { get; }

		/// <summary>
		/// Gets or sets SP Field Type.
		/// </summary>
		public string TypeAsString { get; set; }

		/// <summary>
		/// Gets or sets SP Field Output Type for calculated fields.
		/// </summary>
		public string OutputType { get; set; }

		/// <summary>
		/// Gets or sets whether this SP field is calculated.
		/// </summary>
		public bool IsCalculated { get; set; }

		/// <summary>
		/// Gets or sets whether this SP field is read-only.
		/// </summary>
		public bool ReadOnly { get; set; }

		/// <summary>
		/// Gets or sets whether this SP field is required.
		/// </summary>
		public bool Required { get; set; }

		/// <summary>
		/// Gets or sets whether this SP field is allow multiple values.
		/// </summary>
		public bool AllowMultipleValues { get; set; }

		/// <summary>
		/// Gets or sets List Title if the current field is a Lookup.
		/// </summary>
		[CanBeNull]
		public string LookupList { get; set; }

		/// <summary>
		/// Gets or sets List Field InternalName if the current field is a Lookup.
		/// </summary>
		[CanBeNull]
		public string LookupField { get; set; }

		/// <summary>
		/// Gets or sets converter associated with the current field.
		/// </summary>
		public IFieldConverter Converter { get; set; }

		/// <summary>
		/// Gets or sets converter type if custom field conversion is required.
		/// </summary>
		[CanBeNull]
		public Type CustomConverterType { get; set; }

		/// <summary>
		/// Gets associated <see cref="MemberInfo"/>.
		/// </summary>
		[NotNull]
		public MemberInfo Member { get; }

		/// <summary>
		/// Gets associated member name.
		/// </summary>
		[NotNull]
		public string MemberName { get; private set; }
		
		/// <summary>
		/// Gets associated member type.
		/// </summary>
		[NotNull]
		public Type MemberType { get; private set; }

		/// <summary>
		/// Gets parent <see cref="MetaContentType"/>.
		/// </summary>
		[NotNull]
		public MetaContentType ContentType { get; private set; }

		/// <summary>
		/// Accepts <see cref="IMetaModelVisitor"/> instance.
		/// </summary>
		/// <param name="visitor">Visitor to accept.</param>
		public override void Accept([NotNull]IMetaModelVisitor visitor)
		{
			visitor.VisitField(this);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash*37 + InternalName.GetHashCode();
				hash = hash*37 + MemberInfoComparer.Default.GetHashCode(Member);
				return hash;
			}
		}
	}
}