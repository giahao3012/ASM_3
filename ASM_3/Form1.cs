using ProductLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_3
{
    public partial class frmMain : Form
    {
        ProductDB data = new ProductDB();
        DataTable dtProduct;
        public frmMain()
        {
            CenterToScreen();   
            InitializeComponent();
        }
        private void loadData()
        {
            dtProduct = data.getProducts();
            dtProduct.PrimaryKey = new DataColumn[] { dtProduct.Columns["ProductID"] };
            dtProduct.Columns.Add("SubTotal", typeof(double), "Quantity * UnitPrice");

            bsProducts.DataSource = dtProduct;

            txtProductID.DataBindings.Clear();
            txtProductName.DataBindings.Clear();
            txtPrice.DataBindings.Clear();
            txtQuantity.DataBindings.Clear();

            txtProductID.DataBindings.Add("Text", bsProducts, "ProductID");
            txtProductName.DataBindings.Add("Text", bsProducts, "ProductName");
            txtPrice.DataBindings.Add("Text", bsProducts, "UnitPrice");
            txtQuantity.DataBindings.Add("Text", bsProducts, "Quantity");

            dgvProductList.DataSource = bsProducts;

            bsProducts.Sort = "ProductID DESC";
            //bnproductlist.bindingsource = bsproducts;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = 1;
            string name = string.Empty;
            double price = 0;
            int proQuantity = 0;

                if (dtProduct.Rows.Count > 0)
                {
                    id = int.Parse(dtProduct.Compute("MAX(ProductID)", "").ToString()) + 1;
                }
                Product pro = new Product
                {
                    ProductID = id,
                    ProductName = name,
                    UnitPrice = price,
                    ProductQuantity = proQuantity
                };

                searchFrm productDetails = new searchFrm(true, pro);

                DialogResult r = productDetails.ShowDialog();
                if (r == DialogResult.OK)
                {
                    pro = productDetails.ProductAddorEdit;
                    dtProduct.Rows.Add(pro.ProductID, pro.ProductName, pro.UnitPrice, pro.ProductQuantity);
                }
            loadData();
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProductID.Text);
            string name = txtProductName.Text;
            double price = double.Parse(txtPrice.Text);
            int quantity = int.Parse(txtQuantity.Text);
            Product pro = new Product
            {
                ProductID = id,
                ProductName = name,
                UnitPrice = price,
                ProductQuantity = quantity
            };

            searchFrm productDetails = new searchFrm(false, pro);
            DialogResult r = productDetails.ShowDialog();
            if (r == DialogResult.OK)
            {
                DataRow row = dtProduct.Rows.Find(pro.ProductID);
                row["ProductName"] = pro.ProductName;
                row["UnitPrice"] = pro.UnitPrice;
                row["Quantity"] = pro.ProductQuantity;
            }
            loadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int id = 0;
                if (!int.TryParse(txtSearch.Text, out id))
                {
                    MessageBox.Show("ID must be in number type.");
                    return;
                }
                Product pro = data.findProductbyID(id);
                if (pro == null || string.IsNullOrEmpty(pro.ToString()))
                {
                    MessageBox.Show("Can not found product.");
                }
                else
                {
                    searchFrm productDetails = new searchFrm(false, pro);
                    DialogResult r = productDetails.ShowDialog();
                    if (r == DialogResult.OK)
                    {
                        DataRow row = dtProduct.Rows.Find(pro.ProductID);
                        row["ProductName"] = pro.ProductName;
                        row["UnitPrice"] = pro.UnitPrice;
                        row["Quantity"] = pro.ProductQuantity;
                    }
                }
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Can not found this product.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProductID.Text);
            if (data.deleteProduct(id))
            {
                DataRow row = dtProduct.Rows.Find(id);
                dtProduct.Rows.Remove(row);
                MessageBox.Show("Delete Successfull.");
                loadData();
            }
            else { MessageBox.Show("Delete Failed."); }
        }
    }
}
