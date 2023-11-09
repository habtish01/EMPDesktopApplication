using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using DevExpress.XtraScheduler.Native;
using Patient_Managment_System.Data_Access_Layer;
using Patient_Managment_System.Models;
using Patient_Managment_System.Properties;
using Patient_Managment_System.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;


using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Patient_Managment_System
{
    public partial class PatientMSystem : Form
    {
        #region Global Variables and Instances
        private patientRegistration validationHelper = new patientRegistration();
     
        IdGenerationDataAcess acess = new IdGenerationDataAcess();
        List<Person> persons = new List<Person>();
        DataAccessLayer layer = new DataAccessLayer();
        List<Patient> listofPatientDocument = new List<Patient>();
        IEnumerable<Patient> searchedPatient;
        Patient rowData;
        patientInfo patientInfo =new patientInfo();
        List<patientInfo> allPatients=new List<patientInfo>();
        patientInfo selectedPatient;
      
      
        #endregion

        public PatientMSystem()
        {
            InitializeComponent();
            var id=acess.idGeneration();
            txtId.Text = id.ToString();
            txtId.Enabled = false;
        }

        #region Application On Load Method
        private void PatientMSystem_Load(object sender, EventArgs e)
        {
           
            btnStart.Enabled = false;
            btnClose.Enabled = false;
            btnDelete.Enabled = false;
            /*
             * outer by habtish
             * combo box data for Visit Location
             */
            var dataList = layer.LoadListForVisitTypeCoboBox();
           
            cBoxVisitType.DataSource = dataList;
            cBoxVisitType.ValueMember = "Id";
            cBoxVisitType.DisplayMember = "Description";
            /*
            * outer by habtish
            * combo box data for Finance Type
            */
            
            var dataList1 = layer.LoadListForFinanceTypeCoboBox();
            cBoxFinanceType.DataSource = dataList1;
            cBoxFinanceType.ValueMember = "Id";
            cBoxFinanceType.DisplayMember = "Description";
            /*
            * outer by habtish
            * combo box data for Patient Assignment type
            */
         
            var datalist = layer.LoadListForAssignmentTypeCoboBox();
            cBoxAssignType.DataSource = datalist;
            cBoxAssignType.ValueMember = "Id";
            cBoxAssignType.DisplayMember = "Description";

            /*
             * outer by habtish
             * patient document grid view 
             */
            listofPatientDocument = layer.GetPatients();
            gridControlPatient.DataSource = listofPatientDocument;


            /*
             * outer by habtish
             * load all appointment list to view 
             */
            loadAppointments();

            
        }

        #endregion
        #region Save and Update Patient 
        //outer by Habtish
        //it takes all registration requirment and saves the patient
        //but if the patient is already exit and comes from the patient document, it also performs update action for that patient
        private void btnSave_Click(object sender, EventArgs e)
        {
            string personId;//takes the next id value when registration success                  

            // Validate First Name
            string firstname = txtFirstName.Text.Trim();
            if (!validationHelper.isFirstNameValid(firstname))
            {
                txtFirstName.BackColor = Color.LightPink;
                txtFirstName.Focus();                
                return;
            }

            // Validate Middle Name
            string middlename = txtMiddleName.Text.Trim();
            if (!validationHelper.isMiddleNameValid(middlename))
            {
                txtMiddleName.BackColor = Color.LightPink;
                txtMiddleName.Focus();
                return;
            }

            // Validate last Name
            string lastname = txtLastName.Text.Trim();
            if (!validationHelper.isLastNameValid(lastname))
            {
                txtLastName.BackColor = Color.LightPink; 
                return;
            }

            // Validate age Name
            string age = txtAge.Text.Trim();
            if (!validationHelper.isAgeValid(age))
            {
                txtAge.BackColor = Color.LightPink;
                txtAge.Focus();                
                return;
            }

            // Validate gender Name
            string gender = cBoxGeneder.Text.Trim();
            if (!validationHelper.isGenderValid(gender))
            {
                cBoxGeneder.BackColor = Color.LightPink; 
                cBoxGeneder.Focus();                
                return;
            }

            // Validate phone Name
            string phone = txtPhone.Text.Trim();

            if (!validationHelper.isPhoneNumberValid(phone))
            {
                txtPhone.BackColor = Color.LightPink;
                txtPhone.Focus();               
                return;
            }

            // Validate Registration Fee type
            string feeType = cBoxRegType.Text.Trim();
            if (!validationHelper.isRegistrationFeeTypeValid(feeType))
            {
                cBoxRegType.BackColor = Color.LightPink; 
                cBoxRegType.Focus(); 
                
                return;
            }


            // Validate registration fee amount
            string feeAmount = txtRegAmount.Text.Trim();
            if (!validationHelper.isRegistrationFeeAmountValid(feeAmount))
            {
                txtRegAmount.BackColor = Color.LightPink;
                txtRegAmount.Focus();                
                return;
            }

            // Validate finance type
            string financeType = cBoxFinanceType.Text.Trim();
            if (!validationHelper.isFinanceTypeValid(financeType))
            {
                cBoxFinanceType.BackColor = Color.LightPink; 
                cBoxFinanceType.Focus();                
                return;
            }


            // Validate finance amount
            string financeAmount = txtFinanceAmount.Text.Trim();
            if (!validationHelper.isFinanceAmountValid(financeAmount))
            {
                txtFinanceAmount.BackColor = Color.LightPink; 
                txtFinanceAmount.Focus();                
                return;
            }
            //validate assign type
            string assignType = cBoxAssignType.Text.Trim();
            if (!validationHelper.isDoctorAssignmentTypeValid(assignType))
            {
                cBoxAssignType.BackColor = Color.LightPink; 
                cBoxAssignType.Focus();                 
                return;
            }
            //validate asignment value
            string assignValue = cBoxAssignValue.Text.Trim();
            if (!validationHelper.isDoctorAssignmentValueValid(assignValue))
            {
                cBoxAssignValue.BackColor = Color.LightPink; 
                cBoxAssignValue.Focus();                 
                return;
            }

           
           //creating person object for that patient to save in the person table 
            Person person=new Person();
            person.Id =txtId.Text.Trim();    
            person.FirstName = txtFirstName.Text.Trim();  
            person.MiddleName = txtMiddleName.Text.Trim();
            person.LastName = txtLastName.Text.Trim();  
            person.Gender=cBoxGeneder.Text.Trim();
            person.Age=Convert.ToInt32(txtAge.Text.Trim());
            person.PhoneNumber=txtPhone.Text.Trim();  
            person.DateRegistered=DateTime.Now;
           
            //assign values to patientinfo model class
            patientInfo.PersonId = person.Id;
            patientInfo.FirstName=person.FirstName; 
            patientInfo.MiddleName=person.MiddleName;
            patientInfo.LastName=person.LastName;
            patientInfo.Age = person.Age;
            patientInfo.Gender= person.Gender;
            patientInfo.Phone = int.Parse(person.PhoneNumber);
            patientInfo.RegistrationDate = person.DateRegistered;

            
            //assign address values to address class
            
            Address address = new Address();
            address.City = txtCity.Text.Trim(); 
            address.SubCity = txtSubCity.Text.Trim();   
            address.Kebele = txtKebele.Text.Trim(); 
            address.HouseNo = txtHouseNo.Text.Trim();
            address.PatientId =layer.getPatientID(person.Id);
          

            patientInfo.City= address.City; 
            patientInfo.subCity=address.SubCity;
            patientInfo.Kebele= address.Kebele; 
            patientInfo.HouseNo= address.HouseNo;   

            ComoBoxList selectedListItem = (ComoBoxList)cBoxAssignType.SelectedItem;
            var assignId=selectedListItem.Id;
          
            cBoxAssignValue.DisplayMember = "Description";
            cBoxAssignValue.ValueMember = "Id";

            ComoBoxList selectedListValue = (ComoBoxList)cBoxAssignValue.SelectedItem;
           

            //checks the person exist or not
            if (!layer.checkPersonExistance(person.Id))
            {
                var isPatientInserted = layer.InsertPerson(person);//calls save the patient query
                address.PatientId = layer.getPatientID(person.Id);
                var isAddressRegistered = layer.InsertAddress(address);
                
                // if sucess , make all fields empty and generate the next patient Id value
                if (isPatientInserted && isAddressRegistered)
                {
                    personId = acess.idNo();
                   
                    txtFirstName.Text = string.Empty;
                    txtMiddleName.Text = string.Empty;
                    txtLastName.Text = string.Empty;
                    txtAge.Text = string.Empty;
                    txtPhone.Text = string.Empty;
                    cBoxGeneder.Text = string.Empty;
                    txtSubCity.Text = string.Empty;
                    txtCity.Text = string.Empty;
                    txtKebele.Text = string.Empty;
                    txtHouseNo.Text = string.Empty;
                    cBoxRegType.Text = string.Empty;
                    txtRegAmount.Text = string.Empty;
                    txtFinanceAmount.Text = string.Empty;
                    cBoxFinanceType.Text = string.Empty;
                    cBoxAssignType.Text = string.Empty;
                    cBoxAssignValue.Text = string.Empty;
                    cBoxVisitType.Text = string.Empty;
                    
                    MessageBox.Show("Registration Success", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtId.Text = personId;
                    persons.Add(person);
                    allPatients.Add(patientInfo);
                    


                }
                else
                {
                    MessageBox.Show("Registrationfailed!. No records inserted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            else
            {
                if (layer.isAddressExist(address.PatientId))
                {
                    var isAddressUpdated = layer.UpdateAddress(address);//calls upadte query,
                    var isPatientUpdated = layer.UpdatePatient(person);
                    if (isPatientUpdated && isAddressUpdated)
                    {
                        btnStart.Enabled = true;
                        btnSave.Enabled = false;
                        MessageBox.Show("Patient Update Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //txtId.Text = personId;



                    }
                    else
                    {
                        MessageBox.Show("Patient Update Failed!.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var isAddressUpdated = layer.InsertAddress(address);
                    var isPatientUpdated = layer.UpdatePatient(person);

                    if (isPatientUpdated && isAddressUpdated)
                    {
                        btnStart.Enabled = true;
                        btnSave.Enabled = false;
                        MessageBox.Show("Patient Update Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //txtId.Text = personId;



                    }
                    else
                    {
                        MessageBox.Show("Patient Update Failed!.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
               
              
                //call the data acess layer

               

            }
            
        }
        #endregion
        #region TextChanged Methods for Fields
        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            txtFirstName.BackColor = SystemColors.Window; // Change the background color

        }

        private void txtMiddleName_TextChanged(object sender, EventArgs e)
        {
            txtMiddleName.BackColor = SystemColors.Window; // Change the background color

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            txtLastName.BackColor = SystemColors.Window;
        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            txtAge.BackColor = SystemColors.Window;
        }

        private void cBoxGeneder_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxGeneder.BackColor = SystemColors.Window;
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            txtPhone.BackColor = SystemColors.Window;
        }

        private void txtSubCity_TextChanged(object sender, EventArgs e)
        {
            txtSubCity.BackColor = SystemColors.Window;
        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {
            txtCity.BackColor = SystemColors.Window;
        }

        private void txtKebele_TextChanged(object sender, EventArgs e)
        {
            txtKebele.BackColor = SystemColors.Window;
        }

        private void txtHouseNo_TextChanged(object sender, EventArgs e)
        {
            txtHouseNo.BackColor = SystemColors.Window;
        }

        private void cBoxRegType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxRegType.BackColor = SystemColors.Window;
        }

        private void txtRegAmount_TextChanged(object sender, EventArgs e)
        {
            txtRegAmount.BackColor = SystemColors.Window;
        }
      
        private void cBoxFinanceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxFinanceType.BackColor = SystemColors.Window;
        }

        private void txtFinanceAmount_TextChanged(object sender, EventArgs e)
        {
            txtFinanceAmount.BackColor = SystemColors.Window;
        }

        private void cBoxAssignType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxAssignType.BackColor = SystemColors.Window;
        }

        private void cBoxAssignValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxAssignValue.BackColor = SystemColors.Window;
        }
        private void txtId_TextChanged(object sender, EventArgs e)
        {
            txtId.BackColor = SystemColors.Window;
        }
        private void searchLookUpEditOrgnaization_EditValueChanged(object sender, EventArgs e)
        {

        }
        private void cBoxVisitType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion      
        #region Upload Profile Picture Method
        private void pictureBoxPatientProfile_Click(object sender, EventArgs e)
        {
           
        }
      

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            // Create an instance of the OpenFileDialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Get the selected image file path
                        string imagePath = openFileDialog.FileName;

                        // Load the image into the PictureBox
                        pictureBoxPatientProfile.Image = new System.Drawing.Bitmap(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion
        #region Reset Method For New Registration
        private void btnNew_Click(object sender, EventArgs e)
        {
            var id=acess.idGeneration();
            txtId.Text = id.ToString();
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtAge.Text = string.Empty;
            txtPhone.Text = string.Empty;
            cBoxGeneder.Text = string.Empty;
            txtSubCity.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtKebele.Text = string.Empty;
            txtHouseNo.Text = string.Empty;
            cBoxRegType.Text = string.Empty;
            txtRegAmount.Text = string.Empty;
            txtFinanceAmount.Text = string.Empty;
            cBoxFinanceType.Text = string.Empty; 
            cBoxAssignType.Text = string.Empty; 
            cBoxAssignValue.Text = string.Empty;    
            cBoxVisitType.Text = string.Empty;  
        }
        #endregion
        #region Exit App Method
        private void btnExitt_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
        #region Start Visit Method

        /*outer by habtish
          the method below used for to start visit for the registered patient 
        it takes the visit type and saves the visit data for that patient
        */
        private void btnStart_Click(object sender, EventArgs e)
        {

           var visitType=cBoxVisitType.Text.Trim();
            if(!validationHelper.isVisitTypeValueValid(visitType))
            {
                cBoxVisitType.BackColor = Color.LightPink;
                cBoxVisitType.Focus();
                return;
            }
            ComoBoxList selectedListItem = (ComoBoxList)cBoxVisitType.SelectedItem;
             var id=txtId.Text.Trim(); 
            //calls the   database context to excute the query to save the visit 

            DataAccessLayer layer = new DataAccessLayer();
            if (layer.checkVisitExistanceForPatient(id))
            {
                if (layer.updateVisitType(selectedListItem, id))
                {

                    btnClose.Enabled = true;
                 

                    MessageBox.Show("Visit Type Updated", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Visit Type Update Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                if (layer.InsertVisitType(selectedListItem, id))
                {
                    btnClose.Enabled = true;
                    btnStart.Enabled = false;

                    MessageBox.Show("Visit Type addedd", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Visit Type Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }







        }
        #endregion
        #region Close Visit Method
        private void btnClose_Click(object sender, EventArgs e)
        {
            var id = txtId.Text.Trim();

            DataAccessLayer layer = new DataAccessLayer();
            if (layer.UpdateStatus(id))
            {
                btnStart.Enabled = true;
                btnClose.Enabled = false;
                MessageBox.Show("Visit Status closed Successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion
        #region Refresh Patient Document Method
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var patients = layer.GetPatients();

            gridControlPatient.DataSource = patients;
        }
        #endregion
        #region KeyPress Methods for Fields
        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtMiddleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }
       
      

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch)&& ch!=8)
            {
                e.Handled = true;
            }
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtRegAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtFinanceAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtSubCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsLetter(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }
        private void cBoxAssignType_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void cBoxAssignValue_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cBoxVisitType_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void cBoxGeneder_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void txtKebele_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtHouseNo_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cBoxFinanceType_KeyPress(object sender, KeyPressEventArgs e)
        {

        }





        #endregion
        #region Row Click Method in Patient Document
        /*
         * outer by habtish
         * the method called when the row in the patient document clicked 
         * to update patient data
         * to sart or close the visit data for the patient
         * to delete the patient from the active patients data
         * it navigates to the general tab to see all information of the patient
         * */
        private void gridViewPatients_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
          rowData = (Patient)gridViewPatients.GetRow(e.RowHandle);
            var id = rowData.Id;
           
            selectedPatient = allPatients.FirstOrDefault(patient => patient.PersonId==id);
            var patientId=layer.getPatientId(rowData.Id);
            var adrress = layer.getPatientAdrressInfo(patientId);
            var patienttInfo=layer.getPatientInfo(rowData.Id);
            var location_id = layer.getVisitLocation(patientId);

            txtId.Text = rowData.Id;
            txtFirstName.Text = rowData.FirstName;
            txtMiddleName.Text =rowData.MiddleName;
            txtLastName.Text = rowData.LastName;
            txtAge.Text = patienttInfo.Age.ToString();
            txtPhone.Text = rowData.PhoneNumber;
            cBoxGeneder.Text = rowData.Gender;
            if (adrress != null)
            {
                txtSubCity.Text = adrress.SubCity != null ? adrress.SubCity : "";
                txtCity.Text = adrress.City != null ? adrress.City : "";
                txtKebele.Text = adrress.Kebele != null ? adrress.Kebele : "";
                txtHouseNo.Text = adrress.HouseNo != null ? adrress.HouseNo : "";
            }
            cBoxRegType.Text = string.Empty;
            txtRegAmount.Text = string.Empty;
            txtFinanceAmount.Text = string.Empty;
            cBoxFinanceType.Text = string.Empty;
            cBoxAssignType.Text = string.Empty;
            cBoxAssignValue.Text = string.Empty;
            //cBoxVisitType.Text = string.Empty;

            cBoxVisitType.SelectedValue = location_id;


            xtraTabControlRegistration.SelectedTabPage = xtraTabPageGeneral;
            btnSave.Enabled = true;
            btnClose.Enabled = true;
            btnStart.Enabled = true;
            btnDelete.Enabled = true;   
        }
        #endregion
        #region Soft Delete Patient Method
        /*
         * outer by habtish
         * delets the patient from active patient lists
         * it is soft delete to keep the patient history
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var id = txtId.Text.Trim(); 
            if (layer.DeletePatient(id))
            {
                MessageBox.Show("Patient Deleted Successfully","Success",MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnDelete.Enabled = false;  
            }
            else
            {
                MessageBox.Show("Patient Deleting Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDelete.Enabled=true;

            }
        }

        #endregion
        #region Load Appointments
        public void loadAppointments()
        {
            var appointmentSummaries=layer.loadAppointmentSummary();    

            gridControlAppointmentDocument.DataSource = appointmentSummaries;
        }    

        private void btnAppointmentRefresh_Click(object sender, EventArgs e)
        {
            loadAppointments();
        }
        #endregion
        #region Appointment Row Color Modification Based on Appointment Date
        private void gridViewAppointmentdocument_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {


            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (e.RowHandle >= 0)
            {

                DateTime cellValue = DateTime.Parse(view.GetRowCellValue(e.RowHandle,
                                                        "OrderedDate").ToString());


                if (cellValue > DateTime.Now)
                {
                    e.Appearance.BackColor =Color.LightGray;
                    
                }
                if (cellValue < DateTime.Now)
                {
                    e.Appearance.BackColor = Color.LightPink;
                }

                if (cellValue == DateTime.Now)
                    {
                        e.Appearance.BackColor = Color.Blue;
                        e.HighPriority = true;
                    }

            }
        }

        #endregion
        private void btnAccept_Click(object sender, EventArgs e)
        {

        }

       
    }
}

