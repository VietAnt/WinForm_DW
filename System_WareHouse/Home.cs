using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace System_WareHouse
{
    public partial class Home : Form
    {

        public bool isExit = true;
        SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=TestSales_DW;Integrated Security=True");
        int key = 0;
      

        public Home()
        {
            InitializeComponent();
            GetAllProduct();
            GetAllCustomer();
            GetAllStore();
            GetAllSalePerson();
            GetAllProductSales();
            DisplayProduct();
            DisplayStoreID();
            DisplayCustomerID();
            DisplaySalesID();
            GetStoreSP();
            conn.Open();
            Clear();
        }


        public event EventHandler DangXuat;
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat(this, new EventArgs());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (isExit)
                Application.Exit();
        }

        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isExit)
                Application.Exit();
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isExit)
            {
                if (MessageBox.Show("Ban muon thoat chuong trinh", "Canh bao ", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void GetAllProduct()
        {
            try
            {
                conn.Open();
                string Query = "select * from DimProduct";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                ProductDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Clear()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtProductSales.Text = "";
            txtProductCost.Text = "";
        }

        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            if (txtProductID.Text == "")
            {
                MessageBox.Show("Please Add a Key");
                return;
            }
            if (txtProductName.Text == "")
            {
                MessageBox.Show("Please Add a Name");
                return;
            }
            if (txtProductCost.Text == "")
            {
                MessageBox.Show("Please Add a ProductCost");
                return;
            }
            if (txtProductSales.Text == "")
            {
                MessageBox.Show("Please Add a ProductSale");
                return;
            }
            else if (txtProductID.Text != "" && txtProductName.Text != "" && txtProductCost.Text != "" && txtProductSales.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into DimProduct (ProductAltKey,ProductName,ProductActualCost,ProductSalesCost) values (@PK,@PN,@PC,@PS)", conn);
                    cmd.Parameters.AddWithValue("@PK", txtProductID.Text);
                    cmd.Parameters.AddWithValue("@PN", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@PC", txtProductCost.Text);
                    cmd.Parameters.AddWithValue("@PS", txtProductSales.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product : " + txtProductID.Text + " Added");
                    conn.Close();
                    GetAllProduct();
                    Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            key = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells[0].Value.ToString());
            txtProductID.Text = ProductDGV.SelectedRows[0].Cells[1].Value.ToString();
            txtProductName.Text = ProductDGV.SelectedRows[0].Cells[2].Value.ToString();
            txtProductCost.Text = ProductDGV.SelectedRows[0].Cells[3].Value.ToString();
            txtProductSales.Text = ProductDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("You nedd select a Product");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from DimProduct where ProductKey = @PK", conn);
                    cmd.Parameters.AddWithValue("@PK", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Delete");
                    conn.Close();
                    GetAllProduct();
                    key = 0;
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnProductUpdate_Click(object sender, EventArgs e)
        {
            if (txtProductID.Text == "")
            {
                MessageBox.Show("Please Add a Key");
                return;
            }
            if (txtProductName.Text == "")
            {
                MessageBox.Show("Please Add a Name");
                return;
            }
            if (txtProductCost.Text == "")
            {
                MessageBox.Show("Please Add a ProductCost");
                return;
            }
            if (txtProductSales.Text == "")
            {
                MessageBox.Show("Please Add a ProductSale");
                return;
            }
            else if (txtProductID.Text != "" && txtProductName.Text != "" && txtProductCost.Text != "" && txtProductSales.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update DimProduct set "
                                                   + "ProductAltKey = @PA ,"
                                                   + "ProductName = @PN , "
                                                   + "ProductActualCost = @PC ,"
                                                   + "ProductSalesCost = @PS "
                                                   + " where ProductKey = @PKey", conn);
                    cmd.Parameters.AddWithValue("@PA", txtProductID.Text);
                    cmd.Parameters.AddWithValue("@PN", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@PC", txtProductCost.Text);
                    cmd.Parameters.AddWithValue("@PS", txtProductSales.Text);
                    cmd.Parameters.AddWithValue("@PKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Update");
                    conn.Close();
                    GetAllProduct();
                    key = 0;
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Customer: Khach Hang
        private void GetAllCustomer()
        {
            try
            {
                conn.Open();
                string Query = "select * from DimCustomer";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                CustomerDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void ClearCustomer()
        {
            txtCusID.Text = "";
            txtCusName.Text = "";
            cbCusGender.SelectedIndex = 0;

        }

        private void CustomerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            key = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
            txtCusID.Text = CustomerDGV.SelectedRows[0].Cells[1].Value.ToString();
            txtCusName.Text = CustomerDGV.SelectedRows[0].Cells[2].Value.ToString();
            cbCusGender.Text = CustomerDGV.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void btnCusAdd_Click(object sender, EventArgs e)
        {
            if (txtCusID.Text == "")
            {
                MessageBox.Show("Please Add a Cutomer");
                return;
            }
            if (txtCusName.Text == "")
            {
                MessageBox.Show("Please Add a CustomerName");
                return;
            }
            else if (txtCusID.Text != "" && txtCusName.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into DimCustomer (CustomerAltID,CustomerName,Gender) values (@CD,@CN,@GD)", conn);
                    cmd.Parameters.AddWithValue("@CD", txtCusID.Text);
                    cmd.Parameters.AddWithValue("@CN", txtCusName.Text);
                    cmd.Parameters.AddWithValue("@GD", cbCusGender.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer: " + txtCusID.Text + " Added");
                    conn.Close();
                    GetAllCustomer();
                    ClearCustomer();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnCusDelete_Click(object sender, EventArgs e)
        {

            if (key == 0)
            {
                MessageBox.Show("You nedd select a Customer");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from DimCustomer where CustomerID = @CID", conn);
                    cmd.Parameters.AddWithValue("@CID", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Delete");
                    conn.Close();
                    GetAllCustomer();
                    key = 0;
                    ClearCustomer();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnCusUpdate_Click(object sender, EventArgs e)
        {
            if (txtCusID.Text == "")
            {
                MessageBox.Show("Please Add a Key");
                return;
            }
            if (txtCusName.Text == "")
            {
                MessageBox.Show("Please Add a Name");
                return;
            }
            else if (txtCusID.Text != "" && txtCusName.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update DimCustomer set "
                                                   + "CustomerAltID = @CA ,"
                                                   + "CustomerName = @CN , "
                                                   + "Gender = @GD"
                                                   + " where CustomerID = @CID", conn);
                    cmd.Parameters.AddWithValue("@CA", txtCusID.Text);
                    cmd.Parameters.AddWithValue("@CN", txtCusName.Text);
                    cmd.Parameters.AddWithValue("@GD", cbCusGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@CID", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Update");
                    conn.Close();
                    GetAllCustomer();
                    key = 0;
                    ClearCustomer();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Store: Cua_Hang
        private void GetAllStore()
        {
            try
            {
                conn.Open();
                string Query = "select * from DimStores";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                StoreDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void StoreDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            key = Convert.ToInt32(StoreDGV.SelectedRows[0].Cells[0].Value.ToString());
            txtStoreID.Text = StoreDGV.SelectedRows[0].Cells[1].Value.ToString();
            txtStoreName.Text = StoreDGV.SelectedRows[0].Cells[2].Value.ToString();
            txtStoreLocation.Text = StoreDGV.SelectedRows[0].Cells[3].Value.ToString();
            txtStoreCity.Text = StoreDGV.SelectedRows[0].Cells[4].Value.ToString();
            txtStoreState.Text = StoreDGV.SelectedRows[0].Cells[5].Value.ToString();
            txtStoreCountry.Text = StoreDGV.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void btnStoreDelete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("You nedd select a Store");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from DimStores where StoreID = @SID", conn);
                    cmd.Parameters.AddWithValue("@SID", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Store Delete");
                    conn.Close();
                    GetAllStore();
                    key = 0;
                    ClearStore();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void ClearStore()
        {
            txtStoreID.Text = "";
            txtStoreName.Text = "";
            txtStoreCountry.Text = "";
            txtStoreLocation.Text = "";
            txtStoreCity.Text = "";
            txtStoreState.Text = "";

        }

        private void btnStroreAdd_Click(object sender, EventArgs e)
        {
            if (txtStoreID.Text == "")
            {
                MessageBox.Show("Please Add a Key");
                return;
            }
            if (txtStoreName.Text == "")
            {
                MessageBox.Show("Please Add a Name");
                return;
            }
            if (txtStoreLocation.Text == "")
            {
                MessageBox.Show("Please Add a Location");
                return;
            }
            if (txtStoreCity.Text == "")
            {
                MessageBox.Show("Please Add a City");
                return;
            }
            if (txtStoreState.Text == "")
            {
                MessageBox.Show("Please Add a State");
                return;
            }
            if (txtStoreCountry.Text == "")
            {
                MessageBox.Show("Please Add a Country");
                return;
            }
            else if (txtStoreID.Text != "" && txtStoreName.Text != "" && txtStoreLocation.Text != "" && txtStoreCity.Text != "" && txtStoreState.Text != "" && txtStoreCountry.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into DimStores (StoreAltID,StoreName,StoreLocation,City,State,Country) values (@SAD,@SN,@SL,@SC,@SS,@ST)", conn);
                    cmd.Parameters.AddWithValue("@SAD", txtStoreID.Text);
                    cmd.Parameters.AddWithValue("@SN", txtStoreName.Text);
                    cmd.Parameters.AddWithValue("@SL", txtStoreLocation.Text);
                    cmd.Parameters.AddWithValue("@SC", txtStoreCity.Text);
                    cmd.Parameters.AddWithValue("@SS", txtStoreState.Text);
                    cmd.Parameters.AddWithValue("@ST", txtStoreCountry.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Store : " + txtStoreID.Text + " Added");
                    conn.Close();
                    GetAllStore();
                    ClearStore();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnStoreUpdate_Click(object sender, EventArgs e)
        {
            if (txtStoreID.Text == "")
            {
                MessageBox.Show("Please Add a Key");
                return;
            }
            if (txtStoreName.Text == "")
            {
                MessageBox.Show("Please Add a Name");
                return;
            }
            if (txtStoreLocation.Text == "")
            {
                MessageBox.Show("Please Add a Location");
                return;
            }
            if (txtStoreCity.Text == "")
            {
                MessageBox.Show("Please Add a City");
                return;
            }
            if (txtStoreState.Text == "")
            {
                MessageBox.Show("Please Add a State");
                return;
            }
            if (txtStoreCountry.Text == "")
            {
                MessageBox.Show("Please Add a Country");
                return;
            }
            else if (txtStoreID.Text != "" && txtStoreName.Text != "" && txtStoreLocation.Text != "" && txtStoreCity.Text != "" && txtStoreState.Text != "" && txtStoreCountry.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update DimStores set "
                                                   + "StoreAltID = @SAID,"
                                                   + "StoreName = @SN,"
                                                   + "StoreLocation = @SL,"
                                                   + "City = @CT,"
                                                   + "State = @ST,"
                                                   + "Country = @SC"
                                                   + " where StoreID = @SID", conn);
                    cmd.Parameters.AddWithValue("@SAID", txtStoreID.Text);
                    cmd.Parameters.AddWithValue("@SN", txtStoreName.Text);
                    cmd.Parameters.AddWithValue("@SL", txtStoreLocation.Text);
                    cmd.Parameters.AddWithValue("@CT", txtStoreCity.Text);
                    cmd.Parameters.AddWithValue("@ST", txtStoreState.Text);
                    cmd.Parameters.AddWithValue("@SC", txtStoreCountry.Text);
                    cmd.Parameters.AddWithValue("@SID", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Store Update");
                    conn.Close();
                    GetAllStore();
                    key = 0;
                    ClearStore();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Person
        private void GetAllSalePerson()
        {
            try
            {
                conn.Open();
                string Query = "select * from DimSalesPerson";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                PersonDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnPersonAdd_Click(object sender, EventArgs e)
        {
            if (txtPersonID.Text == "")
            {
                MessageBox.Show("Please Add a SALEID");
                return;
            }
            if (txtPersonName.Text == "")
            {
                MessageBox.Show("Please Add a CustomerName");
                return;
            }
            if (txtPersonCity.Text == "")
            {
                MessageBox.Show("Please Add a Sale City");
                return;
            }
            if (txtPersonState.Text == "")
            {
                MessageBox.Show("Please Add a Sale State");
                return;
            }
            if (txtPersonCountry.Text == "")
            {
                MessageBox.Show("Please Add a Sale Country");
                return;
            }
            else if (txtPersonID.Text != "" && txtPersonName.Text != "" && txtPersonCity.Text != "" &&  txtPersonCountry.Text != "" && txtPersonState.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into DimSalesPerson (SalesPersonAltID,SalesPersonName,StoreID,City,State,Country) " +
                        "values (@SPID,@SPN,@SID,@SPC,@SPS,@SPT)", conn);
                    cmd.Parameters.AddWithValue("@SPID", txtPersonID.Text);
                    cmd.Parameters.AddWithValue("@SPN", txtPersonName.Text);
                    cmd.Parameters.AddWithValue("@SID", txtSPStore.Text);
                    cmd.Parameters.AddWithValue("@SPC", txtPersonCity.Text);
                    cmd.Parameters.AddWithValue("@SPS", txtPersonState.Text);
                    cmd.Parameters.AddWithValue("@SPT", txtPersonCountry.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("SalesPerson: " + txtPersonID.Text + " Added");
                    conn.Close();
                    GetAllSalePerson();
                    ClearSales();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private void ClearSales()
        {
            txtPersonID.Text = "";
            txtPersonName.Text = "";
            txtSPStore.Text = "";
            txtPersonCity.Text = "";
            txtPersonState.Text = "";
            txtPersonCountry.Text = "";

        }

        private void btnPersonDelete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("You nedd select a SalePerson");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from DimSalesPerson where SalesPersonID = @SID", conn);
                    cmd.Parameters.AddWithValue("@SID", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Store Delete");
                    conn.Close();
                    GetAllSalePerson();
                    key = 0;
                    ClearSales();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void PersonDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            key = Convert.ToInt32(PersonDGV.SelectedRows[0].Cells[0].Value.ToString());
            txtPersonID.Text = PersonDGV.SelectedRows[0].Cells[1].Value.ToString();
            txtPersonName.Text = PersonDGV.SelectedRows[0].Cells[2].Value.ToString();
            txtSPStore.Text = PersonDGV.SelectedRows[0].Cells[3].Value.ToString();
            txtPersonCity.Text = PersonDGV.SelectedRows[0].Cells[4].Value.ToString();
            txtPersonState.Text = PersonDGV.SelectedRows[0].Cells[5].Value.ToString();
            txtPersonCountry.Text = PersonDGV.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sale_DWDataSet3.DimSalesPerson' table. You can move, or remove it, as needed.
            this.dimSalesPersonTableAdapter.Fill(this.sale_DWDataSet3.DimSalesPerson);
            // TODO: This line of code loads data into the 'sale_DWDataSet2.DimProduct' table. You can move, or remove it, as needed.
            this.dimProductTableAdapter.Fill(this.sale_DWDataSet2.DimProduct);
            // TODO: This line of code loads data into the 'sale_DWDataSet1.DimCustomer' table. You can move, or remove it, as needed.
            this.dimCustomerTableAdapter.Fill(this.sale_DWDataSet1.DimCustomer);
            // TODO: This line of code loads data into the 'sale_DWDataSet.DimStores' table. You can move, or remove it, as needed.
            this.dimStoresTableAdapter.Fill(this.sale_DWDataSet.DimStores);
            //ProductGetx();
            // StoreGetx();

            //-----///
            DisplayProduct();


        }

        //ProductSales: 
        private void GetAllProductSales()
        {
            try
            {
                conn.Open();
                string Query = "select * from FactProductSales";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                ProductSalesDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void btnPSAdd_Click(object sender, EventArgs e)
        {
            if (txtPSNumber.Text == "")
            {
                MessageBox.Show("Please Add a Product Number");
                return;
            }
            if (cbPSDateKey.Text == "")
            {
                MessageBox.Show("Please Add a DateKey");
                return;
            }
            if (cbPSTimeKey.Text == "")
            {
                MessageBox.Show("Please Add a SALE Time");
                return;
            }
            if (cbPSAltkey.Text == "")
            {
                MessageBox.Show("Please Add a Time Key");
                return;
            }
            if (txtPSProductID.Text == "")
            {
                MessageBox.Show("Please Add a Product ID");
                return;
            }
            if (txtPSSaleID.Text == "")
            {
                MessageBox.Show("Please Add a Person");
                return;
            }
            if (txtPSQuantity.Text == "")
            {
                MessageBox.Show("Please Add a Sale Quantity");
                return;
            }
            if (txtPSActualCost.Text == "")
            {
                MessageBox.Show("Please Add a Sale Actual Cost");
                return;
            }
            if (txtPSTotalCost.Text == "")
            {
                MessageBox.Show("Please Add a Sale Total Cost");
                return;
            }
            if (txtPSCusID.Text == "")
            {
                MessageBox.Show("Please Add a Customer");
                return;
            }
            else if (txtPSNumber.Text != "" && cbPSDateKey.Text != "" && cbPSTimeKey.Text != "" && cbPSAltkey.Text != "" && txtPSStoreID.Text != "" && txtPSCusID.Text != "" && txtPSProductID.Text != "" && txtPSSaleID.Text != "" && txtPSQuantity.Text != "" && txtPSTotalCost.Text != "" && txtPSActualCost.Text != "")
            {
                float totalCost = Convert.ToSingle(txtPSQuantity.Text) * Convert.ToSingle(txtPSTotalCost.Text);
                float actualCost = Convert.ToSingle(txtPSQuantity.Text) * Convert.ToSingle(txtPSActualCost.Text);
                float deviation = totalCost - actualCost;

                
                try
                { 
                    conn.Open();
                    SqlCommand cmd = new SqlCommand
                    ("insert into FactProductSales (SalesInvoiceNumber,SalesDateKey,SalesTimeKey,SalesTimeAltKey," +
                      "StoreID,CustomerID, ProductID,SalesPersonID,Quantity,SalesTotalCost,ProductActualCost,Deviation) " +
                     "values (@PSIN,@PSDK,@PSTK,@PSAK,@PSSID,@PSCID,@PSPID,@PSPerID,@PSQ,@PSTC,@PSAC,@PSD)", conn);
                    cmd.Parameters.AddWithValue("@PSIN", txtPSNumber.Text);
                    cmd.Parameters.AddWithValue("@PSDK", cbPSDateKey.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PSTK", cbPSTimeKey.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PSAK", cbPSAltkey.SelectedItem.ToString());
                    //cmd.Parameters.AddWithValue("@PSSID", cbPSStoreID.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PSSID", txtPSStoreID.Text);
                    //cmd.Parameters.AddWithValue("@PSCID", cbPSCusID.SelectedIndex.ToString());
                    cmd.Parameters.AddWithValue("@PSCID", txtPSCusID.Text);
                    cmd.Parameters.AddWithValue("@PSPID", txtPSProductID.Text);
                    cmd.Parameters.AddWithValue("@PSPerID", txtPSSaleID.Text);
                    //cmd.Parameters.AddWithValue("@PSPerID",cbPSPersonID.SelectedIndex.ToString());
                    cmd.Parameters.AddWithValue("@PSQ", txtPSQuantity.Text);
                    cmd.Parameters.AddWithValue("@PSTC", totalCost.ToString());
                    cmd.Parameters.AddWithValue("@PSAC", actualCost.ToString());
                    cmd.Parameters.AddWithValue("@PSD",  deviation.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Sales: " + txtPSNumber.Text + " Added");
                    conn.Close();
                    GetAllProductSales();
                    ClearProductSale();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void ClearProductSale()
        {
            txtPSNumber.Text = "";
            cbPSDateKey.Text = "";
            cbPSTimeKey.Text = "";
            cbPSAltkey.Text = "";
            txtPSStoreID.Text = "";
            txtPSProductID.Text = "";
            txtPSCusID.Text = "";
            txtPSSaleID.Text = "";
            txtPSQuantity.Text = "";
            txtPSTotalCost.Text = "";
            txtPSActualCost.Text = "";
            //txtDeviation.Text = "";
        }

        private void btnPSDelete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("You nedd select a SalesPerson");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from FactProductSales where TransactionId = @PSTID", conn);
                    cmd.Parameters.AddWithValue("@PSTID", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Sale Delete");
                    conn.Close();
                    GetAllProductSales();
                    key = 0;
                    ClearProductSale();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void ProductSalesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            key = Convert.ToInt32(ProductSalesDGV.SelectedRows[0].Cells[0].Value.ToString());
            txtPSNumber.Text = ProductSalesDGV.SelectedRows[0].Cells[1].Value.ToString();
            cbPSDateKey.Text = ProductSalesDGV.SelectedRows[0].Cells[2].Value.ToString();
            cbPSTimeKey.Text = ProductSalesDGV.SelectedRows[0].Cells[3].Value.ToString();
            cbPSAltkey.Text = ProductSalesDGV.SelectedRows[0].Cells[4].Value.ToString();
            txtPSStoreID.Text = ProductSalesDGV.SelectedRows[0].Cells[5].Value.ToString();
            txtPSCusID.Text = ProductSalesDGV.SelectedRows[0].Cells[6].Value.ToString();
            txtPSProductID.Text = ProductSalesDGV.SelectedRows[0].Cells[7].Value.ToString();
            txtPSSaleID.Text = ProductSalesDGV.SelectedRows[0].Cells[8].Value.ToString();
            txtPSQuantity.Text = ProductSalesDGV.SelectedRows[0].Cells[9].Value.ToString();
            txtPSTotalCost.Text = ProductSalesDGV.SelectedRows[0].Cells[10].Value.ToString();
            txtPSActualCost.Text = ProductSalesDGV.SelectedRows[0].Cells[11].Value.ToString();
            //txtDeviation.Text = ProductSalesDGV.SelectedRows[0].Cells[12].Value.ToString();
        }

        //private void ProductGetx()
        //{
          //  try
           // {
             //   conn.Open();
               // SqlCommand cmd = new SqlCommand("select ProductKey from DimProduct", conn);
               // SqlDataReader rdr;
               //rdr = cmd.ExecuteReader();
               // DataTable dt = new DataTable();
               // dt.Columns.Add("ProductKey", typeof(int));
               //dt.Load(rdr);
               // cb.DisplayMember = "ProductKey";
               // cb.DataSource = dt;
              //conn.Close();
            //}
            //catch (Exception ex)
            //{
              //  MessageBox.Show("There's been a problem ==>" + ex.Message);
            //}
            //finally
            //{
            //    conn.Close();
           // }
        //}

       // private void StoreGetx()
        //{
          //  string mainconn = ConfigurationManager.ConnectionStrings["System_WareHouse.Properties.Settings.Sale_DWConnectionString"].ConnectionString;
          //  SqlConnection sqlconn = new SqlConnection(mainconn);
          //  string sqlquery = "select StoreID from DimStores";
           // SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
           // sqlconn.Open();
            //SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            //DataTable dt = new DataTable();
            //sda.Fill(dt);
            //cb.DisplayMember = "StoreID";
            //cb.DataSource = dt;
        //}



        private void cbPSProID_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void DisplayProduct()
        {
            try
            {
                conn.Open();
                string Query = "select ProductKey,ProductActualCost,ProductSalesCost  from DimProduct";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                PSProductDGV.DataSource = ds.Tables[0];
                conn.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }



        private void PSProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            txtPSActualCost.Text = PSProductDGV.SelectedRows[0].Cells[1].Value.ToString();
            txtPSTotalCost.Text = PSProductDGV.SelectedRows[0].Cells[2].Value.ToString();
            txtPSProductID.Text = PSProductDGV.SelectedRows[0].Cells[0].Value.ToString();
            if (txtPSActualCost.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(PSProductDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void cbPSProID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try
            //{
              //  conn.Open();
              //  SqlCommand cmd = new SqlCommand("select * from DimProduct where ProductKey = @PId", conn);
              //  cmd.Parameters.AddWithValue("@PId", cbPSCusID.SelectedValue);
              //  DataTable dt = new DataTable();
              //  SqlDataAdapter sda = new SqlDataAdapter(cmd);
              //  sda.Fill(dt);
              //  foreach (DataRow dr in dt.Rows)
              //  {
              //      txtPSTotalCost.Text = dr[3].ToString();
              //      txtPSActualCost.Text = dr[4].ToString();
               // }
               // conn.Close();
            //}
            //catch (Exception ex)
           // {
              //  MessageBox.Show("There's been a problem ==>" + ex.Message);
            //}
            //finally
            //{
             //   conn.Close();
           // }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void PSStoreID_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPSStoreID.Text = PSStoreDGV.SelectedRows[0].Cells[0].Value.ToString();
            if (txtPSStoreID.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(PSProductDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DisplayStoreID()
        {
            try
            {
                conn.Open();
                string Query = "select StoreID,StoreName  from DimStores";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                PSStoreDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void DisplayCustomerID()
        {
            try
            {
                conn.Open();
                string Query = "select CustomerID,CustomerName  from DimCustomer";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                PSCustomerDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void PSCustomerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPSCusID.Text = PSCustomerDGV.SelectedRows[0].Cells[0].Value.ToString();
            if (txtPSCusID.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(PSCustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }


        private void DisplaySalesID()
        {
            try
            {
                conn.Open();
                string Query = "select SalesPersonID,SalesPersonName  from  DimSalesPerson";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                PSSalesDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void PSSalesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPSSaleID.Text = PSSalesDGV.SelectedRows[0].Cells[0].Value.ToString();
            if (txtPSSaleID.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(PSSalesDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void txtPSStoreID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbPersonStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            //StoreGetx();
        }

        private void cbPersonStore_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //StoreGetx();
        }

        private void btnPersonUpdate_Click(object sender, EventArgs e)
        {

            if (txtPersonID.Text == "")
            {
                MessageBox.Show("Please Add a Key");
                return;
            }
            if (txtPersonName.Text == "")
            {
                MessageBox.Show("Please Add a Name");
                return;
            }
            if (txtPersonCity.Text == "")
            {
                MessageBox.Show("Please Add a ProductCost");
                return;
            }
            if (txtPersonState.Text == "")
            {
                MessageBox.Show("Please Add a ProductSale");
                return;
            }
            if (txtPersonCountry.Text == "")
            {
                MessageBox.Show("Please Add a ProductSale");
                return;
            }
            else if (txtPersonID.Text != "" && txtPersonName.Text != ""  && txtPersonCity.Text != "" && txtPersonState.Text != "" && txtPersonCountry.Text != "")
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update DimSalesPerson set "
                                                   + "SalesPersonAltID = @SPAID,"
                                                   + "SalesPersonName = @SPN, "
                                                   + "StoreID = @SSID,"
                                                   + "City = @SPC,"
                                                   + "State = @SPS,"
                                                   + "Country = @SPT"
                                                + "  where SalesPersonID = @SID", conn);
                    cmd.Parameters.AddWithValue("@SPAID ",txtPersonID.Text);
                    cmd.Parameters.AddWithValue("@SPN", txtPersonName.Text);
                    cmd.Parameters.AddWithValue("@SSID",txtSPStore.Text);
                    cmd.Parameters.AddWithValue("@SPC", txtPersonCity.Text);
                    cmd.Parameters.AddWithValue("@SPS", txtPersonState.Text);
                    cmd.Parameters.AddWithValue("@SPT", txtPersonCountry.Text);
                    cmd.Parameters.AddWithValue("@SID", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("SalesPerson Update");
                    conn.Close();
                    GetAllSalePerson(); 
                    key = 0;
                    ClearSales();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There's been a problem ==>" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

      
        private void txtPersonState_TextChanged(object sender, EventArgs e)
        {

        }

        private void SPStroreDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSPStore.Text = SPStroreDGV.SelectedRows[0].Cells[0].Value.ToString();
            if (txtSPStore.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(SPStroreDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        void GetStoreSP()
        {

            try
            {
                conn.Open();
                string Query = "select StoreID,StoreName,StoreLocation,City,Country from  DimStores";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                var ds = new DataSet();
                sda.Fill(ds);
                SPStroreDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There's been a problem ==>" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        //Refesh Form

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DisplayProduct();
            DisplayStoreID();
            DisplayCustomerID();
            DisplaySalesID();
            tabPage5.Update();
            tabPage5.Refresh();

        }

        private void btnLoadStrore_Click(object sender, EventArgs e)
        {
            GetStoreSP();
            tabPage4.Update();
            tabPage4.Refresh();
        }
    }
}

