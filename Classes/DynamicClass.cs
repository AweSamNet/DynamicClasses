using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AweSamNet.Data.DynamicClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DynamicClass : Attribute
    {
        /// <summary>
        /// Assigns the DynamicClass attribute to a property.
        /// </summary>
        /// <param name="type">The Type of the property.</param>
        /// <param name="idFieldName">The name of the field that contains the ID for this proeprty.</param>
        public DynamicClass(Type type, String idFieldName)
        {
            this.Type = type;
            this.IDFieldName = idFieldName;
        }

        /// <summary>
        /// They Type of this property.  This must be of base <see cref="Utilities.DynamicClasses.BusinessLogicBase"/>.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The ID field that defines this DynamicClass
        /// </summary>
        public String IDFieldName { get; set; }

    }
}
