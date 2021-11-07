
namespace Mech423Lab3Ex5
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.ConBut = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.PosChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.VelChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SliCon = new System.Windows.Forms.TrackBar();
            this.SliConCheck = new System.Windows.Forms.CheckBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.posBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.velBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.VelCountBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.pwmrotchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.PWM_0_Button = new System.Windows.Forms.Button();
            this.PWM_25_Button = new System.Windows.Forms.Button();
            this.PWM_50_Button = new System.Windows.Forms.Button();
            this.PWM_100_Button = new System.Windows.Forms.Button();
            this.PWM_Custom_Button = new System.Windows.Forms.Button();
            this.PWM_Custom_Box = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PosChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliCon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pwmrotchart)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ConBut
            // 
            this.ConBut.Location = new System.Drawing.Point(164, 37);
            this.ConBut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ConBut.Name = "ConBut";
            this.ConBut.Size = new System.Drawing.Size(128, 42);
            this.ConBut.TabIndex = 0;
            this.ConBut.Text = "Connect";
            this.ConBut.UseVisualStyleBackColor = true;
            this.ConBut.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ConBut_MouseClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(36, 37);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 31);
            this.textBox1.TabIndex = 1;
            // 
            // PosChart
            // 
            chartArea1.Name = "ChartArea1";
            this.PosChart.ChartAreas.Add(chartArea1);
            this.PosChart.ImeMode = System.Windows.Forms.ImeMode.Off;
            legend1.Name = "Legend1";
            this.PosChart.Legends.Add(legend1);
            this.PosChart.Location = new System.Drawing.Point(48, 104);
            this.PosChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PosChart.Name = "PosChart";
            this.PosChart.Size = new System.Drawing.Size(1284, 1090);
            this.PosChart.TabIndex = 2;
            this.PosChart.Text = "PositionChart";
            // 
            // VelChart
            // 
            chartArea2.Name = "ChartArea1";
            this.VelChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.VelChart.Legends.Add(legend2);
            this.VelChart.Location = new System.Drawing.Point(1338, 104);
            this.VelChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.VelChart.Name = "VelChart";
            this.VelChart.Size = new System.Drawing.Size(1296, 1090);
            this.VelChart.TabIndex = 3;
            this.VelChart.Text = "VelocityChart";
            // 
            // SliCon
            // 
            this.SliCon.Location = new System.Drawing.Point(48, 1324);
            this.SliCon.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SliCon.Maximum = 100;
            this.SliCon.Name = "SliCon";
            this.SliCon.Size = new System.Drawing.Size(800, 90);
            this.SliCon.TabIndex = 5;
            this.SliCon.Value = 50;
            // 
            // SliConCheck
            // 
            this.SliConCheck.AutoSize = true;
            this.SliConCheck.Location = new System.Drawing.Point(57, 1304);
            this.SliConCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SliConCheck.Name = "SliConCheck";
            this.SliConCheck.Size = new System.Drawing.Size(235, 29);
            this.SliConCheck.TabIndex = 6;
            this.SliConCheck.Text = "Slider Motor Control";
            this.SliConCheck.UseVisualStyleBackColor = true;
            // 
            // posBox
            // 
            this.posBox.Location = new System.Drawing.Point(164, 1204);
            this.posBox.Name = "posBox";
            this.posBox.Size = new System.Drawing.Size(100, 31);
            this.posBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 1207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Position:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1333, 1210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Velocity:";
            // 
            // velBox
            // 
            this.velBox.Location = new System.Drawing.Point(1445, 1207);
            this.velBox.Name = "velBox";
            this.velBox.Size = new System.Drawing.Size(100, 31);
            this.velBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1333, 1255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(286, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Velocity Counts Per Second:";
            // 
            // VelCountBox
            // 
            this.VelCountBox.Location = new System.Drawing.Point(1625, 1252);
            this.VelCountBox.Name = "VelCountBox";
            this.VelCountBox.Size = new System.Drawing.Size(100, 31);
            this.VelCountBox.TabIndex = 11;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(56, 1255);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 31);
            this.textBox2.TabIndex = 14;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(164, 1255);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 31);
            this.textBox3.TabIndex = 15;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(278, 1255);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 31);
            this.textBox4.TabIndex = 16;
            // 
            // pwmrotchart
            // 
            chartArea3.Name = "ChartArea1";
            this.pwmrotchart.ChartAreas.Add(chartArea3);
            this.pwmrotchart.ImeMode = System.Windows.Forms.ImeMode.Off;
            legend3.Name = "Legend1";
            this.pwmrotchart.Legends.Add(legend3);
            this.pwmrotchart.Location = new System.Drawing.Point(993, 1341);
            this.pwmrotchart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pwmrotchart.Name = "pwmrotchart";
            this.pwmrotchart.Size = new System.Drawing.Size(1226, 425);
            this.pwmrotchart.TabIndex = 17;
            this.pwmrotchart.Text = "PWMRPMChart";
            // 
            // PWM_0_Button
            // 
            this.PWM_0_Button.Location = new System.Drawing.Point(57, 1396);
            this.PWM_0_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PWM_0_Button.Name = "PWM_0_Button";
            this.PWM_0_Button.Size = new System.Drawing.Size(157, 42);
            this.PWM_0_Button.TabIndex = 18;
            this.PWM_0_Button.Text = "0% PWM";
            this.PWM_0_Button.UseVisualStyleBackColor = true;
            this.PWM_0_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PWM_0_Button_MouseClick);
            // 
            // PWM_25_Button
            // 
            this.PWM_25_Button.Location = new System.Drawing.Point(221, 1396);
            this.PWM_25_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PWM_25_Button.Name = "PWM_25_Button";
            this.PWM_25_Button.Size = new System.Drawing.Size(156, 42);
            this.PWM_25_Button.TabIndex = 19;
            this.PWM_25_Button.Text = "25% PWM";
            this.PWM_25_Button.UseVisualStyleBackColor = true;
            this.PWM_25_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PWM_25_Button_MouseClick);
            // 
            // PWM_50_Button
            // 
            this.PWM_50_Button.Location = new System.Drawing.Point(385, 1396);
            this.PWM_50_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PWM_50_Button.Name = "PWM_50_Button";
            this.PWM_50_Button.Size = new System.Drawing.Size(156, 42);
            this.PWM_50_Button.TabIndex = 20;
            this.PWM_50_Button.Text = "50% PWM";
            this.PWM_50_Button.UseVisualStyleBackColor = true;
            this.PWM_50_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PWM_50_Button_MouseClick);
            // 
            // PWM_100_Button
            // 
            this.PWM_100_Button.Location = new System.Drawing.Point(548, 1396);
            this.PWM_100_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PWM_100_Button.Name = "PWM_100_Button";
            this.PWM_100_Button.Size = new System.Drawing.Size(157, 42);
            this.PWM_100_Button.TabIndex = 21;
            this.PWM_100_Button.Text = "100% PWM";
            this.PWM_100_Button.UseVisualStyleBackColor = true;
            this.PWM_100_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PWM_100_Button_MouseClick);
            // 
            // PWM_Custom_Button
            // 
            this.PWM_Custom_Button.Location = new System.Drawing.Point(748, 1445);
            this.PWM_Custom_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PWM_Custom_Button.Name = "PWM_Custom_Button";
            this.PWM_Custom_Button.Size = new System.Drawing.Size(208, 42);
            this.PWM_Custom_Button.TabIndex = 22;
            this.PWM_Custom_Button.Text = "Custom% PWM";
            this.PWM_Custom_Button.UseVisualStyleBackColor = true;
            this.PWM_Custom_Button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PWM_Custom_Button_MouseClick);
            // 
            // PWM_Custom_Box
            // 
            this.PWM_Custom_Box.Location = new System.Drawing.Point(748, 1396);
            this.PWM_Custom_Box.Multiline = true;
            this.PWM_Custom_Box.Name = "PWM_Custom_Box";
            this.PWM_Custom_Box.Size = new System.Drawing.Size(209, 43);
            this.PWM_Custom_Box.TabIndex = 23;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2253, 1700);
            this.Controls.Add(this.PWM_Custom_Box);
            this.Controls.Add(this.PWM_Custom_Button);
            this.Controls.Add(this.PWM_100_Button);
            this.Controls.Add(this.PWM_50_Button);
            this.Controls.Add(this.PWM_25_Button);
            this.Controls.Add(this.PWM_0_Button);
            this.Controls.Add(this.pwmrotchart);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.VelCountBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.velBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.posBox);
            this.Controls.Add(this.SliConCheck);
            this.Controls.Add(this.SliCon);
            this.Controls.Add(this.VelChart);
            this.Controls.Add(this.PosChart);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ConBut);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PosChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliCon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pwmrotchart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button ConBut;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart PosChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart VelChart;
        private System.Windows.Forms.TrackBar SliCon;
        private System.Windows.Forms.CheckBox SliConCheck;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.TextBox posBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox velBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox VelCountBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.DataVisualization.Charting.Chart pwmrotchart;
        private System.Windows.Forms.Button PWM_0_Button;
        private System.Windows.Forms.Button PWM_25_Button;
        private System.Windows.Forms.Button PWM_50_Button;
        private System.Windows.Forms.Button PWM_100_Button;
        private System.Windows.Forms.Button PWM_Custom_Button;
        private System.Windows.Forms.TextBox PWM_Custom_Box;
    }
}

