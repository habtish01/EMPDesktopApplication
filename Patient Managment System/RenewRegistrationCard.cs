using DevExpress.XtraBars.Docking2010.DragEngine;
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
    public partial class RenewRegistrationCard : Form
    {
       public List<Voucher> menuDefinations {  get; set; }   
        public List<Defination> definations { get; set; }   
        public PatientDto Patient { get; set; }
        public List<Person> persons { get; set; }
        public List<RegistrationItem> definationItems { get; set; }
        public List<Configurations> configurations { get; set; }
        public List<IdDefinitionDetail> idDefinitions { get; set; }
        public Visit updateVisit { get; set; }
        public List<Room> rooms { get; set; }

        DbContext dbContext=new DbContext();
        IDGenerationDbContext idDbContext=new IDGenerationDbContext();
        PatientMSystem patientMSystem = new PatientMSystem();
        public RenewRegistrationCard()
        {
            InitializeComponent();
        }

        private void RenewRegistrationCard_Load(object sender, EventArgs e)
        {
            lblPatientName.Text = Patient.FirstName + " " + Patient.MiddleName + " " + Patient.LastName;
            lblPatientID.Text = Patient.PersonID;

            var vouchers = menuDefinations.Where(x => x.parent.Trim().ToLower() == ("EMR").ToLower() &&
                                                                              x.name.Trim().ToLower() == ("Registration").ToLower())
                   .Select(x => new ComoBoxList
                   {
                       Id = x.id,
                       Description = x.name
                   }).ToList();
            comboBoxInvoiceTypes.DataSource = vouchers;
            comboBoxInvoiceTypes.ValueMember = "Id";
            comboBoxInvoiceTypes.DisplayMember = "Description";


            var patientAssignmentType = definations.Where(x =>
                                        x.type.Trim() == "EMR" &&
                                        x.description.Trim() == "registration assignment")
                 .Select(x => new ComoBoxList
                 {
                     Id = x.id,
                     Description = x.value
                 }).ToList();

            cBoxAssignType.DataSource = patientAssignmentType;//combo box for Registration fee Type
            cBoxAssignType.ValueMember = "Id";
            cBoxAssignType.DisplayMember = "Description";

            var registrationFeeItems = definationItems.Where(x => x.value == "Patient Registartion")
                   .Select(x => new ComoBoxList
                   {
                       Id = x.Reference,
                       Description = x.name
                   }).ToList();

            cBoxRegType.DataSource = registrationFeeItems;//combo box for Registration fee Type
            cBoxRegType.ValueMember = "Id";
            cBoxRegType.DisplayMember = "Description";
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // if(cBoxAssignType)
            if (cBoxAssignType.SelectedIndex < 0)
            {
                cBoxAssignType.BackColor = Color.LightPink;
                cBoxAssignType.Focus();
                return;
            }
            if (cBoxAssignValue.SelectedIndex < 0)
            {
                cBoxAssignValue.BackColor = Color.LightPink;
                cBoxAssignValue.Focus();
                return;
            }
            if(cBoxRegType.SelectedIndex < 0)
            {
                cBoxRegType.BackColor = Color.LightPink;    
                cBoxRegType.Focus();
                return;
            }
            if(txtRegAmount is null)
            {
                txtRegAmount.BackColor = Color.LightPink;   
                txtRegAmount.Focus();
                return;
            }
            if (comboBoxInvoiceTypes.SelectedIndex < 0)
            {
                comboBoxInvoiceTypes.BackColor = Color.LightPink;   
                comboBoxInvoiceTypes.Focus();   
                return;
            }
            //checking the Registered Operation is Found in Defination or not
            var OperationType = "EMR";
            var operationDescription = "Operation";
            var operationValue = "registered";
            //assign values for Operation Object
            var RegisterOperation = definations.FirstOrDefault(x => x.type.Trim().ToLower() == OperationType.ToLower() &&
                                                                          x.description.Trim().ToLower() == operationDescription.ToLower() &&
                                                                          x.value.Trim().ToLower() == operationValue.ToLower());
            var RegisterMenudefination = menuDefinations.FirstOrDefault(x => x.name.Trim().ToLower() == ("EMR").ToLower() &&              
           
                                                                                          x.parent.Trim().ToLower() == ("EMR").ToLower());
            Operation operation = new Operation();
            operation.operation = RegisterOperation.id;
            operation.type = RegisterMenudefination.id;
            operation.color = "Blue";
            operation.manual = false;
            operation.is_final = true;
            //excute the dbContext for Operation
            var saveOperation = dbContext.saveOperation(operation);
            if (!saveOperation.IsPassed)
            {
                MessageBox.Show($"{saveOperation.ErrorMessage}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // assign values for invoice Operation Object

            InvoiceOperation invoiceOperation = new InvoiceOperation();
            var VochourCode=generateVouchurID();
            invoiceOperation.operation_id = saveOperation.Data;
            invoiceOperation.invoice_id = VochourCode;//need further detail
            invoiceOperation.operation_datetime = DateTime.Now;
            invoiceOperation.UserName = 1;
            invoiceOperation.device = Environment.MachineName;
            //excute the dbContext for Invoice Operation
            var saveInvoiceOperation = dbContext.saveInvoiceOperation(invoiceOperation);
            if (!saveInvoiceOperation.IsPassed)
            {
                MessageBox.Show($"{saveInvoiceOperation.ErrorMessage}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //crating Period
            Models.Period period = new Models.Period();
            period.description = "Registration";
            period.start_date = DateTime.Now;
            period.end_date = DateTime.Now;
            period.remark = "patient registration";
            //excute the dbContext for Period
            var savePeroid = dbContext.savePeriod(period);
            if (!savePeroid.IsPassed)
            {
                MessageBox.Show($"{savePeroid.ErrorMessage}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // assign values for Invoice object
            Invoice invoice = new Invoice();    
            var invoiceType = (ComoBoxList)comboBoxInvoiceTypes.SelectedItem;
            invoice.code = VochourCode;
            invoice.last_operation = saveInvoiceOperation.Data;
            invoice.type = invoiceType.Id;
            invoice.consignee = Patient.PatientID;
            invoice.period = savePeroid.Data;
            invoice.date = DateTime.Now;
            invoice.is_final = true;
            invoice.is_void = false;
            invoice.subtotal = Decimal.Parse(txtRegAmount.Text.ToString());
            invoice.tax = 0;
            invoice.discount = 0;
            invoice.grand_total = invoice.subtotal;
            //excute the dbContext for Invoice
            var saveInvoice = dbContext.saveInvoice(invoice);
            if (!saveInvoice.IsPassed)
            {
                MessageBox.Show($"{saveInvoice.ErrorMessage}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //assign values for Invoice Line Object
            InvoiceLine invoiceLine = new InvoiceLine();    
            var selectedRegType = (ComoBoxList)cBoxRegType.SelectedItem;
            var selctedItem = definationItems.FirstOrDefault(x => x.Reference == selectedRegType.Id);
            invoiceLine.invoice = invoice.code;
            invoiceLine.itemId = selctedItem.item_Id;
            invoiceLine.qty = 1;
            invoiceLine.unit_amount = invoice.subtotal;
            invoiceLine.total = invoice.subtotal;
            invoiceLine.taxable_amount = invoice.subtotal;
            invoiceLine.tax_amount = invoice.tax;
            //excute the dbContext for InvoiceLine
            var saveInvoiceLine = dbContext.saveInvoiceLine(invoiceLine);
            if (!saveInvoiceLine.IsPassed)
            {
                MessageBox.Show($"{saveInvoiceLine.ErrorMessage}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //assign values for patient assignment object
            PatientAssignment patientAssignment = new PatientAssignment();  
            var selectedAssignentType = (ComoBoxList)cBoxAssignType.SelectedItem;
            patientAssignment.patient_id =Patient.PatientID;
            patientAssignment.assignment_type = selectedAssignentType.Id;
            patientAssignment.assigned_to = cBoxAssignValue.Text.Trim();
            patientAssignment.Invoice = invoice.code;
            //excute the dbContext for Patientassignment
            var savePatientAssignment = dbContext.savePatientAssignment(patientAssignment);
            if (!savePatientAssignment.IsPassed)
            {
                MessageBox.Show($"{savePatientAssignment.ErrorMessage}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ///start the visit and create operation for that
            /// //access the operation started from defination table
            var startedOperation = definations.FirstOrDefault(x => x.description.Trim().ToLower() == ("Operation").ToLower() &&
                                                                                    x.type.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                    x.value.Trim().ToLower() == ("started").ToLower());
            if (startedOperation is null)
            {

                MessageBox.Show("You Can't Perform Start Visit Operation Now! Please Contact The Admnistrator!", "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var typeMenudefination = menuDefinations.FirstOrDefault(x => x.name.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                             x.parent.Trim().ToLower() == ("EMR").ToLower());

            Operation operation1 = new Operation();
            operation1.operation = startedOperation.id;
            operation1.color = "WhiteSmoke";
            operation1.type = typeMenudefination.id;
            operation1.is_final = true;
            operation1.remark = "Started Operation";
            var saveOperation1 = dbContext.saveOperation(operation1);
            if (!saveOperation1.IsPassed)
            {

                MessageBox.Show(saveOperation1.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            InvoiceOperation invoiceOperation1 = new InvoiceOperation();
            invoiceOperation1.invoice_id = VochourCode;
            invoiceOperation1.operation_id = saveOperation1.Data;
            invoiceOperation1.operation_datetime = DateTime.Now;
            invoiceOperation1.device = Environment.MachineName;
            var saveInvoiceOperation1 = dbContext.saveInvoiceOperation(invoiceOperation1);
            if (!saveInvoiceOperation1.IsPassed)
            {

                MessageBox.Show(saveInvoiceOperation1.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
              
                return;
            }
            UpdateInvoice updateInvoice = new UpdateInvoice();
            updateInvoice.consignee = Patient.PatientID;
            updateInvoice.code = VochourCode;
            updateInvoice.last_operation = saveInvoiceOperation1.Data;

            var updateLastOperation = dbContext.updateInvoice(updateInvoice);
            if (!updateLastOperation.IsPassed)
            {

                MessageBox.Show(updateLastOperation.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
             
                return;
            }
            var saveUpdatedVisit = dbContext.updateVisit(updateVisit);
            if (saveUpdatedVisit.IsPassed)
            {

                MessageBox.Show("Registration Card Is Renewed successfully and \n"+saveUpdatedVisit.SuccessMessage, "Success",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();   
                return;
            }
            else
            {
                MessageBox.Show(saveUpdatedVisit.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }
           
            
        }

        private void cBoxAssignType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxAssignType.BackColor = SystemColors.Window;
            var assignType = definations.Where(x => x.description == "registration assignment" && x.type == "EMR").ToList();
            var assignDoctor = assignType.FirstOrDefault(x => x.value == "doctor");
            ComoBoxList selectedItem = (ComoBoxList)cBoxAssignType.SelectedItem;
            if (selectedItem.Id == assignDoctor.id)
            {
                var doctorTypeDefinition = definations.FirstOrDefault(x =>
                                                    x.description == "person type" &&
                                                    x.type == "person" &&
                                                    x.value == "doctor");
                var doctorCategoryDefinition = definations.FirstOrDefault(x =>
                                                 x.description == "person category" &&
                                                 x.type == "person" &&
                                                 x.value == "employee");

                var doctors = persons.Where(x => x.type_Id == doctorTypeDefinition.id &&
                                                         x.category == doctorCategoryDefinition.id)
                                                        .Select(x => new Doctor
                                                        {
                                                            Id = x.Id,
                                                            Name = x.first_name + " " + x.middile_name + " " + x.last_name
                                                        }).ToList();
                cBoxAssignValue.DataSource = doctors;
                cBoxAssignValue.DisplayMember = "Name";
                cBoxAssignValue.ValueMember = "Id";
            }
            else
            {
                rooms.Select(x => new ComoBoxList
                {
                    Id = x.id,
                    Description = x.description,
                }).ToList();
                cBoxAssignValue.DataSource = rooms;
                cBoxAssignValue.DisplayMember = "Description";
                cBoxAssignValue.ValueMember = "Id";
            }
        }

        private void cBoxRegType_SelectedIndexChanged(object sender, EventArgs e)
        {

            cBoxRegType.BackColor = SystemColors.Window;
            ComoBoxList selectedItem = (ComoBoxList)cBoxRegType.SelectedItem;

            var priceItem = definationItems.FirstOrDefault(x => x.Reference == selectedItem.Id);
            txtRegAmount.Text = priceItem.Price.ToString();
            var setting = configurations.FirstOrDefault(x => x.type == 0 &&
                                                                                   x.description.Trim() == "isFlexiableAmount");
            if (bool.Parse(setting.value))
                txtRegAmount.Enabled = true;
            else txtRegAmount.Enabled = false;
        }
        public string generateVouchurID()
        {
            var patientDefination = menuDefinations.FirstOrDefault(x => x.parent.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                 x.name.Trim().ToLower() == ("Registration").ToLower());
            var patientIDDefinationDetail = idDefinitions.FirstOrDefault(x => x.type == patientDefination.id);
            var MaxIdValue = idDbContext.getMaxVouchurID();
            MaxIdValue = MaxIdValue == 0 ? 1 : MaxIdValue + 1;
            var centerPart = MaxIdValue.ToString().PadLeft(patientIDDefinationDetail.length, '0');
            var vouchurId = string.Format($"{patientIDDefinationDetail.prefix.Trim() + patientIDDefinationDetail.prefix_separator.Trim()}" +
                                               $"{centerPart}" + $"{patientIDDefinationDetail.suffix_separator.Trim() + patientIDDefinationDetail.suffix.Trim()}", MaxIdValue);

            return vouchurId;



        }

        private void comboBoxInvoiceTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxInvoiceTypes.BackColor=SystemColors.Window; 
        }

        private void txtRegAmount_TextChanged(object sender, EventArgs e)
        {
            txtRegAmount.BackColor=SystemColors.Window;
        }

        private void cBoxAssignValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxAssignValue.BackColor=SystemColors.Window;  
        }
    }
}
