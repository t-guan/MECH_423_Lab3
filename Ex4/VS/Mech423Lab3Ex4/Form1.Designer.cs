
namespace Mech423Lab3Ex4
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.ConBut = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.PosChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.VelChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.PlotBut = new System.Windows.Forms.Button();
            this.SliCon = new System.Windows.Forms.TrackBar();
            this.SliConCheck = new System.Windows.Forms.CheckBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PosChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliCon)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ConBut
            // 
            this.ConBut.Location = new System.Drawing.Point(82, 19);
            this.ConBut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ConBut.Name = "ConBut";
            this.ConBut.Size = new System.Drawing.Size(64, 22);
            this.ConBut.TabIndex = 0;
            this.ConBut.Text = "Connect";
            this.ConBut.UseVisualStyleBackColor = true;
            this.ConBut.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ConBut_MouseClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 19);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(52, 20);
            this.textBox1.TabIndex = 1;
            // 
            // PosChart
            // 
            chartArea1.Name = "ChartArea1";
            this.PosChart.ChartAreas.Add(chartArea1);
            this.PosChart.ImeMode = System.Windows.Forms.ImeMode.Off;
            legend1.Name = "Legend1";
            this.PosChart.Legends.Add(legend1);
            this.PosChart.Location = new System.Drawing.Point(24, 54);
            this.PosChart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PosChart.Name = "PosChart";
            this.PosChart.Size = new System.Drawing.Size(642, 567);
            this.PosChart.TabIndex = 2;
            this.PosChart.Text = "PositionChart";
            // 
            // VelChart
            // 
            chartArea2.Name = "ChartArea1";
            this.VelChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.VelChart.Legends.Add(legend2);
            this.VelChart.Location = new System.Drawing.Point(669, 54);
            this.VelChart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.VelChart.Name = "VelChart";
            this.VelChart.Size = new System.Drawing.Size(648, 567);
            this.VelChart.TabIndex = 3;
            this.VelChart.Text = "VelocityChart";
            // 
            // PlotBut
            // 
            this.PlotBut.Location = new System.Drawing.Point(196, 19);
            this.PlotBut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PlotBut.Name = "PlotBut";
            this.PlotBut.Size = new System.Drawing.Size(72, 22);
            this.PlotBut.TabIndex = 4;
            this.PlotBut.Text = "Plot";
            this.PlotBut.UseVisualStyleBackColor = true;
            this.PlotBut.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PlotBut_MouseClick);
            // 
            // SliCon
            // 
            this.SliCon.Location = new System.Drawing.Point(24, 667);
            this.SliCon.Maximum = 50;
            this.SliCon.Name = "SliCon";
            this.SliCon.Size = new System.Drawing.Size(400, 45);
            this.SliCon.TabIndex = 5;
            this.SliCon.Value = 25;
            // 
            // SliConCheck
            // 
            this.SliConCheck.AutoSize = true;
            this.SliConCheck.Location = new System.Drawing.Point(24, 644);
            this.SliConCheck.Name = "SliConCheck";
            this.SliConCheck.Size = new System.Drawing.Size(118, 17);
            this.SliConCheck.TabIndex = 6;
            this.SliConCheck.Text = "Slider Motor Control";
            this.SliConCheck.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 891);
            this.Controls.Add(this.SliConCheck);
            this.Controls.Add(this.SliCon);
            this.Controls.Add(this.PlotBut);
            this.Controls.Add(this.VelChart);
            this.Controls.Add(this.PosChart);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ConBut);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PosChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliCon)).EndInit();
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
        private System.Windows.Forms.Button PlotBut;
        private System.Windows.Forms.TrackBar SliCon;
        private System.Windows.Forms.CheckBox SliConCheck;
        private System.Windows.Forms.Timer timer2;
    }
}

