using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ICSLicenseMaint
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region Depreciated RowDrop
        /*protected void RadGrid1_RowDrop(object sender, GridDragDropEventArgs e)
        {
            GridTableView ChildTable = e.DraggedItems[0].OwnerTableView;
            GridTableView ParentTable = e.DestDataItem.OwnerTableView;
            GridDataItem draggedItem = e.DraggedItems[0];
            GridDataItem parentItem = e.DestDataItem;

            string moduleID, oldLicenseID, newLicenseID, newSiteID, newCustomerID, licenseID, oldSiteID, oldCustomerID, siteID;

            if (ParentName(ChildTable.Name) == ParentTable.Name)
            {
                switch (ChildTable.Name)
                {
                    case "Modules":
                        moduleID = ChildTable.Items[draggedItem.ItemIndex]["ModuleID"].Text;
                        oldLicenseID = ChildTable.Items[draggedItem.ItemIndex]["LicenseID"].Text;
                        newLicenseID = ParentTable.Items[parentItem.ItemIndex]["LicenseID"].Text;
                        UpdateModule(moduleID, oldLicenseID, newLicenseID);
                        ParentTable.Rebind();
                        ChildTable.Rebind();
                        break;
                    case "Licenses":
                        newSiteID = ParentTable.Items[parentItem.ItemIndex]["SiteID"].Text;
                        newCustomerID = ParentTable.Items[parentItem.ItemIndex]["CustomerID"].Text;
                        licenseID = ChildTable.Items[draggedItem.ItemIndex]["LicenseID"].Text;
                        oldSiteID = ChildTable.Items[draggedItem.ItemIndex]["SiteID"].Text;
                        oldCustomerID = ChildTable.Items[draggedItem.ItemIndex]["CustomerID"].Text;
                        UpdateLicense(licenseID, oldSiteID, oldCustomerID, newSiteID, newCustomerID);
                        ParentTable.Rebind();
                        ChildTable.Rebind();
                        break;
                    case "Sites":
                        siteID = ChildTable.Items[draggedItem.ItemIndex]["SiteID"].Text;
                        oldCustomerID = ChildTable.Items[draggedItem.ItemIndex]["CustomerID"].Text;
                        newCustomerID = ParentTable.Items[parentItem.ItemIndex]["CustomerID"].Text;
                        UpdateSite(siteID, oldCustomerID, newCustomerID);
                        ParentTable.Rebind();
                        ChildTable.Rebind();
                        break;
                    default:
                        return;
                        break;
                }
            }
        }*/
        private string ParentName(string name)
        {
            switch (name)
            {
                case "Customers":
                    return null;
                    break;
                case "Sites":
                    return "Customers";
                    break;
                case "Licenses":
                    return "Sites";
                    break;
                case "Modules":
                    return "Licenses";
                    break;
                default:
                    return null;
                    break;
            }
        }
        #endregion

        #region SetUp Commands
        private bool isEqualOrParent(GridItem selectedItem, GridItem otherItem)
        {
            if (selectedItem.Equals(otherItem))
                return true;
            GridItem parentItem = selectedItem.OwnerTableView.ParentItem;
            if (parentItem == null)
                return false;
            if (parentItem.Equals(otherItem))
                return true;
            else
                return isEqualOrParent(parentItem,otherItem);
                
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            string command = e.CommandName;
            if (command == "RowClick")
            {
                foreach (GridItem item in RadGrid1.Items)
                {
                    if (isEqualOrParent(e.Item, item))
                    {
                        item.Expanded = true;
                    }
                    else
                    {
                        item.Expanded = false;
                    }
                }
            }

        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                #region on Insert
                if (e.Item.OwnerTableView.IsItemInserted &&  e.Item is GridEditFormInsertItem)
                {
                    GridEditFormInsertItem item = e.Item as GridEditFormInsertItem;
                    
                    switch (item.OwnerTableView.Name)
                    {
                        case "Customers":
                            TextBox CustomerIDBox = FindTb(item,"CustomerIDBox");
                            SetControlReadOnly(ref CustomerIDBox, false);
                            CustomerIDBox.Text = "";

                            TextBox CustomerNameBox = FindTb(item,"CustomerNameBox");
                            CustomerNameBox.Text = "";

                            break;
                        case "Sites":
                            //DropDownList Customers = FindDdl(item,"SitesCustomerIDBox");
                            RadComboBox Customers = FindComboBox(item, "SitesCustomerIDBox");
                            string thisCustomer = item.OwnerTableView.ParentItem.GetDataKeyValue("CustomerID").ToString();
                            string thisCustomerName = item.OwnerTableView.ParentItem["CustomerName"].Text;
                            Customers.Items.Add(new RadComboBoxItem(thisCustomerName,thisCustomer));
                            Customers.Items[0].Selected = true;
                            SetControlReadOnly(ref Customers, true);


                            TextBox SiteIDBox = FindTb(item,"SiteIDBox");
                            SetControlReadOnly(ref SiteIDBox, false);
                            SiteIDBox.Text = "";

                            break;
                        case "Licenses":
                            //since Licenses cannot be created here, no action is taken
                            break;
                        case "Modules":
                            

                            TextBox LicenseIDBox = FindTb(e.Item, "ModulesLicenseIDBox");
                            LicenseIDBox.Text = item.OwnerTableView.ParentItem.GetDataKeyValue("LicenseID").ToString();
                            SetControlReadOnly(ref LicenseIDBox, true);

                            #region Set Up ModuleID Dropdown
                            RadComboBox ModuleIDBox = FindComboBox(item,"ModuleIDBox");
                            DataTable modules = new DataTable("Modules");
                            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);

                            Conn.Open();
                            List<string> ExistingModules = new List<string>();
                            foreach (GridDataItem ModuleItem in item.OwnerTableView.ParentItem.ChildItem.NestedTableViews[0].Items)
                            {
                                ExistingModules.Add(ModuleItem["ModuleID"].Text);
                            }
                            string selectCommandText ="SELECT ModuleID,ModuleName FROM ProductModules";
                            if (ExistingModules.Count > 0)
                            {
                                selectCommandText = "SELECT ModuleID,ModuleName FROM ProductModules "
                                                         + "WHERE ModuleID NOT IN ('" + ExistingModules[0]+"'";


                                for (int i = 1; i < ExistingModules.Count; i++ )
                                {
                                    selectCommandText += ",'" + ExistingModules[i] + "'";
                                }
                                selectCommandText += ")";
                                
                            }
                            selectCommandText += " ORDER BY ModuleName";
                                                     

                            SqlDataAdapter ada = new SqlDataAdapter(selectCommandText, Conn);

                            int rows = ada.Fill(modules);
                            string ItemToAdd, ValueToAdd;
                            for (int i = 0; i < rows; i++)
                            {
                                ItemToAdd = modules.Rows[i].ItemArray[1].ToString();
                                ValueToAdd = modules.Rows[i].ItemArray[0].ToString();
                                ModuleIDBox.Items.Add(new RadComboBoxItem(ItemToAdd,ValueToAdd));
                            }
                            ModuleIDBox.SelectedIndex = 0;
                            #endregion

                            TextBox ModulesProductIDBox = FindTb(e.Item, "ModulesProductIDBox");
                            ModulesProductIDBox.Text = item.OwnerTableView.ParentItem["ProductID"].Text;
                            SetControlReadOnly(ref ModulesProductIDBox, true);

                            CheckBox ModulesTimeOutBox = item.FindControl("ModulesTimeOutBox") as CheckBox;
                            ModulesTimeOutBox.Checked = (item.OwnerTableView.ParentItem.FindControl("LicensesTimeOutCheckBox") as CheckBox).Checked;

                            break;

                    }



                }
                #endregion

                #region On Update
                else if (e.Item.DataItem != null)
                {
                    GridEditFormItem item = e.Item as GridEditFormItem;
                    SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);
                    SqlDataAdapter ada;
                    string selectCommandText;
                    int rows;

                    string ItemToAdd, ValueToAdd;
                    switch (e.Item.OwnerTableView.Name)
                    {
                        case "Sites":

                            #region set up the Customer ID Dropdown
                            //DropDownList CustomerIDBox = FindDdl(item,"SitesCustomerIDBox");
                            RadComboBox CustomerIDBox = FindComboBox(item,"SitesCustomerIDBox");
                            DataTable customers = new DataTable("Customers");
                            string thisCustomer = item["CustomerID"].Text;
                            Conn.Open();
                            selectCommandText = "SELECT CustomerID,CustomerName FROM Customers "
                                              + "WHERE CustomerID='" + thisCustomer + "' "
                                              + "ORDER BY CustomerName"
                                              ;

                            ada = new SqlDataAdapter(selectCommandText, Conn);

                            rows = ada.Fill(customers);
                            int selectedIndex = 0;
                            for (int i = 0; i < rows; i++)
                            {
                                ItemToAdd = customers.Rows[i].ItemArray[1].ToString();
                                ValueToAdd = customers.Rows[i].ItemArray[0].ToString();
                                if (ValueToAdd == thisCustomer)
                                {
                                    selectedIndex = i;
                                }
                                CustomerIDBox.Items.Add(new RadComboBoxItem(ItemToAdd, ValueToAdd));
                                
                            }
                            CustomerIDBox.Items[selectedIndex].Selected = true;
                            
                            //CustomerIDBox.SelectedIndex = CustomerIDBox.Items.IndexOf(new ListItem(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("CustomerID").ToString(),e.Item.OwnerTableView.ParentItem["CustomerName"].Text));


                            SetControlReadOnly(ref CustomerIDBox, true);
                            
                            Conn.Close();
                            Conn.Dispose();

                            #endregion

                            TextBox SiteIDBox = FindTb(item, "SiteIDBox");
                            SiteIDBox.Text = item["SiteID"].Text;
                            SetControlReadOnly(ref SiteIDBox, true);

                            TextBox SiteNameBox = FindTb(item, "SiteNameBox");
                            SiteNameBox.Text = item["SiteName"].Text;

                            TextBox SiteDescBox = FindTb(item, "SiteDescBox");
                            SiteDescBox.Text = item["SiteDescription"].Text;
                            break;
                        case "Licenses":
                            //Set up the Sites
                            #region Set Up Site ID Dropdown
                            RadComboBox LicensesSiteIDBox = FindComboBox(item, "LicensesSiteIDBox");
                            DataTable sites = new DataTable("Sites");
                            string thisSite = item.OwnerTableView.ParentItem["SiteID"].Text;
                            string thisSiteName = item.OwnerTableView.ParentItem["SiteName"].Text;
                            Conn.Open();
                            selectCommandText = "SELECT SiteID,SiteName FROM CustomerSites "
                                                     + "WHERE CustomerID='" + e.Item.OwnerTableView.ParentItem.GetDataKeyValue("CustomerID").ToString() + "'";

                            ada = new SqlDataAdapter(selectCommandText, Conn);

                            rows = ada.Fill(sites);
                            for (int i = 0; i < rows; i++)
                            {
                                ItemToAdd = sites.Rows[i].ItemArray[1].ToString();
                                ValueToAdd = sites.Rows[i].ItemArray[0].ToString();
                                LicensesSiteIDBox.Items.Add(new RadComboBoxItem(ItemToAdd,ValueToAdd));
                            }

                            LicensesSiteIDBox.SelectedIndex = LicensesSiteIDBox.Items.IndexOf(new RadComboBoxItem(thisSiteName, thisSite));

                            Conn.Close();
                            Conn.Dispose();
                            #endregion

                            TextBox MachineIDBox = FindTb(item, "LicensesMachineIDBox");
                            SetControlReadOnly(ref MachineIDBox, true);

                            TextBox InstallPathBox = FindTb(item, "LicensesInstallPathBox");
                            SetControlReadOnly(ref InstallPathBox, true);

                            RadComboBox ProductIDBox = FindComboBox(item, "LicensesProductIDBox");
                            SetControlReadOnly(ref ProductIDBox, true);
                            break;
                        case "Modules":

                            #region Set up Module ID Dropdown
                            RadComboBox ModuleIDBox = FindComboBox(item,"ModuleIDBox");
                            DataTable modules = new DataTable("Modules");

                            string thisID = item["ModuleID"].Text;
                            string thisModuleName = item["ModuleName"].Text;
                            //Conn.Open();

                            //selectCommandText = "SELECT ModuleName,ModuleID FROM ProductModules WHERE ModuleID='" + thisID + "'";

                            //ada = new SqlDataAdapter(selectCommandText, Conn);

                            //rows = ada.Fill(modules);
                            //for (int i = 0; i < rows; i++)
                            //{
                                
                                ModuleIDBox.Items.Add(new RadComboBoxItem(thisModuleName,thisID));
                            //}

                            ModuleIDBox.SelectedIndex = 0;
                            SetControlReadOnly(ref ModuleIDBox, true);
                            //Conn.Close();
                            #endregion

                            TextBox ModulesLicenseIDBox = FindTb(item, "ModulesLicenseIDBox");
                            ModulesLicenseIDBox.Text = item["LicenseID"].Text;
                            SetControlReadOnly(ref ModulesLicenseIDBox, true);

                            TextBox ModulesProductIDBox = FindTb(item, "ModulesProductIDBox");
                            ModulesProductIDBox.Text = item["ProductID"].Text;
                            SetControlReadOnly(ref ModulesProductIDBox, true);

                            TextBox ModulesUserCountBox = FindTb(item, "ModulesUserCountBox");
                            ModulesUserCountBox.Text = item["UserCount"].Text;

                            CheckBox ModulesTimeOutBox = item.FindControl("ModulesTimeOutBox") as CheckBox;
                            ModulesTimeOutBox.Checked = (item.ParentItem.DataItem as DataRowView).Row["TimeOut"].ToString().ToLower() == "true" ? true : false;
                            
                            TextBox ModulesDaysRemainingBox = FindTb(item, "ModulesDaysRemainingBox");
                            ModulesDaysRemainingBox.Text = item["DaysRemaining"].Text;
                            /*
                            TextBox ModulesDateIssuedBox = FindTb(item, "ModulesDateIssuedBox");
                            ModulesDateIssuedBox.Text = item["DateIssued"].Text;

                            TextBox ModulesLRUBox = FindTb(item, "ModulesLRUBox");
                            ModulesLRUBox.Text = item["LastRequestedUpdate"].Text;*/
                            
                            Conn.Dispose();
                            break;
                    }
                    
                }//end if
                #endregion
                (e.Item.FindControl("UpdateButton") as Button).Focus();
            }
        }

        #endregion

        #region Validation
        private bool UserHasPermissions()
        {
            try
            {
                string user = User.Identity.Name;
                string queryText = "SELECT COUNT(*) FROM Permissions WHERE Userid = '" + user + "' AND allowpermlicense = 'true'";
                SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);
                SqlCommand cmd = new SqlCommand(queryText, Conn);
                Conn.Open();
                int i = (int)cmd.ExecuteScalar();
                Conn.Close();

                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion
        
        #region Controls
        private DropDownList FindDdl(GridItem item, string ControlID)
        {
            return item.FindControl(ControlID) as DropDownList;
        }

        private TextBox FindTb(GridItem item, string ControlID)
        {
            return item.FindControl(ControlID) as TextBox;
        }

        private RadComboBox FindComboBox(GridItem item, string ControlID)
        {
            return item.FindControl(ControlID) as RadComboBox;
        }


        protected void LicensesCustomerIDBox_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            GridEditableItem editedItem = (o as RadComboBox).NamingContainer as GridEditableItem;

            RadComboBox SiteIDBox = FindComboBox(editedItem, "LicensesSiteIDBox");
            
            SiteIDBox.Items.Clear();
            SiteIDBox.ClearSelection();
            SiteIDBox.Text = "";
            DataTable sites = new DataTable("Sites");
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);

            Conn.Open();
            string selectCommandText = "SELECT SiteID,SiteName FROM CustomerSites "
                                     + "WHERE CustomerID='" + e.Value + "'";
            SqlDataAdapter ada = new SqlDataAdapter(selectCommandText, Conn);

            int rows = ada.Fill(sites);
            string ItemToAdd, ValueToAdd;
            for (int i = 0; i < rows; i++)
            {
                ItemToAdd = sites.Rows[i].ItemArray[1].ToString();
                ValueToAdd = sites.Rows[i].ItemArray[0].ToString();
                SiteIDBox.Items.Add(new RadComboBoxItem(ItemToAdd, ValueToAdd));
            }
            //SiteIDBox.DataBind();


            Conn.Close();
        }

        #region Depreciated
        /*
        protected void LicensesCustomerIDBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            GridEditableItem editedItem = (sender as DropDownList).NamingContainer as GridEditableItem;

            RadComboBox SiteIDBox = FindComboBox(editedItem, "LicensesSiteIDBox");
            RadComboBox CustomerIDBox = (sender as RadComboBox);
            SiteIDBox.Items.Clear();
            DataTable sites = new DataTable("Sites");
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);

            Conn.Open();
            string selectCommandText = "SELECT SiteID,SiteName FROM CustomerSites "
                                     + "WHERE CustomerID='" + CustomerIDBox.SelectedValue + "'";
            SqlDataAdapter ada = new SqlDataAdapter(selectCommandText, Conn);

            int rows = ada.Fill(sites);
            string ItemToAdd, ValueToAdd;
            for (int i = 0; i < rows; i++)
            {
                ItemToAdd = sites.Rows[i].ItemArray[1].ToString();
                ValueToAdd = sites.Rows[i].ItemArray[0].ToString();
                SiteIDBox.Items.Add(new RadComboBoxItem(ItemToAdd,ValueToAdd));
            }
            //SiteIDBox.DataBind();


            Conn.Close();

        }
        */
        #endregion

        protected void LicensesTimeOutBox_CheckedChanged(object sender, EventArgs e)
        {
            GridEditableItem editedItem = (sender as CheckBox).NamingContainer as GridEditableItem;
            bool oldTimeOut = (editedItem["TimeOut"].FindControl("LicensesTimeOutCheckBox") as CheckBox).Checked;
            if (oldTimeOut == true && UserHasPermissions() == false)
            {
                (sender as CheckBox).Checked = true;
                (sender as CheckBox).Text = "You do not have permission to change this field";
            }
        }

        protected void ModulesTimeOutBox_CheckedChanged(object sender, EventArgs e)
        {
            
            GridEditableItem editedItem = (sender as CheckBox).NamingContainer as GridEditableItem;
            bool oldTimeOut;
            if (editedItem is GridEditFormInsertItem)
            {
                GridEditFormInsertItem insertedItem = editedItem as GridEditFormInsertItem;
                oldTimeOut = (insertedItem.OwnerTableView.ParentItem["TimeOut"].FindControl("LicensesTimeOutCheckBox") as CheckBox).Checked;
            }
            else
            {
                oldTimeOut = (editedItem["TimeOut"].FindControl("ModulesTimeOutCheckBox") as CheckBox).Checked;
                
            }
            if (oldTimeOut == true && UserHasPermissions() == false)
            {
                (sender as CheckBox).Checked = true;
                (sender as CheckBox).Text = "You do not have permission to change this field";
            }
        }

        private void SetControlReadOnly(ref TextBox ControlToSet, bool reset)
        {
            if (reset == true)
            {
                ControlToSet.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Info);
                ControlToSet.ReadOnly = true;
            }
            else
            {
                ControlToSet.ReadOnly = false;
                ControlToSet.BackColor = System.Drawing.Color.White;
            }
        }

        private void SetControlReadOnly(ref DropDownList ControlToSet, bool reset)
        {
            if (reset == true)
            {
                ControlToSet.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Info);
                ControlToSet.Enabled = false;
            }
            else
            {
                ControlToSet.BackColor = System.Drawing.Color.White;
                ControlToSet.Enabled = false;
            }
        }

        private void SetControlReadOnly(ref RadComboBox ControlToSet, bool reset)
        {
            if (reset == true)
            {
                ControlToSet.Enabled = false;
            }
            else
            {
                ControlToSet.Enabled = true;
            }
        }
        #endregion

        #region Inserts
        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;

            string CustomerID, CustomerName, SiteID, SiteName, SiteDescription, LicenseID, ProductID, MachineID,
                InstallPath, MachineName, TotalUserCount, TimeOut, DaysRemaining, DateIssued, LastRequestedUpdate,
                ModuleID, UserCount;
            switch (item.OwnerTableView.Name)
            {
                case "Customers":
                    CustomerID = FindTb(item, "CustomerIDBox").Text;
                    CustomerName = FindTb(item, "CustomerNameBox").Text;
                    InsertCustomer(CustomerID, CustomerName);

                    break;
                case "Sites":
                    CustomerID = FindComboBox(item, "SitesCustomerIDBox").SelectedValue;
                    SiteID = FindTb(item, "SiteIDBox").Text;
                    SiteName = FindTb(item, "SiteNameBox").Text;
                    SiteDescription = FindTb(item, "SiteDescBox").Text;
                    InsertSite(CustomerID, SiteID, SiteName, SiteDescription);
                    break;
                case "Licenses":
                    //Licenses cant be inserted here, so no action is taken.
                    break;
                case "Modules":
                    LicenseID = FindTb(item, "ModulesLicenseIDBox").Text;
                    ModuleID = FindComboBox(item, "ModuleIDBox").SelectedValue.ToString();
                    ProductID = FindTb(item, "ModulesProductIDBox").Text;
                    UserCount = FindTb(item, "ModulesUserCountBox").Text;
                    TimeOut = (item.FindControl("ModulesTimeOutBox") as CheckBox).Checked ? "1" : "0";
                    DaysRemaining = FindTb(item, "ModulesDaysRemainingBox").Text;
                    /*DateIssued = FindTb(item, "ModulesDateIssuedBox").Text;
                    LastRequestedUpdate = FindTb(item, "ModulesLRUBox").Text;*/
                    if(string.IsNullOrEmpty(UserCount))
                        UserCount = "0";
                    if (string.IsNullOrEmpty(DaysRemaining))
                        DaysRemaining = "0";

                    InsertModule(LicenseID, ModuleID, ProductID, UserCount, TimeOut, DaysRemaining);
                    break;
            }
        }

        private void InsertCustomer(string CustomerID, string CustomerName)
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);
            string insertCommandText = "INSERT INTO Customers (CustomerID, CustomerName) "
                                     + "VALUES ('" + CustomerID + "','" + CustomerName + "')"
                                     ;
            SqlCommand cmd = new SqlCommand(insertCommandText, Conn);

            Conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        private void InsertSite(string CustomerID, string SiteID, string SiteName, string SiteDescription)
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);
            string insertCommandText = "INSERT INTO CustomerSites (CustomerID,SiteID,SiteName,SiteDescription) "
                                     + "VALUES ('" + CustomerID + "','" + SiteID + "','" + SiteName + "','" + SiteDescription + "')"
                                     ;
            SqlCommand cmd = new SqlCommand(insertCommandText, Conn);

            Conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        private void InsertModule(string LicenseID, string ModuleID, string ProductID, string UserCount,
            string TimeOut, string DaysRemaining)
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);
            string insertCommandText = "INSERT INTO LicensedModules "
                                     + "(LicenseID, ModuleID, ProductID, UserCount, TimeOut, DaysRemaining) "
                                     + "VALUES (" + LicenseID + ","
                                     + "'" + ModuleID + "',"
                                     + "'" + ProductID + "',"
                                     + UserCount + ","
                                     + TimeOut + ","
                                     + DaysRemaining
                                     /*+ "'" + DateIssued + "',"
                                     + "'" + LastRequestedUpdate */+ ")"
                                     ;
                                     
            SqlCommand cmd = new SqlCommand(insertCommandText, Conn);

            Conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }
        #endregion

        #region Updates
        protected void RadGrid1_UpdateCommand(object source, GridCommandEventArgs e)
        {
            GridItem item = e.Item as GridItem;
            bool istrue = item is GridEditableItem;
            string CustomerID, OldCustID, LicenseID, ModuleID, OldLicenseID, SiteID, OldSiteID;
            string CustomerName, SiteName, SiteDesc, ProductID, MachineID, InstallPath, MachineName, TotalUserCount,
                TimeOut, DaysRemaining, DateIssued, LastRequestedUpdate, UserCount;
            switch (item.OwnerTableView.Name)
            {
                case "Customers":
                    CustomerID = FindTb(item, "CustomerIDBox").Text;
                    CustomerName = FindTb(item, "CustomerNameBox").Text;
                    UpdateCustomer(CustomerID, CustomerName);
                    break;
                case "Sites":
                    CustomerID = FindComboBox(item, "SitesCustomerIDBox").SelectedValue;
                    OldCustID = item.OwnerTableView.Items[item.ItemIndex]["CustomerID"].Text;
                    SiteName = (item.FindControl("SiteNameBox") as TextBox).Text;
                    SiteDesc = (item.FindControl("SiteDescBox") as TextBox).Text;
                    SiteID = item.OwnerTableView.Items[item.ItemIndex]["SiteID"].Text;
                    UpdateSite(SiteID, OldCustID, CustomerID, SiteName, SiteDesc);
                    break;
                case "Licenses":
                    LicenseID = (item.FindControl("LicenseIDBox") as TextBox).Text;
                    OldCustID = item.OwnerTableView.ParentItem["CustomerID"].Text;
                    CustomerID = FindComboBox(item, "LicensesCustomerIDBox").SelectedValue;
                    OldSiteID = item.OwnerTableView.ParentItem["SiteID"].Text;
                    if (FindComboBox(item, "LicensesSiteIDBox").SelectedItem == null)
                        return;
                    SiteID = FindComboBox(item, "LicensesSiteIDBox").SelectedValue;
                    ProductID = FindComboBox(item, "LicensesProductIDBox").SelectedValue;
                    MachineID = FindTb(item, "LicensesMachineIDBox").Text;
                    InstallPath = FindTb(item, "LicensesInstallPathBox").Text;
                    MachineName = FindTb(item, "LicensesMachineNameBox").Text;
                    TotalUserCount = FindTb(item, "LicensesTotalUserCountBox").Text;
                    TimeOut = (item.FindControl("LicensesTimeOutBox") as CheckBox).Checked ? "1" : "0";
                    DaysRemaining = FindTb(item, "LicensesDaysRemainingBox").Text;
                    /*DateIssued = FindTb(item, "LicensesDateIssuedBox").Text;
                    LastRequestedUpdate = FindTb(item, "LicensesLRUBox").Text;*/
                    UpdateLicense(LicenseID, OldCustID, CustomerID, OldSiteID, SiteID, ProductID, MachineID, InstallPath, MachineName, TotalUserCount, TimeOut, DaysRemaining);

                    break;
                case "Modules":
                    LicenseID = (item.FindControl("ModulesLicenseIDBox") as TextBox).Text;
                    OldLicenseID = item.OwnerTableView.ParentItem["LicenseID"].Text;
                    ModuleID = FindComboBox(item, "ModuleIDBox").SelectedValue;
                    ProductID = FindTb(item, "ModulesProductIDBox").Text;
                    UserCount = FindTb(item, "ModulesUserCountBox").Text;
                    DaysRemaining = FindTb(item, "ModulesDaysRemainingBox").Text;
                    /*DateIssued = FindTb(item, "ModulesDateIssuedBox").Text;
                    LastRequestedUpdate = FindTb(item, "ModulesLRUBox").Text;*/
                    TimeOut = (item.FindControl("ModulesTimeOutBox") as CheckBox).Checked ? "1" : "0"; 
                    if (string.IsNullOrEmpty(UserCount))
                        UserCount = "0";
                    if (string.IsNullOrEmpty(DaysRemaining))
                        DaysRemaining = "0";
                    UpdateModule(ModuleID, OldLicenseID, LicenseID, TimeOut, ProductID, UserCount, DaysRemaining);
                    break;
                default:
                    break;
            }
        }

        private void UpdateCustomer(string CustomerID, string CustomerName)
        {
            string ConnectionString = SqlDataSource1.ConnectionString;
            SqlConnection conn = new SqlConnection(ConnectionString);

            string updateCommandText = "UPDATE Customers "
                                     + "SET CustomerName = '" + CustomerName + "' "
                                     + "WHERE CustomerID = '" + CustomerID + "'"
                                     ;

            SqlCommand cmd = new SqlCommand(updateCommandText, conn);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void UpdateSite(string SiteID, string OldCustomerID, string NewCustomerID, string SiteName, string SiteDescription)
        {
            string ConnectionString = SqlDataSource2.ConnectionString;
            SqlConnection conn = new SqlConnection(ConnectionString);

            string updateCommandText = "UPDATE CustomerSites "
                                     + "SET CustomerID = '" + NewCustomerID + "',"
                                     + "    SiteName = '" + SiteName + "',"
                                     + "    SiteDescription = '" + SiteDescription + "' "
                                     + "WHERE SiteID = '" + SiteID + "'"
                                     + "  AND CustomerID = '" + OldCustomerID + "'"
                                     ;

            SqlCommand cmd = new SqlCommand(updateCommandText, conn);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void UpdateLicense(string LicenseID, string OldCustID, string NewCustID, string OldSiteID, string NewSiteID,
            string ProductID, string MachineID, string InstallPath, string MachineName, string TotalUserCount, string Timeout,
            string DaysRemaining)
        {
            string ConnectionString = SqlDataSource3.ConnectionString;
            SqlConnection conn = new SqlConnection(ConnectionString);

            string updateCommandText = "UPDATE Licenses "
                                     + "SET CustomerID='" + NewCustID + "',"
                                     + "    SiteID='" + NewSiteID + "',"
                                     + "    ProductID='" + ProductID + "',"
                                     + "    MachineID='" + MachineID + "',"
                                     + "    InstallPath='" + InstallPath + "',"
                                     + "    MachineName='" + MachineName + "',"
                                     + "    TotalUserCount=" + TotalUserCount + ","
                                     + "    TimeOut=" + Timeout + ","
                                     + "    DaysRemaining=" + DaysRemaining + " "
                /*+ "    DateIssued='" + DateIssued + "',"
                + "    LastRequestedUpdate='" + LastRequestedUpdate + "' "*/
                                     + "WHERE LicenseID = " + LicenseID
                                     + "  AND CustomerID= '" + OldCustID + "'"
                                     + "  AND SiteID = '" + OldSiteID + "'"
                                     ;

            SqlCommand cmd = new SqlCommand(updateCommandText, conn);
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void UpdateModule(string ModuleID, string OldLicenseID, string NewLicenseID, string TimeOut,
            string ProductID, string UserCount, string DaysRemaining)
        {

            string ConnectionString = SqlDataSource4.ConnectionString;
            SqlConnection conn = new SqlConnection(ConnectionString);

            string updateCommandText = "UPDATE LicensedModules "
                                     + "SET LicenseID = " + NewLicenseID + ", "
                                     + "    ProductID = '" + ProductID + "', "
                                     + "    UserCount = " + UserCount + ","
                                     + "    DaysRemaining = " + DaysRemaining + ","
                /*+ "    DateIssued = '" + DateIssued + "',"
                + "    LastRequestedUpdate = '" + LastRequestedUpdate + "',"*/
                                     + "    TimeOut = " + TimeOut + " "
                                     + "WHERE ModuleID = '" + ModuleID + "'"
                                     + "  AND LicenseID = " + OldLicenseID
                                     ;
            SqlCommand cmd = new SqlCommand(updateCommandText, conn);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        #endregion

        #region Deletes
        protected void RadGrid1_DeleteCommand(object source, GridCommandEventArgs e)
        {
            GridDataItem item = e.Item as GridDataItem;
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICSLicensesConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Conn;
            string deleteCommandText = "";

            string CustomerID, SiteID, LicenseID, ModuleID;

            switch (item.OwnerTableView.Name)
            {
                case "Customers":
                    //Customers cannot be deleted here, no action is taken
                    return;
                    break;

                case "Sites":
                    CustomerID = item.GetDataKeyValue("CustomerID").ToString();
                    SiteID = item.GetDataKeyValue("SiteID").ToString();

                    deleteCommandText = "DELETE FROM CustomerSites "
                                      + "WHERE CustomerID = '" + CustomerID + "'"
                                      + "  AND SiteID = '" + SiteID + "'"
                                      ;
                    cmd.CommandText = deleteCommandText;
                                        
                    break;

                case "Licenses":
                    if (UserHasPermissions() == false)
                        return;
                    LicenseID = item.GetDataKeyValue("LicenseID").ToString();
                    deleteCommandText = "DELETE FROM Licenses WHERE LicenseID = " + LicenseID;
                    cmd.CommandText = deleteCommandText;

                    break;

                case "Modules":
                    LicenseID = item.GetDataKeyValue("LicenseID").ToString();
                    ModuleID = item.GetDataKeyValue("ModuleID").ToString();

                    deleteCommandText = "DELETE FROM LicensedModules "
                                      + "WHERE LicenseID = " + LicenseID
                                      + "  AND ModuleID = '" + ModuleID + "'"
                                      ;
                    cmd.CommandText = deleteCommandText;
                                        
                    break;

                default:
                    break;

            }
            Conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }
        #endregion





    }
}
