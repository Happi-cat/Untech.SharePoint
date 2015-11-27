using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Converters.Custom;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings.Annotation;

namespace Untech.SharePoint.Common.Test.Converters.Custom
{
	[TestClass]
	[SuppressMessage("ReSharper", "UnusedVariable")]
	public class JsonFieldConverterTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			var converter = GetFieldConverter<ConverterDataContext, ConverterDataEntity, string>(n => n.Test, m => m.String);

		}

		private IFieldConverter GetFieldConverter<TContext, TEntity, TProp>(Expression<Func<TContext, ISpList<TEntity>>> listAccessor, Expression<Func<TEntity, TProp>> fieldAccessor)
		{
			var contextMapping = new AnnotatedContextMapping<TContext>();
			var listTitle = contextMapping.GetListTitleFromContextMember(((MemberExpression)listAccessor.Body).Member);

			var context = contextMapping.GetMetaContext();

			var converter = new JsonFieldConverter();

			converter.Initialize(context
				.Lists[listTitle]
				.ContentTypes[typeof(TEntity)]
				.Fields[((MemberExpression)fieldAccessor.Body)
				.Member
				.Name]);

			return converter;
		}
	}
}