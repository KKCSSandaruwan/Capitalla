using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QuickAccounting.Utilities.Helper
{
    public static class EntityAttributeHelper
    {
        /// <summary>
        /// Gets a specific attribute of a property from a model.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute to retrieve.</typeparam>
        /// <param name="modelType">The type of the model containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The attribute instance if found; otherwise, null.</returns>
        public static TAttribute GetAttribute<TAttribute>(Type modelType, string propertyName)
            where TAttribute : Attribute
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            var property = modelType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return null; // Return null if the property is not found

            // Retrieve the custom attribute of type TAttribute from the property
            var attribute = property
                .GetCustomAttributes(typeof(TAttribute), false)
                .FirstOrDefault() as TAttribute;

            return attribute; // Return the attribute, or null if not found
        }

        /// <summary>
        /// Retrieves the display name of a property from a model using the DisplayAttribute.
        /// </summary>
        /// <param name="modelType">The type of the model containing the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The display name if available; otherwise, returns the property name.</returns>
        public static string GetDisplayName(Type modelType, string propertyName)
        {
            if (modelType == null) throw new ArgumentNullException(nameof(modelType));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            var property = modelType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            // If property is not found, return the property name
            if (property == null)
                return propertyName;

            // Try to get the DisplayAttribute
            var displayNameAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), false)
                                                 .FirstOrDefault() as DisplayAttribute;

            // Return Display Name if it's available, otherwise fallback to property name
            return displayNameAttribute?.Name ?? propertyName;
        }




        // Method to get the display name from a model property using reflection
        public static string GetDisplayName1<T>(string propertyName)
        {
            // Get the PropertyInfo object for the specified property
            var property = typeof(T).GetProperty(propertyName);

            if (property == null)
                return propertyName;  // Return property name if no property is found

            // Get the DisplayAttribute applied to the property
            var displayAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), false)
                                          .FirstOrDefault() as DisplayAttribute;

            // Return the DisplayName if found, otherwise fallback to the property name
            return displayAttribute?.Name ?? propertyName;
        }
    }
}
