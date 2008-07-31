<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ICSLicenseMaint._Default" MaintainScrollPositionOnPostback="true" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ICS License Maintenance</title>
    <style type="text/css">


.RadGrid_Office2007
{
	font:11px tahoma,verdana,arial,sans-serif;
}

.RadGrid_Office2007
{
	border:1px solid #3b5a82;
	background:#fff;
	color:#27413e;
	
	scrollbar-face-color:#E9E9E9;
	scrollbar-highlight-color:#FFFFFF;
	scrollbar-shadow-color:#E9E9E9;
	scrollbar-3dlight-color:#DBDBDB;
	scrollbar-arrow-color:#787878;
	scrollbar-track-color:#F5F5F5;
	scrollbar-darkshadow-color:#AEAEAE;
}

.MasterTable_Office2007
{
    border-collapse:separate !important;
}

.MasterTable_Office2007
{
	font:11px tahoma,verdana,arial,sans-serif;
}

.GridHeader_Office2007
{
	border-left:1px solid #9eb6ce;
	border-bottom: solid 1px #9eb6ce;
	padding-top:3px;
	padding-bottom:3px;
	background:url('mvwres://Telerik.Web.UI, Version=2008.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Office2007.Grid.sprite.gif') 0 -200px repeat-x #d3dbe9;
	text-align:left;
}

.GridHeader_Office2007
{
	padding-left:4px;
	padding-right:4px;
}

.GridFooter_Office2007
{
	color:#666;
}

        #form1
        {
            width: 926px;
        }
    
.RadComboBox_Default
{
	vertical-align:bottom;
}

.RadComboBox_Default
{
	font:12px arial,verdana,sans-serif;
	color:#000;
	text-align: left;
            margin-top: 1px;
        }


.RadComboBox_Default *
{
	margin:0;
	padding:0;
}

.RadComboBox_Default .rcbInputCell
{
	padding-left: 2px;
}

.RadComboBox_Default .rcbInputCell
{

	height:21px;
	line-height:20px;
	border:1px solid #7d7d7d;
	vertical-align:top;
	background: #fff;
	padding:0;
}

.RadComboBox_Default .rcbInputCell input
{
	padding-left: 2px;
}

.RadComboBox_Default .rcbInputCell input
{
	width:100%;
	background:transparent;
	border:0;
	vertical-align:top;
	padding:3px 0 0 0;
	color: #373737;
}

.RadComboBox_Default input
{
	font:12px arial,verdana,sans-serif;
	color:#000;
	text-align: left;
}

.RadComboBox_Default .rcbArrowCellRight
{
	border-left-width: 0;
}

.RadComboBox_Default .rcbArrowCell
{
	background:url('mvwres://Telerik.Web.UI, Version=2008.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.ComboBox.rcbArrowCell.gif') no-repeat 0 0;
	width:27px;
	border: 1px solid #7d7d7d;
	padding:0;
}

