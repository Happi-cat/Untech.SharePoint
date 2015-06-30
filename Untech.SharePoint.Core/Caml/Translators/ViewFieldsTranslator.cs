using System.Linq.Expressions;
using System.Xml.Linq;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal class ViewFieldsTranslator : ICamlTranslator
	{
		private XElement _root;

		public XElement Translate(ISpModelContext modelContext, Expression node)
		{
			return _root;
		}
	}
}