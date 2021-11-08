
namespace Mech423PIDControllerEx5
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.ConBut = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.PosChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.VelChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.posBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.velBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.VelCountBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.targetPWMbox = new System.Windows.Forms.TextBox();
            this.targetPositionbox = new System.Windows.Forms.TextBox();
            this.sendCtrlbut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PosChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelChart)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ConBut
            // 
            this.ConBut.Location = new System.Drawing.Point(164, 37);
            this.ConBut.Margin = new System.Windows.Forms.Padding(4);
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
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 31);
            this.textBox1.TabIndex = 1;
            // 
            // PosChart
            // 
            chartArea7.Name = "ChartArea1";
            this.PosChart.ChartAreas.Add(chartArea7);
            this.PosChart.ImeMode = System.Windows.Forms.ImeMode.Off;
            legend7.Name = "Legend1";
            this.PosChart.Legends.Add(legend7);
            this.PosChart.Location = new System.Drawing.Point(48, 104);
            this.PosChart.Margin = new System.Windows.Forms.Padding(4);
            this.PosChart.Name = "PosChart";
            this.PosChart.Size = new System.Drawing.Size(1284, 1090);
            this.PosChart.TabIndex = 2;
            this.PosChart.Text = "PositionChart";
            // 
            // VelChart
            // 
            chartArea8.Name = "ChartArea1";
            this.VelChart.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.VelChart.Legends.Add(legend8);
            this.VelChart.Location = new System.Drawing.Point(1338, 104);
            this.VelChart.Margin = new System.Windows.Forms.Padding(4);
            this.VelChart.Name = "VelChart";
            this.VelChart.Size = new System.Drawing.Size(1296, 1090);
            this.VelChart.TabIndex = 3;
            this.VelChart.Text = "VelocityChart";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 1300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "Target PWM";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(212, 1300);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 25);
            this.label5.TabIndex = 14;
            this.label5.Text = "Target Position";
            // 
            // targetPWMbox
            // 
            this.targetPWMbox.Location = new System.Drawing.Point(65, 1328);
            this.targetPWMbox.Name = "targetPWMbox";
            this.targetPWMbox.Size = new System.Drawing.Size(100, 31);
            this.targetPWMbox.TabIndex = 15;
            // 
            // targetPositionbox
            // 
            this.targetPositionbox.Location = new System.Drawing.Point(242, 1328);
            this.targetPositionbox.Name = "targetPositionbox";
            this.targetPositionbox.Size = new System.Drawing.Size(100, 31);
            this.targetPositionbox.TabIndex = 16;
            // 
            // sendCtrlbut
            // 
            this.sendCtrlbut.Location = new System.Drawing.Point(400, 1322);
            this.sendCtrlbut.Margin = new System.Windows.Forms.Padding(4);
            this.sendCtrlbut.Name = "sendCtrlbut";
            this.sendCtrlbut.Size = new System.Drawing.Size(128, 42);
            this.sendCtrlbut.TabIndex = 17;
            this.sendCtrlbut.Text = "Send";
            this.sendCtrlbut.UseVisualStyleBackColor = true;
            this.sendCtrlbut.MouseClick += new System.Windows.Forms.MouseEventHandler(this.sendCtrlbut_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2253, 1793);
            this.Controls.Add(this.sendCtrlbut);
            this.Controls.Add(this.targetPositionbox);
            this.Controls.Add(this.targetPWMbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.VelCountBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.velBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.posBox);
            this.Controls.Add(this.VelChart);
            this.Controls.Add(this.PosChart);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ConBut);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PosChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelChart)).EndInit();
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
        private System.Windows.Forms.TextBox posBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox velBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox VelCountBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox targetPWMbox;
        private System.Windows.Forms.TextBox targetPositionbox;
        private System.Windows.Forms.Button sendCtrlbut;
    }
}

