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
using TextBox = System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using DevExpress.XtraGrid.Views.Card;
using Patient_Managment_System.DTO;

namespace Patient_Managment_System
{
    public partial class PatientMSystem : Form
    {
        #region Global Variables and Instances
        DevExpress.XtraEditors.DateEdit dailyTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit toWeaklyTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit fromWeaklyTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit monthlyTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit toMonthlyTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit yearlyTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit fromTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit toTimePicker = new DateEdit();
        DevExpress.XtraEditors.DateEdit anyDateTimePicker = new DateEdit();
        System.Windows.Forms.ComboBox dateFilterCombo = new System.Windows.Forms.ComboBox();
        System.Windows.Forms.TextBox txtBoxPhoneNumber = new System.Windows.Forms.TextBox();
        System.Windows.Forms.TextBox txtBoxPatientName = new System.Windows.Forms.TextBox();
        List<string> lists = new List<string> { "Daily", "Weakly", "Monthly", "Yearly", "Date Range", "Day Off" };
        List<string> filterByList = new List<string> { "Phone Number", "Patient Name", "Registered Date" };
        bool collapseSidePanel = true;
        private patientRegistration validationHelper = new patientRegistration();
     
        IdGenerationDataAcess acess = new IdGenerationDataAcess();
        List<Person> persons = new List<Person>();
        DataAccessLayer layer = new DataAccessLayer();
        List<Patient> patientDocuments = new List<Patient>(); 
        List<PatientDocument> listofPatientDocument = new List<PatientDocument>(); 
        IEnumerable<Patient> searchedPatient;
        Patient rowData;
        //patientInfo patientInfo =new patientInfo();
        //List<patientInfo> allPatients=new List<patientInfo>();
        patientInfo selectedPatient;
        List<string> genders = new List<string> {"Male","Female"};
        List<string> invoiceTypes = new List<string> {"Cash","Credit"};
        List<string> registrationFees = new List<string> {"Doctor","Specialist"};
        List<string> patientAssignments = new List<string> {"Doctor","Room Number"};
        List<string> roomNumbers = new List<string> {"Room 1","Room 2","Room 3","Room 4"};
        List<string> doctors = new List<string> {"Dr xxxx","Dr yyyyy","Dr zzzzzz"};
        List<string> patienttypes = new List<string> {"Individual","Organization"};
        List<string> organizations = new List<string> {"Heal Africa 1", "Heal Africa 2", "Heal Africa 3", "Heal Africa 4", "Heal Africa 5" };

        public Patient updatedPatient;
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
            //collapse the side panel for patient document page at first load
            sidePanelForPatintInfo.Width = 0;
            collapseSidePanel = false;
            gridControlPatient.Dock = DockStyle.Fill;
            /////////////////////////////////////////
            
            cBoxGeneder.DataSource = genders;//combo box for Gender
            comboBoxInvoiceTypes.DataSource= invoiceTypes;//combo box for Invoice Type
            cBoxRegType.DataSource = registrationFees;//combo box for Registration fee Type
            comboBoxPatientType.DataSource = patienttypes;//combo box for Patient Type
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
            patientDocuments = layer.GetPatients();
            listofPatientDocument = patientDocuments.Select(x=> new PatientDocument
            {
                Id=x.PersonID,
                Name=x.FirstName+" "+ x.MiddleName+" "+ x.LastName,
                Age=x.Age,
                Gender=x.Gender,
                PhoneNumber=x.PhoneNumber,
                VisitType=x.VisitType,  
                DateRegistered=x.DateRegistered,
                City=x.City,    
                SubCity=x.SubCity,  
                Kebele=x.Kebele,    
                HouseNo=x.HouseNo,  
                Active=x.Active
               
            }).Distinct()
            .ToList();

            gridControlPatient.DataSource = listofPatientDocument;


            /*
             * outer by habtish
             * load all appointment list to view 
             */
                    loadAppointments();
            ///combo box for Filter By in patient Document
            comboBoxFilterBy.DataSource=filterByList;



        }

        #endregion
        #region Save and Update Patient 
        //outer by Habtish
        //it takes all registration requirment and saves the patient
        //but if the patient is already exit and comes from the patient document, it also performs update action for that patient
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region
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
                    txtLastName.Focus();
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
                if (comboBoxInvoiceTypes.SelectedIndex < 0)
                {
                    comboBoxInvoiceTypes.SelectedIndex = 0;
                }

                if (cBoxVisitType.SelectedIndex <= 0)
                {
                    cBoxVisitType.BackColor = Color.LightPink;
                    cBoxVisitType.Focus();
                    return;
                }

