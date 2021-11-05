using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Mech423Lab3Ex4
{
    public partial class Form1 : Form
    {
        ConcurrentQueue<Int32> Posvalues = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> Velvalues = new ConcurrentQueue<Int32>();
        int x = 0;
        int pval = 0;
        int vval = 0;
        Series posdata = new Series();
        Series veldata = new Series();
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Position Chart Init
            PosChart.Series.Add(posdata);
            posdata.Name = "Position Data";
            posdata.ChartType = SeriesChartType.Line;
            posdata.BorderWidth = 2;
            posdata.Color = Color.Purple;
            posdata.XValueType = ChartValueType.Int32;
            posdata.YValueType = ChartValueType.Int32;
            //Velocity Chart Init
            VelChart.Series.Add(veldata);
            veldata.Name = "Velocity Data";
            veldata.ChartType = SeriesChartType.Line;
            veldata.BorderWidth = 2;
            veldata.Color = Color.Purple;
            veldata.XValueType = ChartValueType.Int32;
            veldata.YValueType = ChartValueType.Int32;
            //Timer Initialization
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(timer1_Tick);
            //Serial Port Init
            serialPort1.PortName = "COM5";

        }
        private void ConBut_MouseClick(object sender, MouseEventArgs e)
        {
/*            //For Testing purposes only
           for (int i = 0; i < 50000; i++)
            {
                int p = rnd.Next(1, 100);
                Posvalues.Enqueue(p);
                int v = rnd.Next(1, 100);
                Velvalues.Enqueue(v);
            }
            MessageBox.Show("Values Generated", "Notice");*/

        }
        private void UpdateSeries()
        {
            if (x < Posvalues.Count)
            {
                Posvalues.TryDequeue(out pval);
                Velvalues.TryDequeue(out vval);
                if (posdata.Points.Count() > 100) posdata.Points.RemoveAt(0);
                if (veldata.Points.Count() > 100) veldata.Points.RemoveAt(0);
                posdata.Points.AddXY(x, pval);
                veldata.Points.AddXY(x, vval);
                PosChart.ResetAutoValues();
                VelChart.ResetAutoValues();
                x++;
            }
            else
            {
                timer1.Enabled = false;
                MessageBox.Show("No More Data", "Error");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateSeries();
        }

        private void PlotBut_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
