using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraExport.Helpers;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Patient_Managment_System
{
    public partial class PatientMSystem : Form
    {

        private patientRegistration validationHelper = new patientRegistration();
        string personId;
            
        public PatientMSystem()
        {
            InitializeComponent();
        }




        //action for register button

        private void btnSave_Click(object sender, EventArgs e)
        {
            //constant values
            var dateregistered = System.DateTime.Now;
            int typeid = 1;
            var active = true;
            var remark = "registered";

            ///check the validation
            string id = txtId.Text.Trim();
            if (!validationHelper.isIdValid(id)) { 

                txtId.BackColor = Color.LightPink; 
                txtId.Focus();
                return;     

            }

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
              
            //assign values to person object
            Person person=new Person();
            person.Id = txtId.Text.Trim();
            person.FirstName = txtFirstName.Text.Trim();  
            person.MiddleName = txtMiddleName.Text.Trim();
            person.LastName = txtLastName.Text.Trim();  
            person.Gender=cBoxGeneder.Text.Trim();
            person.Age=Convert.ToInt32(txtAge.Text.Trim());
            person.PhoneNumber=txtPhone.Text.Trim();  
            person.DateRegistered=DateTime.Now;
            //call data access layer

            DataAccessLayer accessLayer = new DataAccessLayer();    
            var PatientRegister=accessLayer.InsertPerson(person);   
            
            //assign address values to address class
            Address address = new Address();
            address.City = txtCity.Text.Trim(); 
            address.SubCity = txtSubCity.Text.Trim();   
            address.Kebele = txtKebele.Text.Trim(); 
            address.HouseNo = txtHouseNo.Text.Trim();   
            address.PatientId=txtId.Text.Trim();
            //call the data acess layer
           var AddressRegister= accessLayer.InsertAddress(address);
            ComoBoxList selectedListItem = (ComoBoxList)cBoxAssignType.SelectedItem;
            var assignId=selectedListItem.Id;
           //var dataList= accessLayer.LoadListForAssignmentValueCoboBox(assignId); 
            //cBoxAssignValue.DataSource=dataList;
            cBoxAssignValue.DisplayMember = "Description";
            cBoxAssignValue.ValueMember = "Id";

            ComoBoxList selectedListValue = (ComoBoxList)cBoxAssignValue.SelectedItem;



            if (PatientRegister && AddressRegister)
                {
                 
                    btnStart.Enabled = true;  
                    btnSave.Enabled = false;
                    MessageBox.Show("Registration Success", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registrationfailed!. No records inserted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //          //Sql Connection
            //          SqlConnection conn = new SqlConnection("Data Source=HEAL-AFRICA-HEA;Initial Catalog=HAHC;Integrated Security=True");
            //          //sql command
            //          SqlCommand searchCmd = new SqlCommand(@"SELECT[Id]
            //    , [first_name]
            //    , [middile_name]
            //    , [last_name]

            //    , [date_registered]

            //    , [phone]

            //FROM[general].[person] where searchBy='"+searchText+"'");


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
            // Create an instance of the OpenFileDialog
            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
            //    openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            //    openFileDialog.FilterIndex = 1;

            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        try
            //        {
            //            // Get the selected image file path
            //            string imagePath = openFileDialog.FileName;

            //            // Load the image into the PictureBox
            //            pictureBoxPatientProfile.Image = new System.Drawing.Bitmap(imagePath);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}
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
            txtId.Text = string.Empty;
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

        private void PatientMSystem_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
           DataAccessLayer layer=new DataAccessLayer();
             var dataList=layer.LoadListForVisitTypeCoboBox();
                ///cobo box for visit type
            cBoxVisitType.DataSource = dataList; 
            cBoxVisitType.ValueMember ="Id";
            cBoxVisitType.DisplayMember = "Description";
            //combo box for Finance Type
           var dataList1= layer.LoadListForFinanceTypeCoboBox();
            cBoxFinanceType.DataSource = dataList1; 
            cBoxFinanceType.ValueMember="Id";
            cBoxFinanceType.DisplayMember = "Description";
            //combo box for assignment
            var datalist = layer.LoadListForAssignmentTypeCoboBox();
            cBoxAssignType.DataSource = datalist;
            cBoxAssignType.ValueMember="Id";    
            cBoxAssignType.DisplayMember="Description";


            var id = txtId.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                btnClose.Enabled = false;
            }

            var patients=layer.GetPatients();

            gridControlPatient.DataSource = patients;   
           // GridView gridView = PatientsDataGrid.MainView as GridView;
            // PatientsDataGrid.();
            //PatientsDataGrid.Columns.AddVisible("PropertyName1", "Column Header 1");
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
            if (layer.InsertVisitType(selectedListItem,id))
            {
                btnClose.Enabled = true;
               
                MessageBox.Show("Visit Type addedd", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }







        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var id = txtId.Text.Trim();

            DataAccessLayer layer = new DataAccessLayer();
            if (layer.UpdateStatus(id))
            {
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
    }
}
