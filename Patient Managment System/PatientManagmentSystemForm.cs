using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
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

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Patient_Managment_System
{
    public partial class PatientMSystem : Form
    {

        private patientRegistration validationHelper = new patientRegistration();
     
        IdGenerationDataAcess acess = new IdGenerationDataAcess();
        List<Person> persons = new List<Person>();
        DataAccessLayer layer = new DataAccessLayer();
        List<Patient> DataGridPatients = new List<Patient>();
        IEnumerable<Patient> searchedPatient;
        Patient rowData;
        patientInfo patientInfo =new patientInfo();
        List<patientInfo> allPatients=new List<patientInfo>();
        patientInfo selectedPatient;
        List<string> PatientId=new List<string>();
        List<AppointmentSummary> summary = new List<AppointmentSummary>();


        public PatientMSystem()
        {
            InitializeComponent();
            var id=acess.idGeneration();
            txtId.Text = id.ToString();
            txtId.Enabled = false;
        }

        private void PatientMSystem_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnClose.Enabled = false;
            btnDelete.Enabled = false;

            var dataList = layer.LoadListForVisitTypeCoboBox();
            ///cobo box for visit type
            cBoxVisitType.DataSource = dataList;
            cBoxVisitType.ValueMember = "Id";
            cBoxVisitType.DisplayMember = "Description";
            //combo box for Finance Type
            var dataList1 = layer.LoadListForFinanceTypeCoboBox();
            cBoxFinanceType.DataSource = dataList1;
            cBoxFinanceType.ValueMember = "Id";
            cBoxFinanceType.DisplayMember = "Description";
            //combo box for assignment
            var datalist = layer.LoadListForAssignmentTypeCoboBox();
            cBoxAssignType.DataSource = datalist;
            cBoxAssignType.ValueMember = "Id";
            cBoxAssignType.DisplayMember = "Description";


            var id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                btnClose.Enabled = false;
            }

            DataGridPatients = layer.GetPatients();
            
            //data grid forn patient document
            gridControlPatient.DataSource = DataGridPatients;


          //for patient id combo box

            foreach(var item in DataGridPatients)
            {
                if(item.Id!=null)
                    PatientId.Add(item.Id);

            }
            comboBoxPatientID.DataSource = PatientId;

            //for Services Combo box
            comboBoxServices.DataSource=layer.loadAppointmentServices();
            comboBoxServices.DisplayMember = "Description";
            comboBoxServices.ValueMember = "id";

            //for Location combo box
            comboBoxLocation.DataSource = layer.LoadListForVisitTypeCoboBox();
            comboBoxLocation.DisplayMember = "Description";
            comboBoxLocation.ValueMember = "id";
            ///for appointment summary 
            summary=layer.loadAppointmentSummary();
            gridControlAppointmentList.DataSource = summary.OrderByDescending(x => x.AppointmentDate.Date == DateTime.Today).ThenBy(x => x.AppointmentDate);
        }



        //action for register button

        private void btnSave_Click(object sender, EventArgs e)
        {
           
         
            //constant values
            var dateregistered = System.DateTime.Now;
            int typeid = 1;
            var active = true;
            var remark = "registered";
            string personId;

            ///check the validation



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

            // Validate Middle Name
            string lastname = txtLastName.Text.Trim();
            if (!validationHelper.isLastNameValid(lastname))
            {
                txtLastName.BackColor = Color.LightPink; 
                return;
            }

            // Validate Middle Name
            string age = txtAge.Text.Trim();
            if (!validationHelper.isAgeValid(age))
            {
                txtAge.BackColor = Color.LightPink;
                txtAge.Focus();                
                return;
            }

            // Validate Middle Name
            string gender = cBoxGeneder.Text.Trim();
            if (!validationHelper.isGenderValid(gender))
            {
                cBoxGeneder.BackColor = Color.LightPink; 
                cBoxGeneder.Focus();                
                return;
            }

            // Validate Middle Name
            string phone = txtPhone.Text.Trim();

            if (!validationHelper.isPhoneNumberValid(phone))
            {
                txtPhone.BackColor = Color.LightPink;
                txtPhone.Focus();               
                return;
            }

            //////////////////////////////////////////////////////////////////////////////////


       
            string feeType = cBoxRegType.Text.Trim();
            if (!validationHelper.isRegistrationFeeTypeValid(feeType))
            {
                cBoxRegType.BackColor = Color.LightPink; 
                cBoxRegType.Focus(); 
                
                return;
            }


            // Validate Middle Name
            string feeAmount = txtRegAmount.Text.Trim();
            if (!validationHelper.isRegistrationFeeAmountValid(feeAmount))
            {
                txtRegAmount.BackColor = Color.LightPink;
                txtRegAmount.Focus();                
                return;
            }

            // Validate Middle Name
            string financeType = cBoxFinanceType.Text.Trim();
            if (!validationHelper.isFinanceTypeValid(financeType))
            {
                cBoxFinanceType.BackColor = Color.LightPink; 
                cBoxFinanceType.Focus();                
                return;
            }


            // Validate Middle Name
            string financeAmount = txtFinanceAmount.Text.Trim();
            if (!validationHelper.isFinanceAmountValid(financeAmount))
            {
                txtFinanceAmount.BackColor = Color.LightPink; 
                txtFinanceAmount.Focus();                
                return;
            }

            string assignType = cBoxAssignType.Text.Trim();
            if (!validationHelper.isDoctorAssignmentTypeValid(assignType))
            {
                cBoxAssignType.BackColor = Color.LightPink; 
                cBoxAssignType.Focus();                 
                return;
            }

            string assignValue = cBoxAssignValue.Text.Trim();
            if (!validationHelper.isDoctorAssignmentValueValid(assignValue))
            {
                cBoxAssignValue.BackColor = Color.LightPink; 
                cBoxAssignValue.Focus();                 
                return;
            }

           
            
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
           


            if (!layer.checkPersonExistance(person.Id))
            {
                var isPatientInserted = layer.InsertPerson(person);
                address.PatientId = layer.getPatientID(person.Id);
                var isAddressRegistered = layer.InsertAddress(address);
                //call the data acess layer

                if (isPatientInserted && isAddressRegistered)
                {
                    personId = acess.idNo();
                    btnStart.Enabled = true;
                    //btnSave.Enabled = false;
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
                    var isAddressUpdated = layer.UpdateAddress(address);
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




        private void button5_Click(object sender, EventArgs e)
        {
            var SearchBy = cBoxSearchBy.Text.Trim();
            if (String.IsNullOrWhiteSpace(SearchBy))
            {
                cBoxSearchBy.BackColor = Color.LightPink;
                cBoxSearchBy.Focus();
                return;
            }

            var searchText = txtSearch.Text.Trim();
            if (String.IsNullOrWhiteSpace(searchText))
            {
                txtSearch.BackColor = Color.LightPink;
                txtSearch.Focus();
                return;
            }
            //searchPatient(SearchBy, searchText);
          

            if (SearchBy == "Phone Number")
            {
                searchedPatient = DataGridPatients.Where(member => member.PhoneNumber.Contains(searchText.TrimStart()));
                gridControlPatient.DataSource=searchedPatient.ToList();
                
            }
            else if(SearchBy=="Name")
            {
                searchedPatient = DataGridPatients.Where(member => member.FirstName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || member.MiddleName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || member.LastName.IndexOf(searchText) >= 0);
                gridControlPatient.DataSource=searchedPatient.ToList() ;
            }
            else
            {
                gridControlPatient.DataSource = DataGridPatients;
            }


        }

        private void cBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxSearchBy.BackColor = SystemColors.Window;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            txtSearch.BackColor = SystemColors.Window;
        }

        private void IdLabel_Click(object sender, EventArgs e)
        {

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

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            
            
                txtId.BackColor = SystemColors.Window;
               
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
       
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

       

        private void btnExit_Click_2(object sender, EventArgs e)
        {
            Application.Exit();
        }

     
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

        private void PatientsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gridControlPatient_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var patients = layer.GetPatients();

            gridControlPatient.DataSource = patients;
        }

       
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
       
      

        private void txtLastName_KeyPress_1(object sender, KeyPressEventArgs e)
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
            

            PatienRegTab.SelectedTab = tabPage1;
            btnSave.Enabled = true;
            btnClose.Enabled = true;
            btnStart.Enabled = true;
            btnDelete.Enabled = true;   
        }

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

        private void comboBoxSortby_SelectedIndexChanged(object sender, EventArgs e)
        {
           var value= comboBoxSortby.Text.Trim();
            if(value=="Name")
            gridControlPatient.DataSource=DataGridPatients.OrderBy(x => x.FirstName).ToList();
            else if(value=="ID")
            gridControlPatient.DataSource=DataGridPatients.OrderBy(x => x.Id).ToList();
            else if(value=="Phone Number")
            gridControlPatient.DataSource=DataGridPatients.OrderBy(x => x.PhoneNumber).ToList();
            else if(value=="Gender")
            gridControlPatient.DataSource=DataGridPatients.OrderBy(x => x.Gender).ToList();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment();
            AppointmentSummary appointment1 = new AppointmentSummary();  
           
            if (string.IsNullOrEmpty(comboBoxPatientID.Text.Trim()))
            {
                comboBoxPatientID.BackColor = Color.LightPink;
                comboBoxPatientID.Focus();
                return;
            }
            if(string.IsNullOrEmpty(comboBoxServices.Text.Trim()))
            {
                comboBoxServices.BackColor = Color.LightPink;   
                comboBoxServices.Focus(); return;   
            }
            if (string.IsNullOrEmpty(comboBoxLocation.Text.Trim()))
            {
                comboBoxLocation.BackColor = Color.LightPink;
                comboBoxLocation.Focus();   return;
            }
            if(appointmentDate.DateTime <= DateTime.Now)
            {
                appointmentDate.BackColor = Color.LightPink;    
                appointmentDate.Focus(); return;    
            }
            if (string.IsNullOrEmpty(appointmentNote.Text.Trim()))
            {
                appointmentNote.BackColor = Color.LightPink;
                appointmentNote.Focus(); return;
            }
            //if(!(checkBoxRecurring.Checked || checkBoxWalkIn.Checked)){
            //    checkBoxWalkIn.BackColor= Color.LightPink;  
            //    checkBoxWalkIn.Focus();
            //    checkBoxRecurring.BackColor= Color.LightPink;   
            //    checkBoxRecurring.Focus();  
            //    return; 

            //}
            ComoBoxList selectedListValueforLocation = (ComoBoxList)comboBoxLocation.SelectedItem;
            ComoBoxList selectedListValueforService = (ComoBoxList)comboBoxServices.SelectedItem;

            appointment.PateintId = comboBoxPatientID.Text.Trim();
            appointment.LocationId = selectedListValueforLocation.Id;
            appointment.ServiceId = selectedListValueforService.Id;
            appointment.Date = appointmentDate.DateTime;
            appointment.Note= appointmentNote.Text.Trim();
            appointment.Provider = "Habtish";
            appointment.Status = true;
            ///for summary list
            appointment1.PatientId = appointment.PateintId;
            appointment1.VisitLocation = selectedListValueforLocation.Description;
            appointment1.ServiceType = selectedListValueforService.Description;
            appointment1.Note=appointment.Note.Trim();
            appointment1.Status = appointment.Status == true ? "On Time" : "Late";
            appointment1.AppointmentDate = appointmentDate.DateTime;
            appointment1.Provider = appointment.Provider;

            appointment.Status = true;
            if(layer.createNewAppointment(appointment)){
                summary.Add(appointment1 );
                gridControlAppointmentList.DataSource = summary.OrderByDescending(x => x.AppointmentDate.Date == DateTime.Today).ThenBy(x => x.AppointmentDate);
                MessageBox.Show("Appointement Successfully Created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            else
            {
                MessageBox.Show("Appointment Creating Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
           

            }
        }

        private void appointmentDate_Click(object sender, EventArgs e)
        {
            appointmentDate.BackColor = SystemColors.Window;
            dateLabel.Text=appointmentDate.DateTime.ToString("yyyy-MM-dd");   
        }

        private void appointmentNote_TextChanged(object sender, EventArgs e)
        {
            appointmentNote.BackColor = SystemColors.Window;    
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(comboBox3.Text.Trim()=="Patient ID")
            {
                gridControlAppointmentList.DataSource=summary.OrderBy(x => x.PatientId);
            }
            else if (comboBox3.Text.Trim() == "Visit Location")
            {
                gridControlAppointmentList.DataSource = summary.OrderBy(x => x.VisitLocation);
            }
            else if (comboBox3.Text.Trim() == "Service Type")
            {
                gridControlAppointmentList.DataSource = summary.OrderBy(x => x.ServiceType);
            }
        }

        private void btnAppointmentRefresh_Click(object sender, EventArgs e)
        {
            summary = layer.loadAppointmentSummary();
            gridControlAppointmentList.DataSource = summary.OrderByDescending(x => x.AppointmentDate.Date==DateTime.Today).ThenBy(x=>x.AppointmentDate);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxRecurring.Checked)
            {
                checkBoxWalkIn.BackColor = SystemColors.Window; 
                checkBoxRecurring.BackColor = SystemColors.Window;
                checkBoxWalkIn.Checked = false;
            }
        }

        private void checkBoxWalkIn_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWalkIn.Checked)
            {
                checkBoxWalkIn.BackColor = SystemColors.Window;
                checkBoxRecurring.BackColor = SystemColors.Window;
                checkBoxRecurring.Checked = false;    
            }
        }
    }
    }

