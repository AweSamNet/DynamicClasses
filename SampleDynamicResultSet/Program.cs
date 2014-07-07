using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SampleDynamicResultSet;
using AweSamNet.Data.DynamicClasses;
using System.Diagnostics;
using SampleDynamicResultSet.BusinessObjects;

namespace SampleDynamicResultSet
{
	class Program
	{
		static void Main(string[] args)
		{
			DataTable table = new DataTable("Suppliers");
            table.Columns.Add("sup_CompanyName");
            table.Columns.Add("sup_TotalEmployees");
            table.Columns.Add("sup_FieldNotUsed");

            table.Columns.Add("prod_name");
            table.Columns.Add("prod_instock");
            table.Columns.Add("prod_unitprice");


			DataRow row = table.NewRow();
			row["sup_CompanyName"] = "Mike Milk Man McIsaac the Dairy";
			row["sup_TotalEmployees"] = 9000;
			row["sup_FieldNotUsed"] = "This is some empty field.";
            row["prod_name"] = "2L 2% Lactose Free";
            row["prod_instock"] = 2879;
            row["prod_unitprice"] = 2.54;

			DataRow row2 = table.NewRow();
			row2["sup_CompanyName"] = "Another company";
			//row2["sup_TotalEmployees"] = new int?(1);
			row2["sup_FieldNotUsed"] = "It is not anything but a thing.";

			table.Rows.Add(row);
			table.Rows.Add(row2);


			DynamicResultSet allMyRecords = new DynamicResultSet(table);

            List<Supplier> allSuppliers = allMyRecords.GetList<Supplier>();

            foreach (Supplier supplier in allSuppliers)
			{
                Console.WriteLine("Company: {0}", supplier.Name);
                Console.WriteLine("Total Employees: {0}", supplier.NumberOfEmployees);
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to see what happens when you access a DynamicProperty that has not been set.");
            Console.Read();
            Console.WriteLine();

            foreach (Supplier supplier in allSuppliers)
            {
                DisplaySupplier(supplier);
            }

            Console.WriteLine("Now get all products from this DataTable.  Since only one row has product info, only one product will be returned.");
            
            Console.In.Read();
            Console.Read();

            List<Product> allProducts = allMyRecords.GetList<Product>();

            foreach (Product product in allProducts)
            {
                Console.WriteLine("Product: {0}", product.Name);
                Console.WriteLine("Price: {0:c}", product.Price);
                Console.WriteLine("In Stock: {0}", product.InStock);
            
                Console.WriteLine();

                DisplaySupplier(product.Supplier);

            }

            Console.In.Read();
            Console.Read();



		}

        private static void DisplaySupplier(Supplier supplier)
        {
            Console.WriteLine("Company: {0}", supplier.Name);
            Console.WriteLine("Total Employees: {0}", supplier.NumberOfEmployees);
            try
            {
                Console.WriteLine("Markup %: {0}", supplier.Markup);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Markup %: Exception: {0}", ex.Message);
            }

            Console.WriteLine();
        }
	}
}
