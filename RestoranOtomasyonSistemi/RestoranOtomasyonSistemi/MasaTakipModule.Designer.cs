namespace RestoranOtomasyonSistemi
{
    partial class MasaTakipModule
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
            masa5 = new Button();
            masa3 = new Button();
            masa4 = new Button();
            masa1 = new Button();
            masa2 = new Button();
            SuspendLayout();
            // 
            // masa5
            // 
            masa5.BackColor = Color.GreenYellow;
            masa5.Font = new Font("Segoe UI", 14F);
            masa5.Location = new Point(392, 372);
            masa5.Name = "masa5";
            masa5.Size = new Size(210, 105);
            masa5.TabIndex = 0;
            masa5.Text = "Masa 5";
            masa5.UseVisualStyleBackColor = false;
            masa5.Click += masa5_Click;
            // 
            // masa3
            // 
            masa3.BackColor = Color.GreenYellow;
            masa3.Font = new Font("Segoe UI", 14F);
            masa3.Location = new Point(149, 233);
            masa3.Name = "masa3";
            masa3.Size = new Size(210, 105);
            masa3.TabIndex = 1;
            masa3.Text = "Masa 3";
            masa3.UseVisualStyleBackColor = false;
            masa3.Click += masa3_Click;
            // 
            // masa4
            // 
            masa4.BackColor = Color.GreenYellow;
            masa4.Font = new Font("Segoe UI", 14F);
            masa4.Location = new Point(627, 233);
            masa4.Name = "masa4";
            masa4.Size = new Size(210, 105);
            masa4.TabIndex = 2;
            masa4.Text = "Masa 4";
            masa4.UseVisualStyleBackColor = false;
            masa4.Click += masa4_Click;
            // 
            // masa1
            // 
            masa1.BackColor = Color.GreenYellow;
            masa1.Font = new Font("Segoe UI", 14F);
            masa1.Location = new Point(251, 76);
            masa1.Name = "masa1";
            masa1.Size = new Size(210, 105);
            masa1.TabIndex = 3;
            masa1.Text = "Masa 1";
            masa1.UseVisualStyleBackColor = false;
            masa1.Click += masa1_Click;
            // 
            // masa2
            // 
            masa2.BackColor = Color.GreenYellow;
            masa2.Font = new Font("Segoe UI", 14F);
            masa2.Location = new Point(530, 76);
            masa2.Name = "masa2";
            masa2.Size = new Size(210, 105);
            masa2.TabIndex = 4;
            masa2.Text = "Masa 2";
            masa2.UseVisualStyleBackColor = false;
            masa2.Click += masa2_Click;
            // 
            // MasaTakipModule
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(942, 550);
            Controls.Add(masa2);
            Controls.Add(masa1);
            Controls.Add(masa4);
            Controls.Add(masa3);
            Controls.Add(masa5);
            Name = "MasaTakipModule";
            Text = "MasaTakipModule";
            ResumeLayout(false);
        }

        #endregion

        private Button masa5;
        private Button masa3;
        private Button masa4;
        private Button masa1;
        private Button masa2;
    }
}