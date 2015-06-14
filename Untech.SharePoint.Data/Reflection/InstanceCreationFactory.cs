using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Untech.SharePoint.Data.Reflection
{
	public abstract class InstanceCreationFactory<TObject, TAttribute> where TAttribute : Attribute
	{
		private readonly Dictionary<string, Func<TObject>> _cachedCreators = new Dictionary<string, Func<TObject>>();

		public void Initialize()
		{
			var attributeType = typeof (TAttribute);
			var objectType = typeof (TObject);

			var assembly = GetType().Assembly;

			var types = assembly.GetTypes()
				.Where(type => type.IsDefined(attributeType))
				.Where(type => objectType.IsAssignableFrom(type))
				.ToList();

			foreach (var type in types)
			{
				var creator = InstanceCreatorUtility.GetCreator<TObject>(type);

				var keys = type.GetCustomAttributes(attributeType)
					.Cast<TAttribute>()
					.Select(GetKey)
					.ToList();

				foreach (var key in keys)
				{
					_cachedCreators.Add(key, creator);
				}
			}
		}

		protected abstract string GetKey(TAttribute attribute);

		public TObject Create(string key)
		{
			return _cachedCreators[key]();
		}
	}
}