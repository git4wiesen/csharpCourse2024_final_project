namespace SchulplanOrganisation.Ui.EditUi
{
    partial class EditKlasse
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
            numJahrgang = new NumericUpDown();
            numStufe = new NumericUpDown();
            button1 = new Button();
            button2 = new Button();
            numBuchstabe = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numJahrgang).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStufe).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBuchstabe).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 0;
            label1.Text = "Jahrgang:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 1;
            label2.Text = "Klassenstufe:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 80);
            label3.Name = "label3";
            label3.Size = new Size(109, 15);
            label3.TabIndex = 2;
            label3.Text = "Klassen-Buchstabe:";
            // 
            // numJahrgang
            // 
            numJahrgang.Location = new Point(148, 18);
            numJahrgang.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            numJahrgang.Minimum = new decimal(new int[] { 1960, 0, 0, 0 });
            numJahrgang.Name = "numJahrgang";
            numJahrgang.Size = new Size(120, 23);
            numJahrgang.TabIndex = 3;
            numJahrgang.TextAlign = HorizontalAlignment.Center;
            numJahrgang.Value = new decimal(new int[] { 1960, 0, 0, 0 });
            // 
            // numStufe
            // 
            numStufe.Location = new Point(148, 47);
            numStufe.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            numStufe.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numStufe.Name = "numStufe";
            numStufe.Size = new Size(120, 23);
            numStufe.TabIndex = 4;
            numStufe.TextAlign = HorizontalAlignment.Center;
            numStufe.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // button1
            // 
            button1.Location = new Point(12, 120);
            button1.Name = "button1";
            button1.Size = new Size(114, 23);
            button1.TabIndex = 5;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += CmdOk_Click;
            // 
            // button2
            // 
            button2.Location = new Point(154, 120);
            button2.Name = "button2";
            button2.Size = new Size(114, 23);
            button2.TabIndex = 6;
            button2.Text = "Abbrechen";
            button2.UseVisualStyleBackColor = true;
            button2.Click += CmdCancel_Click;
            // 
            // numBuchstabe
            // 
            numBuchstabe.Location = new Point(148, 76);
            numBuchstabe.Name = "numBuchstabe";
            numBuchstabe.Size = new Size(120, 23);
            numBuchstabe.TabIndex = 7;
            numBuchstabe.TextAlign = HorizontalAlignment.Center;
            // 
            // EditKlasse
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 154);
            Controls.Add(numBuchstabe);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(numStufe);
            Controls.Add(numJahrgang);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "EditKlasse";
            Text = "Klasse editieren";
            Load += EditKlasse_Load;
            ((System.ComponentModel.ISupportInitialize)numJahrgang).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStufe).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBuchstabe).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown numJahrgang;
        private NumericUpDown numStufe;
        private Button button1;
        private Button button2;
        private NumericUpDown numBuchstabe;
    }
}