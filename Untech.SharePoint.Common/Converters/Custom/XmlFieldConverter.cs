using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.MetaModels;
using Untech.SharePoint.Common.Utils;

namespace Untech.SharePoint.Common.Converters.Custom
{
	/// <summary>
	/// Represents field converter that can convert XML to object and vice versa.
	/// </summary>
	[PublicAPI]
	public sealed class XmlFieldConverter : IFieldConverter
	{
		private MetaField Field { get; set; }

		/// <summary>
		/// Initialzes current instance with the specified <see cref="MetaField"/>
		/// </summary>
		/// <param name="field"></param>
		public void Initialize(MetaField field)
		{
			Guard.CheckNotNull("field", field);

			Field = field;
		}

		/// <summary>
		/// Converts SP field value to <see cref="MetaField.MemberType"/>.
		/// </summary>
		/// <param name="value">SP value to convert.</param>
		/// <returns>Member value.</returns>
		public object FromSpValue(object value)
		{
			var stringValue = (string) value;
			if (string.IsNullOrEmpty(stringValue))
			{
				return null;
			}

			var serializer = new DataContractSerializer(Field.MemberType);
			
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringValue)))
			{
				return serializer.ReadObject(stream);
			}
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP field value.
		/// </summary>
		/// <param name="value">Member value to convert.</param>
		/// <returns>SP field value.</returns>
		public object ToSpValue(object value)
		{
			if (value == null)
			{
				return null;
			}

			var serializer = new DataContractSerializer(Field.MemberType);
			var sb = new StringBuilder();

			using (var textWriter = new StringWriter(sb))
			using (var xmlWriter = new XmlTextWriter(textWriter))
			{
				serializer.WriteObject(xmlWriter, value);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Converts <see cref="MetaField.Member"/> value to SP Caml value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Caml value.</returns>
		public string ToCamlValue(object value)
		{
			return (string) ToSpValue(value);
		}
	}
}
