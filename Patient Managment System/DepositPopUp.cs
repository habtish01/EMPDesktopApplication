using Patient_Managment_System.Data_Access_Layer;
using Patient_Managment_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Patient_Managment_System
{
    public partial class DepositPopUp : Form
    {
        DbContext dbContext=new DbContext();
        public PatientDto patient { get; set; }
        public List<Defination> definations;
        public List<Voucher> menuDefinitions;
        public List<IdDefinitionDetail> IdDefinitions;
        IDGenerationDbContext idDbContext=new IDGenerationDbContext();
        Voucher patientDefination;
       IdDefinitionDetail patientIDDefinationDetail;
        public DepositPopUp()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DepositPopUp_Load(object sender, EventArgs e)
        {
            lblPatientName.Text=patient.FirstName+" "+patient.MiddleName+" "+patient.LastName;
            lblPatientID.Text = patient.PersonID;

            patientDefination = menuDefinitions.FirstOrDefault(x => x.parent.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                x.name.Trim().ToLower() == ("Deposit").ToLower());
             patientIDDefinationDetail = IdDefinitions.FirstOrDefault(x => x.type == patientDefination.id);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDepositAmount.Text.Trim()))
            {
                txtDepositAmount.BackColor = Color.LightPink;
                txtDepositAmount.Focus();
                return;
            }


            //access the operation started from defination table
            var closedOperation = definations.FirstOrDefault(x => x.description.Trim().ToLower() == ("Operation").ToLower() &&
                                                                                    x.type.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                    x.value.Trim().ToLower() == ("closed").ToLower());
            if (closedOperation is null)
            {

                MessageBox.Show("You Can't Perform This Operation Now! Please Contact The Admnistrator!", "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var typeMenudefination = menuDefinitions.FirstOrDefault(x => x.name.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                              x.parent.Trim().ToLower() == ("EMR").ToLower());

            Operation operation = new Operation();
            operation.operation = closedOperation.id;
            operation.color = "LightPink";
            operation.type = typeMenudefination.id;
            operation.is_final = true;
            operation.remark = "Closed Operation";
            var saveOperation = dbContext.saveOperation(operation);
            if (!saveOperation.IsPassed)
            {

                MessageBox.Show(saveOperation.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            InvoiceOperation invoiceOperation = new InvoiceOperation();
            var depositID = generateDepositID();
            invoiceOperation.invoice_id = depositID;
            invoiceOperation.operation_id = saveOperation.Data;
            invoiceOperation.operation_datetime = DateTime.Now;
            invoiceOperation.device = Environment.MachineName;
            var saveInvoiceOperation = dbContext.saveInvoiceOperation(invoiceOperation);
            if (!saveInvoiceOperation.IsPassed)
            {

                MessageBox.Show(saveInvoiceOperation.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //crating Period
            Models.Period period = new Models.Period();
            period.description = "Deposit";
            period.start_date = DateTime.Now;
            period.end_date = DateTime.Now;
            period.remark = "patient adde Deposit Balance";
            //excute the dbContext for Period
            var savePeroid = dbContext.savePeriod(period);
            if (!savePeroid.IsPassed)
            {
                MessageBox.Show($"{savePeroid.ErrorMessage}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //crearing an Id for Deposit

            Invoice invoice = new Invoice();
            
            invoice.code = depositID;
            invoice.last_operation = saveInvoiceOperation.Data;
            invoice.type = patientDefination.id;
            invoice.consignee = patient.PatientID;
            invoice.period = savePeroid.Data;
            invoice.date = DateTime.Now;
            invoice.is_final = true;
            invoice.is_void = false;
            invoice.subtotal = int.Parse(txtDepositAmount.Text.Trim());
            invoice.tax = 0;
            invoice.discount = 0;
            invoice.grand_total = invoice.subtotal;
            var saveDepositInvoice = dbContext.saveInvoice(invoice);
            ID newDepositID = new ID();
            newDepositID.current_value = invoice.code;
            newDepositID.defination = patientIDDefinationDetail.id;
            var saveNewDepositID=idDbContext.saveID(newDepositID);
            if (!(saveDepositInvoice.IsPassed && saveNewDepositID.IsPassed))
            {

                MessageBox.Show(saveDepositInvoice.ErrorMessage + saveNewDepositID.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            
            MessageBox.Show(txtDepositAmount.Text.Trim()+" Birr Deposited for this patient!", "Success",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            return;

        }

        private void txtDepositAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtDepositAmount_EditValueChanged(object sender, EventArgs e)
        {
            txtDepositAmount.BackColor=SystemColors.Window; 
        }
        public string generateDepositID()
        {
            var MaxIdValue = idDbContext.getMaxDepositID(patientIDDefinationDetail.id);
            MaxIdValue = MaxIdValue == 0 ? 1 : MaxIdValue + 1;
            var centerPart = MaxIdValue.ToString().PadLeft(patientIDDefinationDetail.length, '0');
            var depositId = string.Format($"{patientIDDefinationDetail.prefix.Trim() + patientIDDefinationDetail.prefix_separator.Trim()}" +
                                               $"{centerPart}" + $"{patientIDDefinationDetail.suffix_separator.Trim() + patientIDDefinationDetail.suffix.Trim()}", MaxIdValue);

            return depositId;
        }
    }
}
