using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace AweSamNet.Data.DynamicClasses
{
	/// <summary>
	/// Manipulates data in a DataTable and returns a new List where T : <see cref="BusinessLogicBase"/>  
	/// </summary>
	[Serializable]
	public partial class DynamicResultSet
	{
        static int _maxLevelCounter = 3; //set this to limit the level of child objects
		#region Properties

		private DataTable _data = new DataTable();
		private DataTable Data
		{
			get 
			{
				if (_data == null)
					_data = new DataTable();
				return _data; 
			}
			set { _data = value; }
		}
		
		#endregion

        #region Contructors
        public DynamicResultSet(DataTable data)
		{
			this.Data = data;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Parses stored DataTable and returns a List of type T
        /// </summary>
        /// <typeparam name="T">Type of list to return.  
        /// This type must inherit <see cref="Utilities.DynamicClasses.BusinessLogicBase"/></typeparam>
        /// <returns>List of type T constructed from the stored DataTable.</returns>
        public List<T> GetList<T>() 
			where T : BusinessLogicBase, new()
		{
			List<T> list = new List<T>();
			if (this.Data == null)
				return list;

			foreach (DataRow row in this.Data.Rows)
			{
                T newObject = RowToType<T>(row);

                if(newObject != null)
				    list.Add(newObject);

			}

			return list;
		}

		static public T RowToType<T>(DataRow row) where T : BusinessLogicBase, new()
		{
			T nullObject = null;
			return RowToType<T>(row, nullObject);
		}

		static public T RowToType<T>(DataRow row, T existingObject) where T : BusinessLogicBase, new()
		{
			return RowToType<T>(row, 0, existingObject);
		}
		static private T RowToType<T>(DataRow row, int _currentLevelCursor, T returnObject) where T : BusinessLogicBase, new()
        {

            Type type = typeof(T);

            PropertyInfo[] properties = type.GetProperties();
			if(returnObject == null)
				returnObject = new T();
  
            foreach (PropertyInfo property in properties)
            {
                #region Do DynamicPropery's
                object[] attrs = null;
                try
                {
                    //get custom attributes to see if this property has a DynamicProperty associated
                    attrs = property.GetCustomAttributes(typeof(DynamicProperty), false);
                }
                catch (Exception ex)
                {
                    //do nothing.  if it fails just continue.
                }

                if (attrs != null && attrs.Length != 0)
                {
                    DynamicProperty attr = attrs[0] as DynamicProperty;
                    if (attr != null && row.Table!= null && row.Table.Columns.Contains(attr.ColumnName))
                    {
                        SetPropertyValue<T>(returnObject, row, property, attr);

						continue; //skip to the next property.  A property can't be both a SQLDBType and a dynamic class
                    }
                }
                #endregion

                //if the DynamicProperty failed or did not exist then check for DynamicClass Attribute
                #region Do DynamicClass'
                object[] typeAttrs = null;
                try
                {
                    //get custom attributes to see if this property has a DynamicProperty associated
                    typeAttrs = property.GetCustomAttributes(typeof(DynamicClass), false);
                }
                catch (Exception ex)
                {
                    continue;
                }

                if (typeAttrs != null && typeAttrs.Length != 0)
                {
                    DynamicClass attr = typeAttrs[0] as DynamicClass;
                    if (attr != null)
                    {
                        //to avoid possible endless loops, limit the level ob child objects.
                        if (_currentLevelCursor >= _maxLevelCounter)
                            continue;

                        _currentLevelCursor++;

                        MethodInfo genericMethod = typeof(DynamicResultSet).GetMethod("RowToType", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(attr.Type);




                        //var genericMethod = (from x in
                        //                         (from m in typeof(DynamicResultSet).GetMethods()
                        //                          where m.Name == "RowToType"
                        //                          select new { Method = m, Params = m.GetParameters(), Args = m.GetGenericArguments() })
                        //                     where x.Params.Length == 1 && x.Args.Length == 1 && x.Params[0].ParameterType == typeof(DataRow)
                        //                     select x.Method).First();

                        object newChildObject = genericMethod.Invoke(null, new object[] { row, _currentLevelCursor, null });
						if(newChildObject != null && !(newChildObject as BusinessLogicBase).IsEmpty())
							property.SetValue(returnObject, newChildObject, null);

                        _currentLevelCursor--;
                    }
                }
                #endregion

            }
			if (returnObject != null && !returnObject.IsEmpty())
			{
				returnObject.OnLoaded();
				return returnObject;
			}
			else
				return null;
        }

		private static void SetPropertyValue<T>(T newObject, DataRow row, PropertyInfo property, DynamicProperty attr) where T : BusinessLogicBase, new()
		{
			object value = row[attr.ColumnName];
            if (System.DBNull.Value.Equals(value))
            {
                value = null;
            }
            else
            {
                #region switch
                switch (attr.DatabaseType)
                {
                    case SqlDbType.BigInt:
                        value = Convert.ToInt64(value);
                        break;
                    case SqlDbType.Binary:
                    case SqlDbType.Image:
                    case SqlDbType.Timestamp:
                    case SqlDbType.VarBinary:
                        //May need to properly convert.  Testing required.
                        value = (byte[])value;
                        break;
                    case SqlDbType.Bit:
                        value = Convert.ToBoolean(value);
                        break;
                    case SqlDbType.Char:
                    case SqlDbType.NChar:
                    case SqlDbType.NText:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Text:
                    case SqlDbType.VarChar:
                    case SqlDbType.Xml:
                        value = value.ToString();

                        break;
                    case SqlDbType.DateTime:
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.Date:
                    case SqlDbType.Time:
                    case SqlDbType.DateTime2:
                        value = Convert.ToDateTime(value);
                        break;
                    case SqlDbType.Decimal:
                    case SqlDbType.Money:
                    case SqlDbType.SmallMoney:
                        value = Convert.ToDecimal(value);
                        break;
                    case SqlDbType.Float:
                        value = Convert.ToDouble(value);
                        break;
                    case SqlDbType.Int:
                        value = Convert.ToInt32(value);
                        break;
                    case SqlDbType.Real: value = Convert.ToSingle(value);
                        break;
                    case SqlDbType.UniqueIdentifier:
						Guid guid = new Guid(value.ToString());
						value = guid;

                        break;
                    case SqlDbType.SmallInt: value = Convert.ToInt16(value);
                        break;
                    case SqlDbType.TinyInt: value = Convert.ToByte(value);
                        break;
                    case SqlDbType.Variant:
                    case SqlDbType.Udt:
                        break;
                    case SqlDbType.Structured:
                        //May need to properly convert.  Testing required.
                        value = (DataTable)value;
                        break;
                    case SqlDbType.DateTimeOffset:
                        //May need to properly convert.  Testing required.
                        value = (DateTimeOffset?)value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("DatabaseType");
                }
                #endregion
            }
			property.SetValue(newObject, value, null) ;
        }

        #endregion
    }
}
