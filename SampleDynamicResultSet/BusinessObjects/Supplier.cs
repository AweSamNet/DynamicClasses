using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AweSamNet.Data.DynamicClasses;

namespace SampleDynamicResultSet.BusinessObjects
{
	public class Supplier : BusinessLogicBase
	{
		#region Properties
		//==========================================================

		#region Name

		private String _name;

		[DynamicProperty(ColumnName="sup_CompanyName", DatabaseType=SqlDbType.NVarChar)]
		public String Name
		{
			get 
			{
				if (!IsNameSet)
					throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

				return _name; 
			}
			set
			{
				#region Optional Additional Code Here

				_name = value;
				
				#endregion

				IsNameSet = true;
			}
		}
		public bool IsNameSet { get; set; }

		#endregion

		//==========================================================

		#region NumberOfEmployees

		private int? _numberOfEmployees;
		[DynamicProperty(ColumnName="sup_TotalEmployees", DatabaseType=SqlDbType.Int)]
		public int? NumberOfEmployees
		{
			get
			{
				if (!IsNumberOfEmployeesSet)
					throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

				return _numberOfEmployees;
			}
			set
			{
				#region Optional Additional Code Here

				_numberOfEmployees = value;

				#endregion

				IsNumberOfEmployeesSet = true;
			}
		}
		public bool IsNumberOfEmployeesSet { get; set; }
		
		#endregion

		//==========================================================

		public double notDataboundProperty { get; set; }



		#endregion

		#region Overrides

		public override string ToString()
		{
			//what to show on ToString().  May differ from class to class.
			return this.Name;
		}

		public override bool IsEmpty()
		{
			return !this.IsNameSet || String.IsNullOrWhiteSpace(this.Name);
		}

		public override void OnLoaded()
		{
		}
		#endregion

		//==========================================================

        #region Markup
        private decimal _Markup;

        [DynamicProperty(ColumnName = "sup_markup", DatabaseType = SqlDbType.Decimal)]
        public decimal Markup
        {
            get
            {
                if (!IsMarkupSet)
                    throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

                return _Markup;
            }
            set
            {
                #region Optional Additional Code Here

                _Markup = value;

                #endregion

                IsMarkupSet = true;
            }
        }
        public bool IsMarkupSet { get; set; }
        #endregion
        
	}
}
