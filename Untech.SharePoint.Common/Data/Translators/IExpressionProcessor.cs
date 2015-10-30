using System.Linq.Expressions;

namespace Untech.SharePoint.Common.Data.Translators
{
	internal interface IExpressionProcessor<out T>
	{
		T Process(Expression node);
	}
}