using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Configuration;
using Untech.SharePoint.Common.Converters;
using Untech.SharePoint.Common.Data;
using Untech.SharePoint.Common.Mappings;
using Untech.SharePoint.Common.Mappings.Annotation;
using Untech.SharePoint.Common.MetaModels;

namespace Untech.SharePoint.Common.Test.Converters
{
	public abstract class BaseConverterTest
	{
		protected abstract IFieldConverter GetConverter();

		[PublicAPI]
		protected TestScenario Given<TField>()
		{
			var converter = GetConverter();

			converter.Initialize(GetField<TField>());

			return new TestScenario(converter);
		}

		private MetaField GetField<T>()
		{
			var metaCtx = new AnnotatedMappingSource<Context<T>>().GetMetaContext();
			return metaCtx.Lists["List"].ContentTypes[typeof(Entity<T>)].Fields["Field"];
		}

		#region [Nested Classes]

		[PublicAPI]
		public class TestScenario
		{
			private readonly IFieldConverter _fieldConverter;

			public TestScenario(IFieldConverter fieldConverter)
			{
				_fieldConverter = fieldConverter;
			}

			public TestScenario CanConvertFromSp(object original, object expected)
			{
				return CanConvertFromSp<object, object>(original, expected);
			}

			public TestScenario CanConvertFromSp<T1, T2>(T1 original, T2 expected)
			{
				var actual = _fieldConverter.FromSpValue(original);

				Assert.AreEqual(expected, actual);

				return this;
			}

			public TestScenario CanConvertFromSp<T1, T2>(T1 original, T2 expected, IEqualityComparer comparer)
			{
				var actual = _fieldConverter.FromSpValue(original);

				Assert.IsTrue(comparer.Equals(actual, expected));

				return this;
			}

			public TestScenario CanConvertToSp(object original, object expected)
			{
				return CanConvertToSp<object, object>(original, expected);
			}

			public TestScenario CanConvertToSp<T1, T2>(T1 original, T2 expected)
			{
				var actual = _fieldConverter.ToSpValue(original);

				Assert.AreEqual(expected, actual);

				return this;
			}

			public TestScenario CanConvertToSp<T1, T2>(T1 original, T2 expected, IEqualityComparer comparer)
			{
				var actual = _fieldConverter.ToSpValue(original);

				Assert.IsTrue(comparer.Equals(actual, expected));

				return this;
			}


			public TestScenario CanConvertToCaml(object original, string expected)
			{
				return CanConvertToCaml<object>(original, expected);
			}

			public TestScenario CanConvertToCaml<T1>(T1 original, string expected)
			{
				var actual = _fieldConverter.ToCamlValue(original);

				Assert.AreEqual(expected, actual);

				return this;
			}
		}

		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public class Context<T> : ISpContext
		{
			[SpList]
			public ISpList<Entity<T>> List { get; set; }

			public Config Config { get; private set; }
			public IMappingSource MappingSource { get; private set; }
			public MetaContext Model { get; private set; }
		}

		[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
		public class Entity<T>
		{
			[SpField]
			public T Field { get; set; }
		}

		#endregion

	}
}