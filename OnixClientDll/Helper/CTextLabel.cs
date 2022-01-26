using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Onix.Client.Helper
{
	public class CTextLabel : INotifyPropertyChanged
	{
		private static CTextLabel obj = new CTextLabel("TH");
		private static String language = "TH";

		public event PropertyChangedEventHandler PropertyChanged;

		public CTextLabel(String lang)
		{
			CLanguage.setLanguage(lang);
		}

		public static CTextLabel Instance
		{
			get
			{
				return (obj);
			}
		}

		public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public static void SetLanguage(String lang)
		{
			language = lang;
			CLanguage.setLanguage(lang);
			//CConfig.SetLanguage(lang);

			foreach (PropertyInfo prop in obj.GetType().GetProperties())
			{
				obj.NotifyPropertyChanged(prop.Name);
			}
		}

		public static String GetLanguage()
		{
			return (language);
		}

        public Boolean IsThai
		{
			get
			{
				return (language.Equals("TH"));
			}

			set
			{
				SetLanguage("TH");
			}
		}

		public Boolean IsEng
		{
			get
			{
				return (language.Equals("EN"));
			}

			set
			{
				SetLanguage("EN");

			}
		}

        #region Properties        
        
        public String adjust_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hiring_duration
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String jan
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String feb
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String mar
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String apr
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String may
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String jun
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String jul
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String aug
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sep
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String oct
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String nov
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String dec
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hiring_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String abnormal_leave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String deduction_leave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String annual_leave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String extra_personal_leave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String personal_leave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String late
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sick_leave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String leave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String PromptPayID
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String has_resigned
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String total_minute
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String duration
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String add_multiple_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String add_multiple_service
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String display_sale_cost
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String out_stock_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String in_stock_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String end_stock_balance_year
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String begin_stock_balance_year
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String begin_stock_balance
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String end_stock_balance
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String stock_in_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String stock_out_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ap_stock_cost
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String deduction_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hr_revenue_tax_1
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hr_revenue_tax_1kor
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sort_order
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tax_deductable
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String rv_tax_1
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String item_info
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String add_payroll
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String payroll_cash_account
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_account_for_payroll
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hr_payroll_withdraw
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hr_payroll_deposit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String work_amount_money
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_amount_money
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String work_hour
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String work_adjust_hour
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String work_from_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String expense_fuel
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String expense_tollway
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String expense_carpark
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String expense_vehicle
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String adjust_hour
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hour_rate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hour_rate_unit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hr_ot_form
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_from_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_to_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_from_time
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_to_time
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_hour
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_rate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot_multiplier
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ot_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String total_receive_emp
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String coverage
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String return_advance
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String deduct_total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String absent_late
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String social_security
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tax
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue_total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String account
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue_transportation
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue_telephone
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue_commission
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue_allowance
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue_bonus
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue_position
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ot
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String salary
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hr_report
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String balance_document
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String document_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String remain
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String deduct
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String add_multiple
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String payroll
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String for_expense_revenue
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_salary
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String calculate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_pct
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tax_pp30
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String month
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String year
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tax_document_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ap_revenue_department
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String district
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String town
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String province
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String zip_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String road
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String soi_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String moo_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String home_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String village_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String floor_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String room_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String building_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String company_register_addr_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String company_register_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tax_form
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String organize_chart
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String monthly
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String daily
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String finger_print_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String line_id
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String department
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String male
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String female
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String gender
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String last_name_eng
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String last_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String hr
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String access_right
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String group_permission
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_xfer_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String regular_cash_xfer
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String external_cash_xfer
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_account_for_owner
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String general_report
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String reason_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String total_vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_claimable
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String disk_image_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String disk_status
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String disk_image
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String return_need
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String borrow_return_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String borrower
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String returner
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String return_due_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vm_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vm_role
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String stage
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String inventory_return
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String inventory_borrow
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vm_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String borrow_eligible
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vm_machine_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vm_disk_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vm_image_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String virtual_machine
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vm_status
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cloud_project
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String microservice_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String microservice_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String docker_url
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String micro_service
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sass
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_in_used
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String contact_person
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String unlink
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String service_category
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String fee
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tax_revenue_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String quotation_attached_sheet
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_page_range
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String to_page
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String from_page
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String quotation_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String quotation_by_detail
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String quotation_by_summary
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String appoint_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bill_summary_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bill_summary
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String project_group_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String copy_head
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String custom_seq
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String invoice_from_saleorder
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String so_by_quotation
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String so_by_manual
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String internal_doc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_month
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String quotation
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String gen_doc_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String invoice_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String deposit_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_deposit_ap
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_deposit_ar
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_pay_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_pay_type1
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_pay_type2
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_pay_type3
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_pay_type4
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String rpt_cheque_receivable
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String to_cheque_duedate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String from_cheque_duedate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String document_adjust
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_invoice_available
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String select_all
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cheque_bank_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String rv_tax_3
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String rv_tax_53
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ac_payee_only
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_misc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String final_discount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cheque_due_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String payable_cheque
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String receivable_cheque
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cheque_status
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String payee_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bank_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cheque_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cheque_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String duedate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String from_duedate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String to_duedate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_misc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String service_other
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String master_ref_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String po_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_doc_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String receipt_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String invoice_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String personal_account
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cr_note
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_by_pct
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String actual_receive_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String item_cost_balance
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String change_by_credit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String change_by_cash
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_ap_receipt
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String APRec
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String employee_name_eng
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String optional
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String optional_eng
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String description_eng
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ref_by_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ref_by_criteria
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String add_by_po
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String free_text
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String reference_doc_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String include_invoice_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_overdue
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String day_overdue
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String for_sale
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String for_purchase
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String for_both
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_history
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_history
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_purchase_history
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_overrided
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String english
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String thai
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String all
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String system_variable
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String languages
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String variable_value
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String variable_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String variable_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }


        public String variable
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String style
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String position
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sort_asc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sort_desc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sorting_by
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String column_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sorting_local_side
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String sorting_server_side
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_credit_note
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_debit_note
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String manual_calculate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String auto_calculate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String validate_until
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String day_validity
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String currency
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String payment_term
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String quotation_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_quotation
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String adjust_by_delta
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String adjust_by_total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String supplier_bank_account
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bank_account
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pricing_by_manual
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pricing_by_promotion
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String document_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String allow_arap_negative
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String void_reason
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String void_document
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String Cheque
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String project_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String percent
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String payment_criteria
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String expense
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_amt
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_eligible
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String same_tax_id
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String project_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String project_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String project_description
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String shift_to
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String project
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ok_print
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String delivery_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String po_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String po_doc_status
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_po
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String selected_document
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ar_ap_document
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String debt_remain
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String debt_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String receipt_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_ar_receipt
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String form_config
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_eligible
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String name_prefix
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String lang_thai
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String lang_english
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String width_cm
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String height_cm
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String size_custom
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String size_predefine
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String report_setting
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String margin_top_cm
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String margin_bottom_cm
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String margin_left_cm
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String margin_right_cm
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_debt
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_ap
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_cash
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_supplier
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String supplier_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String supplier_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String supplier_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String supplier_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String item_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String preview
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String approve_print
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String xfer_add_delta
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String xfer_add_net
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String payment_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String receipt_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String paid_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String remain_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String id_card_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String address_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bill_correction
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String allow_inventory_negative_nv
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String allow_cash_negative_nv
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String import_status
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String primary_profit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String CashAccNV
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String location_name_nv
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String general_vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String general_novat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pos_machine
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String branch_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String branch_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bill_issue
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String final_discount_explain
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String mapping_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String default_customer_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String default_employee_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_by_cash
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_by_debt
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String item_quantity
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String server_connection
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String diff
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String coin
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bank_note
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String coin_note
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String machine
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_in
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_out
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_reconcile
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String reconcile_amt
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tx_in
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tx_out
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ar_ap_balance
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revenue
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sale_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_salesman_specific
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String allow_negative
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String logout_date_time
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String employee_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String employee_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_by_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_by_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }

        }

        public String comm_profile_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_profile_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_profile_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_profile
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tray_package_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_standard_price
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tray_package_bundle
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tray_package_bonus
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tray_package_price
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tray_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String tray_package
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ratio
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String post_gift
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String branch_specific
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_tax_pct
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String minimum_quantity
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String product_specify
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String exclude_item_discounted
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String include_item_discounted
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String basket_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String final_discount_package
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String default_sell_price
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String barcode_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String create
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bundle_package
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bundle
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_output_percent
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_output_per_unit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_output_fix
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_map_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_map_quantity
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String discount_mapping_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String for_unit_price
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String for_total_price
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sort
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String value
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String operator_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pkg_group_operation
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pkg_group_grouping
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pkg_group_pricing
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pkg_group_discount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pkg_group_final_discount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }


        public String free_voucher
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String free_in_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String net
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String result
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String package_reload
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_movement
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_include
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_exclude
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String reference_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String coupon_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String voucher
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String promotion_marketting
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String customer_specify
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String buy
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bonus
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String voucher_package
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String price_test
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String member_edit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String member_register
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String item_category
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String inventory_item_by_category
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String inventory_item_by_table
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_report
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String status
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String account_doc_status
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String account_flow_status
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_debit_note
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_credit_note
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String disconnect
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String session
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String login_duration
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String login_session
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String a4
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

        public String active
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String inactive
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }


        public String letter
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String potrait
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String landscape
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String paper_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String margin_left
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String margin_right
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String margin_top
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String margin_bottom
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String total_record
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String login_date_time
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String error_desc
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ip_address
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String is_login_success
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String login_history
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String minimum_allowed
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String progress
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String rpt_inv_movement_summary
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String rpt_inv_movement
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String connect_fail
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String connect_success
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String returned_xml
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String submited_xml
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String test
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String url
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String key
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String server_setting
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String theme
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String action
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String add
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADD_ITEM
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADD_Price
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADD_SERVICE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADD_StepPrice
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String address
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String address_eng
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String adjust_down
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String adjust_item
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String adjust_up
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String admin
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String admin_audit
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADMIN_DEMOTE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADMIN_EDIT
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String admin_group
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADMIN_PASSWD
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ADMIN_PROMOTE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String admin_report
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String admin_user
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String all_day
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String approve
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String approve_error
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_amount_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_amount_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_avg_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_forward
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_quantity
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String bonus_package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String cancel
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String clear
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String commission
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String company
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String company_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String company_name_eng
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String company_name_thai
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String company_profile
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CONFIRM_DELETE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CONFIRM_EXIT
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String confirm_password
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CONFIRM_SAVE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String copy
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String coupon
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String coupon_amount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String coupon_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String coupon_desc
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String coupon_used_by_document
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String credit_limit
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String credit_term
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String customer_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String customer_group
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String customer_member
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String customer_name
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String customer_package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String customer_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String day
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String day_effective
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String day_expire
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String delete
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String description
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String discount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String discount_package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String discount_val
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Docu_Status
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String DocuDate
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String E
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String edit
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String effective_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String effective_time
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String email
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String end_time
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_APPROVED_INVALID_SEQ
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_APPROVED_NOT_ENOUGH
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_DUPLICATE_KEY
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_LOGIN
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_NO_ITEM_SELECTED
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_NO_SELECTED
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_PASSWORD_VALIDATE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_TEXT_VALIDATE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_USER_ADD
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_USER_EDIT
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_USER_INFO
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_VERIFY_ACCESS
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ERROR_XFER_SAME_LOCATION
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String exit
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String expire_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String export_item
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String fax
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String filter
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String friday
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String from_amnt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String from_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String from_date_effective
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String from_date_expire
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String from_location
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String from_time
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String general
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String give_amnt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String giveaway
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String go_to
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String I
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String import_item
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String in_amount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String in_amount_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String in_amount_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String in_quantity
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String INV_DOC_STATUS_APPROVED
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

        public String INV_DOC_STATUS_CANCEL_APPROVED
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String INV_DOC_STATUS_PENDING
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_adjust
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_doc_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_doc_desc
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_doc_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_doc_status
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_export
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_import
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_item
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_location
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_report
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inventory_xfer
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String is_active
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String is_admin
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String is_assigned
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String is_enable
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String is_time_internal
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String is_used
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_balance_check
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_barcode
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_brand
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_finish_good
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_lot_info
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_movement_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_movement_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_movement_summary
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_name_en
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_name_thai
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_pacakage
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_part
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_production
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_purchase
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_raw_material
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_reference
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_sale
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_uom
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_uom_sale
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String left_amount_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String left_amount_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String left_quantity
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String location_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String location_for_sale
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String location_name
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String location_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String logo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String logout
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String lot_amount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String lot_annonymous
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String lot_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String lot_info
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String lot_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String lot_note
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String lot_quantity
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String master_ref
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

        public String format_doc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String member_desc
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String member_number
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String member_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String monday
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String net_price
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String new_password
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String note
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ok
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String operator_name_eng
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String operator_name_thai
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String out_amount_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String out_amount_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String out_quantity
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PACKAGE_ALL
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item_commission
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item_discount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item_name
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item_point
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item_price
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_item_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_name
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_name_en
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String package_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PACKAGE_TYPE_ITEM
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PACKAGE_TYPE_ITEM_TYPE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PACKAGE_TYPE_SERVICE
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PACKAGE_VIEW
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PackGive
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PackPoint
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PackPrice
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String passwd
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String password
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String percent_discount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String point
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String point_uom
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String price
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String price_step
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String price_uom
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String pricing_package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String program
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String quantity
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String query
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String reference_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String refresh
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_ar
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_cash_saling
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_customer
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_debt_saling
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_member
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_report
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_standard_package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sathurday
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String save
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String search
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sequence
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String server
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String service
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String service_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String service_name
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String service_package
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String service_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String service_uom
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String special_time
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String start_time
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_code
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_detail
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_name
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_row1
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_row2
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_row3
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_row4
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String step_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String STEP_TYPE_AMOUNT
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String STEP_TYPE_QUANTITY
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sunday
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String tax_id
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String telephone
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String thursday
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String to_amnt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String to_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String to_date_effective
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String to_date_expire
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String to_location
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String to_time
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String total_amount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String total_amount_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String tuesday
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String unit_price
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String unit_price_avg
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String unit_price_fifo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String user
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String user_group
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String verify
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String VERIFY_SUCCESS
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String version
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String view
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String website
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String wednesday
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String X
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String xfer_item
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String yes
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String hour
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String minute
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String VAT
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Bath
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Unit
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String DiscUnit
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PayType
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Cash
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CreditCard
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Transfer
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ProdServ
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_uom_stock
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String AddDebt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ProdValue
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String SumAddDebt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String SumRecDebt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String balance_amount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String out_amount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CashFlow
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CashAcc
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CashIn
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String CashOut
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Trans
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ARRec
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String AccNo
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String AccName
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Bank
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Branch
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}
		public String num
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String money_quantity
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String FromAcc
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ToAcc
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String TransBal
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Member
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Promotion
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String couponcode
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String amount
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String coupon_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ItemGroup
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Close
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String pos_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String StandardPrice
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_order
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String req_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_person
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String order_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ref_doc_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ref_doc_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String adv
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String adv_percent
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String adv_ratio
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String adv_amt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String remark
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String amt_bef_vat
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String vat_amt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String number
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item_detail
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String stock
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String stock_cfm
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String stock_so
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String Path
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PurchaseReqisition
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String vendor
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String PurchaseAP
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}
		public String size
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String bal_qty
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String DemandT
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ReqDep
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String ReqStaff
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String itempic
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String item
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String pay_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String bill_type
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String control_cr
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String bill_name
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String bill_addr
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String head_office
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String cus_branch
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String acc_recept
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String sale_dep
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String export
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String cncy
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String exchg_rate
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String incoterms
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}
        
        public String GNralData
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String PO
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String ProcOfficer
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String RefDoc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String CreatePO
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String CopyPR
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String BuyReqList
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String DiscType
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String DiscPrice
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String netBuyPrice
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

            public String BuyQuantity
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String disprice
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String TotalVat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

		public String sale_inv
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String tab_payment
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}
		
		public String inv_doc_tpy
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String so_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String export_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String due_date
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String cb_vat
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String cb_order
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String exp_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inv_adv_no
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String inv_adv_amt
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String per_vat
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}
        
        public String PONo
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String payment
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String vendor_branch
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String creditor
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String import
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

		public String amount_per_unit
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String from_qty
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String to_qty
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String stepprice
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String tier
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

		public String linear
		{
			get
			{
				return (CLanguage.getValueEx());
			}
		}

        public String approve_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String other
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String simulation_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String simulation_desc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String simulation_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String simulation_test
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String total_amount_afterDiscount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String real_total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        
        public String wh_tax_total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String wh_tax_value
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String is_employee_specific
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String byQuantity
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bySaleUnitPrice
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String isProductSpecific
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String company_commission_profile
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String allow_inventory_negative
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String allow_cash_negative
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String no_vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String include_vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String exclude_vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_batch_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_batch_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_batch_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_batch
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String batch_doc_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String batch_desc
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String batch_doc_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String batch_doc_status
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cricle_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cricle
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String doc_date_batch
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String allow_ar_ap_negative
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ar_ap_movement
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bill_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sum_bill
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String comm_cal
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String over_due_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String with_holding
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String report_commission
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String initial
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cost
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String import_by
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ref_id
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String personal_id
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String void_bill_pass
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String amout_total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String amount_unit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String menu_tax
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String PurchaseOrderService_Form
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        #region Report
        public String rpt_sale_per_bill
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String SaleInvoiceReceiptFull
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String Rd_PIT_90
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String RD_PP_30
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String RD_PP_30_Attach
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String report_SaleInvoice
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String PurchaseOrder_Form
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String SaleInvoiceDeptReceiptFull
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String SaleInvoiceDeptReceiptShort
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_report
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String rpt_ap_movement
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        #endregion

        #region Label Cycle
        public String cycle
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String cycle_weekly
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String cycle_monthly
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String cycle_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String cycle_description
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String cycle_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String cycle_day_monthly
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String cycle_day_weekly
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #region Label ItemCategory WinAddEditDeleteItemCategory.xaml
        public String ItemCategoryCount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ItemCategoryID
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ItemCategoryName
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String ItemCategoryParentID
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #region Label Document Number WinAddEditFormatDoument.xaml
        public String formatDoc_doctype
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_LastRunYAER
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_LastRunMONTH
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_Fomula
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_Reset_Criteria
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_Current_SEQ
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_Start_SEQ
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_SEQ_Lenght
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_YEAR_Offset
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_YEAR
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String formatDoc_MONTH
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #region Label ItemInventory WinAddEditInventory.xaml
        public String vat_percent
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat_total
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cost_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }


        public String cost_perUnit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        #endregion

        #region Label Package WinAddEditPackage.xaml
        public String package_price_item_category
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String package_price_service
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String package_addPrice_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String package_addPrice_itemcategory
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String package_addPrice_service
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String discount_specify
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String calcutate
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String customer
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String time
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String selected
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String package_sort
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String package_typeName
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #region Employee
        public String salesman
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String salesman_id
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String salesman_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String salesman_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String salesman_type
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String salesman_group
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String employee_code
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String employee_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String employee
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String employee_category
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String employee_general
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String employee_saleman
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #region DocumentFormat
        public String ACCOUNT_DOC_CASH_TEMP
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ACCOUNT_DOC_CASH_APPROVED
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ACCOUNT_DOC_DEPT_TEMP
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ACCOUNT_DOC_DEPT_APPROVED
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ACCOUNT_DOC_CR_TEMP
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ACCOUNT_DOC_CR_APPROVED
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ACCOUNT_DOC_DR_TEMP
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String ACCOUNT_DOC_DR_APPROVED
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String rpt_comm_batch
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DOC_SEQ_LENGTH_DEFAULT
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DOC_NO_CASH_DEFAULT
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DOC_NO_DEBT_DEFAULT
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DOC_NO_CASH_DEFAULT_NV
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DOC_NO_DEBT_DEFAULT_NV
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DOC_NO_YEAR_OFFSET_DEFAULT
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DOC_NO_RESET_DEFAULT
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String SALE_PETTY_CASH_ACCT_NO
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #region Label Global Variable
        public String global_variable
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String MAX_OVERHEAD_APPROVE_DATE
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String VAT_PERCENTAGE
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String dataType_integer
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String dataType_double
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String dataType_text
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String dataType_boolean
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String boolean_true
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        public String boolean_false
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #region Purchase WinAddEditAccountPurchaseDoc.xaml
        public String allow_creditor_negative
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String purchase_item
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion

        #endregion

        #region POS    

        public String upload_status
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String uploaded
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String cash_payment
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String receive
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String change
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String open_drawer
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String close_drawer
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pos
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String year_offset
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String doc_cash_vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String doc_debt_vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String doc_cash_novat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String doc_debt_novat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String computer_name
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String pos_serial_no
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String default_setting
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String branch_setting
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String port
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String online
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String offline
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String DataSource
        {
            get
            {
                return (CLanguage.getValue("server"));
            }
        }

        public String database
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String docno
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String setting
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String report
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String sync
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String revoke
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String newbill
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String novat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String username
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String mode
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String modeselect
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String store
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String paid
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String print
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vat
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String product
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String unit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String totalprice
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String addproduct
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String paymethod
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String paycash
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String paycredit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String salesave
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String creditnum
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bankcode
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String creditowner
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String receivemoney
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String opentray
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String storereport
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String goto_
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String select
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String savecomplete
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String confirmexit
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String defaultdata
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String connection
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String countersetting
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String countercode
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String branchcode
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String vatsetting
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String download
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String upload
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String close
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String log
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String currentstatus
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String menu
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String currentamount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String lastin
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String lastout
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String moneyin
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String moneyout
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String checkamount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String viewlogs
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String process
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String datetime
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String result_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String viewdetail
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String at_date
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String current_amount
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String different
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String updatedata
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String customersearch
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String customercode
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String customername
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String customertype
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String customergroup
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String telnum
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String category
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String outstanding_balance
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String bill_itemtype
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String include_bill
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String exclude_bill
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }

        public String void_bill
        {
            get
            {
                return (CLanguage.getValueEx());
            }
        }
        #endregion
    }
}


