using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class ViewFieldsTranslator : ICamlTranslator
	{
		public XElement Translate(ISpModelContext modelContext, Expression node)
		{
			var internalNames = modelContext.GetSpFieldsInternalNames(modelContext.ElementType);

			return new XElement(Tags.ViewFields, internalNames.Select(GetFieldRef));
		}

		private static XElement GetFieldRef(string internalName)
		{
			return new XElement(Tags.FieldRef, new XAttribute(Tags.Name, internalName));
		}
	}
}