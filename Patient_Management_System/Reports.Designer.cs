namespace Patient_Management_System
{
    partial class Reports
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reports));
            this.grpSystemReports = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chartFinancials = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTotalUnpaid = new System.Windows.Forms.Label();
            this.lblTotalPaidAmount = new System.Windows.Forms.Label();
            this.lblTotalBills = new System.Windows.Forms.Label();
            this.lblTotalMedicalRecords = new System.Windows.Forms.Label();
            this.lblTotalAppointment = new System.Windows.Forms.Label();
            this.lblTotalDoctors = new System.Windows.Forms.Label();
            this.lblTotalPatients = new System.Windows.Forms.Label();
            this.grpSystemReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFinancials)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSystemReports
            // 
            this.grpSystemReports.Controls.Add(this.button1);
            this.grpSystemReports.Controls.Add(this.chartFinancials);
            this.grpSystemReports.Controls.Add(this.pictureBox1);
            this.grpSystemReports.Controls.Add(this.lblTotalUnpaid);
            this.grpSystemReports.Controls.Add(this.lblTotalPaidAmount);
            this.grpSystemReports.Controls.Add(this.lblTotalBills);
            this.grpSystemReports.Controls.Add(this.lblTotalMedicalRecords);
            this.grpSystemReports.Controls.Add(this.lblTotalAppointment);
            this.grpSystemReports.Controls.Add(this.lblTotalDoctors);
            this.grpSystemReports.Controls.Add(this.lblTotalPatients);
            this.grpSystemReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSystemReports.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.grpSystemReports.Location = new System.Drawing.Point(12, 30);
            this.grpSystemReports.Name = "grpSystemReports";
            this.grpSystemReports.Size = new System.Drawing.Size(1271, 546);
            this.grpSystemReports.TabIndex = 0;
            this.grpSystemReports.TabStop = false;
            this.grpSystemReports.Text = "System Statistics & Financial Reports";
            this.grpSystemReports.Enter += new System.EventHandler(this.grpSystemReports_Enter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(80, 503);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 37);
            this.button1.TabIndex = 17;
            this.button1.Text = "back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chartFinancials
            // 
            chartArea1.Name = "ChartArea1";
            this.chartFinancials.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartFinancials.Legends.Add(legend1);
            this.chartFinancials.Location = new System.Drawing.Point(292, 29);
            this.chartFinancials.Name = "chartFinancials";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartFinancials.Series.Add(series1);
            this.chartFinancials.Size = new System.Drawing.Size(973, 508);
            this.chartFinancials.TabIndex = 16;
            this.chartFinancials.Text = "chart1";
            this.chartFinancials.Click += new System.EventHandler(this.chartFinancials_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(26, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // lblTotalUnpaid
            // 
            this.lblTotalUnpaid.AutoSize = true;
            this.lblTotalUnpaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalUnpaid.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalUnpaid.Location = new System.Drawing.Point(22, 445);
            this.lblTotalUnpaid.Name = "lblTotalUnpaid";
            this.lblTotalUnpaid.Size = new System.Drawing.Size(159, 22);
            this.lblTotalUnpaid.TabIndex = 8;
            this.lblTotalUnpaid.Text = "Total Unpaid: $0";
            // 
            // lblTotalPaidAmount
            // 
            this.lblTotalPaidAmount.AutoSize = true;
            this.lblTotalPaidAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPaidAmount.ForeColor = System.Drawing.Color.Green;
            this.lblTotalPaidAmount.Location = new System.Drawing.Point(24, 401);
            this.lblTotalPaidAmount.Name = "lblTotalPaidAmount";
            this.lblTotalPaidAmount.Size = new System.Drawing.Size(136, 22);
            this.lblTotalPaidAmount.TabIndex = 9;
            this.lblTotalPaidAmount.Text = "Total Paid: $0";
            // 
            // lblTotalBills
            // 
            this.lblTotalBills.AutoSize = true;
            this.lblTotalBills.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalBills.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblTotalBills.Location = new System.Drawing.Point(22, 355);
            this.lblTotalBills.Name = "lblTotalBills";
            this.lblTotalBills.Size = new System.Drawing.Size(107, 20);
            this.lblTotalBills.TabIndex = 10;
            this.lblTotalBills.Text = "Total Bills: 0";
            // 
            // lblTotalMedicalRecords
            // 
            this.lblTotalMedicalRecords.AutoSize = true;
            this.lblTotalMedicalRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalMedicalRecords.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblTotalMedicalRecords.Location = new System.Drawing.Point(19, 296);
            this.lblTotalMedicalRecords.Name = "lblTotalMedicalRecords";
            this.lblTotalMedicalRecords.Size = new System.Drawing.Size(162, 20);
            this.lblTotalMedicalRecords.TabIndex = 11;
            this.lblTotalMedicalRecords.Text = "Medical Records: 0";
            // 
            // lblTotalAppointment
            // 
            this.lblTotalAppointment.AutoSize = true;
            this.lblTotalAppointment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAppointment.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblTotalAppointment.Location = new System.Drawing.Point(24, 224);
            this.lblTotalAppointment.Name = "lblTotalAppointment";
            this.lblTotalAppointment.Size = new System.Drawing.Size(140, 20);
            this.lblTotalAppointment.TabIndex = 12;
            this.lblTotalAppointment.Text = "Appointments: 0";
            // 
            // lblTotalDoctors
            // 
            this.lblTotalDoctors.AutoSize = true;
            this.lblTotalDoctors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDoctors.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblTotalDoctors.Location = new System.Drawing.Point(19, 164);
            this.lblTotalDoctors.Name = "lblTotalDoctors";
            this.lblTotalDoctors.Size = new System.Drawing.Size(137, 20);
            this.lblTotalDoctors.TabIndex = 13;
            this.lblTotalDoctors.Text = "Total Doctors: 0";
            // 
            // lblTotalPatients
            // 
            this.lblTotalPatients.AutoSize = true;
            this.lblTotalPatients.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPatients.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblTotalPatients.Location = new System.Drawing.Point(19, 112);
            this.lblTotalPatients.Name = "lblTotalPatients";
            this.lblTotalPatients.Size = new System.Drawing.Size(140, 20);
            this.lblTotalPatients.TabIndex = 14;
            this.lblTotalPatients.Text = "Total Patients: 0";
            // 
            // Reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 588);
            this.Controls.Add(this.grpSystemReports);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Reports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patient Management System - Analytics Dashboard";
            this.Load += new System.EventHandler(this.Reports_Load);
            this.grpSystemReports.ResumeLayout(false);
            this.grpSystemReports.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFinancials)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSystemReports;
        private System.Windows.Forms.Label lblTotalUnpaid;
        private System.Windows.Forms.Label lblTotalPaidAmount;
        private System.Windows.Forms.Label lblTotalBills;
        private System.Windows.Forms.Label lblTotalMedicalRecords;
        private System.Windows.Forms.Label lblTotalAppointment;
        private System.Windows.Forms.Label lblTotalDoctors;
        private System.Windows.Forms.Label lblTotalPatients;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFinancials;
        private System.Windows.Forms.Button button1;
    }
}