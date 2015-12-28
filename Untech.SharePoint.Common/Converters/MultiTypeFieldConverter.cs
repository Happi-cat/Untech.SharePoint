using System;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters
{
	/// <summary>
	/// Represents base field converter that supports multiple member types.
	/// </summary>
	[PublicAPI]
	public class MultiTypeFieldConverter : IFieldConverter
	{
		/// <summary>
		/// Gets model of the associated field.
		/// </summary>
		protected MetaField Field { get; private set; }

		/// <summary>
		/// Gets internal field converter that should be used with specified <see cref="MetaField.MemberType"/>.
		/// </summary>
		protected IFieldConverter Internal { get; set; }

		public virtual void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		public object FromSpValue(object value)
		{
			if (Internal == null)
			{
				throw new InvalidOperationException("This converter wasn't initialized completely");
			}
			return Internal.FromSpValue(value);
		}

		public object ToSpValue(object value)
		{
			if (Internal == null)
			{
				throw new InvalidOperationException("This converter wasn't initialized completely");
			}
			return Internal.ToSpValue(value);
		}

		public string ToCamlValue(object value)
		{
			if (Internal == null)
			{
				throw new InvalidOperationException("This converter wasn't initialized completely");
			}
			return Internal.ToCamlValue(value);
		}
	}
}