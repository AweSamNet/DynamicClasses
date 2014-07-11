using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AweSamNet.Data.DynamicClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DynamicProperty : Attribute
    {
        private String _columnName;
        private SqlDbType _databaseType;
        private String _propertyErrorMessageField;

        /// <summary>
        /// The exact column name of the <see cref="DynamicResultSet"/> to bind to this property.
        /// </summary>
        public String ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        /// <summary>
        /// The database type to expect from the <see cref="DynamicResultSet"/>.
        /// </summary>
        public System.Data.SqlDbType DatabaseType
        {
            get { return _databaseType; }
            set { _databaseType = value; }
        }

    }
}
