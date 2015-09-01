using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Untech.SharePoint.Client.Mapping
{
	public class ModelMapping
	{

	}

	public interface IModelMappingProvider
	{
		ModelMapping GetModelMapping();
	}

	public interface IPropertyMappingProvider
	{


	}

	public class PropertyPart : IPropertyMappingProvider
	{
		public PropertyPart Field(string internalName)
		{
			return this;
		}

		public PropertyPart FieldType(string typeAsString)
		{
			return this;
		}

		public PropertyPart Converter<IFieldConverter>()
		{
			return this;
		}

		public PropertyPart Converter(Type converterType)
		{
			return this;
		}


	}

	public class ModelMap<T> : IModelMappingProvider
	{

		public PropertyPart Map(Expression<Func<T, object>> memberExpression)
		{

		}

		public PropertyPart Map(Expression<Func<T, object>> memberExpression, string internalName)
		{

		}

		public ModelMapping GetModelMapping()
		{
			throw new NotImplementedException();
		}
	}

	public class ListMap<T>
	{



	}
}
