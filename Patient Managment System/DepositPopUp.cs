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
    public partial class DepositPopUp : Form
    {
        public Patient patient { get; set; }    
        public DepositPopUp()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DepositPopUp_Load(object sender, EventArgs e)
        {
            lblPatientName.Text=patient.FirstName+" "+patient.MiddleName+" "+patient.LastName;
            lblPatientID.Text = patient.PersonID;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDepositAmount.Text.Trim()))
            {
                txtDepositAmount.BackColor = Color.LightPink;
                txtDepositAmount.Focus();
                return;
            }

            MessageBox.Show(txtDepositAmount.Text.Trim()+" Birr Deposited for this patient!", "Success",
                                                  MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void txtDepositAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void txtDepositAmount_EditValueChanged(object sender, EventArgs e)
        {
            txtDepositAmount.BackColor=SystemColors.Window; 
        }
    }
}
