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
using Clinical_Managment_System;


using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using DevExpress.XtraGrid.Views.Card;
using Patient_Managment_System.DTO;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
using DevExpress.XtraLayout.Filtering.Templates;
using DevExpress.XtraEditors.TextEditController.Win32;
using static Patient_Managment_System.Constants.EnumCollection;
using DevExpress.XtraSpellChecker;
using System.IO;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Internal;


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
     
        IDGenerationDbContext idDbContext = new IDGenerationDbContext();   
        DbContext dbContext = new DbContext();
        List<PatientDto> patientDocuments = new List<PatientDto>(); 
        List<PatientDocument> listofPatientDocument = new List<PatientDocument>();       
        List<ComoBoxList> registrationFeeType= new List<ComoBoxList>();
        List<RegistrationItem> DefinitionItems;
   
        List<string> genders = new List<string> {"Male","Female"};
        List<string> patienttypes = new List<string> {"Individual","Organization"};
        List<Organization> organizations = new List<Organization>();

        public PatientDto updatedPatient;//comes form patient document for updation
      
        List<Voucher> menuDefinitions;
        Voucher patientDefination;
        List<Defination> defination;
        List<Person> persons;
        List<Room> rooms;   
        List<Configurations> configurations;
        int MaxIdValue;
        List<IdDefinitionDetail> IdDefinitions;
        List<ComoBoxList> visitLocations=new List<ComoBoxList>();
        List<ComoBoxList> patientAssignmentType=new List<ComoBoxList>();
        List<RegisterationInvoiceHistory> registrationInvoiceHistory = new List<RegisterationInvoiceHistory>();
        List<DepositHistory> depositHistories = new List<DepositHistory>();
        List<PatientKinData> patientKinInfos = new List<PatientKinData>();
        List<PatientRelation> patientOrgRelations = new List<PatientRelation>();
        Voucher invoiceCategory;
    int patientDefinationID;
        #endregion
        public PatientMSystem()
        {
            InitializeComponent();
          
        }
        #region Application On Load Method
        private void PatientMSystem_Load(object sender, EventArgs e)
        {
            try
            {
              
                IdDefinitions = idDbContext.getIDDefinitionDetail();
                menuDefinitions = dbContext.getVouchers();
                patientDefinationID = generateID();
                persons =dbContext.getPersons();
                configurations = dbContext.getConfigurations();
                patientKinInfos=dbContext.getPatientKinInfo();
                patientOrgRelations=dbContext.getPatientOrganizationName();
                //for Registration Invoice Type
                invoiceCategory = menuDefinitions.FirstOrDefault(x => x.parent.Trim().ToLower().Equals(("EMR").ToLower()) &&
                                                                                    x.name.Trim().ToLower().Equals(("Registration").ToLower()));


              registrationInvoiceHistory= dbContext.getRegistrationInvoiceHistory(invoiceCategory.id);
                depositHistories = dbContext.getDepositHistory();
                //load all avialable rooms
                rooms = dbContext.getRooms();

                //patient Registration Fee Item
                DefinitionItems = dbContext.getRegistrationfeeItem();
                ComoBoxList comoBoxList = new ComoBoxList();
                comoBoxList.Id = 0;
                comoBoxList.Description = "--select--";
               
               var getRegistrationFeeItems = DefinitionItems.Where(x => x.value== "Patient Registartion")
                    .Select(x => new ComoBoxList
                    {
                        Id=x.Reference,
                        Description=x.name
                    } ).ToList();
                registrationFeeType.Add(comoBoxList);
                registrationFeeType.AddRange(getRegistrationFeeItems); 
                cBoxRegType.DataSource = registrationFeeType;//combo box for Registration fee Type
                cBoxRegType.ValueMember = "Id";
                cBoxRegType.DisplayMember="Description";

                //all defination from defination table
                defination = dbContext.getDefinitionDetail();

                var getPatientAssignmentType = defination.Where(x => 
                                            x.type.Trim()== "EMR" &&
                                            x.description.Trim()== "registration assignment")
                     .Select(x => new ComoBoxList
                     {
                         Id = x.id,
                         Description = x.value
                     }).ToList();
                patientAssignmentType.Add(comoBoxList);
                patientAssignmentType.AddRange(getPatientAssignmentType);
                cBoxAssignType.DataSource = patientAssignmentType;//combo box for Registration fee Type
                cBoxAssignType.ValueMember = "Id";
                cBoxAssignType.DisplayMember = "Description";
                //invoice type
               var invoiceType = configurations.Where(x => x.description.Trim().ToLower() == ("Registration Type").ToLower() &&
                                                                               x.type== invoiceCategory.id)
                    .Select(x => new ComoBoxList
                    {
                        Id = x.id,
                        Description = x.value
                    }).ToList();
                comboBoxInvoiceTypes.DataSource = invoiceType;
                comboBoxInvoiceTypes.ValueMember = "Id";    
                comboBoxInvoiceTypes.DisplayMember = "Description";  

                //collapse the side panel for patient document page at first load
                sidePanelForPatintInfo.Width = 0;
                collapseSidePanel = false;
                gridControlPatient.Dock = DockStyle.Fill;
                /////////////////////////////////////////
                generateVouchurID();
                cBoxGeneder.DataSource = genders;//combo box for Gender
            
                
                comboBoxPatientType.DataSource = patienttypes;//combo box for Patient Type
                /*
                 * outer by habtish
                 * combo box data for Visit Location
                 */
                var getvisitLocations = dbContext.LoadListForVisitTypeCoboBox();
                visitLocations.Add(comoBoxList);
                visitLocations.AddRange(getvisitLocations);
                cBoxVisitType.DataSource = visitLocations;
                cBoxVisitType.ValueMember = "Id";
                cBoxVisitType.DisplayMember = "Description";


                //for organization list
                var organizationType = menuDefinitions.FirstOrDefault(x => x.parent.Trim().ToLower().Equals(("Maintain").ToLower()) &&
                                                                                    x.name.Trim().ToLower().Equals(("Organization Customer").ToLower()));
              var getOrganizations = dbContext.getOrganization(organizationType.id);
                Organization organization = new Organization();
                organization.id = "";
                organization.brandName = "--select--";
                organizations.Add(organization);
                organizations.AddRange(getOrganizations);
                /*
                 * outer by habtish
                 * patient document grid view 
                 */
                patientDocuments = dbContext.GetPatients();
                patientDocuments = patientDocuments.GroupBy(x=>x.PersonID).
                                                                            Select(group=>group.OrderByDescending(x=>x.LastArrivalDate).First()).ToList(); 

                listofPatientDocument = patientDocuments.Select(x => new PatientDocument
                {
                    Id = x.PersonID.Trim(),
                    Name = x.FirstName.Trim() + " " + x.MiddleName.Trim() + " " + x.LastName.Trim(),
                    Age = x.Age,
                    Gender = x.Gender.Trim(),
                    PhoneNumber = x.PhoneNumber.Trim(),
                    VisitLocation = x.VisitLocation.Trim(),
                    VisitStatus = x.VisitStatus.Trim(),
                    VisitStartDate = x.VisitStartDate,
                    VisitEndDate = x.VisitEndDate,            
                    LastInvoiceDate = (x.LastInvoiceDate).ToString().Trim(),
                    LastArrivalDate = (x.LastArrivalDate).ToString().Trim(),
                    DateRegistered = (x.DateRegistered),
                    Address = x.City.Trim() != "" ? $"[{x.HouseNo.Trim()}] {x.City.Trim()}, {x.SubCity.Trim()}, {x.Kebele.Trim()}" : "",
                    DeviceName = x.DeviceName.Trim()
                }).ToList();
                 gridControlPatient.DataSource = listofPatientDocument;
            


                /*
                 * outer by habtish
                 * load all appointment list to view 
                 */
                loadAppointments();
                ///combo box for Filter By in patient Document
                comboBoxFilterBy.DataSource = filterByList;
                //for deposit card hitory in patient Document

                patientDefination = menuDefinitions.FirstOrDefault(x => x.parent.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                    x.name.Trim().ToLower() == ("Deposit").ToLower());

            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);    
            }
        }
        public int generateID()
        {
            var patientDefination = menuDefinitions.FirstOrDefault(x => x.parent.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                 x.name.Trim().ToLower() == ("patient").ToLower());
            var patientIDDefinationDetail = IdDefinitions.FirstOrDefault(x => x.type == patientDefination.id);
            MaxIdValue = idDbContext.getMaxIdValue(patientIDDefinationDetail.id);           
            MaxIdValue=MaxIdValue==0? 1 : MaxIdValue+1;
           var centerPart = MaxIdValue.ToString().PadLeft(patientIDDefinationDetail.length, '0');
            var personId = string.Format($"{patientIDDefinationDetail.prefix.Trim()+patientIDDefinationDetail.prefix_separator.Trim()}" + 
                                               $"{centerPart}" + $"{patientIDDefinationDetail.suffix_separator.Trim()+patientIDDefinationDetail.suffix.Trim()}", MaxIdValue);
            txtId.Text = personId;
            return patientIDDefinationDetail.id;



        }
        public string generateVouchurID()
        {
            var patientDefination = menuDefinitions.FirstOrDefault(x => x.parent.Trim().ToLower() == ("EMR").ToLower() &&
                                                                                 x.name.Trim().ToLower() == ("Registration").ToLower());
            var patientIDDefinationDetail = IdDefinitions.FirstOrDefault(x => x.type == patientDefination.id);
            MaxIdValue = idDbContext.getMaxVouchurID();
            MaxIdValue = MaxIdValue == 0 ? 1 : MaxIdValue + 1;
            var centerPart = MaxIdValue.ToString().PadLeft(patientIDDefinationDetail.length, '0');
            var vouchurId = string.Format($"{patientIDDefinationDetail.prefix.Trim() + patientIDDefinationDetail.prefix_separator.Trim()}" +
                                               $"{centerPart}" + $"{patientIDDefinationDetail.suffix_separator.Trim() + patientIDDefinationDetail.suffix.Trim()}", MaxIdValue);
         
            return vouchurId;



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
                #region Validation
               
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
                var selectedItem=(ComoBoxList)cBoxRegType.SelectedItem; 
                if (!validationHelper.isRegistrationFeeTypeValid(feeType) || selectedItem.Id==0)
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
                var assignTypeItem=(ComoBoxList)cBoxAssignType.SelectedItem;  
                if (!validationHelper.isDoctorAssignmentTypeValid(assignType)|| assignTypeItem.Id==0)
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
                
                var visitType = (ComoBoxList)cBoxVisitType.SelectedItem;

                if (visitType.Id==0||string.IsNullOrEmpty(cBoxVisitType.Text))
                {
                    cBoxVisitType.BackColor = Color.LightPink;
                    cBoxVisitType.Focus();
                    return;
                }
               var selectedOrg=(Organization)comboBoxOrgnization.SelectedItem;
               
                if (selectedOrg != null &&selectedOrg.id == "")
                {
                     comboBoxOrgnization.BackColor = Color.LightPink;
                     comboBoxOrgnization.Focus();
                     return;
                }
                
                 

                #endregion          

                //checking the Registered Operation is Found in Defination or not
                var OperationType = "EMR";
                var operationDescription = "Operation";
                var operationValue = "registered";
                var RegisterOperation = defination.FirstOrDefault(x => x.type.Trim().ToLower() == OperationType.ToLower() &&
                                                                          x.description.Trim().ToLower() == operationDescription.ToLower() &&
                                                                          x.value.Trim().ToLower() == operationValue.ToLower());

                if (RegisterOperation is null)
                {
                    MessageBox.Show("No Registered Operation Found! We will Fix it Soon!", "Error", 
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //getting the person type and person category id form defination table
                var type = "person";
                var typeDescription = "person type";
                var categoryDescription = "person category";
                var personTypeRow = defination.FirstOrDefault(x => x.type.Trim().ToLower() == type.ToLower()&&
                                                                                  x.description.Trim().ToLower()== typeDescription.ToLower());
                var personCategoryRow = defination.FirstOrDefault(x => x.type.Trim().ToLower() == type.ToLower() &&
                                                                                  x.description.Trim().ToLower() == categoryDescription.ToLower());

               //creating objects for all class
                Person person = new Person();
                Address address = new Address();
                Operation operation = new Operation();
                InvoiceOperation invoiceOperation = new InvoiceOperation();
                Invoice invoice = new Invoice();
                InvoiceLine invoiceLine = new InvoiceLine();
                PatientAssignment patientAssignment = new PatientAssignment();

                //assign values for person objects
                person.Id = txtId.Text.Trim();
                person.first_name = txtFirstName.Text.Trim();
                person.middile_name = txtMiddleName.Text.Trim();
                person.last_name = txtLastName.Text.Trim();
                person.gender = cBoxGeneder.Text.Trim();
                person.age = Convert.ToInt32(txtAge.Text.Trim());
                person.phone = txtPhone.Text.Trim();
                person.date_registered = DateTime.Now;
                person.type_Id = personTypeRow.id;
                person.category = personCategoryRow.id;
                person.tax = 0;
                person.active = true;
                person.remark = "Patient Registered";

                //assign values for Address Object
                address.city = txtCity.Text.Trim();
                address.subcity = txtSubCity.Text.Trim();
                address.kebele = txtKebele.Text.Trim();
                address.house_no = txtHouseNo.Text.Trim();



                //for visit Location
              
                //creating visit Object
                Visit visit = new Visit();
                visit.location_id = visitType.Id;
                visit.start_date = DateTime.Now;
                visit.end_date = null;
                visit.status_id = 1;

                //checks the person exist or not
                if (!dbContext.checkPersonExistance(txtId.Text.Trim()))
                {
                 
                //calling the dbContext for person
                var personResponse=dbContext.insertPerson(person);
                if (!personResponse.IsPassed)
                {
                    MessageBox.Show($"{personResponse.ErrorMessage}", "Error",
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                 //creating ID object
                 ID patientID = new ID();
                 patientID.current_value = person.Id;
                 patientID.defination = patientDefinationID;
                 var saveIdValue = idDbContext.saveID(patientID);
                 if (saveIdValue.IsPassed)
                    {
                        generateID();
                    }
                //creating patient Object
                Patient patient = new Patient();
                patient.person_id = person.Id;
                //excute the dbContext for patient
                var patientResponse=dbContext.insertPatient(patient);
                if (!patientResponse.IsPassed)
                {
                    MessageBox.Show($"{patientResponse.ErrorMessage}", "Error",
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(pictureBoxPatientProfile.Image != null)
                    {
                        var imageUrl=UploadImage();
                        SaveImageUrl saveImageUrl=new SaveImageUrl();
                        saveImageUrl.patient_id = patientResponse.Data;
                        saveImageUrl.image_url = imageUrl;
                        var saveImageurl=dbContext.saveImageUrl(saveImageUrl);
                        if(!saveImageurl.IsPassed)
                        {
                            MessageBox.Show($"{saveImageurl.ErrorMessage}", "Error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    NextofKin nextofKin = new NextofKin();
                    nextofKin.patient_id = patientResponse.Data;
                    nextofKin.kin_name=txtBoxKinName.Text.Trim();
                    nextofKin.kin_phone = txtBoxKinPhone.Text.Trim();
                    nextofKin.remark = "Next of Kin Added";
                    var saveNextofKin = dbContext.saveNextOfKin(nextofKin);
                    if (!saveNextofKin.IsPassed)
                    {
                        MessageBox.Show($"{saveNextofKin.ErrorMessage}", "Error",
                                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (selectedOrg!=null && selectedOrg.id != "")
                    {
                        OrganizationalCustomer organizationalCustomer = new OrganizationalCustomer();
                        organizationalCustomer.patient_id=nextofKin.patient_id;
                        organizationalCustomer.organization_id=selectedOrg.id;
                        var saveOrganizationCustomer = dbContext.saveOrganizationalCustomer(organizationalCustomer);
                        if (!saveOrganizationCustomer.IsPassed)
                        {
                            MessageBox.Show($"{saveOrganizationCustomer.ErrorMessage}", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    //take the new patient id for address object
                    address.patient_id=patientResponse.Data;
                   
                    //excute the dbContext for Address
                    var addressResponse = dbContext.insertAddress(address);
                if (!addressResponse.IsPassed)
                {
                    MessageBox.Show($"{addressResponse.ErrorMessage}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }               

                //assign values for Operation Object
                   
              var RegisterMenudefination = menuDefinitions.FirstOrDefault(x => x.name.Trim().ToLower()== ("EMR").ToLower() &&
                                                                                              x.parent.Trim().ToLower() == ("EMR").ToLower());              
                operation.operation = RegisterOperation.id; 
                operation.type = RegisterMenudefination.id;
                operation.color = "Blue";
                operation.manual = false;
                operation.is_final = true;
                //excute the dbContext for Operation
                var saveOperation = dbContext.saveOperation(operation);
                if(!saveOperation.IsPassed)
                {
                    MessageBox.Show($"{saveOperation.ErrorMessage}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                    // assign values for invoice Operation Object

                var VochourCode = generateVouchurID();              
                invoiceOperation.operation_id = saveOperation.Data;
                invoiceOperation.invoice_id = VochourCode;//need further detail
                invoiceOperation.operation_datetime = DateTime.Now;
                invoiceOperation.UserName = 1;
                invoiceOperation.device=Environment.MachineName;
                //excute the dbContext for Invoice Operation
                var saveInvoiceOperation = dbContext.saveInvoiceOperation(invoiceOperation);
                if(!saveInvoiceOperation.IsPassed)
                {
                    MessageBox.Show($"{saveInvoiceOperation.ErrorMessage}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //crating Period
                Models.Period period = new Models.Period();                
                period.description = "Registration";
                period.start_date= DateTime.Now;    
                period.end_date= DateTime.Now;
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
            
                //var invoiceType = (ComoBoxList)comboBoxInvoiceTypes.SelectedItem;
                invoice.code = VochourCode;
                invoice.last_operation = saveInvoiceOperation.Data;
                invoice.type = invoiceCategory.id;
                invoice.consignee = address.patient_id;
                invoice.period = savePeroid.Data;
                invoice.date = DateTime.Now;  
                invoice.is_final=true;
                invoice.is_void = false;
                invoice.subtotal=Decimal.Parse(txtRegAmount.Text.ToString());
                invoice.tax = 0;
                invoice.discount = 0;
                invoice.grand_total = invoice.subtotal;
                //excute the dbContext for Invoice
                var saveInvoice=dbContext.saveInvoice(invoice);
                if (!saveInvoice.IsPassed)
                {
                    MessageBox.Show($"{saveInvoice.ErrorMessage}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //assign values for Invoice Line Object
               
                var selectedRegType = (ComoBoxList)cBoxRegType.SelectedItem;
                var selctedItem = DefinitionItems.FirstOrDefault(x => x.Reference == selectedRegType.Id);
                invoiceLine.invoice = invoice.code;
                invoiceLine.itemId = selctedItem.item_Id;
                invoiceLine.qty = 1;
                invoiceLine.unit_amount = invoice.subtotal;
                invoiceLine.total= invoice.subtotal;
                invoiceLine.taxable_amount = invoice.subtotal;
                invoiceLine.tax_amount = invoice.tax;
                //excute the dbContext for InvoiceLine
                var saveInvoiceLine = dbContext.saveInvoiceLine(invoiceLine);
                if(!saveInvoiceLine.IsPassed)
                {
                    MessageBox.Show($"{saveInvoiceLine.ErrorMessage}", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                    //assign values for patient assignment object
               
                    var selectedAssignentType = (ComoBoxList)cBoxAssignType.SelectedItem;
                    patientAssignment.patient_id = address.patient_id;
                    patientAssignment.assignment_type = selectedAssignentType.Id;
                    patientAssignment.assigned_to = cBoxAssignValue.Text.Trim();
                    patientAssignment.Invoice = invoice.code;
                    //excute the dbContext for Patientassignment
                    var savePatientAssignment=dbContext.savePatientAssignment(patientAssignment);
                    if (!savePatientAssignment.IsPassed)
                    {
                        MessageBox.Show($"{savePatientAssignment.ErrorMessage}", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //add the patient_id for vist object
                    visit.patient_id = address.patient_id;


                    var saveVisit = dbContext.insertVisitType(visit);
                    if (!saveVisit.IsPassed)
                    {
                        MessageBox.Show($"{saveVisit.ErrorMessage}", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


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
                    pictureBoxPatientProfile.Image = null;
                   

                        MessageBox.Show("Registration Success", "Sucess", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        persons.Add(person);                     
                                                         
                    
                }
                else
                {
                    //updating the person data
                    var updatePatient = dbContext.updatePerson(person);                   
                    address.patient_id = updatedPatient.PatientID;//patient Id                 
                    var updatePatientAddress = dbContext.updateAddress(address);
                     

                        if (!(updatePatientAddress.IsPassed && updatePatient.IsPassed))
                        {

                          
                            MessageBox.Show(updatePatient.SuccessMessage, "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);                       

                        }
                        else
                        {
                            MessageBox.Show(updatePatient.ErrorMessage+updatePatientAddress.ErrorMessage, "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    //updating visit Data                  
                   
                    visit.patient_id=updatedPatient.PatientID;                    
                    var updateVist = dbContext.updateVisit(visit);
                    //updating invoice data
                   // var invoiceType = (ComoBoxList)comboBoxInvoiceTypes.SelectedItem;                  
                    invoice.type = invoiceCategory.id;
                    invoice.consignee = address.patient_id;                 
                    invoice.subtotal = Decimal.Parse(txtRegAmount.Text.ToString());                  
                    invoice.grand_total = invoice.subtotal;
                   // var updateInvoice = dbContext.updateRegistrationInvoice();

                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"Something UnExcepected Happened.Please Try Again\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            ComoBoxList selectedItem=(ComoBoxList)cBoxRegType.SelectedItem;
            if (selectedItem.Id == 0)
            {
                txtRegAmount.Text = string.Empty;
                txtRegAmount.Enabled = false;
                return;
            }
            else
            {
                var priceItem = DefinitionItems.FirstOrDefault(x => x.Reference == selectedItem.Id);
                txtRegAmount.Text = priceItem.Price.ToString();
                var setting = configurations.FirstOrDefault(x => x.type == 0 &&
                                                                                       x.description.Trim() == "isFlexiableAmount");
                if (bool.Parse(setting.value))
                    txtRegAmount.Enabled = true;
                else txtRegAmount.Enabled = false;
            }
            
        }

        private void txtRegAmount_TextChanged(object sender, EventArgs e)
        {
            txtRegAmount.BackColor = SystemColors.Window;
        }
      
       
       
        private void cBoxAssignType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxAssignType.BackColor = SystemColors.Window;
            ComoBoxList selectedItem = (ComoBoxList)cBoxAssignType.SelectedItem;
            if (selectedItem.Id== 0)
            {
                cBoxAssignValue.Text = string.Empty;
                return;
            }
            var assignType= defination.Where(x => x.description == "registration assignment" && x.type == "EMR").ToList();
            var assignDoctor = assignType.FirstOrDefault(x => x.value == "doctor");
          
            if (selectedItem.Id== assignDoctor.id)
            {
                var doctorTypeDefinition = defination.FirstOrDefault(x =>
                                                    x.description == "person type" && 
                                                    x.type == "person" &&
                                                    x.value=="doctor");
                var doctorCategoryDefinition = defination.FirstOrDefault(x =>
                                                 x.description == "person category" &&
                                                 x.type == "person" &&
                                                 x.value == "employee");

                var doctors = persons.Where(x => x.type_Id == doctorTypeDefinition.id && 
                                                         x.category== doctorCategoryDefinition.id)
                                                        .Select(x=>new Doctor
                                                        {
                                                            Id=x.Id,
                                                            Name=x.first_name +" "+ x.middile_name+" "+x.last_name
                                                        }).ToList();
                cBoxAssignValue.DataSource = doctors;
                cBoxAssignValue.DisplayMember = "Name";
                cBoxAssignValue.ValueMember = "Id";
            }
            else if(selectedItem.Id!=0) 
            {
               rooms .Select(x => new ComoBoxList
                {
                    Id=x.id,
                    Description=x.description,
                }).ToList();
                cBoxAssignValue.DataSource = rooms;
                cBoxAssignValue.DisplayMember = "Description";
                cBoxAssignValue.ValueMember= "Id";
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
            if (comboBoxInvoiceTypes.SelectedIndex < 0)
            {
                comboBoxInvoiceTypes.SelectedIndex = 0;
            }
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

        string imagePath;
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
                        imagePath = openFileDialog.FileName;
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
        public string UploadImage()
        {
           
            var workingDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            

            string projectDir = Directory.GetParent(workingDir).Parent.FullName;
            string localFolderPath = Path.Combine(projectDir, "PatientImages");
            

            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
                
            }
            string patientName= txtFirstName.Text.Trim();  
            string fileName = patientName+"s" + Path.GetExtension(imagePath);
            string newFilePath = Path.Combine(localFolderPath, fileName);
            File.Copy(imagePath, newFilePath, true);
            return newFilePath;

           
        }
        #endregion
        #region Reset Method For New Registration
        private void btnNew_Click(object sender, EventArgs e)
        {
             generateID();

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
            cBoxRegType.SelectedIndex = 0;
            cBoxAssignType.SelectedIndex = 0;            
            cBoxVisitType.SelectedIndex = 0;
            comboBoxInvoiceTypes.SelectedIndex = 0;
            txtBoxKinName.Text = string.Empty;  
            txtBoxKinPhone.Text = string.Empty;
            comboBoxPatientType.SelectedIndex = 0;
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
            try
            {
                registrationInvoiceHistory = dbContext.getRegistrationInvoiceHistory(invoiceCategory.id);//for invoice history
                depositHistories = dbContext.getDepositHistory();//for deposit history
                patientKinInfos = dbContext.getPatientKinInfo();//for patient additional info 
                patientOrgRelations = dbContext.getPatientOrganizationName();//for organization names
                patientDocuments = dbContext.GetPatients();//for grid view data source
                patientDocuments = patientDocuments.GroupBy(x => x.PersonID).
                                                    Select(group => group.OrderByDescending(x => x.LastArrivalDate).First()).ToList();

                listofPatientDocument = patientDocuments.Select(x => new PatientDocument
                {
                    Id = x.PersonID.Trim(),
                    Name = x.FirstName.Trim() + " " + x.MiddleName.Trim() + " " + x.LastName.Trim(),
                    Age = x.Age,
                    Gender = x.Gender.Trim(),
                    PhoneNumber = x.PhoneNumber.Trim(),
                    VisitLocation = x.VisitLocation.Trim(),
                    VisitStatus = x.VisitStatus.Trim(),
                    VisitStartDate = x.VisitStartDate,
                    VisitEndDate = x.VisitEndDate,
                    LastInvoiceDate = (x.LastInvoiceDate).ToString().Trim(),
                    LastArrivalDate = (x.LastArrivalDate).ToString().Trim(),
                    DateRegistered = (x.DateRegistered),
                    Address = x.City.Trim() != "" ? $"[{x.HouseNo.Trim()}] {x.City.Trim()}, {x.SubCity.Trim()}, {x.Kebele.Trim()}" : "",
                    DeviceName = x.DeviceName.Trim()

                }).ToList();
                listofPatientDocument = listofPatientDocument.GroupBy(x => x.Id).Select(group => group.First()).ToList();

                gridControlPatient.DataSource = listofPatientDocument;

                //gridControlPatient.DataSource = listofPatientDocument;
                gridViewPatients.RefreshData();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
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
            try
            {
                var appointmentSummaries = dbContext.loadAppointmentSummary();

                gridControlAppointmentdocument.DataSource = appointmentSummaries;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }    

        private void btnAppointmentRefresh_Click(object sender, EventArgs e)
        {
            loadAppointments();
        }
        #endregion
        #region Appointment Row Color Modification Based on Appointment Date
        private void gridViewAppointmentDocument_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {

                DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

                if (e.RowHandle >= 0)
                {

                    DateTime cellValue = DateTime.Parse(view.GetRowCellValue(e.RowHandle,
                                                            "OrderedDate").ToString());


                    if (cellValue.Date > DateTime.Today)
                    {
                        e.Appearance.BackColor = Color.LightGray;

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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        #endregion               
        #region Patient Document Menu
        private void gridViewPatients_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.HitInfo.InRow)
                {
                    // Show the context menu only when right-clicking on a row
                    e.Menu.Items.Clear();
                    PatientDocument rowData = gridViewPatients.GetRow(e.HitInfo.RowHandle) as PatientDocument;
                    var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == rowData.Id);

                    var updateItem = new DevExpress.Utils.Menu.DXMenuItem("UPDATE",
                                                 (s, args) => OnCustomActionUpdate(rowData));

                    var startVisit = new DevExpress.Utils.Menu.DXMenuItem("START VISIT",
                                                 (s, args) => OnCustomActionStartVisit(rowData));
                    e.Menu.Items.Add(startVisit);

                    var closeVisit = new DevExpress.Utils.Menu.DXMenuItem("CLOSE VISIT",
                                                 (s, args) => OnCustomActionCloseVisit(rowData));
                    if (clickedPatient.VisitStatus.Trim().ToLower() == ("Started").ToLower())
                    {
                        closeVisit.Enabled = true;

                        startVisit.Enabled = false;

                    }
                    if (clickedPatient.VisitStatus.Trim().ToLower() == ("closed").ToLower())
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void OnCustomActionPatientDeposit(PatientDocument patient)
        {
            var clickedPatient= patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
            DepositPopUp depositPop=new DepositPopUp();
            depositPop.patient = clickedPatient;
            depositPop.definations = defination;
            depositPop.menuDefinitions = menuDefinitions;
            depositPop.IdDefinitions = IdDefinitions;
            DialogResult result=depositPop.ShowDialog();    
        }

        private void OnCustomActionUpdate(PatientDocument patient)
        {
            var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
            updatedPatient = clickedPatient;

            txtId.Text = patient.Id;
            txtFirstName.Text = clickedPatient.FirstName.Trim();
            txtMiddleName.Text = clickedPatient.MiddleName.Trim();
            txtLastName.Text = clickedPatient.LastName.Trim();
            txtAge.Text = clickedPatient.Age.ToString().Trim();
            txtPhone.Text = clickedPatient.PhoneNumber.Trim();
            cBoxGeneder.Text = clickedPatient.Gender.Trim();
            txtCity.Text = clickedPatient.City.Trim();
            txtSubCity.Text = clickedPatient.SubCity.Trim();
            txtKebele.Text = clickedPatient.Kebele.Trim();
            txtHouseNo.Text = clickedPatient.HouseNo.Trim();

            cBoxRegType.Text = string.Empty;
            txtRegAmount.Text = string.Empty;
            cBoxAssignType.Text = string.Empty;
            cBoxAssignValue.Text = string.Empty;
            //cBoxVisitType.Text = string.Empty;

         
            var vistLocation= visitLocations.FirstOrDefault(x => x.Id== clickedPatient.LocationID);
            cBoxVisitType.SelectedItem=vistLocation;
            comboBoxInvoiceTypes.Text = clickedPatient.InvoiceType.Trim();
            RegistrationItem regstrationFeeType = DefinitionItems.FirstOrDefault(x =>
                                                                 x.item_Id.Trim().ToLower() == clickedPatient.ItemID.Trim().ToLower());
            ComoBoxList comoBoxList = new ComoBoxList
            {
                Description = regstrationFeeType.name,
                 Id = regstrationFeeType.id
           }; 
            txtRegAmount.Text=regstrationFeeType.Price.ToString();  
            cBoxRegType.SelectedItem = comoBoxList;

            var assignmentType = patientAssignmentType.FirstOrDefault(x => x.Id == clickedPatient.AssignmentType);
            cBoxAssignType.SelectedItem = assignmentType;
            cBoxAssignValue.Text = clickedPatient.AssignedValue;


            xtraTabControlRegistration.SelectedTabPage = xtraTabPageGeneral;
            
        }
        private void OnCustomActionStartVisit(PatientDocument patient)
        {
            var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
            VisitTypePopUp visitTypePopUp = new VisitTypePopUp();
            visitTypePopUp.patient = clickedPatient;
            visitTypePopUp.configurations = configurations;
            visitTypePopUp.definations = defination;
            visitTypePopUp.menuDefinitions= menuDefinitions;    
            visitTypePopUp.persons= persons;
            visitTypePopUp.definationItems = DefinitionItems;
            visitTypePopUp.idDefinations = IdDefinitions;
            visitTypePopUp.rooms = rooms;
            DialogResult result = visitTypePopUp.ShowDialog();
       
            
        }
        private void OnCustomActionCloseVisit(PatientDocument patient)
        {
            var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);

            //access the operation started from defination table
            var closedOperation = defination.FirstOrDefault(x => x.description.Trim().ToLower() == ("Operation").ToLower() &&
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
            invoiceOperation.invoice_id = clickedPatient.VochourCode;
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
            UpdateInvoice updateInvoice = new UpdateInvoice();
            updateInvoice.consignee = clickedPatient.PatientID;
            updateInvoice.code = clickedPatient.VochourCode;
            updateInvoice.last_operation = saveInvoiceOperation.Data;
            var updateLastOperation = dbContext.updateInvoice(updateInvoice);
            if (!updateLastOperation.IsPassed)
            {

                MessageBox.Show(updateLastOperation.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //craete UpdateVist Object
            UpdateVisit updateVisit = new UpdateVisit();
            updateVisit.status_id = 2;
            updateVisit.end_date = DateTime.Now;    
            updateVisit.patient_id= clickedPatient.PatientID;   
            var closedVisit=dbContext.updateVisitStatus(updateVisit);
            if (closedVisit.IsPassed)
            {

                MessageBox.Show(closedVisit.SuccessMessage, "Success",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            else
            {

                MessageBox.Show(closedVisit.ErrorMessage, "Error",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
              
            }
        }
        private void OnCustomActionPatientAssign(PatientDocument patient)
        {
            var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == patient.Id);
             PatientAssignPopUp assignPopUp = new PatientAssignPopUp();
            assignPopUp.patient= clickedPatient;
            assignPopUp.persons = persons;
            assignPopUp.definations = defination;
            assignPopUp.rooms = rooms;
            DialogResult result=assignPopUp.ShowDialog();
           
        }
        #endregion
        #region Patient Type Selected Changed
        private void comboBoxPatientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxPatientType.BackColor = Color.White;
                if (comboBoxPatientType.SelectedIndex == 0)
                {
                    groupBoxPatientType.Controls.Remove(comboBoxOrgnization);
                    groupBoxPatientType.Controls.Remove(lblOrganization);
                    groupBoxPatientType.Controls.Remove(lblAstrixOrg);

                }
                if (comboBoxPatientType.SelectedIndex == 1)
                {
                    groupBoxPatientType.Controls.Add(comboBoxOrgnization);
                    groupBoxPatientType.Controls.Add(lblOrganization);
                    groupBoxPatientType.Controls.Add(lblAstrixOrg);
                    comboBoxOrgnization.DataSource = organizations;
                    comboBoxOrgnization.DisplayMember = "brandName";
                    comboBoxOrgnization.ValueMember = "id";

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion
        #region Search Pateints
        private void btnShowSearch_Click(object sender, EventArgs e)
        {
            try
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
                    var searchedresult = listofPatientDocument.Where(x => x.PhoneNumber.Contains(phoneNumber.TrimStart('0'))).ToList();
                    if (searchedresult.Count == 0)
                    {
                        MessageBox.Show("Patient Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridControlPatient.DataSource = listofPatientDocument;
                    }
                    else if (searchedresult.Count >= 1)
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
                    else if (searchedresult.Count >= 1)
                    {
                        gridControlPatient.DataSource = searchedresult;
                    }
                }
                //search by Date 
                if (comboBoxFilterBy.SelectedIndex == 2)
                {
                    //filter today registered patients
                    if (dateFilterCombo.SelectedIndex == 0)
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
                        else if (searchedResult.Count >= 1)
                        {
                            gridControlPatient.DataSource = searchedResult;
                        }
                    }
                    //filter in this weak registered patients
                    if (dateFilterCombo.SelectedIndex == 1)
                    {

                        var dayOfaWeak = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).Date;
                        var today = DateTime.Today.Date;

                        var searchedResult = listofPatientDocument.Where(x =>
                                                             x.DateRegistered.Date >= dayOfaWeak && x.DateRegistered.Date <= today).ToList();
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
                        if (string.IsNullOrEmpty(fromTimePicker.Text.Trim()))
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
                        var fromDate = fromTimePicker.DateTime.Date;
                        var toDate = toTimePicker.DateTime.Date;


                        var searchedResult = listofPatientDocument.Where(x =>
                                                             x.DateRegistered.Date >= fromDate && x.DateRegistered.Date <= toDate).ToList();
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void comboBoxFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFilterBy.SelectedIndex == 0)//phone number
            {

                txtBoxPhoneNumber.Location = new Point(3, 25);
                txtBoxPhoneNumber.Size = new Size(200, 30);
                txtBoxPhoneNumber.Multiline = true;
                txtBoxPhoneNumber.TextChanged+=txtBoxPhoneNumber_TextChanged;   
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
                txtBoxPatientName.TextChanged += txtBoxPatientName_TextChanged;
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

        private void txtBoxPatientName_TextChanged(object sender, EventArgs e)
        {
            txtBoxPatientName.BackColor=SystemColors.Window;
        }

        private void txtBoxPhoneNumber_TextChanged(object sender, EventArgs e)
        {
           txtBoxPhoneNumber.BackColor=SystemColors.Window;
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
        List<PatientHistory> patientData = new List<PatientHistory>();
        PatientDocument clickedRowData;
        private void gridViewPatients_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                // Check if a cell within a row is clicked (not the header or empty area)
                if (e.RowHandle >= 0 && e.Column != null)
                {
                    // Get the clicked row's data
                    clickedRowData = gridViewPatients.GetRow(e.RowHandle) as PatientDocument;
                    var clickedPatient = patientDocuments.FirstOrDefault(x => x.PersonID == clickedRowData.Id);
                                                             
                    if (clickedPatient.VisitStatus.Trim().ToLower() == ("Started").ToLower())
                    {

                        btnPatientCloseVisit.Enabled = true;
                        btnPatientStartVisit.Enabled = false;
                    }
                    if (clickedPatient.VisitStatus.Trim().ToLower() == ("Closed").ToLower())
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
                    var invoiceHistory= registrationInvoiceHistory.Where(x=>x.PatientID==clickedPatient.PatientID).ToList();
                    PatientHistory patientHistory = new PatientHistory
                    {
                        Id = clickedPatient.PersonID,                     
                      
                        FullName=clickedPatient.FirstName+" "+clickedPatient.MiddleName+" "+clickedPatient.LastName,
                        VisitLocation = clickedPatient.VisitLocation,
                        VisitStatus = clickedPatient.VisitStatus,   
                        DateRegistered = (clickedPatient.DateRegistered).ToString(),
                        LastArrivalDate = (clickedPatient.LastArrivalDate).ToString(),
                        LastInvoiceDate=(clickedPatient.LastInvoiceDate).ToString()
                    };
                    patientData.Add(patientHistory);
                    gridControlPatientDataSide.DataSource = patientData;
                    cardViewArrivalHistory.RefreshData();
                    var registerCodeHistory = invoiceHistory.Select(x => new RegisterCodeHostory
                    {
                        Code=x.Code,
                        GrandTotal=x.GrandTotal.ToString().Trim(),
                        Date=x.Date,    
                        CardType=x.CardType
                    }).ToList();
                    gridControlVochourHistoryCard.DataSource = registerCodeHistory;
                    cardViewPatientVochourHistory.RefreshData();

                    //for Patient Addtional History Card
                    List<PatientInfoDisplay> patientRelation = new List<PatientInfoDisplay>();
                    var patientKinInfo=patientKinInfos.FirstOrDefault(x => x.patient_id == clickedPatient.PatientID);
                    var patientOrg= patientOrgRelations.FirstOrDefault(x => x.patient_id == clickedPatient.PatientID);
                    
                    PatientInfoDisplay patientInfoDisplay = new PatientInfoDisplay();
                    patientInfoDisplay.Address = clickedRowData.Address == "" ? "No Address Entered yet!" : clickedRowData.Address;
                    patientInfoDisplay.Orgaanization = patientOrg is null? "No organization" : patientOrg.brandName;
                    patientInfoDisplay.KinName = patientKinInfo is null?"No Kin Data is Entered": patientKinInfo.kin_name;
                    patientInfoDisplay.KinPhone = patientKinInfo is null ? "No Kin Data is Entered" :patientKinInfo.kin_phone;
                    patientRelation.Add(patientInfoDisplay);
                    gridControlPatienLog.DataSource = patientRelation;
                    cardViewPatientLog.RefreshData();
                   

                    //for Deposit History Card
                    var patientDepositHistory=depositHistories.Where(x=>x.PatientID==clickedPatient.PatientID)
                                                                                          .Select(x => new DepositHistoryDocument
                    {
                        DepositID = x.Code,
                        DepositAmount=x.GrandTotal,
                        DepositedDate=x.Date
                    });
                    gridControlPatientDepositHistory.DataSource= patientDepositHistory;
                    cardViewPatientDepositHistory.RefreshData();    




                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            try
            {
                if (clickedRowData is null)
                {
                    MessageBox.Show("You did not select any Patient","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                OnCustomActionUpdate(clickedRowData);
                //MessageBox.Show(clickedRowData.Id + "\n" + clickedRowData.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnPatientAssign_Click(object sender, EventArgs e)
        {
            try
            {

                if (clickedRowData is null)
                {
                    MessageBox.Show("You did not select any Patient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OnCustomActionPatientAssign(clickedRowData);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPatientDeposit_Click(object sender, EventArgs e)
        {
            try
            {
                if (clickedRowData is null)
                {
                    MessageBox.Show("You did not select any Patient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OnCustomActionPatientDeposit(clickedRowData);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        
        private void btnPatientStartVisit_Click(object sender, EventArgs e)
        {
            try
            {
                if (clickedRowData is null)
                {
                    MessageBox.Show("You did not select any Patient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OnCustomActionStartVisit(clickedRowData);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPatientCloseVisit_Click(object sender, EventArgs e)
        {
            try
            {
                if (clickedRowData is null)
                {
                    MessageBox.Show("You did not select any Patient");
                    return;
                }
                OnCustomActionCloseVisit(clickedRowData);
            }
            catch( Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion
        #region Row color Modification for Patient Document
        private void gridViewPatients_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {

                DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

                if (e.RowHandle >= 0)
                {

                    string Id = (view.GetRowCellValue(e.RowHandle,
                                                            "Id").ToString());
                    var patientData = patientDocuments.FirstOrDefault(x => x.PersonID == Id);

                    if (patientData.VisitStatus.Trim().ToLower()==("started").ToLower())
                    {
                        e.Appearance.ForeColor = Color.Blue;
                        //e.HighPriority = true;
                    }
                    if (patientData.VisitStatus.Trim().ToLower() == ("closed").ToLower())
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                   



                }
            }
            catch( Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Doctors doctor = new Doctors();
            HomeClinicalSystem system=new HomeClinicalSystem();
            doctor.DoctorID = "HEAL0001-2023";
            doctor.DoctorName = "Habtamu";
            doctor.DoctorType = "Specialist";
            doctor.PhoneNumber="1234567890";
            string json = JsonConvert.SerializeObject(doctor);
            system.doctor=JsonConvert.DeserializeObject<Clinical_Managment_System.DTOs.Doctor>(json);      
            system.Show();
        }
        private void PatientMSystem_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender,e);
                //MessageBox.Show("Enter Key is clicked");
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sidePanelForPatintInfo_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void panelPatientHistoryCard2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridControlVochourHistoryCard_Click(object sender, EventArgs e)
        {

        }
    }
}