.RadComboBox_Default .rcbArrowCell a
{
	line-height:21px;
	width:27px;
	height:21px;
	text-decoration:none;
	text-indent: -9999px;
	font-size: 0;
}

        #LicensesEditFormTable
        {
            width: 688px;
        }
        .style2
        {
            width: 136px;
            height: 55px;
        }
        .style3
        {
            height: 62px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width:100%;">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ICSLicensesConnectionString %>" 
        DeleteCommand="DELETE FROM [Customers] WHERE [CustomerID] = @CustomerID" 
        InsertCommand="INSERT INTO [Customers] ([CustomerID], [CustomerName]) VALUES (@CustomerID, @CustomerName)" 
        SelectCommand="SELECT * FROM [Customers] ORDER BY [CustomerName]" 
        
        UpdateCommand="UPDATE [Customers] SET [CustomerName] = @CustomerName WHERE [CustomerID] = @CustomerID" 
        ProviderName="<%$ ConnectionStrings:ICSLicenses.ProviderName %>">
        <DeleteParameters>
            <asp:Parameter Name="CustomerID" Type="String" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="CustomerName" Type="String" />
            <asp:Parameter Name="CustomerID" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="CustomerID" Type="String" />
            <asp:Parameter Name="CustomerName" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ICSLicensesConnectionString %>" 
        DeleteCommand="DELETE FROM [CustomerSites] WHERE [CustomerID] = @CustomerID AND [SiteID] = @SiteID" 
        InsertCommand="INSERT INTO [CustomerSites] ([CustomerID], [SiteID], [SiteName], [SiteDescription]) VALUES (@CustomerID, @SiteID, @SiteName, @SiteDescription)" 
        SelectCommand="SELECT CustomerID, SiteID, SiteName, SiteDescription FROM CustomerSites WHERE (CustomerID = @CustomerID) ORDER BY SiteName" 
        
        
        
        UpdateCommand="UPDATE [CustomerSites] SET [SiteName] = @SiteName, [SiteDescription] = @SiteDescription WHERE [CustomerID] = @CustomerID AND [SiteID] = @SiteID">
        <SelectParameters>
            <asp:Parameter Name="CustomerID" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="CustomerID" Type="String" />
            <asp:Parameter Name="SiteID" Type="String" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="SiteName" Type="String" />
            <asp:Parameter Name="SiteDescription" Type="String" />
            <asp:Parameter Name="CustomerID" Type="String" />
            <asp:Parameter Name="SiteID" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="CustomerID" Type="String" />
            <asp:Parameter Name="SiteID" Type="String" />
            <asp:Parameter Name="SiteName" Type="String" />
            <asp:Parameter Name="SiteDescription" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ICSLicensesConnectionString %>" 
        DeleteCommand="DELETE FROM [Licenses] WHERE [LicenseID] = @LicenseID" 
        InsertCommand="INSERT INTO [Licenses] ([CustomerID], [ProductID], [SiteID], [MachineID], [InstallPath], [MachineName], [TotalUserCount], [TimeOut], [DaysRemaining], [DateIssued], [LastRequestedUpdate]) VALUES (@CustomerID, @ProductID, @SiteID, @MachineID, @InstallPath, @MachineName, @TotalUserCount, @TimeOut, @DaysRemaining, @DateIssued, @LastRequestedUpdate)" 
        SelectCommand="SELECT LicenseID, CustomerID, ProductID, SiteID, MachineID, InstallPath, MachineName, TotalUserCount, TimeOut, DaysRemaining, DateIssued, LastRequestedUpdate FROM Licenses WHERE (SiteID = @SiteID) AND (CustomerID = @CustomerID) ORDER BY LicenseID" 
        
        
        
        UpdateCommand="UPDATE [Licenses] SET [CustomerID] = @CustomerID, [ProductID] = @ProductID, [SiteID] = @SiteID, [MachineID] = @MachineID, [InstallPath] = @InstallPath, [MachineName] = @MachineName, [TotalUserCount] = @TotalUserCount, [TimeOut] = @TimeOut, [DaysRemaining] = @DaysRemaining, [DateIssued] = @DateIssued, [LastRequestedUpdate] = @LastRequestedUpdate WHERE [LicenseID] = @LicenseID">
        <SelectParameters>
            <asp:Parameter Name="SiteID" Type="String" />
            <asp:Parameter Name="CustomerID" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="LicenseID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="CustomerID" Type="String" />
            <asp:Parameter Name="ProductID" Type="String" />
            <asp:Parameter Name="SiteID" Type="String" />
            <asp:Parameter Name="MachineID" Type="String" />
            <asp:Parameter Name="InstallPath" Type="String" />
            <asp:Parameter Name="MachineName" Type="String" />
            <asp:Parameter Name="TotalUserCount" Type="Int32" />
            <asp:Parameter Name="TimeOut" Type="Boolean" />
            <asp:Parameter Name="DaysRemaining" Type="Int32" />
            <asp:Parameter Name="DateIssued" Type="DateTime" />
            <asp:Parameter Name="LastRequestedUpdate" Type="DateTime" />
            <asp:Parameter Name="LicenseID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="CustomerID" Type="String" />
            <asp:Parameter Name="ProductID" Type="String" />
            <asp:Parameter Name="SiteID" Type="String" />
            <asp:Parameter Name="MachineID" Type="String" />
            <asp:Parameter Name="InstallPath" Type="String" />
            <asp:Parameter Name="MachineName" Type="String" />
            <asp:Parameter Name="TotalUserCount" Type="Int32" />
            <asp:Parameter Name="TimeOut" Type="Boolean" />
            <asp:Parameter Name="DaysRemaining" Type="Int32" />
            <asp:Parameter Name="DateIssued" Type="DateTime" />
            <asp:Parameter Name="LastRequestedUpdate" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ICSLicensesConnectionString %>" 
        DeleteCommand="DELETE FROM [LicensedModules] WHERE [LicenseID] = @LicenseID AND [ModuleID] = @ModuleID" 
        InsertCommand="INSERT INTO [LicensedModules] ([LicenseID], [ModuleID], [ProductID], [UserCount], [TimeOut], [DaysRemaining], [DateIssued], [LastRequestedUpdate]) VALUES (@LicenseID, @ModuleID, @ProductID, @UserCount, @TimeOut, @DaysRemaining, @DateIssued, @LastRequestedUpdate)" 
        
        
        SelectCommand="SELECT LicensedModules.LicenseID, LicensedModules.ModuleID, ProductModules.ModuleName, LicensedModules.ProductID, LicensedModules.UserCount, LicensedModules.TimeOut, LicensedModules.DaysRemaining, LicensedModules.DateIssued, LicensedModules.LastRequestedUpdate FROM LicensedModules INNER JOIN ProductModules ON LicensedModules.ModuleID = ProductModules.ModuleID WHERE (LicensedModules.LicenseID = @LicenseID) ORDER BY ProductModules.ModuleName, LicensedModules.ModuleID">
        <SelectParameters>
            <asp:Parameter Name="LicenseID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="LicenseID" Type="Int32" />
            <asp:Parameter Name="ModuleID" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="LicenseID" Type="Int32" />
            <asp:Parameter Name="ModuleID" Type="String" />
            <asp:Parameter Name="ProductID" Type="String" />
            <asp:Parameter Name="UserCount" Type="Int32" />
            <asp:Parameter Name="TimeOut" Type="Boolean" />
            <asp:Parameter Name="DaysRemaining" Type="Int32" />
            <asp:Parameter Name="DateIssued" Type="DateTime" />
            <asp:Parameter Name="LastRequestedUpdate" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlDataSource5" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ICSLicensesConnectionString %>" 
        SelectCommand="SELECT * FROM [Products] ORDER BY [ProductName]"></asp:SqlDataSource>
        
        <a href="Default.aspx"><img alt="Refresh" 
        class="style2" src="icslogo.JPG" 
        style="border-color:#ffffff; background-color:#ffffff;" /></a>
<telerik:RadGrid ID="RadGrid1" runat="server" 
        DataSourceID="SqlDataSource1" GridLines="Horizontal" 
        style="margin-bottom: 1px" AllowPaging="True" 
        BorderStyle="None" PageSize="20" onitemcommand="RadGrid1_ItemCommand" 
        onitemcreated="RadGrid1_ItemCreated" 
        oninsertcommand="RadGrid1_InsertCommand" 
        onupdatecommand="RadGrid1_UpdateCommand" CellPadding="0" ondeletecommand="RadGrid1_DeleteCommand" 
        Width="95%" BackColor="White" Font-Bold="False" Font-Italic="False" 
        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
        AllowSorting="True">
        <AlternatingItemStyle Font-Bold="False" Font-Italic="False" 
            Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
            Wrap="True" />
        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
            Font-Strikeout="False" Font-Underline="False" Wrap="True" />
        <PagerStyle Mode="NextPrevAndNumeric" NextPageText="Next" 
            PrevPageText="Previous" />
<MasterTableView DataKeyNames="CustomerID" DataSourceID="SqlDataSource1" 
            Name="Customers" AutoGenerateColumns="False" AllowFilteringByColumn="True" 
            AllowMultiColumnSorting="True" 
            CommandItemDisplay="Top" NoDetailRecordsText="No Customers to display." 
            insertitempageindexaction="ShowItemOnCurrentPage" 
            showheaderswhennorecords="False" 
            CellPadding="0" GridLines="Horizontal" HorizontalAlign="NotSet" 
            Width="100%">
    <DetailTables>
        <telerik:GridTableView runat="server" DataKeyNames="CustomerID,SiteID" 
            DataSourceID="SqlDataSource2" Name="Sites" CommandItemDisplay="Top" 
            NoDetailRecordsText="No Sites to display." 
            InsertItemPageIndexAction="ShowItemOnCurrentPage" 
            AdditionalDataFieldNames="SiteDescription,SiteName" 
            AutoGenerateColumns="False" CellPadding="0" GridLines="Horizontal" 
            Width="100%">
            <DetailTables>
                <telerik:GridTableView runat="server" 
                    DataKeyNames="LicenseID" DataSourceID="SqlDataSource3" 
                    Name="Licenses" NoDetailRecordsText="No Licenses to display." 
                    InsertItemPageIndexAction="ShowItemOnCurrentPage" 
                    ShowHeadersWhenNoRecords="False" EnableNoRecordsTemplate="False" 
                    
                    AdditionalDataFieldNames="CustomerID,SiteID,ProductID,MachineID,InstallPath,MachineName,TotalUserCount,TimeOut,DaysRemaining,DateIssued,LastRequestedUpdate" 
                    AutoGenerateColumns="False" CellPadding="0" GridLines="Horizontal" 
                    AllowMultiColumnSorting="True" Width="100%">
                    <DetailTables>
                        <telerik:GridTableView runat="server" DataKeyNames="LicenseID,ModuleID" 
                            DataSourceID="SqlDataSource4" Name="Modules" CommandItemDisplay="Top" 
                            InsertItemPageIndexAction="ShowItemOnCurrentPage" 
                            NoDetailRecordsText="No Modules to display." 
                            ShowHeadersWhenNoRecords="False" 
                            
                            
                            AdditionalDataFieldNames="ProductID,UserCount,TimeOut,DaysRemaining,DateIssued,LastRequestedUpdate,ModuleName" 
                            AutoGenerateColumns="False" CellPadding="0" GridLines="Horizontal" 
                            Width="100%">
                            <ParentTableRelation>
                                <telerik:GridRelationFields DetailKeyField="LicenseID" 
                                    MasterKeyField="LicenseID" />
                            </ParentTableRelation>
                            <CommandItemSettings AddNewRecordText="Add new Module" />
                            <RowIndicatorColumn Visible="False">
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Resizable="False" Visible="False">
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridEditCommandColumn Resizable="false">
                                    <ItemStyle Width="50px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn CommandName="Delete" Text="Delete" 
                                    UniqueName="column1">
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn DataField="LicenseID" DataType="System.Int32" 
                                    HeaderText="License ID" UniqueName="LicenseID" Visible="False">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModuleID" HeaderText="Module ID" 
                                    UniqueName="ModuleID" Visible="False">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ModuleName" HeaderText="Module Name" 
                                    UniqueName="ModuleName">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ProductID" HeaderText="Product ID" 
                                    UniqueName="ProductID" Visible="False">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="UserCount" DataType="System.Int32" 
                                    HeaderText="User Count" UniqueName="UserCount">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="TimeOut" HeaderText="Time Out" 
                                    UniqueName="TimeOut">
                                    <ItemTemplate>
                            <asp:CheckBox ID="ModulesTimeOutCheckBox" runat="server" Checked ='<%# BIND("TimeOut") %>' Enabled ="false"/>
                            </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="DaysRemaining" DataType="System.Int32" 
                                    HeaderText="Days Remaining" UniqueName="DaysRemaining">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DateIssued" DataType="System.DateTime" 
                                    HeaderText="Date Issued" UniqueName="DateIssued">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LastRequestedUpdate" 
                                    DataType="System.DateTime" HeaderText="Last Requested Update" 
                                    UniqueName="LastRequestedUpdate">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                            </Columns>
                            <EditFormSettings EditFormType="Template">
<EditColumn UniqueName="EditCommandColumn1"></EditColumn>

                                <FormTemplate>
                                    <table>
                                        <tr style="height:22px;">
                                            <td align="right">
                                                License ID:</td>
                                            <td>
                                                <asp:TextBox ID="ModulesLicenseIDBox" runat="server" Width="200px"></asp:TextBox>
                                            </td>
                                            <td width="10px">
                                                &nbsp;</td>
                                            <td align="right" width="146px">
                                                Time Out:</td>
                                            <td>
                                                <asp:CheckBox ID="ModulesTimeOutBox" runat="server" AutoPostBack="True" 
                                                    oncheckedchanged="ModulesTimeOutBox_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr style="height:22px;">
                                            <td align="right">
                                                Module Name:</td>
                                            <td>
                                                <telerik:RadComboBox ID="ModuleIDBox" Runat="server" AllowCustomText="True" 
                                                    MarkFirstMatch="True" ToolTip="Module Name" Width="180px">
                                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                                </telerik:RadComboBox>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td align="right">
                                                Days Remaining:</td>
                                            <td width="161px">
                                                <asp:TextBox ID="ModulesDaysRemainingBox" runat="server" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="height:22px;">
                                            <td align="right">
                                                Product ID:</td>
                                            <td>
                                                <asp:TextBox ID="ModulesProductIDBox" runat="server" Width="200px"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td align="right">
                                                User Count:</td>
                                            <td>
                                                <asp:TextBox ID="ModulesUserCountBox" runat="server" Width="180px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="height:22px;">
                                            <td align="right">
                                                &nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="ModulesDateIssuedBox" runat="server" Visible="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td align="right">
                                                &nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="ModulesLRUBox" runat="server" Visible="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="height:22px;">
                                            <td>
                                                &nbsp;</td>
                                            <td align="center">
                                                <asp:Button ID="UpdateButton" runat="server" CommandName='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "PerformInsert" : "Update" %>' Height="22px" 
                        Text='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "Insert" : "Update" %>'/>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td align="center">
                                                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                                                    Text="Cancel" />
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </FormTemplate>

                                <PopUpSettings ScrollBars="None" />
                            </EditFormSettings>
                        </telerik:GridTableView>
                    </DetailTables>
                    <ParentTableRelation>
                        <telerik:GridRelationFields DetailKeyField="CustomerID" 
                            MasterKeyField="CustomerID" />
                        <telerik:GridRelationFields DetailKeyField="SiteID" MasterKeyField="SiteID" />
                    </ParentTableRelation>
                    <CommandItemSettings AddNewRecordText="Add new License" />
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Resizable="False">
                        <HeaderStyle Width="20px" />
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridEditCommandColumn Resizable="false">
                            <ItemStyle Width="50px" />
                        </telerik:GridEditCommandColumn>
                        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" 
                            UniqueName="column1">
                            
                        </telerik:GridButtonColumn>
                        <telerik:GridBoundColumn DataField="LicenseID" DataType="System.Int32" 
                            HeaderText="License ID" UniqueName="LicenseID">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle HorizontalAlign="Justify" VerticalAlign="Middle" Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CustomerID" HeaderText="Customer ID" 
                            UniqueName="CustomerID" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SiteID" HeaderText="Site ID" 
                            UniqueName="SiteID" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ProductID" HeaderText="Product ID" 
                            UniqueName="ProductID" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MachineID" HeaderText="Machine ID" 
                            UniqueName="MachineID" Visible="False">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="InstallPath" HeaderText="Install Path" 
                            UniqueName="InstallPath">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="MachineName" HeaderText="Machine Name" 
                            UniqueName="MachineName">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TotalUserCount" DataType="System.Int32" 
                            HeaderText="Total User Count" UniqueName="TotalUserCount">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn DataField="TimeOut" DataType="System.Boolean" 
                            HeaderText="Time Out" UniqueName="TimeOut">
                            <ItemTemplate>
                            <asp:CheckBox ID="LicensesTimeOutCheckBox" runat="server" Checked ='<%# BIND("TimeOut") %>' Enabled ="false"/>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="DaysRemaining" DataType="System.Int32" 
                            HeaderText="Days Remaining" UniqueName="DaysRemaining">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DateIssued" DataType="System.DateTime" 
                            HeaderText="Date Issued" UniqueName="DateIssued">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LastRequestedUpdate" 
                            DataType="System.DateTime" HeaderText="Last Requested Update" 
                            UniqueName="LastRequestedUpdate">
                            <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle Wrap="False" />
                        </telerik:GridBoundColumn>
                    </Columns>
                    <EditFormSettings editformtype="Template">
<EditColumn UniqueName="EditCommandColumn1"></EditColumn>

                        <FormTemplate>
                            <table ID="LicensesEditFormTable" align="left" width="688px">
                                <tr style="height:22px;" valign="top">
                                    <td align="right">
                                        License ID:</td>
                                    <td class="style13">
                                        <asp:TextBox ID="LicenseIDBox" runat="server" 
                                            Text='<%# BIND("LicenseID") %>' Width="180px" BackColor="#FFFFCC" 
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        </td>
                                    <td align="right">
                                        Machine Name:</td>
                                    <td>
                                        <asp:TextBox ID="LicensesMachineNameBox" runat="server" 
                                            Text='<%# BIND("MachineName") %>' Width="180px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height:22px;" valign="top">
                                    <td align="right">
                                        Customer ID:</td>
                                    <td >
                                        <telerik:RadComboBox ID="LicensesCustomerIDBox" Runat="server" 
                                            AllowCustomText="True" AutoPostBack="True" DataSourceID="SqlDataSource1" 
                                            DataTextField="CustomerName" DataValueField="CustomerID" MarkFirstMatch="True" 
                                            onselectedindexchanged="LicensesCustomerIDBox_SelectedIndexChanged" 
                                            SelectedValue='<%# BIND("CustomerID") %>' Width="180px">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="right">
                                        Total User Count:</td>
                                    <td >
                                        <asp:TextBox ID="LicensesTotalUserCountBox" runat="server" 
                                            Text='<%# BIND("TotalUserCount") %>' Width="180px"></asp:TextBox>
                                        </td>
                                </tr>
                                <tr align="left" valign="top">
                                    <td align="right">
                                        Site ID:</td>
                                    <td align="left">
                                        <telerik:RadComboBox ID="LicensesSiteIDBox" Runat="server" 
                                            AllowCustomText="True" MarkFirstMatch="True" 
                                            SelectedValue='<%# BIND("SiteID") %>' Width="180px">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td align="right">
                                        Time Out:</td>
                                    <td align="left">
                                        <asp:CheckBox ID="LicensesTimeOutBox" runat="server" AutoPostBack="True" 
                                            Checked='<%# BIND("TimeOut") %>' 
                                            oncheckedchanged="LicensesTimeOutBox_CheckedChanged" Text='<%# "" %>' />
                                    </td>
                                </tr>
                                <tr style="height:22px;" valign="top">
                                    <td align="right">
                                        Product ID:</td>
                                    <td>
                                        <telerik:RadComboBox ID="LicensesProductIDBox" Runat="server" 
                                            DataSourceID="SqlDataSource5" DataTextField="ProductName" 
                                            DataValueField="ProductID" SelectedValue='<%# BIND("ProductID") %>' 
                                            Width="180px" AllowCustomText="True" MarkFirstMatch="True">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </td>
                                    <td >
                                        &nbsp;</td>
                                    <td align="right">
                                        Days Remaining:</td>
                                    <td>
                                        <asp:TextBox ID="LicensesDaysRemainingBox" runat="server" 
                                            Text='<%# BIND("DaysRemaining") %>' Width="180px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" class="style3">
                                        Machine ID:<td class="style3">
                                            <asp:TextBox ID="LicensesMachineIDBox" runat="server" Height="60px" 
                                                Text='<%# BIND("MachineID") %>' TextMode="MultiLine" Width="180px"></asp:TextBox>
                                        </td>
                                        <td class="style3">
                                            </td>
                                        <td align="right" class="style3">
                                            Install Path:</td>
                                        <td class="style3">
                                            <asp:TextBox ID="LicensesInstallPathBox" runat="server" 
                                                Text='<%# BIND("InstallPath") %>' Width="180px"></asp:TextBox>
                                        </td>
                                    </td>
                                </tr>
                                <tr style="height:22px;">
                                    <td align="right">
                                        &nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="LicensesDateIssuedBox" runat="server" Enabled="False" 
                                            Text='<%# BIND("DateIssued") %>' Visible="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td align="right">
                                        &nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="LicensesLRUBox" runat="server" 
                                            Text='<%# BIND("LastRequestedUpdate") %>' Enabled="False" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height:22px;">
                                    <td>
                                        </td>
                                    <td align="center">
                                        <asp:Button ID="UpdateButton" runat="server" CommandName='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "PerformInsert" : "Update" %>' Height="22px" 
                        Text='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "Insert" : "Update" %>' />
                                    </td>
                                    <td>
                                        </td>
                                    <td align="center">
                                        <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                                            Text="Cancel" />
                                    </td>
                                    <td>
                                        </td>
                                </tr>
                            </table>
                        </FormTemplate>

                        <PopUpSettings ScrollBars="None" />
                    </EditFormSettings>
                </telerik:GridTableView>
            </DetailTables>
            <ParentTableRelation>
                <telerik:GridRelationFields DetailKeyField="CustomerID" 
                    MasterKeyField="CustomerID" />
            </ParentTableRelation>
            <CommandItemSettings AddNewRecordText="Add new Site" />
            <RowIndicatorColumn Visible="False">
                <HeaderStyle Width="20px" />
            </RowIndicatorColumn>
            <ExpandCollapseColumn Resizable="False">
                <HeaderStyle Width="20px" />
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridEditCommandColumn Resizable="false">
                    <ItemStyle Width="50px" />
                </telerik:GridEditCommandColumn>
                <telerik:GridButtonColumn CommandName="Delete" Text="Delete" 
                    UniqueName="column1">
                </telerik:GridButtonColumn>
                <telerik:GridBoundColumn DataField="CustomerID" HeaderText="Customer ID" 
                    UniqueName="CustomerID" Visible="False">
                    <HeaderStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SiteID" HeaderText="Site ID" 
                    UniqueName="SiteID" Visible="False">
                    <HeaderStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SiteName" HeaderText="Site Name" 
                    UniqueName="SiteName">
                    <HeaderStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SiteDescription" 
                    HeaderText="Site Description" UniqueName="SiteDescription">
                    <HeaderStyle HorizontalAlign="Center" />
                </telerik:GridBoundColumn>
            </Columns>
            <EditFormSettings EditFormType="Template">
<EditColumn UniqueName="EditCommandColumn1"></EditColumn>

                <FormTemplate>
                    <table style="width: 360px;">
                        <tr style="height:22px;">
                            <td align="right">
                                Customer:</td>
                            <td>
                                <telerik:RadComboBox ID="SitesCustomerIDBox" Runat="server" 
                                    AllowCustomText="True" MarkFirstMatch="True">
                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr style="height:22px">
                            <td align="right">
                                Site ID:</td>
                            <td>
                                <asp:TextBox ID="SiteIDBox" runat="server" BackColor="#FFFFCC"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:22px;">
                            <td align="right">
                                Site Name:</td>
                            <td>
                                <asp:TextBox ID="SiteNameBox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height:22px;">
                            <td align="right">
                                Site Description:</td>
                            <td>
                                <asp:TextBox ID="SiteDescBox" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="center" valign="bottom">
                            </td>
                            <td>
                                <asp:Button ID="UpdateButton" runat="server" CommandName='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "PerformInsert" : "Update" %>' Height="22px" 
                        Text='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "Insert" : "Update" %>' />
                            <td align="center" valign="bottom">
                                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                                    Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </FormTemplate>

                <PopUpSettings ScrollBars="None" />
            </EditFormSettings>
            <CommandItemStyle ForeColor="Silver" />
        </telerik:GridTableView>
    </DetailTables>
    <CommandItemSettings AddNewRecordText="Add new Customer" />
<RowIndicatorColumn Visible="False">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Resizable="False">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridEditCommandColumn Resizable="false">
        <ItemStyle Width="50px" />
        </telerik:GridEditCommandColumn>
        <telerik:GridBoundColumn DataField="CustomerID" HeaderText="Customer ID" 
            SortExpression="CustomerID" UniqueName="CustomerID" Visible="False">
            <HeaderStyle HorizontalAlign="Center" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="CustomerName" HeaderText="Customer Name" 
            SortExpression="CustomerName" UniqueName="CustomerName">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="450px" />
        </telerik:GridBoundColumn>
    </Columns>

<EditFormSettings EditFormType="Template">
<EditColumn UniqueName="EditCommandColumn1"></EditColumn>

    <FormTemplate>
        <table style="height: 90px; width: 275px;">
            <tr style="height:22px;">
                <td align="right">
                    Customer ID :</td>
                <td>
                    <asp:TextBox ID="CustomerIDBox" runat="server" BackColor="#FFFFCC" 
                        ReadOnly="True" Text='<%# Bind("CustomerID") %>'></asp:TextBox>
                </td>
            </tr>
            <tr style="height:22px;">
                <td align="right">
                    Customer Name :</td>
                <td>
                    <asp:TextBox ID="CustomerNameBox" runat="server" 
                        Text='<%#Bind("CustomerName") %>'></asp:TextBox>
                </td>
            </tr>
            <tr style="height:22px;">
                <td align="center">
                    <asp:Button ID="UpdateButton" runat="server" CommandName='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "PerformInsert" : "Update" %>' Height="22px" 
                        Text='<%# ((bool)DataBinder.Eval(Container, "OwnerTableView.IsItemInserted")) ? "Insert" : "Update" %>' />
                </td>
                <td align="center">
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Height="22px" 
                        Text="Cancel" />
                </td>
            </tr>
        </table>
        &nbsp;<br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FormTemplate>

<PopUpSettings ScrollBars="None"></PopUpSettings>
</EditFormSettings>
    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
        Font-Strikeout="False" Font-Underline="False" Wrap="True" />
    <AlternatingItemStyle Font-Bold="False" Font-Italic="False" 
        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
        Wrap="True" />
    <HeaderStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
        Font-Strikeout="False" Font-Underline="False" Wrap="True" />
</MasterTableView>
<ClientSettings EnablePostBackOnRowClick="true"><Selecting AllowRowSelect="true" /></ClientSettings>
        <GroupingSettings CaseSensitive="False" />
   </telerik:RadGrid>
    </form>
</body>
</html>
