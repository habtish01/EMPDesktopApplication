using Patient_Managment_System.Data_Access_Layer;
using Patient_Managment_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Patient_Managment_System
{
    public partial class VisitTypePopUp : Form
    {
        DbContext dbContext = new DbContext();
       public PatientDto patient { get; set; }  
       public List<Defination> definations { get; set; }
       public List<Configurations> configurations { get; set; }
        public   List<Voucher> menuDefinitions { get; set; }
        public List<Person> persons { get; set; }
        public List<RegistrationItem> definationItems { get; set; }
        public List<IdDefinitionDetail> idDefinations { get; set; }
        public List<Room> rooms { get; set; }
        public VisitTypePopUp()
        {
            InitializeComponent();
        }
        
        private void VisitTypePopUp_Load(object sender, EventArgs e)
        {
            lblPatientName.Text = patient.FirstName +" "+patient.MiddleName+" "+patient.LastName;
            lblPatientID.Text = patient.PersonID;
            List<ComoBoxList> dataList = dbContext.LoadListForVisitTypeCoboBox();

            comboBoxVisitType.DataSource = dataList;
            comboBoxVisitType.ValueMember = "Id";
            comboBoxVisitType.DisplayMember = "Description";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if(comboBoxVisitType.SelectedIndex < 0)
            {
                comboBoxVisitType.BackColor = Color.LightPink;
                comboBoxVisitType.Focus();
                return;
            }
            ComoBoxList selctedItm=(ComoBoxList)comboBoxVisitType.SelectedItem;
           Visit updateVisit = new Visit();
            updateVisit.patient_id = patient.PatientID;
            updateVisit.location_id = selctedItm.Id;
            updateVisit.status_id = 1;
            var isCardValid = configurations.FirstOrDefault(x=>x.description.Trim().ToLower()==("re-visit payment").ToLower());
            var lastRegGenerated = patient.LastInvoiceDate;
            var today= DateTime.Now;
            var dayRange = today.Day - lastRegGenerated.Day;
            
            if (dayRange > int.Parse(isCardValid.value))//the value should be int for re=visit payment entry
            {
                MessageBox.Show("The Card Is Expired. Please Re-Valid It!", "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                this.Close();
                RenewRegistrationCard renewRegistrationCard = new RenewRegistrationCard();
                renewRegistrationCard.definations = definations;
                renewRegistrationCard.menuDefinations = menuDefinitions;
                renewRegistrationCard.persons = persons;
                renewRegistrationCard.Patient=patient;
                renewRegistrationCard.definationItems = definationItems;
                renewRegistrationCard.configurations = configurations;
                renewRegistrationCard.idDefinitions = idDefinations;
                renewRegistrationCard.updateVisit = updateVisit;
                renewRegistrationCard.rooms = rooms;
                DialogResult result = renewRegistrationCard.ShowDialog();

                
                return;

            }
            //access the operation started from defination table
            var startedOperation = definations.FirstOrDefault(x => x.description.Trim().ToLower() == ("Operation").ToLower()&&
                                                                                    x.type.Trim().ToLower()==("EMR").ToLower()&&
                                                                                    x.value.Trim().ToLower()==("started").ToLower());
            if(startedOperation is null)
            {

                MessageBox.Show("You Can't Perform This Operation Now! Please Contact The Admnistrator!", "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var typeMenudefination = menuDefinitions.FirstOrDefault(x => x.name.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                              x.parent.Trim().ToLower() == ("EMR").ToLower());

            Operation operation = new Operation();
            operation.operation = startedOperation.id;
            operation.color = "WhiteSmoke";
            operation.type = typeMenudefination.id;
            operation.is_final = true;
            operation.remark = "Started Operation";
            var saveOperation=dbContext.saveOperation(operation);
            if (!saveOperation.IsPassed)
            {

                MessageBox.Show(saveOperation.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            InvoiceOperation invoiceOperation= new InvoiceOperation();
            invoiceOperation.invoice_id = patient.VochourCode;
            invoiceOperation.operation_id = saveOperation.Data;
            invoiceOperation.operation_datetime= DateTime.Now;  
            invoiceOperation.device=Environment.MachineName;
            var saveInvoiceOperation=dbContext.saveInvoiceOperation(invoiceOperation);
            if (!saveInvoiceOperation.IsPassed)
            {

                MessageBox.Show(saveInvoiceOperation.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateInvoice updateInvoice = new UpdateInvoice();
            updateInvoice.consignee = patient.PatientID;
            updateInvoice.code = patient.VochourCode;
            updateInvoice.last_operation = saveInvoiceOperation.Data;

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

                MessageBox.Show(saveUpdatedVisit.SuccessMessage, "Success",
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

        private void comboBoxVisitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxVisitType.BackColor=SystemColors.Window;
        }
    }
}
