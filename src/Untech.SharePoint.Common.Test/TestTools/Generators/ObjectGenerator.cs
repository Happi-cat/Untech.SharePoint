using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Untech.SharePoint.CodeAnnotations;
using Untech.SharePoint.Utils.Reflection;

namespace Untech.SharePoint.TestTools.Generators
{
	public class ObjectGenerator<T> : IValueGenerator<T>
	{
		private readonly Dictionary<MemberInfo, IValueGeneratorPart> _memberFillers = new Dictionary<MemberInfo, IValueGeneratorPart>();

		private readonly Func<T> _constructor;

		public ObjectGenerator()
		{
			_constructor = InstanceCreationUtility.GetCreator<T>(typeof(T));
		}

		public ObjectGenerator(Func<T> constructor)
		{
			_constructor = constructor;
		}

		public ObjectGenerator<T> With<TProp>([NotNull] Expression<Func<T, TProp>> selector, [NotNull] IValueGenerator<TProp> valueGenerator, GeneratorBehaviour behaviour = GeneratorBehaviour.IfNull)
		{
			var member = ((MemberExpression)selector.Body).Member;
			if (_memberFillers.ContainsKey(member))
			{
				_memberFillers[member] = new MemberGeneratorWrapper<TProp>(member, valueGenerator, behaviour);
			}
			else
			{
				_memberFillers.Add(member, new MemberGeneratorWrapper<TProp>(member, valueGenerator, behaviour));
			}

			return this;
		}

		public T Generate()
		{
			var item = _constructor();
			foreach (var filler in _memberFillers.Values)
			{
				filler.GeneratePart(item);
			}
			return item;
		}

		#region [Nested Classes]

		private interface IValueGeneratorPart
		{
			void GeneratePart(T item);
		}

		private class MemberGeneratorWrapper<TProp> : IValueGeneratorPart
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

			public void GeneratePart(T item)
			{
				var value = _valueGetter(item);
				if (_behaviour == GeneratorBehaviour.IfNull && value != null)
				{
					return;
				}
				_valueSetter(item, _valueGenerator.Generate());
			}
		}

		#endregion
	}
}