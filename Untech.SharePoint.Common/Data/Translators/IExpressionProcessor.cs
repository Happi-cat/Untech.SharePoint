using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators
{
	public interface IExpressionProcessor<out T>
	{
		T Process(Expression node);
	}
}