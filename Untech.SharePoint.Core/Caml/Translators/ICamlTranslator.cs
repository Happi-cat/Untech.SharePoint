using System.Linq.Expressions;
using System.Xml.Linq;

namespace Untech.SharePoint.Core.Caml.Translators
{
	internal interface ICamlTranslator
	{
		XElement Translate(ISpModelContext modelContext, Expression node);
	}
}