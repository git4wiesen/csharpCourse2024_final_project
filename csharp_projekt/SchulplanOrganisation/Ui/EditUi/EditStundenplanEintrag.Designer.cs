namespace SchulplanOrganisation.Ui.EditUi
{
    partial class EditStundenplanEintrag
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            cmdOk = new Button();
            cmdCancel = new Button();
            label4 = new Label();
            label5 = new Label();
            dataLehrer = new DataGridView();
            dataUnterrichtsfach = new DataGridView();
            dataGridView3 = new DataGridView();
            cbSchultag = new ComboBox();
            cbZeitSlot = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataLehrer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataUnterrichtsfach).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 0;
            label1.Text = "Schultag:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 1;
            label2.Text = "Zeit-Slot:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label3.Location = new Point(12, 100);
            label3.Name = "label3";
            label3.Size = new Size(63, 21);
            label3.TabIndex = 2;
            label3.Text = "Lehrer:";
            // 
            // cmdOk
            // 
            cmdOk.Location = new Point(436, 613);
            cmdOk.Name = "cmdOk";
            cmdOk.Size = new Size(114, 23);
            cmdOk.TabIndex = 5;
            cmdOk.Text = "OK";
            cmdOk.UseVisualStyleBackColor = true;
            cmdOk.Click += CmdOk_Click;
            // 
            // cmdCancel
            // 
            cmdCancel.Location = new Point(578, 613);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new Size(114, 23);
            cmdCancel.TabIndex = 6;
            cmdCancel.Text = "Abbrechen";
            cmdCancel.UseVisualStyleBackColor = true;
            cmdCancel.Click += CmdCancel_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label4.Location = new Point(372, 100);
            label4.Name = "label4";
            label4.Size = new Size(132, 21);
            label4.TabIndex = 7;
            label4.Text = "Unterrichtsfach:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label5.Location = new Point(732, 100);
            label5.Name = "label5";
            label5.Size = new Size(61, 21);
            label5.TabIndex = 10;
            label5.Text = "Klasse:";
            // 
            // dataLehrer
            // 
            dataLehrer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataLehrer.Location = new Point(12, 124);
            dataLehrer.Name = "dataLehrer";
            dataLehrer.Size = new Size(345, 455);
            dataLehrer.TabIndex = 11;
            // 
            // dataUnterrichtsfach
            // 
            dataUnterrichtsfach.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataUnterrichtsfach.Location = new Point(372, 124);
            dataUnterrichtsfach.Name = "dataUnterrichtsfach";
            dataUnterrichtsfach.Size = new Size(345, 455);
            dataUnterrichtsfach.TabIndex = 12;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(732, 124);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(345, 455);
            dataGridView3.TabIndex = 13;
            // 
            // cbSchultag
            // 
            cbSchultag.FormattingEnabled = true;
            cbSchultag.Location = new Point(86, 17);
            cbSchultag.Name = "cbSchultag";
            cbSchultag.Size = new Size(271, 23);
            cbSchultag.TabIndex = 14;
            // 
            // cbZeitSlot
            // 
            cbZeitSlot.FormattingEnabled = true;
            cbZeitSlot.Location = new Point(86, 46);
            cbZeitSlot.Name = "cbZeitSlot";
            cbZeitSlot.Size = new Size(271, 23);
            cbZeitSlot.TabIndex = 15;
            // 
            // EditStundenplanEintrag
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1091, 648);
            Controls.Add(cbZeitSlot);
            Controls.Add(cbSchultag);
            Controls.Add(dataGridView3);
            Controls.Add(dataUnterrichtsfach);
            Controls.Add(dataLehrer);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(cmdCancel);
            Controls.Add(cmdOk);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "EditStundenplanEintrag";
            Text = "Klassenschulplan-Eintrag editieren";
            Load += EditKlasse_Load;
            ((System.ComponentModel.ISupportInitialize)dataLehrer).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataUnterrichtsfach).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Button cmdOk;
        private Button cmdCancel;
        private Label label4;
        private TextBox txtErsterVorname;
        private Label label5;
        private DataGridView dataLehrer;
        private DataGridView dataUnterrichtsfach;
        private DataGridView dataGridView3;
        private ComboBox cbSchultag;
        private ComboBox cbZeitSlot;
    }
}