﻿

namespace RestoranOtomasyonSistemi
{
    partial class FoodOrderModule
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            button3 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 424);
            button1.Name = "button1";
            button1.Size = new Size(101, 23);
            button1.TabIndex = 0;
            button1.Text = "Personel Girişi";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.YellowGreen;
            button2.Location = new Point(678, 368);
            button2.Name = "button2";
            button2.Size = new Size(248, 47);
            button2.TabIndex = 1;
            button2.Text = "Sipariş Ver";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(409, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(517, 295);
            dataGridView1.TabIndex = 2;
            // 
            // button3
            // 
            button3.BackColor = Color.RosyBrown;
            button3.Location = new Point(409, 368);
            button3.Name = "button3";
            button3.Size = new Size(248, 47);
            button3.TabIndex = 3;
            button3.Text = "Temizle";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 162);
            textBox1.Location = new Point(678, 313);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(248, 39);
            textBox1.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label1.Location = new Point(491, 316);
            label1.Name = "label1";
            label1.Size = new Size(166, 32);
            label1.TabIndex = 5;
            label1.Text = "Toplam Tutar :";
            label1.Click += label1_Click;
            // 
            // FoodOrderModule
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(951, 450);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(dataGridView1);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FoodOrderModule";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sipariş Sistemi";
            Activated += OnFormActivated;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog(); 
        }


        private Button button1;
        private Button button2;
        private DataGridView dataGridView1;
        private Button button3;
        private TextBox textBox1;
        private Label label1;
    }
}

#endregion