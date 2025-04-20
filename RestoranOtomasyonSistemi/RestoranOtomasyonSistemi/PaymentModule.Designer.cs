namespace RestoranOtomasyonSistemi
{
    partial class PaymentModule
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
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 20F);
            button1.ForeColor = SystemColors.ControlText;
            button1.Location = new Point(49, 455);
            button1.Name = "button1";
            button1.Size = new Size(285, 111);
            button1.TabIndex = 0;
            button1.Text = "Ödemeyi Tamamla";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(49, 400);
            label1.Name = "label1";
            label1.Size = new Size(135, 28);
            label1.TabIndex = 1;
            label1.Text = "Toplam Tutar :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(190, 400);
            label2.Name = "label2";
            label2.Size = new Size(75, 28);
            label2.TabIndex = 2;
            label2.Text = "2220TL";
            label2.TextAlign = ContentAlignment.MiddleRight;
            label2.Click += label2_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(49, 25);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(285, 372);
            dataGridView1.TabIndex = 3;
            // 
            // PaymentModule
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.BlanchedAlmond;
            ClientSize = new Size(375, 595);
            Controls.Add(dataGridView1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "PaymentModule";
            Text = "PaymentModule";
            Load += PaymentModule_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private DataGridView dataGridView1;
    }
}