using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AweSamNet.Data.DynamicClasses;


namespace SampleDynamicResultSet.BusinessObjects
{
    public class Product : BusinessLogicBase
    {
        #region Properties
        #region Name
        private String _Name;

        [DynamicProperty(ColumnName = "prod_name", DatabaseType = SqlDbType.NVarChar)]
        public String Name
        {
            get
            {
                if (!IsNameSet)
                    throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

                return _Name;
            }
            set
            {
                #region Optional Additional Code Here

                _Name = value;

                #endregion

                IsNameSet = true;
            }
        }
        public bool IsNameSet { get; protected set; }
        #endregion

        //==========================================================

        #region InStock
        private int? _InStock;

        [DynamicProperty(ColumnName = "prod_instock", DatabaseType = SqlDbType.Int)]
        public int? InStock
        {
            get
            {
                if (!IsInStockSet)
                    throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

                return _InStock;
            }
            set
            {
                #region Optional Additional Code Here

                _InStock = value;

                #endregion

                IsInStockSet = true;
            }
        }
        public bool IsInStockSet { get; set; }
        #endregion

        //==========================================================

        #region Price
        private decimal? _Price;

        [DynamicProperty(ColumnName = "prod_unitprice", DatabaseType = SqlDbType.Decimal)]
        public decimal? Price
        {
            get
            {
                if (!IsPriceSet)
                    throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

                return _Price;
            }
            set
            {
                #region Optional Additional Code Here

                _Price = value;

                #endregion

                IsPriceSet = true;
            }
        }
        public bool IsPriceSet { get; set; }
        #endregion

        //==========================================================

        #region Supplier
        private Supplier _Supplier;

		[DynamicClass(typeof(Supplier), "SupplierID")]
        public Supplier Supplier
        {
            get
            {
                if (!IsSupplierSet)
                    throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

                return _Supplier;
            }
            set
            {
                #region Optional Additional Code Here

                _Supplier = value;

                #endregion

                IsSupplierSet = true;
            }
        }
        public bool IsSupplierSet { get; set; }
        #endregion

		#region SupplierID
		private int? _SupplierID;

		[DynamicProperty(ColumnName = "prod_SupplierId", DatabaseType = SqlDbType.Int)]
		public int? SupplierID
		{
			get
			{
				if (!IsSupplierIDSet)
					throw new Exception(CST_PROPERTY_VALUE_NOT_RETURNED);

				return _SupplierID;
			}
			set
			{
				#region Optional Additional Code Here

				_SupplierID = value;

				#endregion

				IsSupplierIDSet = true;
			}
		}
		public bool IsSupplierIDSet { get; set; }
		#endregion


        #endregion

        #region Overrides
        public override string ToString()
        {
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
    }
}
