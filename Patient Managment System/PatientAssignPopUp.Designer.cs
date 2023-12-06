namespace Patient_Managment_System
{
    partial class PatientAssignPopUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelPopUp = new System.Windows.Forms.Panel();
            this.lblPatientID = new System.Windows.Forms.Label();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cBoxAssignValue = new System.Windows.Forms.ComboBox();
            this.cBoxAssignType = new System.Windows.Forms.ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnApply = new DevExpress.XtraEditors.SimpleButton();
            this.panelPopUp.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPopUp
            // 
            this.panelPopUp.BackColor = System.Drawing.Color.AliceBlue;
            this.panelPopUp.Controls.Add(this.groupBox4);
            this.panelPopUp.Controls.Add(this.lblPatientID);
            this.panelPopUp.Controls.Add(this.lblPatientName);
            this.panelPopUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPopUp.Location = new System.Drawing.Point(0, 0);
            this.panelPopUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelPopUp.Name = "panelPopUp";
            this.panelPopUp.Size = new System.Drawing.Size(442, 273);
            this.panelPopUp.TabIndex = 1;
            // 
            // lblPatientID
            // 
            this.lblPatientID.AutoSize = true;
            this.lblPatientID.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientID.Location = new System.Drawing.Point(235, 5);
            this.lblPatientID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPatientID.Name = "lblPatientID";
            this.lblPatientID.Size = new System.Drawing.Size(41, 19);
            this.lblPatientID.TabIndex = 7;
            this.lblPatientID.Text = "xxxx";
            // 
            // lblPatientName
            // 
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.Location = new System.Drawing.Point(43, 5);
            this.lblPatientName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(41, 19);
            this.lblPatientName.TabIndex = 6;
            this.lblPatientName.Text = "xxxx";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.White;
            this.groupBox4.Controls.Add(this.btnCancel);
            this.groupBox4.Controls.Add(this.btnApply);
            this.groupBox4.Controls.Add(this.cBoxAssignValue);
            this.groupBox4.Controls.Add(this.cBoxAssignType);
            this.groupBox4.Controls.Add(this.label34);
            this.groupBox4.Controls.Add(this.label41);
            this.groupBox4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 39);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(418, 202);
            this.groupBox4.TabIndex = 34;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Patient Assignment";
            // 
            // cBoxAssignValue
            // 
            this.cBoxAssignValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cBoxAssignValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cBoxAssignValue.DisplayMember = "description";
            this.cBoxAssignValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxAssignValue.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBoxAssignValue.FormattingEnabled = true;
            this.cBoxAssignValue.Location = new System.Drawing.Point(114, 96);
            this.cBoxAssignValue.Name = "cBoxAssignValue";
            this.cBoxAssignValue.Size = new System.Drawing.Size(226, 30);
            this.cBoxAssignValue.TabIndex = 25;
            this.cBoxAssignValue.ValueMember = "description";
            this.cBoxAssignValue.SelectedIndexChanged += new System.EventHandler(this.cBoxAssignValue_SelectedIndexChanged);
            // 
            // cBoxAssignType
            // 
            this.cBoxAssignType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cBoxAssignType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cBoxAssignType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxAssignType.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBoxAssignType.FormattingEnabled = true;
            this.cBoxAssignType.Location = new System.Drawing.Point(114, 49);
            this.cBoxAssignType.Name = "cBoxAssignType";
            this.cBoxAssignType.Size = new System.Drawing.Size(226, 30);
            this.cBoxAssignType.TabIndex = 15;
            this.cBoxAssignType.SelectedIndexChanged += new System.EventHandler(this.cBoxAssignType_SelectedIndexChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(45, 99);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(64, 23);
            this.label34.TabIndex = 23;
            this.label34.Text = "Value:";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(50, 57);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(59, 23);
            this.label41.TabIndex = 24;
            this.label41.Text = "Type:";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseBackColor = true;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.AppearanceHovered.BackColor = System.Drawing.Color.LightCoral;
            this.btnCancel.AppearanceHovered.Options.UseBackColor = true;
            this.btnCancel.Location = new System.Drawing.Point(35, 158);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 38);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Exit";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Appearance.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnApply.Appearance.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Appearance.Options.UseBackColor = true;
            this.btnApply.Appearance.Options.UseFont = true;
            this.btnApply.AppearanceHovered.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnApply.AppearanceHovered.Options.UseBackColor = true;
            this.btnApply.Location = new System.Drawing.Point(222, 158);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(89, 38);
            this.btnApply.TabIndex = 26;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // PatientAssignPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(442, 273);
            this.Controls.Add(this.panelPopUp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PatientAssignPopUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PatientAssignPopUp";
            this.Load += new System.EventHandler(this.PatientAssignPopUp_Load);
            this.panelPopUp.ResumeLayout(false);
            this.panelPopUp.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPopUp;
        private System.Windows.Forms.Label lblPatientID;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cBoxAssignValue;
        private System.Windows.Forms.ComboBox cBoxAssignType;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label41;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnApply;
    }
}