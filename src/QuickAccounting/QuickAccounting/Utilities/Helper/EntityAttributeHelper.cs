using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickAccounting.Utilities.Helper
{
    public static class EntityAttributeHelper
    {
        /// <summary>
        /// Gets the value of a specific attribute from a property in a model.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <typeparam name="TAttribute">The attribute type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The attribute instance if found; otherwise, null.</returns>
        public static TAttribute? GetAttribute<TModel, TAttribute>(Expression<Func<TModel, object>> propertyExpression)
            where TAttribute : Attribute
        {
            // Unwrap the property from expression body
            MemberExpression? memberExpression = propertyExpression.Body as MemberExpression;

            // If body is UnaryExpression, extract the operand (MemberExpression)
            if (memberExpression == null && propertyExpression.Body is UnaryExpression unaryExpression)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            // If we still don't have a MemberExpression, return null
            if (memberExpression == null)
                return null;

            var property = memberExpression.Member;
            return property.GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Gets the display name or default name of a property using the DisplayAttribute.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The display name if specified; otherwise, the property name.</returns>
        public static string GetDisplayName<TModel>(Expression<Func<TModel, object>> propertyExpression)
        {
            // Try to get DisplayAttribute first
            var displayAttribute = GetAttribute<TModel, DisplayAttribute>(propertyExpression);

            if (displayAttribute != null)
            {
                // If Name is set, use it; otherwise, use the property name
                return string.IsNullOrEmpty(displayAttribute.Name) ? propertyExpression.GetMemberName() : displayAttribute.Name;
            }

            // Fall back to the property name if DisplayAttribute is not found
            return propertyExpression.GetMemberName();
        }

        /// <summary>
        /// Extension method to get the member name from an expression.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="expression">The property expression.</param>
        /// <returns>The name of the property.</returns>
        private static string GetMemberName<TModel>(this Expression<Func<TModel, object>> expression)
        {
            // Ensure the expression is a MemberExpression (which it should be for properties)
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            // In case it's a UnaryExpression, extract the operand (which should be a MemberExpression)
            if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression operand)
            {
                return operand.Member.Name;
            }

            // Return an empty string if the expression type isn't expected
            return string.Empty;
        }
    }
}
