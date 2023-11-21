namespace Patient_Managment_System
{
    partial class VisitTypePopUp
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
            this.lblPatientName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnApply = new DevExpress.XtraEditors.SimpleButton();
            this.lblVisitType = new System.Windows.Forms.Label();
            this.comboBoxVisitType = new System.Windows.Forms.ComboBox();
            this.lblPatientID = new System.Windows.Forms.Label();
            this.panelPopUp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPopUp
            // 
            this.panelPopUp.BackColor = System.Drawing.Color.AliceBlue;
            this.panelPopUp.Controls.Add(this.lblPatientID);
            this.panelPopUp.Controls.Add(this.lblPatientName);
            this.panelPopUp.Controls.Add(this.groupBox1);
            this.panelPopUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPopUp.Location = new System.Drawing.Point(0, 0);
            this.panelPopUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelPopUp.Name = "panelPopUp";
            this.panelPopUp.Size = new System.Drawing.Size(343, 207);
            this.panelPopUp.TabIndex = 0;
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
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnApply);
            this.groupBox1.Controls.Add(this.lblVisitType);
            this.groupBox1.Controls.Add(this.comboBoxVisitType);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 32);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(311, 168);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Visit Type";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.BackColor = System.Drawing.Color.Pink;
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseBackColor = true;
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.AppearanceHovered.BackColor = System.Drawing.Color.LightCoral;
            this.btnCancel.AppearanceHovered.Options.UseBackColor = true;
            this.btnCancel.Location = new System.Drawing.Point(37, 109);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 38);
            this.btnCancel.TabIndex = 7;
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
            this.btnApply.Location = new System.Drawing.Point(210, 109);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(89, 38);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblVisitType
            // 
            this.lblVisitType.AutoSize = true;
            this.lblVisitType.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisitType.Location = new System.Drawing.Point(6, 39);
            this.lblVisitType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVisitType.Name = "lblVisitType";
            this.lblVisitType.Size = new System.Drawing.Size(120, 19);
            this.lblVisitType.TabIndex = 5;
            this.lblVisitType.Text = "Visit Location:";
            // 
            // comboBoxVisitType
            // 
            this.comboBoxVisitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVisitType.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxVisitType.FormattingEnabled = true;
            this.comboBoxVisitType.Location = new System.Drawing.Point(116, 34);
            this.comboBoxVisitType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBoxVisitType.Name = "comboBoxVisitType";
            this.comboBoxVisitType.Size = new System.Drawing.Size(179, 30);
            this.comboBoxVisitType.TabIndex = 4;
            this.comboBoxVisitType.SelectedIndexChanged += new System.EventHandler(this.comboBoxVisitType_SelectedIndexChanged);
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
            // VisitTypePopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(343, 207);
            this.Controls.Add(this.panelPopUp);
            this.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisitTypePopUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Visit Type";
            this.Load += new System.EventHandler(this.VisitTypePopUp_Load);
            this.panelPopUp.ResumeLayout(false);
            this.panelPopUp.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPopUp;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnApply;
        private System.Windows.Forms.Label lblVisitType;
        private System.Windows.Forms.ComboBox comboBoxVisitType;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.Label lblPatientID;
    }
}