                #endregion

                //creating person object for that patient to save in the person table 
                Person person = new Person();
                person.Id = txtId.Text.Trim();
                person.FirstName = txtFirstName.Text.Trim();
                person.MiddleName = txtMiddleName.Text.Trim();
                person.LastName = txtLastName.Text.Trim();
                person.Gender = cBoxGeneder.Text.Trim();
                person.Age = Convert.ToInt32(txtAge.Text.Trim());
                person.PhoneNumber = txtPhone.Text.Trim();
                person.DateRegistered = DateTime.Now;              

                Address address = new Address();
                address.City = txtCity.Text.Trim();
                address.SubCity = txtSubCity.Text.Trim();
                address.Kebele = txtKebele.Text.Trim();
                address.HouseNo = txtHouseNo.Text.Trim();


                //for visit Location
                ComoBoxList selectedVisitLocationItem = (ComoBoxList)cBoxVisitType.SelectedItem;


                //checks the person exist or not
                if (!layer.checkPersonExistance(person.Id))
                {
                    Response response = layer.InsertPerson(person);//calls save the patient query

                    int patientId = layer.getPatientID(person.Id);
                      address.PatientId = patientId;

                    //insert its visit location
                    var visitLocationId = selectedVisitLocationItem.Id;
                    var isVisitInserted = layer.InsertVisitType(visitLocationId, patientId);

                    //                   
                    var isAddressRegistered = layer.InsertAddress(address);

                    // if sucess , make all fields empty and generate the next patient Id value
                    if (response.IsPassed && isAddressRegistered)//add address check
                    {

                        ////////next patient id////////
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
                        cBoxAssignType.Text = string.Empty;
                        cBoxAssignValue.Text = string.Empty;
                        cBoxVisitType.Text = string.Empty;

                        MessageBox.Show("Registration Success", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtId.Text = personId;
                        persons.Add(person);
                        



                    }
                    else
                    {
                        MessageBox.Show("Registrationfailed!. No records inserted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                else
                {
                    address.PatientId = updatedPatient.PatientID;//patient Id
                    if (layer.isAddressExist(address.PatientId))
                    {
                        var isAddressUpdated = layer.UpdateAddress(address);//calls upadte query,
                        var isPatientUpdated = layer.UpdatePatient(person);
                        if (isPatientUpdated && isAddressUpdated)
                        {
                    
                            MessageBox.Show("Patient Update Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        }
                        else
                        {
                            MessageBox.Show("Patient Update Failed!.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        var isAddressInserted = layer.InsertAddress(address);
                        var isPatientUpdated = layer.UpdatePatient(person);

                        if (isPatientUpdated && isAddressInserted)
                        {

                          
                            MessageBox.Show("Patient Update Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          



                        }
                        else
                        {
                            MessageBox.Show("Patient Update Failed!.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"Something UnExcepected Happened.Please Try Again\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


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
      
       
       
        private void cBoxAssignType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxAssignType.BackColor = SystemColors.Window;

            if (cBoxAssignType.SelectedIndex == 0)
            {
                cBoxAssignValue.DataSource = doctors;
            }
            if (cBoxAssignType.SelectedIndex == 1)
            {
                cBoxAssignValue.DataSource = roomNumbers;
            }
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
            cBoxVisitType.BackColor = SystemColors.Window;  
        }
        private void comboBoxOrgnization_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxOrgnization.BackColor = Color.White;
        }

        private void comboBoxInvoiceTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxInvoiceTypes.BackColor = Color.White;
        }

        private void txtRegAmount_TextChanged_1(object sender, EventArgs e)
        {
            txtRegAmount.BackColor = Color.White;
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
        */
        #endregion
        #region Close Visit Method
        /*private void btnClose_Click(object sender, EventArgs e)
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
        */
        #endregion
        #region Refresh Patient Document Method
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            patientDocuments = layer.GetPatients();
            listofPatientDocument = patientDocuments.Select(x => new PatientDocument
            {
                Id = x.PersonID,
                Name = x.FirstName + " " + x.MiddleName + " " + x.LastName,
                Age = x.Age,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNumber,
                VisitType = x.VisitType,
                DateRegistered = x.DateRegistered,
                City = x.City,
                SubCity = x.SubCity,
                Kebele = x.Kebele,
                HouseNo = x.HouseNo,
                Active = x.Active
            }).Distinct().ToList();
            gridControlPatient.DataSource = listofPatientDocument;
            gridViewPatients.RefreshData();     
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


        #endregion       
        #region Soft Delete Patient Method
        /*
         * outer by habtish
         * delets the patient from active patient lists
         * it is soft delete to keep the patient history
         */
        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    var id = txtId.Text.Trim(); 
        //    if (layer.DeletePatient(id))
        //    {
        //        MessageBox.Show("Patient Deleted Successfully","Success",MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        btnDelete.Enabled = false;  
        //    }
        //    else
        //    {
        //        MessageBox.Show("Patient Deleting Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        btnDelete.Enabled=true;

        //    }
        //}

        #endregion
        #region Load Appointments
        public void loadAppointments()
        {
            var appointmentSummaries=layer.loadAppointmentSummary();    

            gridControlAppointmentdocument.DataSource = appointmentSummaries;
        }    

        private void btnAppointmentRefresh_Click(object sender, EventArgs e)
        {
            loadAppointments();
        }
        #endregion
        #region Appointment Row Color Modification Based on Appointment Date
        private void gridViewAppointmentDocument_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {


            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (e.RowHandle >= 0)
            {

                DateTime cellValue = DateTime.Parse(view.GetRowCellValue(e.RowHandle,
                                                        "OrderedDate").ToString());


                if (cellValue.Date > DateTime.Today)
                {
                    e.Appearance.BackColor =Color.LightGray;
                    
                }
                if (cellValue.Date < DateTime.Today)
                {
                    e.Appearance.BackColor = Color.LightPink;
                }

                if (cellValue.Date == DateTime.Today)
                    {
                        e.Appearance.BackColor = Color.LightBlue;
                        e.HighPriority = true;
                    }

            }
        }

        #endregion               
        #region Patient Document Menu
        private void gridViewPatients_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.HitInfo.InRow)
            {
                // Show the context menu only when right-clicking on a row
                e.Menu.Items.Clear();
                PatientDocument rowData = gridViewPatients.GetRow(e.HitInfo.RowHandle) as PatientDocument;
                var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == rowData.Id);

                var updateItem =new DevExpress.Utils.Menu.DXMenuItem("UPDATE",
                                             (s, args)=>OnCustomActionUpdate( rowData));
                
                    var startVisit = new DevExpress.Utils.Menu.DXMenuItem("START VISIT",
                                                 (s, args) => OnCustomActionStartVisit(rowData));
                    e.Menu.Items.Add(startVisit);
               
                    var closeVisit = new DevExpress.Utils.Menu.DXMenuItem("CLOSE VISIT",
                                                 (s, args) => OnCustomActionCloseVisit(rowData));
                if (clickedPatient.VisitStatusID == 1)
                {
                    closeVisit.Enabled = true;
         
                    startVisit.Enabled = false;
          
                }
                if (clickedPatient.VisitStatusID == 2)
                {
                    closeVisit.Enabled = false;
                
                    startVisit.Enabled = true;
                  
                }


                e.Menu.Items.Add(closeVisit);
                

                var patientAssign = new DevExpress.Utils.Menu.DXMenuItem("ASSIGN PATEINT",
                                             (s, args) => OnCustomActionPatientAssign(rowData));
                var deposit = new DevExpress.Utils.Menu.DXMenuItem("Deposit",
                                            (s, args) => OnCustomActionPatientDeposit(rowData));

                // showRowDataItem.ImageOptions.Image = Properties.Resources.;
                SuperToolTip superToolTip = new SuperToolTip();
                superToolTip.Items.Add("Click to show detailed row data");
                updateItem.SuperTip = superToolTip;

                //add to the menu items
                e.Menu.Items.Add(updateItem);
                
              
                e.Menu.Items.Add(patientAssign);
                e.Menu.Items.Add(deposit);
            }
        }

        private void OnCustomActionPatientDeposit(PatientDocument patient)
        {
            var clickedPatient= patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
            DepositPopUp depositPop=new DepositPopUp();
            depositPop.patient = clickedPatient;
            DialogResult result=depositPop.ShowDialog();    
        }

        private void OnCustomActionUpdate(PatientDocument patient)
        {
            var clickedPatient= patientDocuments.FirstOrDefault(x=>x.PersonID==patient.Id);
            updatedPatient = clickedPatient;

            txtId.Text = patient.Id;
            txtFirstName.Text = clickedPatient.FirstName.Trim();
            txtMiddleName.Text = clickedPatient.MiddleName.Trim();
            txtLastName.Text = clickedPatient.LastName.Trim();
            txtAge.Text = clickedPatient.Age.ToString().Trim();
            txtPhone.Text = clickedPatient.PhoneNumber.Trim();
            cBoxGeneder.Text = clickedPatient.Gender.Trim();
            txtCity.Text = clickedPatient.City.Trim();
            txtSubCity.Text =clickedPatient.SubCity.Trim();
            txtKebele.Text =clickedPatient.Kebele.Trim();
            txtHouseNo.Text =clickedPatient.HouseNo.Trim();
            
            cBoxRegType.Text = string.Empty;
            txtRegAmount.Text = string.Empty;
            cBoxAssignType.Text = string.Empty;
            cBoxAssignValue.Text = string.Empty;
            //cBoxVisitType.Text = string.Empty;

            cBoxVisitType.SelectedValue = clickedPatient.LocationID;


            xtraTabControlRegistration.SelectedTabPage = xtraTabPageGeneral;
            
        }
        private void OnCustomActionStartVisit(PatientDocument patient)
        {
            var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
            VisitTypePopUp visitTypePopUp = new VisitTypePopUp();
            visitTypePopUp.patient = clickedPatient;          
            DialogResult result = visitTypePopUp.ShowDialog();
       
            
        }
        private void OnCustomActionCloseVisit(PatientDocument patient)
        {
            var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
            var closed=layer.UpdateStatus(clickedPatient.PatientID);
            if (closed)
            {

                MessageBox.Show("Patient Visit Closed!", "Success",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void OnCustomActionPatientAssign(PatientDocument patient)
        {
            var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
             PatientAssignPopUp assignPopUp = new PatientAssignPopUp(); 
            assignPopUp.patient= clickedPatient;
            DialogResult result=assignPopUp.ShowDialog();
           
        }
        #endregion
        #region Patient Type Selected Changed
        private void comboBoxPatientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxPatientType.BackColor = Color.White;
            if(comboBoxPatientType.SelectedIndex == 0)
            {
                groupBoxPatientType.Controls.Remove(comboBoxOrgnization);
                groupBoxPatientType.Controls.Remove(lblOrganization);

            }
            if(comboBoxPatientType.SelectedIndex == 1)
            {
                groupBoxPatientType.Controls.Add(comboBoxOrgnization);
                groupBoxPatientType.Controls.Add(lblOrganization);
                comboBoxOrgnization.DataSource = organizations;

            }
        }
        #endregion
        #region Search Pateints
        private void btnShowSearch_Click(object sender, EventArgs e)
        {
            //search by phone 
            if (comboBoxFilterBy.SelectedIndex == 0)
            {
                if (string.IsNullOrWhiteSpace(txtBoxPhoneNumber.Text.Trim()))
                {
                    txtBoxPhoneNumber.BackColor = Color.LightPink;
                    txtBoxPhoneNumber.Focus();
                    return;
                }
                var phoneNumber = txtBoxPhoneNumber.Text.Trim();
                var searchedresult= listofPatientDocument.Where(x=>x.PhoneNumber.Contains(phoneNumber.TrimStart('0'))).ToList(); 
                if(searchedresult.Count == 0)
                {
                    MessageBox.Show("Patient Not Found","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    gridControlPatient.DataSource = listofPatientDocument;
                }
                else if(searchedresult.Count >=1)
                {
                    gridControlPatient.DataSource = searchedresult;
                }
            }
            //search by name 
            if (comboBoxFilterBy.SelectedIndex == 1)
            {
                if (string.IsNullOrWhiteSpace(txtBoxPatientName.Text.Trim()))
                {
                    txtBoxPatientName.BackColor = Color.LightPink;
                    txtBoxPatientName.Focus();
                    return;
                }
                var name = txtBoxPatientName.Text.Trim().ToLower();
                var searchedresult = listofPatientDocument.Where(x => 
                                               x.Name.ToLower().Contains(name)).ToList();

                if (searchedresult.Count == 0)
                {
                    MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gridControlPatient.DataSource = listofPatientDocument;

                }
                else if (searchedresult.Count >=1)
                {
                    gridControlPatient.DataSource = searchedresult;
                }
            }
            //search by Date 
            if(comboBoxFilterBy.SelectedIndex == 2)
            {
                //filter today registered patients
                if(dateFilterCombo.SelectedIndex == 0)
                {
                    if (string.IsNullOrEmpty(dailyTimePicker.DateTime.ToString()))
                    {
                        dailyTimePicker.BackColor = Color.LightPink;    
                        dailyTimePicker.Focus();    
                        return;
                    }
                    var dailyDate = DateTime.Today.ToString();
                    var searchedResult = listofPatientDocument.Where(x =>
                                                         x.DateRegistered.Date.ToString().Contains(dailyDate)).ToList();
                    if (searchedResult.Count == 0)
                    {
                        MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlPatient.DataSource = listofPatientDocument;
                    }
                    else if(searchedResult.Count>=1)
                    {
                        gridControlPatient.DataSource = searchedResult;
                    }
                }
                //filter in this weak registered patients
                if (dateFilterCombo.SelectedIndex == 1)
                {
                    
                    var dayOfaWeak = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).Date;
                    var today=DateTime.Today.Date;
                   
                    var searchedResult = listofPatientDocument.Where(x =>
                                                         x.DateRegistered.Date>=dayOfaWeak && x.DateRegistered.Date<=today).ToList();
                    if (searchedResult.Count == 0)
                    {
                        MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlPatient.DataSource = listofPatientDocument;
                    }
                    else if (searchedResult.Count >= 1)
                    {
                        gridControlPatient.DataSource = searchedResult;
                    }
                }
                //filter in this month registered patients
                if (dateFilterCombo.SelectedIndex == 2)
                {


                    var today = DateTime.Today.Date;
                    DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1).Date;

                    

                    var searchedResult = listofPatientDocument.Where(x =>
                                                         x.DateRegistered.Date >= firstDayOfMonth && x.DateRegistered.Date <= today).ToList();
                    if (searchedResult.Count == 0)
                    {
                        MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlPatient.DataSource = listofPatientDocument;
                    }
                    else if (searchedResult.Count >= 1)
                    {
                        gridControlPatient.DataSource = searchedResult;
                    }
                }
                //filter in this year registered patients
                if (dateFilterCombo.SelectedIndex == 3)
                {


                    var todayYear = DateTime.Today.Year.ToString();               



                    var searchedResult = listofPatientDocument.Where(x =>
                                                         x.DateRegistered.Year.ToString().Contains(todayYear)).ToList();
                    if (searchedResult.Count == 0)
                    {
                        MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlPatient.DataSource = listofPatientDocument;
                    }
                    else if (searchedResult.Count >= 1)
                    {
                        gridControlPatient.DataSource = searchedResult;
                    }
                }
                //filter in selected date Range registered patients
                if (dateFilterCombo.SelectedIndex == 4)
                {
                    if(string.IsNullOrEmpty(fromTimePicker.Text.Trim()))
                    {
                        fromTimePicker.BackColor = Color.LightPink;
                        fromTimePicker.Focus();
                        return;

                    }
                    if (string.IsNullOrEmpty(toTimePicker.Text.Trim()))
                    {
                        toTimePicker.BackColor = Color.LightPink;
                        toTimePicker.Focus();
                        return;

                    }
                    var fromDate=fromTimePicker.DateTime.Date;    
                    var toDate=toTimePicker.DateTime.Date;  


                    var searchedResult = listofPatientDocument.Where(x =>
                                                         x.DateRegistered.Date>=fromDate && x.DateRegistered.Date<=toDate).ToList();
                    if (searchedResult.Count == 0)
                    {
                        MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlPatient.DataSource = listofPatientDocument;
                    }
                    else if (searchedResult.Count >= 1)
                    {
                        gridControlPatient.DataSource = searchedResult;
                    }
                }
                //filter in selected date Range registered patients
                if (dateFilterCombo.SelectedIndex == 5)
                {
                    if (string.IsNullOrEmpty(anyDateTimePicker.Text.Trim()))
                    {
                        fromTimePicker.BackColor = Color.LightPink;
                        fromTimePicker.Focus();
                        return;
                    }
               
                    var anyDate = anyDateTimePicker.DateTime.Date.ToString();                


                    var searchedResult = listofPatientDocument.Where(x =>
                                                         x.DateRegistered.Date.ToString().Contains(anyDate)).ToList();
                    if (searchedResult.Count == 0)
                    {
                        MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlPatient.DataSource = listofPatientDocument;
                    }
                    else if (searchedResult.Count >= 1)
                    {
                        gridControlPatient.DataSource = searchedResult;
                    }
                }

            }

        }
        private void comboBoxFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFilterBy.SelectedIndex == 0)//phone number
            {

                txtBoxPhoneNumber.Location = new Point(3, 25);
                txtBoxPhoneNumber.Size = new Size(200, 30);
                txtBoxPhoneNumber.Multiline = true;
                panelFilterCriteria.Controls.Remove(dateFilterCombo);
                panelFilterCriteria.Controls.Remove(txtBoxPatientName);
                panelFilterCriteria.Controls.Add(txtBoxPhoneNumber);

                panelDateCriteria.Controls.Remove(dailyTimePicker);
                panelDateCriteria.Controls.Remove(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(monthlyTimePicker);
                panelDateCriteria.Controls.Remove(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(yearlyTimePicker);
                panelDateCriteria.Controls.Remove(fromTimePicker);
                panelDateCriteria.Controls.Remove(toTimePicker);
                panelDateCriteria.Controls.Remove(anyDateTimePicker);
                //label managing
                lblFilterrName.Text = comboBoxFilterBy.Text.Trim() + ":";
                lblDateFilterTitle.Text = string.Empty;
                lblFromDate.Text = string.Empty;
                lblToDate.Text = string.Empty;
            }
            if (comboBoxFilterBy.SelectedIndex == 1)//name
            {

                txtBoxPatientName.Location = new Point(3, 25);
                txtBoxPatientName.Size = new Size(200, 30);
                txtBoxPatientName.Multiline = true;
                panelFilterCriteria.Controls.Remove(txtBoxPhoneNumber);
                panelFilterCriteria.Controls.Remove(dateFilterCombo);
                panelFilterCriteria.Controls.Add(txtBoxPatientName);

                panelDateCriteria.Controls.Remove(dailyTimePicker);
                panelDateCriteria.Controls.Remove(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(monthlyTimePicker);
                panelDateCriteria.Controls.Remove(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(yearlyTimePicker);
                panelDateCriteria.Controls.Remove(fromTimePicker);
                panelDateCriteria.Controls.Remove(toTimePicker);
                panelDateCriteria.Controls.Remove(anyDateTimePicker);
                //label managing
                lblFilterrName.Text = comboBoxFilterBy.Text.Trim() + ":";
                lblDateFilterTitle.Text = string.Empty;
                lblFromDate.Text = string.Empty;
                lblToDate.Text = string.Empty;

            }

            if (comboBoxFilterBy.SelectedIndex == 2)//date
            {

                dateFilterCombo.Location = new Point(8, 25);
                dateFilterCombo.Size = new Size(150, 30);
                dateFilterCombo.SelectedIndexChanged += Date_SelectedIndexChanged;
                panelFilterCriteria.Controls.Remove(txtBoxPhoneNumber);
                panelFilterCriteria.Controls.Remove(txtBoxPatientName);
                panelFilterCriteria.Controls.Add(dateFilterCombo);
                dateFilterCombo.DataSource = lists;
                dateFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                //lable managing
                lblFilterrName.Text = comboBoxFilterBy.Text.Trim() + ":";
                //lblToDate.Text= string.Empty;   
                //lblFromDate.Text = string.Empty;
                //lblDateFilterTitle.Text = string.Empty;    


            }

        }


        private void Date_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dateFilterCombo.SelectedIndex == 0)
            {
                dailyTimePicker.Location = new Point(0, 30);
                dailyTimePicker.Size = new Size(150, 30);
                dailyTimePicker.Font = new Font("Times New Roman", 11);
                dailyTimePicker.Enabled = false;
                panelDateCriteria.Controls.Add(dailyTimePicker);
                panelDateCriteria.Controls.Remove(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(monthlyTimePicker);
                panelDateCriteria.Controls.Remove(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(yearlyTimePicker);
                panelDateCriteria.Controls.Remove(fromTimePicker);
                panelDateCriteria.Controls.Remove(toTimePicker);
                panelDateCriteria.Controls.Remove(anyDateTimePicker);
                //label managing
                lblDateFilterTitle.Text = "Today's Date";
                lblToDate.Text = string.Empty;
                lblFromDate.Text = string.Empty;


            }
            if (dateFilterCombo.SelectedIndex == 1)
            {
                fromWeaklyTimePicker.Location = new Point(0, 30);
                fromWeaklyTimePicker.Size = new Size(150, 30);
                fromWeaklyTimePicker.Font = new Font("Times New Roman", 11);
                fromWeaklyTimePicker.Enabled = false;
                fromWeaklyTimePicker.DateTime = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);

                toWeaklyTimePicker.Location = new Point(0, fromWeaklyTimePicker.Location.Y + fromWeaklyTimePicker.Height + 20);
                toWeaklyTimePicker.Size = new Size(150, 30);
                toWeaklyTimePicker.Font = new Font("Times New Roman", 11);
                toWeaklyTimePicker.Enabled = false;
                toWeaklyTimePicker.DateTime = DateTime.Today;

                panelDateCriteria.Controls.Add(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Add(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(dailyTimePicker);
                panelDateCriteria.Controls.Remove(monthlyTimePicker);
                panelDateCriteria.Controls.Remove(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(yearlyTimePicker);
                panelDateCriteria.Controls.Remove(fromTimePicker);
                panelDateCriteria.Controls.Remove(toTimePicker);
                panelDateCriteria.Controls.Remove(anyDateTimePicker);
                //lable managing
                lblDateFilterTitle.Text = "Today's Weak Range";
                lblFromDate.Text = "From:";
                lblToDate.Text = "To:";

            }
            if (dateFilterCombo.SelectedIndex == 2)
            {
                //from
                monthlyTimePicker.Location = new Point(0, 30);
                monthlyTimePicker.Size = new Size(150, 30);
                monthlyTimePicker.Font = new Font("Times New Roman", 11);
                monthlyTimePicker.Enabled = false;
                monthlyTimePicker.DateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).Date;
                //to
                toMonthlyTimePicker.Location = new Point(0, monthlyTimePicker.Location.Y + monthlyTimePicker.Height + 20);
                toMonthlyTimePicker.Size = new Size(150, 30);
                toMonthlyTimePicker.Font = new Font("Times New Roman", 11);
                toMonthlyTimePicker.Enabled = false;
                toMonthlyTimePicker.DateTime = DateTime.Today.Date;

                panelDateCriteria.Controls.Add(monthlyTimePicker);
                panelDateCriteria.Controls.Add(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(dailyTimePicker);
                panelDateCriteria.Controls.Remove(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(yearlyTimePicker);
                panelDateCriteria.Controls.Remove(fromTimePicker);
                panelDateCriteria.Controls.Remove(toTimePicker);
                panelDateCriteria.Controls.Remove(anyDateTimePicker);
                //label managing
                lblDateFilterTitle.Text = "Today's Month Range";
                lblFromDate.Text = "From:";
                lblToDate.Text = "To:";

            }
            if (dateFilterCombo.SelectedIndex == 3)
            {
                yearlyTimePicker.Location = new Point(0, 30);
                yearlyTimePicker.Size = new Size(150, 30);
                yearlyTimePicker.Font = new Font("Times New Roman", 11);
                yearlyTimePicker.Enabled = false;
                yearlyTimePicker.Text = DateTime.Today.Year.ToString();

                panelDateCriteria.Controls.Add(yearlyTimePicker);
                panelDateCriteria.Controls.Remove(dailyTimePicker);
                panelDateCriteria.Controls.Remove(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(monthlyTimePicker);
                panelDateCriteria.Controls.Remove(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(fromTimePicker);
                panelDateCriteria.Controls.Remove(toTimePicker);
                panelDateCriteria.Controls.Remove(anyDateTimePicker);
                //managing label
                lblDateFilterTitle.Text = "Today's Year Range";
                lblFromDate.Text = string.Empty;
                lblToDate.Text = string.Empty;


            }
            if (dateFilterCombo.SelectedIndex == 4)
            {
                fromTimePicker.Location = new Point(0, 30);
                fromTimePicker.Size = new Size(150, 30);
                fromTimePicker.Font = new Font("Times New Roman", 11);
                panelDateCriteria.Controls.Add(fromTimePicker);
                toTimePicker.Location = new Point(0, fromTimePicker.Location.Y + fromTimePicker.Height + 20);
                toTimePicker.Size = new Size(150, 30);
                toTimePicker.Font = new Font("Times New Roman", 11);
                panelDateCriteria.Controls.Add(toTimePicker);

                panelDateCriteria.Controls.Remove(dailyTimePicker);
                panelDateCriteria.Controls.Remove(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(monthlyTimePicker);
                panelDateCriteria.Controls.Remove(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(yearlyTimePicker);
                panelDateCriteria.Controls.Remove(anyDateTimePicker);
                //managing label
                lblDateFilterTitle.Text = "Select Any Range of Date:";
                lblFromDate.Text = "From:";
                lblToDate.Text = "To:";


            }
            if (dateFilterCombo.SelectedIndex == 5)
            {
                anyDateTimePicker.Location = new Point(0, 30);
                anyDateTimePicker.Size = new Size(150, 30);
                anyDateTimePicker.Font = new Font("Times New Roman", 11);
                panelDateCriteria.Controls.Add(anyDateTimePicker);
                panelDateCriteria.Controls.Remove(fromTimePicker);
                panelDateCriteria.Controls.Remove(toTimePicker);
                panelDateCriteria.Controls.Remove(dailyTimePicker);
                panelDateCriteria.Controls.Remove(fromWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(toWeaklyTimePicker);
                panelDateCriteria.Controls.Remove(monthlyTimePicker);
                panelDateCriteria.Controls.Remove(toMonthlyTimePicker);
                panelDateCriteria.Controls.Remove(yearlyTimePicker);
                //label managing
                lblDateFilterTitle.Text = "Select Any Date";
                lblToDate.Text = string.Empty;
                lblFromDate.Text = string.Empty;
            }

        }
        #endregion
        #region Collapse Side Panel
        List<PatientDocument> patientData = new List<PatientDocument>();
        PatientDocument clickedRowData;
        private void gridViewPatients_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            // Check if a cell within a row is clicked (not the header or empty area)
            if (e.RowHandle >= 0 && e.Column != null)
            {
                // Get the clicked row's data
               clickedRowData = gridViewPatients.GetRow(e.RowHandle) as PatientDocument;
                var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == clickedRowData.Id);
                if (clickedPatient.VisitStatusID == 1)
                {
                
                    btnPatientCloseVisit.Enabled = true;                
                    btnPatientStartVisit.Enabled = false;
                }
                if (clickedPatient.VisitStatusID == 2)
                {
                   
                    btnPatientCloseVisit.Enabled = false;                 
                    btnPatientStartVisit.Enabled = true;
                }
                lblArrivalHisttory.Text = clickedRowData.Name + "'s Arrival History:";
                lblPatientLog.Text = clickedRowData.Name + "'s Log History:";
                if (patientData.Count > 0)
                {
                    patientData.Clear();
                }
                if (sidePanelForPatintInfo.Width == 0)
                {
                    sidePanelForPatintInfo.Width = 300;
                    gridControlPatient.Dock = DockStyle.None;
                    gridControlPatient.Location = new Point(sidePanelForPatintInfo.Location.X + sidePanelForPatintInfo.Width + 5, gridControlPatient.Location.Y);
                    collapseSidePanel = true;
                    gridControlPatient.Dock = DockStyle.None;
                }
                patientData.Add(clickedRowData);
                gridControlPatientDataSide.DataSource = patientData;
                cardViewArrivalHistory.RefreshData();
                gridControlPatienLog.DataSource = patientData;
                cardViewPatientLog.RefreshData();   




            }

        }
        private void btnCollapseSidePanel_Click(object sender, EventArgs e)
        {
            if(collapseSidePanel)
            {
                sidePanelForPatintInfo.Width = 0;
                collapseSidePanel= false;   
                gridControlPatient.Dock=DockStyle.Fill;
            }
            else
            {
                sidePanelForPatintInfo.Width = 300;
                gridControlPatient.Dock = DockStyle.None;
                gridControlPatient.Location = new Point(sidePanelForPatintInfo.Location.X + sidePanelForPatintInfo.Width + 5, gridControlPatient.Location.Y);
                collapseSidePanel = true;
                gridControlPatient.Dock = DockStyle.None;
            }

        }


        #endregion
        #region Actions On Patient Document
        private void btnPatientUpdate_Click(object sender, EventArgs e)
        {
            if(clickedRowData is null)
            {
                MessageBox.Show("You did not select any Patient");
                return;
            }
            OnCustomActionUpdate(clickedRowData);
            //MessageBox.Show(clickedRowData.Id + "\n" + clickedRowData.Name);
        }

        private void btnPatientAssign_Click(object sender, EventArgs e)
        {

            if (clickedRowData is null)
            {
                MessageBox.Show("You did not select any Patient");
                return;
            }
            OnCustomActionPatientAssign(clickedRowData);
        }

        private void btnPatientDeposit_Click(object sender, EventArgs e)
        {
            if (clickedRowData is null)
            {
                MessageBox.Show("You did not select any Patient");
                return;
            }
            OnCustomActionPatientDeposit(clickedRowData); 

        }
        
        private void btnPatientStartVisit_Click(object sender, EventArgs e)
        {
            if (clickedRowData is null)
            {
                MessageBox.Show("You did not select any Patient");
                return;
            }
            OnCustomActionStartVisit(clickedRowData);
        }

        private void btnPatientCloseVisit_Click(object sender, EventArgs e)
        {
            if (clickedRowData is null)
            {
                MessageBox.Show("You did not select any Patient");
                return;
            }
            OnCustomActionCloseVisit(clickedRowData);
        }
        #endregion
        #region Row color Modification for Patient Document
        private void gridViewPatients_RowStyle(object sender, RowStyleEventArgs e)
        {

            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            if (e.RowHandle >= 0)
            {

                bool active = Boolean.Parse(view.GetRowCellValue(e.RowHandle,
                                                        "Active").ToString());


                if (active)
                {
                    e.Appearance.BackColor = Color.LightBlue;
                    e.HighPriority = true;
                }
                if (!active)
                {
                    e.Appearance.BackColor = Color.WhiteSmoke;
                }



            }
        }
        #endregion
    }
}

