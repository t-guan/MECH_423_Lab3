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
        ConcurrentQueue<double> Posvalues = new ConcurrentQueue<double>();
        ConcurrentQueue<double> Velvalues = new ConcurrentQueue<double>();
        ConcurrentQueue<Int32> databyte = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> DirectionByte = new ConcurrentQueue<Int32>();
        ConcurrentQueue<Int32> PWMByte = new ConcurrentQueue<Int32>();
        int x = 0;
        double pval = 0;
        double vval = 0;
        int freq = 1000;
        double circ = 0.523;
        private static int dirval;
        private static int pwmval;
        private static int sliderticks = 8;
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
            posdata.XValueType = ChartValueType.Double;
            posdata.YValueType = ChartValueType.Double;
            //Velocity Chart Init
            VelChart.Series.Add(veldata);
            veldata.Name = "Velocity Data";
            veldata.ChartType = SeriesChartType.Line;
            veldata.BorderWidth = 2;
            veldata.Color = Color.Purple;
            veldata.XValueType = ChartValueType.Double;
            veldata.YValueType = ChartValueType.Double;
            //Timer Initialization
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Interval = 100;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Enabled = true;
            //Serial Port Init
            serialPort1.PortName = "COM5";

        }
        private void ConBut_MouseClick(object sender, MouseEventArgs e)
        {
            serialPort1.Open();
            //For Testing purposes only
           /* for (int i = 0; i < 50000; i++)
            {
                double p = rnd.Next(-100, 100);
                p = p * circ;
                Posvalues.Enqueue(p);
                double v= rnd.Next(-100, 100);
                v = v * circ;
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
        private void PreparePackets(int direction, int pwm)
        {
            if (direction == 0 || direction == 1 || direction == 2)
            {
                PrepareDirPacket(direction);
                PreparePWMPacket(pwm);
                if (DirectionByte.Count == 5 && PWMByte.Count == 5)
                {
                    SendPackets();
                }
                else
                {
                    DestroyPackets();
                }
            }
            else
            {
                MessageBox.Show("Invalid Direction, No Action Taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PrepareDirPacket(int direction)
        {
            //Enqueue first byte
            DirectionByte.Enqueue(255);
            //Enqueue direction byte
            DirectionByte.Enqueue(direction);
            //Fill rest with Zeros
            for (int i = 0; i < 3; i++)
            {
                DirectionByte.Enqueue(0);
            }
        }
        private void DestroyPackets()
        {
            int garbage;
            while (DirectionByte.Count > 0)
            {
                DirectionByte.TryDequeue(out garbage);
            }
            while (PWMByte.Count > 0)
            {
                PWMByte.TryDequeue(out garbage);
            }
        }
        private void PreparePWMPacket(int pwm)
        {
            if (pwm > 100)
            {
                pwm = 65535;
            }
            else if (pwm < 0)
            {
                pwm = 0;
            }
            else
            {
                pwm = (int)((double)pwm / 100 * 65535);
            }
            ushort pwmnum16 = Convert.ToUInt16(pwm);
            byte upperpwm = (byte)(pwmnum16 >> 8);
            byte lowerpwm = (byte)(pwmnum16 & 0xff);
            byte esc = 0;

            if (upperpwm == 255 && lowerpwm == 255)
            {
                upperpwm = 0;
                lowerpwm = 0;
                esc = 3;
            }
            else if (upperpwm == 255 && lowerpwm != 255)
            {
                upperpwm = 0;
                esc = 2;
            }
            else if (upperpwm != 255 && lowerpwm == 255)
            {
                lowerpwm = 0;
                esc = 1;
            }
            else
            {
                esc = 0;
            }

            //Enqueue First Byte
            PWMByte.Enqueue(255);
            //Enqeue 0 for direction, so the direction is not chagned
            PWMByte.Enqueue(0);
            //Enqueue the Upper Byte first
            PWMByte.Enqueue(upperpwm);
            //Enqueue the lower byte
            PWMByte.Enqueue(lowerpwm);
            // Enqueue escape byte
            PWMByte.Enqueue(esc);
        }
        private void SendPackets()
        {
            byte[] Dir_Byte = { 0, 0, 0, 0, 0 };
            byte[] PWM_Byte = { 0, 0, 0, 0, 0 };
            int value;
            for (int i = 0; i < 5; i++)
            {
                DirectionByte.TryDequeue(out value);
                Dir_Byte[i] = (byte)value;
                PWMByte.TryDequeue(out value);
                PWM_Byte[i] = (byte)value;
            }
            serialPort1.Write(Dir_Byte, 0, 5);
            serialPort1.Write(PWM_Byte, 0, 5);

        }
        //TODO: Variable method for changing and calculating based on slider ticks,
        //Check if it is above or below 25
        //Depending on distance from 25, or half of the ticks, then scale from 0-100, divide appropriately per tick spacing to end
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (SliConCheck.Checked)
            {
                int sliderpos = SliCon.Value;

                switch (sliderpos)
                {
                    case 8:
                        //Assuming 1 is forwards
                        dirval = 1;
                        //25% PWM, each increment will represent another 25% increase
                        pwmval = 0;
                        break;
                    case 7:
                        dirval = 1;
                        pwmval = 25;
                        break;
                    case 6:
                        dirval = 1;
                        pwmval = 50;
                        break;
                    case 5:
                        dirval = 1;
                        pwmval = 75;
                        break;
                    case 4:
                        dirval = 0;
                        pwmval = 100;
                        break;
                    case 3:
                        //Assuming 2 is backwards
                        dirval = 2;
                        // 25% PWM, Each increment will represent 25% PWM
                        pwmval = 75;
                        break;
                    case 2:
                        dirval = 2;
                        pwmval = 50;
                        break;
                    case 1:
                        dirval = 2;
                        pwmval = 25;
                        break;
                    case 0:
                        dirval = 2;
                        pwmval = 0;
                        break;
                }
                //Send the read values to PreparePackets
                PreparePackets(dirval, pwmval);

            }
        }

        private void PlotBut_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
