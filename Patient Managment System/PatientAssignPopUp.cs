using DevExpress.Office;
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
using static Patient_Managment_System.Constants.EnumCollection;

namespace Patient_Managment_System
{
    public partial class PatientAssignPopUp : Form
    {
        List<string> patientAssignments = new List<string> { "Doctor", "Room Number" };
        List<string> roomNumbers = new List<string> { "Room 1", "Room 2", "Room 3", "Room 4" };
        List<string> doctors = new List<string> { "Dr xxxx", "Dr yyyyy", "Dr zzzzzz" };
        public PatientDto patient { get; set; }
        public List<Person> persons { get; set; }
        public List<Defination> definations { get; set; }
        public List<Room> rooms { get; set; }
        DbContext dbContext=new DbContext();    
        public PatientAssignPopUp()
        {
            InitializeComponent();
        }

       

        private void PatientAssignPopUp_Load(object sender, EventArgs e)
        {
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
           
            lblPatientID.Text=patient.PersonID.ToString();  
            lblPatientName.Text=patient.FirstName+" "+patient.MiddleName+" "+patient.LastName; 
        }

        private void cBoxAssignValue_SelectedIndexChanged(object sender, EventArgs e)
        {
           cBoxAssignValue.BackColor=SystemColors.Window;   
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.Cancel; 
            this.Close();   
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if(cBoxAssignType.SelectedIndex < 0)
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
            //ASSIGN doctor or room for patient
            //assign values for patient assignment object
            PatientAssignment patientAssignment = new PatientAssignment();
            var selectedAssignentType = (ComoBoxList)cBoxAssignType.SelectedItem;
            patientAssignment.patient_id = patient.PatientID;
            patientAssignment.assignment_type = selectedAssignentType.Id;
            patientAssignment.assigned_to = cBoxAssignValue.Text.Trim();
            patientAssignment.Invoice = patient.VochourCode;
            //excute the dbContext for Patientassignment
            var savePatientAssignment = dbContext.savePatientAssignment(patientAssignment);
            if (!savePatientAssignment.IsPassed)
            {
                MessageBox.Show($"{savePatientAssignment.ErrorMessage}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            MessageBox.Show("Patient Assigned Successfully!", "Success",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
            return;

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
    }
}
