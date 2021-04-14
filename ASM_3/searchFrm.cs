using ProductLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_3
{
    public partial class searchFrm : Form
    {
        private bool addOredit;
        public Product ProductAddorEdit { get; set; }
        public searchFrm(bool flag, Product p) : this()
        {
            CenterToScreen();
            addOredit = flag;
            ProductAddorEdit = p;
            InitData();
        }
        private void InitData()
        {
            txtProductID.Text = ProductAddorEdit.ProductID.ToString();
            txtProductName.Text = ProductAddorEdit.ProductName;
            txtPrice.Text = ProductAddorEdit.UnitPrice.ToString();
            txtQuantity.Text = ProductAddorEdit.ProductQuantity.ToString();
        }
        public searchFrm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool flag;
            ProductAddorEdit.ProductID = int.Parse(txtProductID.Text);
            ProductAddorEdit.ProductName = txtProductName.Text;
            if (txtProductName.Text == string.Empty)
            {
                MessageBox.Show("Name is not be empty.");
                return;
            }
            int quantity = 0;
            if (!int.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Quantity must be a number.");
                return;
            }
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must greater than 0.");
                return;
            }
            ProductAddorEdit.ProductQuantity = quantity;

            double price=0;
            if (!double.TryParse(txtPrice.Text,out price))
            {
                MessageBox.Show("Price must be a number.");
                return;
            }if(price <= 0)
            {
                MessageBox.Show("Price must greater than 0.");
                return;
            }
            ProductAddorEdit.UnitPrice = price;
            
            ProductDB data = new ProductDB();
            if (addOredit == true)
            {
                flag = data.addProduct(ProductAddorEdit);
            }
            else
            {
                flag = data.updateProduct(ProductAddorEdit);
            }
            if (flag == true)
            {
                MessageBox.Show("Save Succesfully.");
            }
            else { MessageBox.Show("Save Failed"); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
