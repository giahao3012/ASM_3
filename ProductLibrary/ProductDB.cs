using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductLibrary
{
    public class ProductDB
    {
        string strConnection;
        public ProductDB()
        {
            getConnectionString();
        }
        public string getConnectionString()
        {
            strConnection = ConfigurationManager.ConnectionStrings["SaleDB"].ConnectionString;
            return strConnection;
        }

        public DataTable getProducts()
        {
            string SQL = "select * from Products";
            SqlConnection cnn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SQL, cnn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dtProduct = new DataTable();
            try
            {
                if (cnn.State == ConnectionState.Closed)
                { cnn.Open(); }
                adapter.Fill(dtProduct);
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cnn.Close();
            }
            return dtProduct;
        }
        public bool addProduct(Product p)
        {
            string SQL = "Insert Products values(@ID,@Name,@Price,@Quantity)";
            SqlConnection cnn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SQL, cnn);
            bool result;

            cmd.Parameters.AddWithValue("@ID", p.ProductID);
            cmd.Parameters.AddWithValue("@Name", p.ProductName);
            cmd.Parameters.AddWithValue("@Price", p.UnitPrice);
            cmd.Parameters.AddWithValue("@Quantity", p.ProductQuantity);
            try
            {
                if (cnn.State == ConnectionState.Closed)
                { cnn.Open(); }
                result = cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }
        public bool updateProduct(Product p)
        {
            string SQL = "Update Products set ProductName=@Name,UnitPrice=@Price,Quantity=@Quantity" +
                " where ProductID=@ID";
            SqlConnection cnn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SQL, cnn);
            bool result;

            cmd.Parameters.AddWithValue("@ID", p.ProductID);
            cmd.Parameters.AddWithValue("@Name", p.ProductName);
            cmd.Parameters.AddWithValue("@Price", p.UnitPrice);
            cmd.Parameters.AddWithValue("@Quantity", p.ProductQuantity);
            try
            {
                if (cnn.State == ConnectionState.Closed)
                { cnn.Open(); }
                result = cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }

        public bool deleteProduct(int ProductID)
        {
            string SQL = "Delete Products where ProductID=@ID";
            SqlConnection cnn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SQL, cnn);
            bool result;

            cmd.Parameters.AddWithValue("@ID", ProductID);
            try
            {
                if (cnn.State == ConnectionState.Closed)
                { cnn.Open(); }
                result = cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cnn.Close();
            }
            return result;
        }
        public Product findProductbyID(int ProductID)
        {
            string SQL = "Select * from Products where ProductID=@ID";
            SqlConnection cnn = new SqlConnection(strConnection);
            SqlCommand cmd = new SqlCommand(SQL, cnn);
            cmd.Parameters.AddWithValue("@ID",ProductID);
            Product p=new Product();
            try
            {
                if (cnn.State == ConnectionState.Closed)
                { cnn.Open(); }
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    int ID = int.Parse(reader["ProductID"].ToString());
                    string name = reader["ProductName"].ToString();
                    int quantity = int.Parse(reader["Quantity"].ToString());
                    double price = double.Parse(reader["UnitPrice"].ToString());
                    p.ProductID = ID;
                    p.ProductName = name;
                    p.ProductQuantity = quantity;
                    p.UnitPrice = price;

                    return p;
                }
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cnn.Close();
            }
            return null;
        }
    }
}
