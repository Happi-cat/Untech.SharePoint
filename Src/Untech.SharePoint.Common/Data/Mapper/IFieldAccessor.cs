namespace Untech.SharePoint.Common.Data.Mapper
{
	/// <summary>
	/// Defines methods to access field values.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IFieldAccessor<in T>
	{
		/// <summary>
		/// Gets a value indicating whether the value can be read by <see cref="GetValue"/> method.
		/// </summary>
		bool CanGetValue { get; }

		/// <summary>
		/// Gets a value indicating whether the value can be set by <see cref="SetValue"/> method.
		/// </summary>
		bool CanSetValue { get; }

		/// <summary>
		/// Gets the value from the specified instance.
		/// </summary>
		/// <param name="instance">Value source instance.</param>
		/// <returns>Value of the field.</returns>
		object GetValue(T instance);

		/// <summary>
		/// Sets the value for the specified instance.
		/// </summary>
		/// <param name="instance">Value destination instance.</param>
		/// <param name="value">Value to set.</param>
		void SetValue(T instance, object value);
	}
}