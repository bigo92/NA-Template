using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace tci.common.Constants
{
    public class ClaimConstant
    {
        [DisplayName()]
        [Description("Manager Roles")]
        class Roles
        {
            [DisplayName("View")]
            [Description("View Data")]
            const string View = "Api.Roles.GetData";
            [DisplayName("Add")]
            [Description("Add Data")]
            const string Add = "Api.Roles.PostData";
            [DisplayName("Edit")]
            [Description("Edit Data")]
            const string Edit = "Api.Roles.PutData";
            [DisplayName("Delete")]
            [Description("Delete Data")]
            const string Delete = "Api.Roles.DeleteData";
        }

        [DisplayName()]
        [Description("SALES MANAGEMENT")]
        class SaleManagerment
        {
            [DisplayName("View")]
            [Description("View Order")]
            const string View = "Api.SaleManagerment.GetData";
            [DisplayName("Create")]
            [Description("Create Order")]
            const string Add = "Api.SaleManagerment.PostData";
            [DisplayName("Update")]
            [Description("Update Order")]
            const string Edit = "Api.SaleManagerment.PutData";
            [DisplayName("Delete")]
            [Description("Delete Order")]
            const string Delete = "Api.SaleManagerment.DeleteData";
            [DisplayName("Print")]
            [Description("Print")]
            const string Print = "Api.SaleManagerment.Print";
            [DisplayName("Export to excel")]
            [Description("Export to excel")]
            const string ExportExcel = "Api.SaleManagerment.ExportExcel";

            [DisplayName("Edit Price")]
            [Description("Edit Price")]
            const string EditPrice = "Api.SaleManagerment.EditPrice";
            [DisplayName("View Price")]
            [Description("View Price")]
            const string ViewPrice = "Api.SaleManagerment.ViewPrice";
        }

        [DisplayName()]
        [Description("GOODS (Goods Management)")]
        class GoodManagement
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.GoodManagement.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.GoodManagement.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.GoodManagement.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.GoodManagement.DeleteData";
            [DisplayName("Active")]
            [Description("Active")]
            const string Active = "Api.GoodManagement.Active";
            [DisplayName("De Active")]
            [Description("De Active")]
            const string DeActivce = "Api.GoodManagement.DeActivce";
            [DisplayName("Barcode Printing")]
            [Description("Barcode Printing")]
            const string Barcode = "Api.GoodManagement.Barcode";
            [DisplayName("Search Warehouse")]
            [Description("Search Warehouse")]
            const string SearchWarehouse = "Api.GoodManagement.SearchWarehouse";

            [DisplayName("Show Import Price")]
            [Description("Show Import Price")]
            const string ShowImportPrice = "Api.GoodManagement.ShowImportPrice";

            [DisplayName("Import Excel")]
            [Description("Import Excel")]
            const string ImportExcel = "Api.GoodManagement.ImportExcel";

            [DisplayName("Add To Cart")]
            [Description("Add To Cart")]
            const string AddToCart = "Api.GoodManagement.AddToCart";
        }

        [DisplayName()]
        [Description("GOODS (Configure Price)")]
        class ConfigurePrice
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.ConfigurePrice.GetData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.ConfigurePrice.PutData";
        }

        [DisplayName()]
        [Description("WAREHOUSE (Goods Voucher Receipt)")]
        class GoodVoucherReceipt
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.GoodVoucherReceipt.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.GoodVoucherReceipt.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.GoodVoucherReceipt.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.GoodVoucherReceipt.DeleteData";
            [DisplayName("Print")]
            [Description("Print")]
            const string Print = "Api.GoodVoucherReceipt.Print";
            [DisplayName("Assign")]
            [Description("Assign")]
            const string Assign = "Api.GoodVoucherReceipt.Assign";
            [DisplayName("Proceed")]
            [Description("Proceed")]
            const string Proceed = "Api.GoodVoucherReceipt.Proceed";
            [DisplayName("ConfirmChange")]
            [Description("ConfirmChange")]
            const string ConfirmChange = "Api.GoodVoucherTransfer.ConfirmChange";
        }

        [DisplayName()]
        [Description("WAREHOUSE (Goods Voucher Transfer)")]
        class GoodVoucherTransfer
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.GoodVoucherTransfer.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.GoodVoucherTransfer.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.GoodVoucherTransfer.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.GoodVoucherTransfer.DeleteData";
            [DisplayName("Print")]
            [Description("Print")]
            const string Print = "Api.GoodVoucherTransfer.Print";
            [DisplayName("Assign")]
            [Description("Assign")]
            const string Assign = "Api.GoodVoucherTransfer.Assign";
            [DisplayName("Pakaging")]
            [Description("Pakaging")]
            const string Pakaging = "Api.GoodVoucherTransfer.Pakaging";
            [DisplayName("Delivering")]
            [Description("Delivering")]
            const string Delivering = "Api.GoodVoucherTransfer.Delivering";
            [DisplayName("Confirm")]
            [Description("Confirm")]
            const string Confirm = "Api.GoodVoucherTransfer.Confirm";
        }

        [DisplayName()]
        [Description("WAREHOUSE (Goods Receipt)")]
        class GoodReceipt
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.GoodReceipt.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.GoodReceipt.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.GoodReceipt.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.GoodReceipt.DeleteData";
            [DisplayName("Print")]
            [Description("Print")]
            const string Print = "Api.GoodReceipt.Print";
        }

        [DisplayName()]
        [Description("WAREHOUSE (Goods Issue)")]
        class GoodIssue
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.GoodIssue.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.GoodIssue.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.GoodIssue.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.GoodIssue.DeleteData";
            [DisplayName("Print")]
            [Description("Print")]
            const string Print = "Api.GoodIssue.Print";
            [DisplayName("Assign")]
            [Description("Assign")]
            const string Assign = "Api.GoodIssue.Assign";
            [DisplayName("Proceed")]
            [Description("Proceed")]
            const string Proceed = "Api.GoodIssue.Proceed";
            [DisplayName("Complete")]
            [Description("Complete")]
            const string Complete = "Api.GoodIssue.Complete";
            [DisplayName("Reject")]
            [Description("Reject")]
            const string Reject = "Api.GoodIssue.Reject";
        }

        [DisplayName()]
        [Description("WAREHOUSE (Warehouse Management)")]
        class WarehouseManagement
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.WarehouseManagement.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.WarehouseManagement.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.WarehouseManagement.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.WarehouseManagement.DeleteData";
            [DisplayName("Active")]
            [Description("Active")]
            const string Active = "Api.WarehouseManagement.Active";
            [DisplayName("De Active")]
            [Description("De Active")]
            const string DeActive = "Api.WarehouseManagement.DeActive";
        }

        [DisplayName()]
        [Description("STAFF")]
        class Staff
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.Staff.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.Staff.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.Staff.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.Staff.DeleteData";
            [DisplayName("Active")]
            [Description("Active")]
            const string Active = "Api.Staff.Active";
            [DisplayName("De Active")]
            [Description("De Active")]
            const string DeActive = "Api.Staff.DeActive";
        }


        [DisplayName()]
        [Description("PARTNER MANAGEMENT")]
        class PartnerManagement
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.PartnerManagement.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.PartnerManagement.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.PartnerManagement.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.PartnerManagement.DeleteData";
            [DisplayName("Active")]
            [Description("Active")]
            const string Active = "Api.PartnerManagement.Active";
            [DisplayName("De Active")]
            [Description("De Active")]
            const string DeActive = "Api.PartnerManagement.DeActive";
        }

        [DisplayName()]
        [Description("REPORTS")]
        class Report
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.Report.GetData";
        }

        [DisplayName()]
        [Description("RECOVER PASSWORD")]
        class RecoverPassword
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.RecoverPassword.GetData";

            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.RecoverPassword.PutData";
        }

        [DisplayName()]
        [Description("GENERAL CONFIGURE")]
        class GeneralConfigure
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.GeneralConfigure.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.GeneralConfigure.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.GeneralConfigure.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.GeneralConfigure.DeleteData";
        }

        [DisplayName()]
        [Description("LANGUAGE MANAGEMENT")]
        class Languages
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.Languages.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.Languages.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.Languages.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.Languages.DeleteData";
        }

        [DisplayName()]
        [Description("VAT CONFIGURE")]
        class VatConfigure
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.VATConfigure.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.VATConfigure.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.VATConfigure.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.VATConfigure.DeleteData";
        }

        [DisplayName()]
        [Description("CITY MANAGEMENT")]
        class CityManagement
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.CityManagement.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.CityManagement.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.CityManagement.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.CityManagement.DeleteData";
        }

        [DisplayName()]
        [Description("DELIVERY MANAGEMENT")]
        class DeliveryManagement
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.DeliveryManagement.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.DeliveryManagement.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.DeliveryManagement.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.DeliveryManagement.DeleteData";
        }

        [DisplayName()]
        [Description("UNIT MANAGEMENT")]
        class UnitManagement
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.UnitManagement.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.UnitManagement.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.UnitManagement.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.UnitManagement.DeleteData";
        }

        [DisplayName()]
        [Description("BANK ACCOUNT")]
        class BankAccount
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.BankAccount.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.BankAccount.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.BankAccount.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.BankAccount.DeleteData";
        }

        [DisplayName()]
        [Description("SPENDING GROUP")]
        class SpendingGroup
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.SpendingGroup.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.SpendingGroup.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.SpendingGroup.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.SpendingGroup.DeleteData";
        }

        [DisplayName()]
        [Description("FORM OF PAYMENT")]
        class PaymentForm
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.PaymentForm.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.PaymentForm.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.PaymentForm.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.PaymentForm.DeleteData";
        }

        [DisplayName()]
        [Description("CATEGORY MANAGEMENT")]
        class CategoryManagement
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.CategoryManagement.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.CategoryManagement.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.CategoryManagement.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.CategoryManagement.DeleteData";
        }



        [DisplayName()]
        [Description("ACCRUED EXPENSES MANAGEMENT")]
        class AccruedExpenses
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.AccruedExpenses.GetData";
            [DisplayName("Create")]
            [Description("Create")]
            const string Add = "Api.AccruedExpenses.PostData";
            [DisplayName("Update")]
            [Description("Update")]
            const string Edit = "Api.AccruedExpenses.PutData";
            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.AccruedExpenses.DeleteData";
        }

        [DisplayName()]
        [Description("FINALCIALS")]
        class Finalcials
        {
            [DisplayName("ReceivePayments")]
            [Description("ReceivePayments")]
            const string ReceivePayments = "Api.ReceivePayments.GetData";

            [DisplayName("ExpensePayable")]
            [Description("ExpensePayable")]
            const string ExpensePayable = "Api.ExpensePayable.GetData";

            [DisplayName("ExpensesPayableManagement")]
            [Description("ExpensesPayableManagement")]
            const string ExpensesPayableManagement = "Api.ExpensesPayableManagement.GetData";
        }

        [DisplayName()]
        [Description("REPORTS")]
        class Reports
        {
            [DisplayName("ReportCostOfGoodsSold")]
            [Description("ReportCostOfGoodsSold")]
            const string ReportCostOfGoodsSold = "Api.ReportCostOfGoodsSold.GetData";

            [DisplayName("ReportCostOfImportedGoods")]
            [Description("ReportCostOfImportedGoods")]
            const string ReportCostOfImportedGoods = "Api.ReportCostOfImportedGoods.GetData";

            [DisplayName("ReportGoodsInStockByDate")]
            [Description("ReportGoodsInStockByDate")]
            const string ReportGoodsInStockByDate = "Api.ReportGoodsInStockByDate.GetData";

            [DisplayName("ReportDebtbyCustomer")]
            [Description("ReportDebtbyCustomer")]
            const string ReportDebtbyCustomer = "Api.ReportDebtbyCustomer.GetData";

            [DisplayName("ReportDebtbyInvoice")]
            [Description("ReportDebtbyInvoice")]
            const string ReportDebtbyInvoice = "Api.ReportDebtbyInvoice.GetData";

            [DisplayName("ReportDebtbySupplier")]
            [Description("ReportDebtbySupplier")]
            const string ReportDebtbySupplier = "Api.ReportDebtbySupplier.GetData";

            [DisplayName("ReportPurchasebyInvoice")]
            [Description("ReportPurchasebyInvoice")]
            const string ReportPurchasebyInvoice = "Api.ReportPurchasebyInvoice.GetData";

            [DisplayName("ReportIncomeExpense")]
            [Description("ReportIncomeExpense")]
            const string ReportIncomeExpense = "Api.ReportIncomeExpense.GetData";


            [DisplayName("ReportProfitandLoss")]
            [Description("ReportProfitandLoss")]
            const string ReportProfitandLoss = "Api.ReportProfitandLoss.GetData";
        }

        [DisplayName()]
        [Description("BACKUP")]
        class Backup
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.Backup.GetData";

            [DisplayName("Backup Data")]
            [Description("Backup Data")]
            const string BackupData = "Api.Backup.BackupData";

            [DisplayName("Insert Data")]
            [Description("Insert Data")]
            const string InsertData = "Api.Backup.InsertData";

            [DisplayName("Backup Full Data")]
            [Description("Backup Full Data")]
            const string BackupFullData = "Api.Backup.BackupFullData";

            [DisplayName("Restore Data")]
            [Description("Restore Data")]
            const string RestoreData = "Api.Backup.RestoreData";

            [DisplayName("Download")]
            [Description("Download")]
            const string Download = "Api.Backup.DownloadData";

            [DisplayName("Delete")]
            [Description("Delete")]
            const string Delete = "Api.Backup.DeleteData";
        }

        [DisplayName()]
        [Description("Calculate")]
        class Calculate
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.Calculate.GetData";

            [DisplayName("CalculateData")]
            [Description("CalculateData")]
            const string CalculateData = "Api.Calculate.CalculateData";

            [DisplayName("Apply Data")]
            [Description("Apply Data")]
            const string ApplyData = "Api.Calculate.ApplyData";
        }
        [DisplayName()]
        [Description("Setting Virtual Warehouse")]
        class VirtualWarehouse
        {
            [DisplayName("View")]
            [Description("View")]
            const string View = "Api.VirtualWarehouse.GetData";

            [DisplayName("VirtualWarehouseData")]
            [Description("VirtualWarehouseData")]
            const string VirtualWarehouseData = "Api.VirtualWarehouse.PostData";

        }

    }
}
