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
    public partial class PatientAssignPopUp : Form
    {
        List<string> patientAssignments = new List<string> { "Doctor", "Room Number" };
        List<string> roomNumbers = new List<string> { "Room 1", "Room 2", "Room 3", "Room 4" };
        List<string> doctors = new List<string> { "Dr xxxx", "Dr yyyyy", "Dr zzzzzz" };
        public Patient patient { get; set; }    
        public PatientAssignPopUp()
        {
            InitializeComponent();
        }

        private void panelPopUp_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PatientAssignPopUp_Load(object sender, EventArgs e)
        {
            cBoxAssignType.DataSource = patientAssignments;
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
            MessageBox.Show("Patient Assigned Successfully!", "Success",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void cBoxAssignType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cBoxAssignType.BackColor = SystemColors.Window;
            if (cBoxAssignType.SelectedIndex == 0)
            {
                cBoxAssignValue.DataSource = doctors;
            }
            else
            {
                cBoxAssignValue.DataSource = roomNumbers;
            }

        }
    }
}
