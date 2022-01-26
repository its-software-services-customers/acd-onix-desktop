using System;
using System.Collections;
using Onix.Client.Report;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Reports
{
    public enum ReportGroupEnum
    {
        ReportGroupInventory = 1,
        ReportGroupCash = 2,
        ReportGroupSale = 3,
        ReportGroupPurchase = 4,
        ReportGroupGeneral = 5,
        ReportGroupHr = 6,
    }

    public static class CReportFactory
    {
        private static Hashtable reports = null;
        private static Hashtable extends = new Hashtable();

        private static void addReport(ReportGroupEnum grp, String key, String className, CTable dat)
        {
            String prefix = "Onix.ClientCenter.Reports.";
            String fqName = prefix + className;

            MMasterRef mr = new MMasterRef(new CTable(""));
            mr.Code = key;
            mr.Description = className;
            mr.DescriptionEng = fqName;
            mr.Optional = CLanguage.getValue(key);

            ArrayList arr = null;
            if (!reports.ContainsKey(grp))
            {
                arr = new ArrayList();
                reports.Add(grp, arr);
            }
            else
            {
                arr = (ArrayList) reports[grp];
            }

            extends.Add(key, dat);
            arr.Add(mr);

            mr.MasterID = arr.Count.ToString();
        }

        public static void InitReports()
        {
            if (reports == null)
            {
                reports = new Hashtable();

                CTable o1 = new CTable("");
                o1.SetFieldValue("COSTING_TYPE", "AVG");

                CTable o2 = new CTable("");
                o2.SetFieldValue("COSTING_TYPE", "FIFO");

                CTable o3 = new CTable("");
                o3.SetFieldValue("COSTING_TYPE", "IMPORT");

                CTable o4 = new CTable("");
                o4.SetFieldValue("COSTING_TYPE", "EXPORT");

                CTable o5 = new CTable("");
                o5.SetFieldValue("COSTING_TYPE", "ADJUST");

                CTable o6 = new CTable("");
                o6.SetFieldValue("COSTING_TYPE", "MOVE");

                //Inventory Report
                addReport(ReportGroupEnum.ReportGroupInventory, "rpt_inv_item_list", "CReportInventory000_01_ItemList", null);
                addReport(ReportGroupEnum.ReportGroupInventory, "rpt_inv_balance_summary_avg", "CReportInventory001_01_ItemBalance", o1);
                addReport(ReportGroupEnum.ReportGroupInventory, "rpt_inv_movement_avg", "CReportInv002_01_InventoryMovementSum", o1);
                addReport(ReportGroupEnum.ReportGroupInventory, "rpt_inv_movement_summary_avg", "CReportInv003_01_InventoryMoveSumDay", o1);
                addReport(ReportGroupEnum.ReportGroupInventory, "rpt_inv_movement_summary_all", "CReportInventory003_03_MovementSummary", null);

                addReport(ReportGroupEnum.ReportGroupCash, "rpt_cash_movement", "CReportCash001_01_CashMovementByAccount", null);

                CTable cq1 = new CTable("");
                cq1.SetFieldValue("DIRECTION", "2");
                addReport(ReportGroupEnum.ReportGroupCash, "rpt_cheque_payable", "CReportCheque001_01_SalePurchase", cq1);

                CTable cq2 = new CTable("");
                cq2.SetFieldValue("DIRECTION", "1");
                addReport(ReportGroupEnum.ReportGroupCash, "rpt_cheque_receivable", "CReportCheque001_01_SalePurchase", cq2);


                //Sale Report
                CTable sl0 = new CTable("");
                sl0.SetFieldValue("CATEGORY", "1");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_ar_movement_debtor", "CReportArAp001_01_Movement", sl0);

                CTable sl1 = new CTable("");
                sl1.SetFieldValue("CATEGORY", "1");
                sl1.SetFieldValue("INTERNAL_DRCR_FLAG", "N"); //ทำเป็น flag ให้เลือกได้ใน gui จะเหมาะกว่า
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_ar_invoiceList002_01", "CReportInvoice001_03_SalePurchase", sl1);

                CTable st0 = new CTable("");
                st0.SetFieldValue("CATEGORY", "1");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_tax_by_document", "CReportPurchase001_01_SalePurchaseTax", st0);

                CTable st0_1 = new CTable("");
                st0_1.SetFieldValue("CATEGORY", "1");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_wh_tax", "CReportPurchase001_02_SalePurchaseWhTax", st0_1);

                CTable st1 = new CTable("");
                st1.SetFieldValue("CATEGORY", "1");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_daily_per_day", "CReportSale004_01_SalePurchaseDailyPerDay", st1);

                CTable s0 = new CTable("");
                s0.SetFieldValue("CATEGORY", "1");                
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_payment", "CReportPayment001_01_SalePurchase", s0);

                CTable s1 = new CTable("");
                s1.SetFieldValue("CATEGORY", "1");
                s1.SetFieldValue("REPORT_TYPE", "1");
                s1.SetFieldValue("INTERNAL_DRCR_FLAG", "N"); //ทำเป็น flag ให้เลือกได้ใน gui จะเหมาะกว่า
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_invoice_list", "CReportInvoice001_01_SalePurchase", s1);

                CTable s2 = new CTable("");
                s2.SetFieldValue("CATEGORY", "1");
                s2.SetFieldValue("REPORT_TYPE", "2");
                s2.SetFieldValue("INTERNAL_DRCR_FLAG", "N"); //ทำเป็น flag ให้เลือกได้ใน gui จะเหมาะกว่า
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_invoice_by_project", "CReportInvoice001_01_SalePurchase", s2);

                CTable s3 = new CTable("");
                s3.SetFieldValue("CATEGORY", "1");
                s3.SetFieldValue("REPORT_TYPE", "3");
                s3.SetFieldValue("INTERNAL_DRCR_FLAG", "N"); //ทำเป็น flag ให้เลือกได้ใน gui จะเหมาะกว่า
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_invoice_by_project_group", "CReportInvoice001_02_SalePurchase", s3);

                CTable p4 = new CTable("");
                p4.SetFieldValue("CATEGORY", "1");
                p4.SetFieldValue("REPORT_TYPE", "1");
                p4.SetFieldValue("INTERNAL_DRCR_FLAG", "N"); //ทำเป็น flag ให้เลือกได้ใน gui จะเหมาะกว่า
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_sale_invoice_detail_list", "CReportInvoice001_02_SalePurchase", p4);

                CTable prf1 = new CTable("");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_profit_summary_by_month", "CReportProfit001_01_ProfitByMonth", prf1);

                CTable prf2 = new CTable("");
                prf2.SetFieldValue("REPORT_TYPE", "1");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_profit_summary_by_project", "CReportProfit001_02_ProfitByProject", prf2);

                CTable prf3 = new CTable("");
                prf3.SetFieldValue("REPORT_TYPE", "2");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_profit_summary_by_project_group", "CReportProfit001_02_ProfitByProject", prf3);

                //ACD
                CTable prfA = new CTable("");
                prf3.SetFieldValue("REPORT_TYPE", "2");
                addReport(ReportGroupEnum.ReportGroupSale, "rpt_profit_summary_by_month_acd", "AcDesign.CReportProfitACD_01_ProfitByMonth", prfA);



                //Purchase Report
                CTable tx0 = new CTable("");
                tx0.SetFieldValue("CATEGORY", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_tax", "CReportPurchase001_01_SalePurchaseTax", tx0);

                CTable tx1 = new CTable("");
                tx1.SetFieldValue("CATEGORY", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_wh_tax", "CReportPurchase001_02_SalePurchaseWhTax", tx1);

                CTable p0 = new CTable("");
                p0.SetFieldValue("CATEGORY", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_payment", "CReportPayment001_01_SalePurchase", p0);

                CTable p1 = new CTable("");
                p1.SetFieldValue("CATEGORY", "2");
                p1.SetFieldValue("REPORT_TYPE", "1");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_invoice_list", "CReportInvoice001_01_SalePurchase", p1);

                CTable p2 = new CTable("");
                p2.SetFieldValue("CATEGORY", "2");
                p2.SetFieldValue("REPORT_TYPE", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_invoice_by_project", "CReportInvoice001_01_SalePurchase", p2);

                CTable p2_1 = new CTable("");
                p2_1.SetFieldValue("CATEGORY", "2");
                p2_1.SetFieldValue("REPORT_TYPE", "3");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_invoice_by_project_group", "CReportInvoice001_02_SalePurchase", p2_1);

                CTable p2_3 = new CTable("");
                p2_3.SetFieldValue("CATEGORY", "2");
                p2_3.SetFieldValue("REPORT_TYPE", "3");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_invoice_summ_by_project_group", "CReportInvoice001_04_SalePurchase", p2_3);


                CTable p2_2 = new CTable("");
                p2_2.SetFieldValue("CATEGORY", "2");
                p2_2.SetFieldValue("REPORT_TYPE", "4");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_invoice_by_project_code", "CReportInvoice001_02_SalePurchase", p2_2);

                CTable p3 = new CTable("");
                p3.SetFieldValue("CATEGORY", "2");
                p3.SetFieldValue("REPORT_TYPE", "1");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_purchase_invoice_detail_list", "CReportInvoice001_02_SalePurchase", p3);

                CTable p3_a = new CTable("");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_account_owner_payment", "CReportPayment001_02_OwnerAccountPurchase", p3_a);

                CTable p3_b = new CTable("");
                p3_b.SetFieldValue("CATEGORY", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_invoice_detail", "CReportInvoice004_01_InvoiceTransaction", p3_b);

                CTable p3_c = new CTable("");
                p3_c.SetFieldValue("CATEGORY", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_po_detail", "CReportInvoice005_PoTransaction", p3_c);

                CTable p3_0 = new CTable("");
                p3_0.SetFieldValue("CATEGORY", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_ap_movement", "CReportArAp001_01_Movement", p3_0);

                CTable p3_1 = new CTable("");
                p3_1.SetFieldValue("CATEGORY", "2");
                addReport(ReportGroupEnum.ReportGroupPurchase, "rpt_ap_invoiceList002_01", "CReportInvoice001_03_SalePurchase", p3_1);


                //General
                addReport(ReportGroupEnum.ReportGroupGeneral, "rpt_service_list", "CReportGeneral001_01_ServicesList", null);

                //Hr
                addReport(ReportGroupEnum.ReportGroupHr, "rpt_employee_payroll_list", "CReportPayroll001_01_EmployeeListPayroll", null);
                addReport(ReportGroupEnum.ReportGroupHr, "rpt_employee_payroll_by_date", "CReportPayroll001_02_EmployeeListPayroll", null);
                addReport(ReportGroupEnum.ReportGroupHr, "rpt_employee_leave", "CReportEmployee00_01_EmployeeLeave", null);
                addReport(ReportGroupEnum.ReportGroupHr, "rpt_employee_tax", "CReportEmployee00_02_EmployeeTax", null);
                addReport(ReportGroupEnum.ReportGroupHr, "rpt_employee_social_insurance", "CReportEmployee00_03_EmployeeSocialInsure", null);
                addReport(ReportGroupEnum.ReportGroupHr, "rpt_employee_revenue", "CReportEmployee00_04_EmployeeRevenue", null);
                addReport(ReportGroupEnum.ReportGroupHr, "rpt_employee_yearly_tax", "CReportEmployee00_05_EmployeeYearlyTax", null);
            }
        }

        public static ArrayList GetReportArray(ReportGroupEnum grp)
        {
            ArrayList arr = (ArrayList) reports[grp];
            return (arr);
        }

        public static CBaseReport GetReportObject(MMasterRef mr)
        {
            String clssName = (String) mr.DescriptionEng;

            CBaseReport rp = (CBaseReport)Activator.CreateInstance(Type.GetType(clssName));
            rp.SetReportObject(mr);

            CTable o = (CTable)extends[mr.Code];
            rp.SetExtraParam(o);
            
            return (rp);
        }

        public static void UpdateExtendedParam(MMasterRef mr, CTable prm)
        {
            CTable o = (CTable) extends[mr.Code];
            if (o == null)
            {
                return;
            }

            foreach (CField f in o.GetTableFields())
            {
                String nm = f.getName();
                String v = f.getValue();

                prm.SetFieldValue(nm, v);
            }

        }
    }
}
