using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AweSamNet.Data.DynamicClasses
{
    /// <summary>
    /// This class allows for databinding of properties by the <see cref="DynamicResultSet"/>.  
    /// </summary>
    /// <usage>
    /// 1) All properties that are to be databound to a given column in a dataset must inherit the <see cref="DynamicProperty"/> using the overload constructor.
    ///     e.g. [DynamicProperty(ColumnName="sup_TotalEmployees", DatabaseType=SqlDbType.Int)]
    ///     - ColumnName: The exact column name of the <see cref="DynamicResultSet"/> to bind to this property.
    ///     - DatabaseType: The database type to expect from the <see cref="DynamicResultSet"/>.
    /// 
    /// 2) All dynamic properties must be nullable.  This is to account for database fields potentially being null.  To make a value type nullable use the ? operator.  e.g. int? nullableInt;
    /// 
    /// 3) When adding a dynamic property (A property with the <see cref="DynamicProperty"/> Attribute), 
    ///     you must also add a corresponding property of type bool to indicate whether or not this field has been set.
    ///     In the event that the data source did not return the specified column, the dynamic property will not be set.  In this case it will not be possible
    ///     to know if the field was empty or actually not populated.  Therefore adding a Is[corresponding property]Set property will clarify that the column was never returned.
    ///      
    /// 4) The Is[corresponding property]Set property must always be set to true at the end of the dynamic property's setter.
    /// 
    /// 5) The getter of a dynamic property must throw an exception with a default message CST_PROPERTY_VALUE_NOT_RETURNED in the event that the Is[corresponding property]Set is false.
    /// </usage>
    /// <example>
    /// <code>
    ///         private int? _numberOfEmployees;
    ///         [DynamicProperty(ColumnName="sup_TotalEmployees", DatabaseType=SqlDbType.Int)]
    ///         public int? NumberOfEmployees
    ///         {
    ///             get
    ///             {
    ///                 if (!IsNumberOfEmployeesSet)
    ///                     throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);
    ///
    ///                 return _numberOfEmployees;
    ///             }
    ///             set
    ///             {
    ///                 #region Optional Additional Code Here
    ///
    ///                 _numberOfEmployees = value;
    ///
    ///                 #endregion
    ///
    ///                 IsNumberOfEmployeesSet = true;
    ///             }
    ///         }
    ///         public bool IsNumberOfEmployeesSet { get; protected set; }
    ///
    /// </code>
    /// </example>
    [Serializable]
    public abstract partial class BusinessLogicBase
    {
        public const String CST_PROPERTY_VALUE_NOT_RETURNED = "The value for this property was not returned from the dataset.  \r\n Please check the stored procedure's output if this value should be included.";

        #region Comparers
        /// <summary>
        /// Provide a default ascending comparer for all BusinessLogicBase derived classes.  Comparisson is made against ToString() method.
        /// </summary>
        /// <param name="x">First parameter of BusinessLogicBase type</param>
        /// <param name="y">Second parameter of BusinessLogicBase type</param>
        /// <returns>Comparisson result</returns>
        public static int DefaultComparerAsc(BusinessLogicBase x, BusinessLogicBase y)
        {
            try
            {
                return x.ToString().CompareTo(y.ToString());
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Provide a default descending comparer for all BusinessLogicBase derived classes.  Comparisson is made against ToString() method.
        /// </summary>
        /// <param name="x">First parameter of BusinessLogicBase type</param>
        /// <param name="y">Second parameter of BusinessLogicBase type</param>
        /// <returns>Comparisson result</returns>
        public static int DefaultComparerDesc(BusinessLogicBase x, BusinessLogicBase y)
        {
            return y.ToString().CompareTo(x.ToString());
        }
        #endregion

        #region IEquatable<BusinessLogicBase> Members

        public new abstract String ToString();

        #endregion

        public abstract bool IsEmpty();

        /// <summary>
        /// Code to run once all properties are dynamically loaded.
        /// </summary>
        public abstract void OnLoaded();

        protected void Refresh<T>(T newObject) where T : BusinessLogicBase, new()
        {

        }

        /// <summary>
        /// Returns an array of DynamicProperty attributes of a given property.
        /// </summary>
        /// <param name="propertyName">Name of the property to pull attributes from.</param>
        /// <returns>An array of DynamicProperty attributes of a given property.</returns>
        /// <exception cref="PropertyNotFoundException: Thrown if propertyName does not exist." ></exception>
        public DynamicProperty[] GetDynamicPropertyAttributes(String propertyName)
        {
            return this.GetPropertyAttributes<DynamicProperty>(propertyName);
        }

        /// <summary>
        /// Returns an array of DynamicClass of a given property.
        /// </summary>
        /// <param name="propertyName">Name of the property to pull attributes from.</param>
        /// <returns>An array of DynamicClass of a given property.</returns>
        /// <exception cref="PropertyNotFoundException: Thrown if propertyName does not exist." ></exception>
        public DynamicClass[] GetDynamicClassAttributes(String propertyName)
        {
            return this.GetPropertyAttributes<DynamicClass>(propertyName);
        }
    }
}
