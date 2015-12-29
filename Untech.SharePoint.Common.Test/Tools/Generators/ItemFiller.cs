using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.Common.CodeAnnotations;
using Untech.SharePoint.Common.Utils.Reflection;

namespace Untech.SharePoint.Common.Test.Tools.Generators
{
	public class ItemFiller<T> : IValueFiller<T>
	{
		private readonly Dictionary<MemberInfo, IValueFillerPart> _memberFillers = new Dictionary<MemberInfo, IValueFillerPart>();

		private readonly Func<T> _constructor;

		public ItemFiller()
		{
			_constructor = InstanceCreationUtility.GetCreator<T>(typeof(T));
		}

		public ItemFiller(Func<T> constructor)
		{
			_constructor = constructor;
		}

		public ItemFiller<T> With<TProp>([NotNull] Expression<Func<T, TProp>> selector, [NotNull] IValueGenerator<TProp> valueGenerator, GeneratorBehaviour behaviour = GeneratorBehaviour.IfNull)
		{
			var member = ((MemberExpression) selector.Body).Member;
			_memberFillers[member] = new MemberGeneratorWrapper<TProp>(member, valueGenerator, behaviour);

			return this;
		}

		public ItemFiller<T> With<TProp>([NotNull] Expression<Func<T, TProp>> selector, [NotNull] IValueFiller<TProp> valueGenerator)
		{
			var member = ((MemberExpression)selector.Body).Member;
			_memberFillers[member] = new MemberValueFillerWrapper<TProp>(member, valueGenerator);

			return this;
		}


		public T Generate()
		{
			var item = _constructor();
			Fill(item);
			return item;
		}

		public void Fill(T item)
		{
			foreach (var filler in _memberFillers.Values)
			{
				filler.FillPart(item);
			}
		}

		#region [Nested Classes]

		private interface IValueFillerPart
		{
			void FillPart(T item);
		}

		private class MemberGeneratorWrapper<TProp> : IValueFillerPart
		{
			private readonly IValueGenerator<TProp> _valueGenerator;

			private readonly Action<T, TProp> _valueSetter;

			private readonly Func<T, TProp> _valueGetter;

			private readonly GeneratorBehaviour _behaviour;

			public MemberGeneratorWrapper(MemberInfo member, IValueGenerator<TProp> valueGenerator, GeneratorBehaviour behaviour)
			{
				_valueGenerator = valueGenerator;
				_valueSetter = MemberAccessUtility.CreateSetter<T, TProp>(member);
				_valueGetter = MemberAccessUtility.CreateGetter<T, TProp>(member);
				_behaviour = behaviour;
			}

			public void FillPart(T item)
			{
				var value = _valueGetter(item);
				if (_behaviour == GeneratorBehaviour.IfNull && value != null)
				{
					return;
				}
				_valueSetter(item, _valueGenerator.Generate());
			}
		}

		private class MemberValueFillerWrapper<TProp> : IValueFillerPart
		{
			private readonly IValueFiller<TProp> _valueValueFiller;

			private readonly Func<T, TProp> _valueGetter;

			private readonly Action<T, TProp> _valueSetter;

			public MemberValueFillerWrapper(MemberInfo member, IValueFiller<TProp> valueValueFiller)
			{
				_valueValueFiller = valueValueFiller;
				_valueGetter = MemberAccessUtility.CreateGetter<T, TProp>(member);
				_valueSetter = MemberAccessUtility.CreateSetter<T, TProp>(member);
			}

			public void FillPart(T item)
			{
				var value = _valueGetter(item);
				if (value != null)
				{
					_valueValueFiller.Fill(value);
				}
				else
				{
					_valueSetter(item, _valueValueFiller.Generate());
				}
			}
		}

		#endregion

	}
}