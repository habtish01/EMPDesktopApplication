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
        DataAccessLayer layer = new DataAccessLayer();
       public Patient patient { get; set; }  
        public VisitTypePopUp()
        {
            InitializeComponent();
        }
        
        private void VisitTypePopUp_Load(object sender, EventArgs e)
        {
            lblPatientName.Text = patient.FirstName +" "+patient.MiddleName+" "+patient.LastName;
            lblPatientID.Text = patient.PersonID;
            List<ComoBoxList> dataList = layer.LoadListForVisitTypeCoboBox();

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
            if(comboBoxVisitType.SelectedIndex <= 0)
            {
                comboBoxVisitType.BackColor = Color.LightPink;
                comboBoxVisitType.Focus();
                return;
            }
            ComoBoxList selctedItm=(ComoBoxList)comboBoxVisitType.SelectedItem;

               var closed = layer.StartVisit(patient.PatientID, selctedItm.Id);
            if (closed)
            {

                MessageBox.Show("Patient Visit Started!", "Success",
                                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBoxVisitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxVisitType.BackColor=SystemColors.Window;
        }
    }
}
