using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AweSamNet.Data.DynamicClasses
{
    public class PropertyNotFoundException : Exception
    {
        const String PROPERTY_NOT_FOUND_MESSAGE = "Could not retrieve {2} attributes of the property {0} in class {1}.  Property {0} does not exist.";

        public PropertyNotFoundException(String propertyName, String className, String attributeName)
            : base(String.Format(PROPERTY_NOT_FOUND_MESSAGE, propertyName, attributeName, className))
        {
        }

        public PropertyNotFoundException(String propertyName, String className, String attributeName, Exception innerException)
            : base(String.Format(PROPERTY_NOT_FOUND_MESSAGE, propertyName, attributeName, className), innerException)
        {
        }

    }

    public static class ExtensionMethods
    /// <summary>
    /// Returns an array of DynamicClass of a given property.
    /// </summary>
    /// <param name="propertyName">Name of the property to pull attributes from.</param>
    /// <returns>An array of DynamicClass of a given property.</returns>
    {
        /// <summary>
        /// Returns an array of attributes of type T on a given property.
        /// </summary>
        /// <typeparam name="T">Type of attributes to return.  Must derive from System.Attribute.</typeparam>
        /// <param name="source">Object to attach extension method to.</param>
        /// <param name="propertyName">Name of the property to pull attributes from.</param>
        /// <returns>An array of attributes of type T on a given property.</returns>
        /// <exception cref="PropertyNotFoundException: Thrown if propertyName does not exist." ></exception>
        public static T[] GetPropertyAttributes<T>(this object source, String propertyName) where T : Attribute
        {
            T[] returnValue = new T[0];
            Type type = source.GetType();
            Type attributeType = typeof(T);
            PropertyInfo property = type.GetProperty(propertyName);

            if (property == null)
            {
                throw new PropertyNotFoundException(propertyName, type.Name, attributeType.Name);
            }

            object[] attrs = null;

            //get custom attributes to see if this property has a DynamicProperty associated
            attrs = property.GetCustomAttributes(attributeType, true);
            if (attrs.Length > 0)
            {
                returnValue = new T[attrs.Length];

                for (int i = 0; i < attrs.Length; i++)
                {
                    //cast to dynamic property and add to the array.
                    returnValue[i] = attrs[i] as T;
                }
            }

            return returnValue;
        }

    }
}
