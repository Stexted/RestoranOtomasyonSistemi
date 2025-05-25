

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
            components = new System.ComponentModel.Container();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            button3 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            button1 = new Button();
            button4 = new Button();
            label2 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            label3 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
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
            // button1
            // 
            button1.BackColor = Color.Orange;
            button1.Location = new Point(27, 368);
            button1.Name = "button1";
            button1.Size = new Size(124, 47);
            button1.TabIndex = 6;
            button1.Text = "Müşteri Oturdu";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.GreenYellow;
            button4.Location = new Point(157, 368);
            button4.Name = "button4";
            button4.Size = new Size(124, 47);
            button4.TabIndex = 7;
            button4.Text = "Müşteri Kalktı";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(669, 444);
            label2.Name = "label2";
            label2.Size = new Size(170, 21);
            label2.TabIndex = 8;
            label2.Text = "Masa Oturma Süresi :";
            label2.TextAlign = ContentAlignment.MiddleRight;
            label2.Click += label2_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.Location = new Point(860, 444);
            label3.Name = "label3";
            label3.Size = new Size(66, 21);
            label3.TabIndex = 9;
            label3.Text = "ss:dd:ss";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(12, 12);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(376, 295);
            flowLayoutPanel1.TabIndex = 10;
            flowLayoutPanel1.WrapContents = false;
            // 
            // FoodOrderModule
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(951, 474);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button4);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(dataGridView1);
            Controls.Add(button2);
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

        private Button button2;
        private DataGridView dataGridView1;
        private Button button3;
        private TextBox textBox1;
        private Label label1;
        private Button button1;
        private Button button4;
        private Label label2;
        private System.Windows.Forms.Timer timer1;
        private Label label3;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}

#endregion