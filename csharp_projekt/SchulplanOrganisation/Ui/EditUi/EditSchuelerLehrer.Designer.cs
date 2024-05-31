namespace SchulplanOrganisation.Ui.EditUi
{
    partial class EditSchuelerLehrer
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
            button1 = new Button();
            button2 = new Button();
            label4 = new Label();
            txtNachname = new TextBox();
            txtErsterVorname = new TextBox();
            txtAndereVornamen = new TextBox();
            dateGeburtstag = new DateTimePicker();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 0;
            label1.Text = "Nachname:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(89, 15);
            label2.TabIndex = 1;
            label2.Text = "Erster Vorname:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 80);
            label3.Name = "label3";
            label3.Size = new Size(105, 15);
            label3.TabIndex = 2;
            label3.Text = "Andere Vornamen:";
            // 
            // button1
            // 
            button1.Location = new Point(172, 170);
            button1.Name = "button1";
            button1.Size = new Size(114, 23);
            button1.TabIndex = 5;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += CmdOk_Click;
            // 
            // button2
            // 
            button2.Location = new Point(314, 170);
            button2.Name = "button2";
            button2.Size = new Size(114, 23);
            button2.TabIndex = 6;
            button2.Text = "Abbrechen";
            button2.UseVisualStyleBackColor = true;
            button2.Click += CmdCancel_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 110);
            label4.Name = "label4";
            label4.Size = new Size(68, 15);
            label4.TabIndex = 7;
            label4.Text = "Geburtstag:";
            // 
            // txtNachname
            // 
            txtNachname.Location = new Point(165, 19);
            txtNachname.Margin = new Padding(2, 1, 2, 1);
            txtNachname.Name = "txtNachname";
            txtNachname.Size = new Size(422, 23);
            txtNachname.TabIndex = 8;
            // 
            // txtErsterVorname
            // 
            txtErsterVorname.Location = new Point(165, 50);
            txtErsterVorname.Margin = new Padding(2, 1, 2, 1);
            txtErsterVorname.Name = "txtErsterVorname";
            txtErsterVorname.Size = new Size(422, 23);
            txtErsterVorname.TabIndex = 9;
            // 
            // txtAndereVornamen
            // 
            txtAndereVornamen.Location = new Point(165, 80);
            txtAndereVornamen.Margin = new Padding(2, 1, 2, 1);
            txtAndereVornamen.Name = "txtAndereVornamen";
            txtAndereVornamen.Size = new Size(422, 23);
            txtAndereVornamen.TabIndex = 10;
            // 
            // dateGeburtstag
            // 
            dateGeburtstag.DropDownAlign = LeftRightAlignment.Right;
            dateGeburtstag.Format = DateTimePickerFormat.Custom;
            dateGeburtstag.Location = new Point(165, 110);
            dateGeburtstag.Margin = new Padding(2, 1, 2, 1);
            dateGeburtstag.Name = "dateGeburtstag";
            dateGeburtstag.Size = new Size(422, 23);
            dateGeburtstag.TabIndex = 11;
            // 
            // EditSchuelerLehrer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(616, 209);
            Controls.Add(dateGeburtstag);
            Controls.Add(txtAndereVornamen);
            Controls.Add(txtErsterVorname);
            Controls.Add(txtNachname);
            Controls.Add(label4);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "EditSchuelerLehrer";
            Text = "<?> editieren";
            Load += EditKlasse_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Button button1;
        private Button button2;
        private Label label4;
        private TextBox txtNachname;
        private TextBox txtErsterVorname;
        private TextBox txtAndereVornamen;
        private DateTimePicker dateGeburtstag;
    }
}